using Google.Events.Protobuf.Cloud.Dataflow.V1Beta3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserAuthManagementService.Domain.DTO.Common;
public class APILog
{
    //package RequestResponse;
    public string SystemCalledName { get; set; }  // The system that initiated or received the API call
    public string MethodName { get; set; }  // The system that initiated or received the API call
    public string APICalled { get; set; }         // The name or endpoint of the API being called
    public string APIMethod { get; set; }         // HTTP method used (GET, POST, etc.)
    public DateTime LogDate { get; set; }         // Date and time when the log entry was created
    public string RequestDetails { get; set; }    // Details of the request being made
    public DateTime RequestDateTime { get; set; } // Timestamp of the request
    public string ResponseDetails { get; set; }   // Details of the response received
    public DateTime ResponseDateTime { get; set; } // Timestamp of the response
    public string ExceptionDetails { get; set; }  // Details of any exceptions that occurred
}
