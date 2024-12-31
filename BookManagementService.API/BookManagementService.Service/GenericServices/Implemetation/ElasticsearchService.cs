using Microsoft.Extensions.Configuration;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using Elasticsearch.Net;
using Nest;
namespace BookManagementService.Service.GenericServices.Implemetation
{
    public class ElasticsearchService
    {
        private readonly IElasticClient _elasticClient;
        private readonly string _index;
        public ElasticsearchService(IConfiguration configuration)
        {
            _index = configuration["ELASTIC:INDEX"];

            var settings = new ConnectionSettings(new Uri(configuration["ELASTIC:URI"]))
                                .DefaultIndex(configuration["ELASTIC:INDEX"])
                .BasicAuthentication(configuration["ELASTIC:USERNAME"], configuration["ELASTIC:PASSWORD"]);

            _elasticClient = new ElasticClient(settings);
        }

        public async Task<IEnumerable<IDictionary<string, object>>> GetLogs(DateTime? startDate, DateTime? endDate, string? message, string? logLevel)
        {
            var queryContainer = new QueryContainer();

            // Filter by date range
            if (startDate.HasValue || endDate.HasValue)
            {
                queryContainer &= new DateRangeQuery
                {
                    Field = "@timestamp",
                    GreaterThanOrEqualTo = startDate,
                    LessThanOrEqualTo = endDate
                };
            }

            // Filter by message
            if (!string.IsNullOrWhiteSpace(message))
            {
                queryContainer &= new MatchQuery
                {
                    Field = "message",
                    Query = message
                };
            }

            // Filter by log level
            if (!string.IsNullOrWhiteSpace(logLevel))
            {
                queryContainer &= new TermQuery
                {
                    Field = "logLevel",
                    Value = logLevel
                };
            }

            var searchRequest = new SearchRequest(_index)
            {
                Query = queryContainer
            };

            var response = await _elasticClient.SearchAsync<IDictionary<string, object>>(searchRequest);

            if (!response.IsValid)
            {
                throw new Exception($"Failed to query Elasticsearch: {response.ServerError?.Error?.Reason}");
            }

            return response.Documents;
        }

        public async Task<byte[]> ExportLogsToTxt(DateTime? startDate, DateTime? endDate, string? message, string? logLevel)
        {
            // Fetch logs
            var logs = await GetLogs(startDate, endDate, message, logLevel);

            // Transform logs to strings
            var logStrings = logs.Select(log =>
            {
                var timestamp = log.ContainsKey("@timestamp") ? log["@timestamp"].ToString() : "N/A";
                var level = log.ContainsKey("logLevel") ? log["logLevel"].ToString() : "N/A";
                var msg = log.ContainsKey("message") ? log["message"].ToString() : "N/A";

                return $"{timestamp} | {level} | {msg}";
            });

            // Convert to byte array
            var logContent = string.Join(Environment.NewLine, logStrings);
            return System.Text.Encoding.UTF8.GetBytes(logContent);
        }



    }
}
