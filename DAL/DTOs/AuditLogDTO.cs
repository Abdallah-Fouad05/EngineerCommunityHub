using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTOs
{
    public class AuditLogDTO
    {
        public long Id { get; set; }
        public string Action { get; set; }
        public int PerformedBy { get; set; }
        public int TargetId { get; set; }
        public string Details { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
