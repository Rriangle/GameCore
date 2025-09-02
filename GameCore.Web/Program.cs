using GameCore.Infrastructure.Data;
using GameCore.Core.Interfaces;
using GameCore.Core.Services;
using GameCore.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.Facebook;

var builder = WebApplication.CreateBuilder(args);

// 設定資料庫連接
builder.Services.AddDbContext<GameCoreDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection") ?? 
        "Server=(localdb)\\mssqllocaldb;Database=GameCore;Trusted_Connection=true;MultipleActiveResultSets=true"
    ));

// 設定身份驗證
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/Logout";
        options.AccessDeniedPath = "/Account/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromDays(30);
        options.SlidingExpiration = true;
        options.Cookie.HttpOnly = true;
        options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
    })
    .AddGoogle(options =>
    {
        options.ClientId = builder.Configuration["Authentication:Google:ClientId"] ?? "";
        options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"] ?? "";
        options.CallbackPath = "/signin-google";
    })
    .AddFacebook(options =>
    {
        options.AppId = builder.Configuration["Authentication:Facebook:AppId"] ?? "";
        options.AppSecret = builder.Configuration["Authentication:Facebook:AppSecret"] ?? "";
        options.CallbackPath = "/signin-facebook";
    });

// 設定授權政策
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireLogin", policy => policy.RequireAuthenticatedUser());
    options.AddPolicy("RequireAdmin", policy => policy.RequireRole("Admin"));
    options.AddPolicy("RequireSalesAuthority", policy => policy.RequireClaim("SalesAuthority", "true"));
});

// 設定 MVC 和 API
builder.Services.AddControllersWithViews(options =>
{
    // 全域授權過濾器
    options.Filters.Add(new Microsoft.AspNetCore.Authorization.AuthorizeAttribute());
})
.AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
    options.SerializerSettings.DateTimeZoneHandling = Newtonsoft.Json.DateTimeZoneHandling.Utc;
});

// 設定 AutoMapper
builder.Services.AddAutoMapper(typeof(Program));

// 設定 CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:5000", "https://localhost:5001")
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});

// 設定 Session
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// 設定記憶體快取
builder.Services.AddMemoryCache();

// 註冊 Repository
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IPetRepository, PetRepository>();
builder.Services.AddScoped<ISignInRepository, SignInRepository>();
builder.Services.AddScoped<IMiniGameRepository, MiniGameRepository>();
builder.Services.AddScoped<IGameRepository, GameRepository>();
builder.Services.AddScoped<IForumRepository, ForumRepository>();
builder.Services.AddScoped<IStoreRepository, StoreRepository>();
builder.Services.AddScoped<IPlayerMarketRepository, PlayerMarketRepository>();
builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
builder.Services.AddScoped<IChatRepository, ChatRepository>();
builder.Services.AddScoped<IManagerRepository, ManagerRepository>();

// 註冊服務
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IPetService, PetService>();
builder.Services.AddScoped<ISignInService, SignInService>();
builder.Services.AddScoped<IMiniGameService, MiniGameService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IForumService, ForumService>();
builder.Services.AddScoped<IStoreService, StoreService>();
builder.Services.AddScoped<IPlayerMarketService, PlayerMarketService>();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<IChatService, ChatService>();
builder.Services.AddScoped<IWalletService, WalletService>();
builder.Services.AddScoped<ISalesService, SalesService>();

// 設定 Logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

// 設定 SignalR (即時通訊)
builder.Services.AddSignalR();

var app = builder.Build();

// 設定 HTTP 請求管道
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.UseSession();

// 設定路由
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// API 路由
app.MapControllerRoute(
    name: "api",
    pattern: "api/{controller}/{action=Index}/{id?}");

// SignalR Hub 路由
app.MapHub<ChatHub>("/chathub");
app.MapHub<NotificationHub>("/notificationhub");

// 允許匿名訪問的頁面
app.MapControllerRoute(
    name: "public",
    pattern: "{controller=Home}/{action=Index}").AllowAnonymous();

