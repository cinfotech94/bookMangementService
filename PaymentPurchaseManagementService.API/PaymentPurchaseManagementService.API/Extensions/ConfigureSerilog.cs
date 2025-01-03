
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Sinks.Elasticsearch;
using Serilog.Sinks.File;
using System;

namespace ThirdPartyCardAPIs.API.Extensions
{
    public static class ConfigureSerilog
    {
        public static WebApplicationBuilder AddSerilog(this WebApplicationBuilder applicationBuilder)
        {
            try
            {
                var elasticsearchUri = applicationBuilder.Configuration["ELASTIC:URI"];
                var elasticsearchUsername = applicationBuilder.Configuration["ELASTIC:USERNAME"];
                var elasticsearchPassword = applicationBuilder.Configuration["ELASTIC:PASSWORD"];
                var elasticsearchindex = applicationBuilder.Configuration["ELASTIC:INDEX"];

                Log.Logger = new LoggerConfiguration()
                    .WriteTo.Console()  // Console output (optional)
                    .WriteTo.File("logs/BookUserManagement4Payment-log-{Date}.txt", rollingInterval: RollingInterval.Day, restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information)  // File logging
                    .WriteTo.Elasticsearch(
                        new ElasticsearchSinkOptions(new Uri(elasticsearchUri))
                        {
                            AutoRegisterTemplate = true,
                            IndexFormat = elasticsearchindex,
                            ModifyConnectionSettings = conn => conn.BasicAuthentication(elasticsearchUsername, elasticsearchPassword)
                        })
                    .CreateLogger();

                return applicationBuilder;
            }
            catch (Exception ex)
            {
                return applicationBuilder;

            }
        }
    }
}
