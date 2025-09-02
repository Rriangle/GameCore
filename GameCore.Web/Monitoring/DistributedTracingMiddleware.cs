using System.Diagnostics;
using OpenTelemetry;
using OpenTelemetry.Trace;
using Microsoft.Extensions.Logging;

namespace GameCore.Web.Monitoring
{
    public class DistributedTracingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<DistributedTracingMiddleware> _logger;
        private readonly ActivitySource _activitySource;
        
        public DistributedTracingMiddleware(
            RequestDelegate next, 
            ILogger<DistributedTracingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
            _activitySource = new ActivitySource("GameCore.Web");
        }

        public async Task InvokeAsync(HttpContext context)
        {
            using var activity = _activitySource.StartActivity($"{context.Request.Method} {context.Request.Path}");
            
            try
            {
                // Add request metadata to trace
                activity?.SetTag("http.method", context.Request.Method);
                activity?.SetTag("http.url", context.Request.GetDisplayUrl());
                activity?.SetTag("http.route", context.Request.Path);
                activity?.SetTag("user.id", context.User?.Identity?.Name);
                activity?.SetTag("user.agent", context.Request.Headers.UserAgent.ToString());
                activity?.SetTag("correlation.id", context.TraceIdentifier);

                // Custom business metrics
                if (context.User?.Identity?.IsAuthenticated == true)
                {
                    activity?.SetTag("user.authenticated", "true");
                    activity?.SetTag("user.roles", string.Join(",", context.User.Claims
                        .Where(c => c.Type == "role")
                        .Select(c => c.Value)));
                }

                var stopwatch = Stopwatch.StartNew();
                
                await _next(context);
                
                stopwatch.Stop();

                // Add response metadata
                activity?.SetTag("http.status_code", context.Response.StatusCode);
                activity?.SetTag("http.response_time_ms", stopwatch.ElapsedMilliseconds);
                
                // Set activity status based on response
                if (context.Response.StatusCode >= 400)
                {
                    activity?.SetStatus(ActivityStatusCode.Error, $"HTTP {context.Response.StatusCode}");
                }
                
                // Log performance metrics
                if (stopwatch.ElapsedMilliseconds > 1000) // Slow request threshold
                {
                    _logger.LogWarning("Slow request detected: {Method} {Path} took {ElapsedMs}ms", 
                        context.Request.Method, 
                        context.Request.Path, 
                        stopwatch.ElapsedMilliseconds);
                        
                    activity?.SetTag("performance.slow_request", "true");
                }

                // Business-specific tracing
                await AddBusinessMetricsAsync(context, activity);
            }
            catch (Exception ex)
            {
                activity?.SetStatus(ActivityStatusCode.Error, ex.Message);
                activity?.SetTag("exception.type", ex.GetType().Name);
                activity?.SetTag("exception.message", ex.Message);
                
                _logger.LogError(ex, "Request failed: {Method} {Path}", 
                    context.Request.Method, context.Request.Path);
                    
                throw;
            }
        }

        private async Task AddBusinessMetricsAsync(HttpContext context, Activity? activity)
        {
            // Pet interaction tracking
            if (context.Request.Path.StartsWithSegments("/api/Pet"))
            {
                activity?.SetTag("business.module", "pet");
                
                if (context.Request.Method == "POST" && context.Request.Path.Value?.Contains("interact") == true)
                {
                    activity?.SetTag("business.action", "pet_interaction");
                }
            }
            
            // Wallet transaction tracking
            else if (context.Request.Path.StartsWithSegments("/api/Wallet"))
            {
                activity?.SetTag("business.module", "wallet");
                
                if (context.Request.Method == "POST" && context.Request.Path.Value?.Contains("transfer") == true)
                {
                    activity?.SetTag("business.action", "points_transfer");
                }
            }
            
            // Store purchase tracking
            else if (context.Request.Path.StartsWithSegments("/api/Store"))
            {
                activity?.SetTag("business.module", "store");
                
                if (context.Request.Method == "POST" && context.Request.Path.Value?.Contains("purchase") == true)
                {
                    activity?.SetTag("business.action", "product_purchase");
                }
            }
        }
    }
} 