app.MapControllerRoute(
    name: "auth",
    pattern: "Account/{action}",
    defaults: new { controller = "Account" }).AllowAnonymous();

// 確保資料庫已建立並初始化假資料
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<GameCoreDbContext>();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    
    try
    {
        // 建立資料庫
        await context.Database.EnsureCreatedAsync();
        
        // 初始化假資料
        await SeedData.InitializeAsync(app.Services);
        
        // 初始化基礎資料
        await SeedBasicDataAsync(context, logger);
        
        logger.LogInformation("資料庫初始化完成");
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "資料庫初始化失敗");
    }
}

app.Run();

/// <summary>
/// 初始化基礎資料
/// </summary>
static async Task SeedBasicDataAsync(GameCoreDbContext context, ILogger logger)
{
    try
    {
        // 檢查是否已有基礎資料
        if (await context.NotificationSources.AnyAsync())
        {
            return; // 已有資料，跳過初始化
        }

        // 插入通知來源
        var notificationSources = new[]
        {
            new NotificationSource { SourceName = "system" },
            new NotificationSource { SourceName = "forum" },
            new NotificationSource { SourceName = "store" },
            new NotificationSource { SourceName = "market" },
            new NotificationSource { SourceName = "pet" },
            new NotificationSource { SourceName = "admin" }
        };
        
        context.NotificationSources.AddRange(notificationSources);

        // 插入通知行為
        var notificationActions = new[]
        {
            new NotificationAction { ActionName = "signin" },
            new NotificationAction { ActionName = "pet_interact" },
            new NotificationAction { ActionName = "pet_color_change" },
            new NotificationAction { ActionName = "points_adjustment" },
            new NotificationAction { ActionName = "order_created" },
            new NotificationAction { ActionName = "order_completed" },
            new NotificationAction { ActionName = "message_received" },
            new NotificationAction { ActionName = "admin_notice" }
        };
        
        context.NotificationActions.AddRange(notificationActions);

        // 插入基礎遊戲資料
        var games = new[]
        {
            new Game { Name = "英雄聯盟", Genre = "MOBA", CreatedAt = DateTime.UtcNow },
            new Game { Name = "原神", Genre = "RPG", CreatedAt = DateTime.UtcNow },
            new Game { Name = "Valorant", Genre = "FPS", CreatedAt = DateTime.UtcNow },
            new Game { Name = "Apex Legends", Genre = "FPS", CreatedAt = DateTime.UtcNow },
            new Game { Name = "Minecraft", Genre = "Sandbox", CreatedAt = DateTime.UtcNow },
            new Game { Name = "Among Us", Genre = "Social", CreatedAt = DateTime.UtcNow }
        };
        
        context.Games.AddRange(games);

        await context.SaveChangesAsync();

        // 為每個遊戲建立論壇版面
        var forums = games.Select(g => new Forum
        {
            GameId = g.GameId,
            Name = $"{g.Name} 討論版",
            Description = $"{g.Name} 遊戲專屬討論區",
            CreatedAt = DateTime.UtcNow
        });
        
        context.Forums.AddRange(forums);

        // 插入管理員角色權限
        var adminRole = new ManagerRolePermission
        {
            RoleName = "超級管理員",
            AdministratorPrivilegesManagement = true,
            UserStatusManagement = true,
            ShoppingPermissionManagement = true,
            MessagePermissionManagement = true,
            PetRightsManagement = true,
            CustomerService = true
        };
        
        context.ManagerRolePermissions.Add(adminRole);

        await context.SaveChangesAsync();
        
        logger.LogInformation("基礎資料初始化完成");
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "基礎資料初始化失敗");
        throw;
    }
}

// SignalR Hub 類別 (簡化版)
public class ChatHub : Microsoft.AspNetCore.SignalR.Hub
{
    public async Task SendMessage(string user, string message)
    {
        await Clients.All.SendAsync("ReceiveMessage", user, message);
    }
}

public class NotificationHub : Microsoft.AspNetCore.SignalR.Hub
{
    public async Task JoinGroup(string groupName)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
    }

    public async Task LeaveGroup(string groupName)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
    }
}
