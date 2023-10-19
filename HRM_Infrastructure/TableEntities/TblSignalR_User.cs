using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM_Infrastructure.TableEntities
{
    [Table("TblSignalR_User")]
    public class TblSignalR_User
    {
        [Key]
        public long SignalRUserID { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public bool? isOnline { get; set; }
        public DateTime? LastOnlineDate { get; set; }
        public long ActualUserID { get; set; }
        public string photoPath { get; set; }
    }
}
