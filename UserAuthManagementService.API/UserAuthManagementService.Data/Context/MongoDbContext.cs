﻿using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using UserAuthManagementService.Domain.DTO.Common;
using UserAuthManagementService.Domain.Entity;
using UserAuthManagementService.Domain.DTO.Request;

namespace UserAuthManagementService.Data.Context;
public class MongoDbContext
{
    private readonly IConfiguration _configuration;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly bool _isAudit;

    public IMongoCollection<InboundLog> InboundLogs { get; private set; }
    public IMongoCollection<Audit> Audits { get; private set; }

    public MongoDbContext(IMongoClient mongoClient, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
    {
        _configuration = configuration;
        _httpContextAccessor = httpContextAccessor;
        // Retrieve the current request URL path
        var requestPath = _httpContextAccessor.HttpContext.Request.Path.Value;

        // Logic to select the database collection based on the request path
        var database = mongoClient.GetDatabase(_configuration["MongoDbSettings:DatabaseName"]);
        // Determine the collection name based on the URL
        string collectionName = "BookManagementService4User";

        // Assign the collection based on the dynamically determined name
        InboundLogs = database.GetCollection<InboundLog>(collectionName+"inboundLogs");
        Audits = database.GetCollection<Audit>(collectionName + "Audit");
    }
}
