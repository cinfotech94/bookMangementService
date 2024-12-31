﻿
using MongoDB.Bson;
using UserAuthManagementService.Domain.DTO.Common;
using UserAuthManagementService.Domain.Entity;

namespace UserAuthManagementService.Data.Repository.Interface
{
    public interface IAuditRepository
    {
        Task<(Guid, Exception)> CreateAudit(Audit Audit);
    }
}
