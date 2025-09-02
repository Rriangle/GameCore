using GraphQL.Types;
using GraphQL.MicrosoftDI;
using GameCore.Web.GraphQL.Types;
using GameCore.Web.GraphQL.Queries;
using GameCore.Web.GraphQL.Mutations;
using GameCore.Web.GraphQL.Subscriptions;

namespace GameCore.Web.GraphQL.Schema
{
    public class GameCoreSchema : GraphQL.Types.Schema
    {
        public GameCoreSchema(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            Query = serviceProvider.GetRequiredService<GameCoreQuery>();
            Mutation = serviceProvider.GetRequiredService<GameCoreMutation>();
            Subscription = serviceProvider.GetRequiredService<GameCoreSubscription>();
            
            // Advanced schema configuration
            RegisterTypes(
                typeof(UserType),
                typeof(PetType),
                typeof(ProductType),
                typeof(ForumPostType),
                typeof(WalletTransactionType),
                typeof(PlayerMarketItemType),
                typeof(NotificationType),
                typeof(GameSessionType),
                typeof(AchievementType),
                typeof(LeaderboardType)
            );
            
            // Custom scalars
            RegisterType<DateTimeGraphType>();
            RegisterType<DecimalGraphType>();
            RegisterType<UriGraphType>();
            RegisterType<JsonGraphType>();
            
            // Input types
            RegisterTypes(
                typeof(CreateUserInputType),
                typeof(UpdatePetInputType),
                typeof(CreateProductInputType),
                typeof(TransferPointsInputType),
                typeof(CreateForumPostInputType),
                typeof(PlayerMarketListingInputType)
            );
            
            // Enums
            RegisterTypes(
                typeof(UserStatusEnumType),
                typeof(PetSpeciesEnumType),
                typeof(ProductCategoryEnumType),
                typeof(TransactionTypeEnumType),
                typeof(ForumCategoryEnumType),
                typeof(NotificationTypeEnumType)
            );
        }
    }

    // Advanced GraphQL Query Root
    public class GameCoreQuery : ObjectGraphType
    {
        public GameCoreQuery()
        {
            Name = "Query";
            Description = "GameCore GraphQL API查詢根節點";

            // User queries
            FieldAsync<UserType, User>(
                "user",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<IntGraphType>> { Name = "id", Description = "用戶ID" }
                ),
                resolve: async context =>
                {
                    var userService = context.RequestServices.GetRequiredService<IUserService>();
                    var userId = context.GetArgument<int>("id");
                    return await userService.GetByIdAsync(userId);
                }
            );

            FieldAsync<ListGraphType<UserType>, IEnumerable<User>>(
                "users",
                arguments: new QueryArguments(
                    new QueryArgument<IntGraphType> { Name = "first", DefaultValue = 10, Description = "前N筆" },
                    new QueryArgument<IntGraphType> { Name = "skip", DefaultValue = 0, Description = "跳過筆數" },
                    new QueryArgument<StringGraphType> { Name = "search", Description = "搜尋關鍵字" }
                ),
                resolve: async context =>
                {
                    var userService = context.RequestServices.GetRequiredService<IUserService>();
                    var first = context.GetArgument<int>("first");
                    var skip = context.GetArgument<int>("skip");
                    var search = context.GetArgument<string>("search");
                    
                    return await userService.GetUsersAsync(first, skip, search);
                }
            );

            // Pet queries with advanced filtering
            FieldAsync<ListGraphType<PetType>, IEnumerable<Pet>>(
                "pets",
                arguments: new QueryArguments(
                    new QueryArgument<IntGraphType> { Name = "userId", Description = "用戶ID" },
                    new QueryArgument<PetSpeciesEnumType> { Name = "species", Description = "寵物種類" },
                    new QueryArgument<IntGraphType> { Name = "minLevel", Description = "最低等級" },
                    new QueryArgument<IntGraphType> { Name = "maxLevel", Description = "最高等級" },
                    new QueryArgument<BooleanGraphType> { Name = "isActive", Description = "是否活躍" }
                ),
                resolve: async context =>
                {
                    var petService = context.RequestServices.GetRequiredService<IPetService>();
                    var filter = new PetFilter
                    {
                        UserId = context.GetArgument<int?>("userId"),
                        Species = context.GetArgument<PetSpecies?>("species"),
                        MinLevel = context.GetArgument<int?>("minLevel"),
                        MaxLevel = context.GetArgument<int?>("maxLevel"),
                        IsActive = context.GetArgument<bool?>("isActive")
                    };
                    
                    return await petService.GetPetsAsync(filter);
                }
            );
        }
    }
} 