
using Dapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using System;
using System.Text.Json;
using UserAuthManagementService.Data.Context;
using UserAuthManagementService.Data.Repository.Interface;
using UserAuthManagementService.Domain.DTO.Common;
using UserAuthManagementService.Data.context;
using UserAuthManagementService.Domain.Entity;

namespace UserAuthManagementService.Data.Repository.Implementation
{
    public class AuditRepository : IAuditRepository
    {

        private readonly ILogger<AuditRepository> _logger;
        private readonly MongoDbContext _context;

        public AuditRepository(ILogger<AuditRepository> logger, MongoDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<(string, Exception)> CreateAudit(Audit audit)
        {
            try
            {
                // Set the audit's creation date as the current UTC time
                audit.happenOn = DateTime.UtcNow;

                // Insert the audit document into MongoDB
                await _context.Audits.InsertOneAsync(audit);
                return (audit.auditId.ToString(),null);
            }
            catch (Exception ex)
            {
                return (ObjectId.Empty.ToString(), ex);
            }
        }
    }
}
