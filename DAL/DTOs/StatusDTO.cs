using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTOs
{
    public class StatusDTO
    {
        public int StatusId {  get; set; }
        public string StatusName { get; set; }
        public StatusDTO(int statusId, string statusName)
        {
            StatusId = statusId;
            StatusName = statusName;
        }
    }
}
