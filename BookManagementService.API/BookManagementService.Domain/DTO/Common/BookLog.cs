using Microsoft.VisualBasic;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookManagementService.Domain.DTO.Common
{
    public class BookLog
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId LogId { get; set; }
        public string APIMethod {  get; set; }
        public string Description {  get; set; }
        public string? ExceptionDetails {  get; set; }
        public string? CorrelationId {  get; set; }
        public DateTime LogDate {  get; set; }
        public DateTime RequestDateTime {  get; set; }
        public string Level {  get; set; }
        public string? ip {  get; set; }
        public string? user {  get; set; }
        
    }
}
