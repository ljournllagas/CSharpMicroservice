using System;
using System.Collections.Generic;
using System.Text;

namespace AuditLogService.Model
{
    public class AuditLog
    {
        public AuditLog(string eventType, string message)
        {
            Message = message;
            EventType = eventType;
        }

        public int Id { get; set; }
        public string EventType { get; set; }
        public string Message { get; set; }
        public string EventDateTime { get; set; } = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss.fffffff");

    }
}
