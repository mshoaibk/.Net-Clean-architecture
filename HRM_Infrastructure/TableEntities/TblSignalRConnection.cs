using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM_Infrastructure.TableEntities
{
    [Table("TblSignalRConnection")]
    public class TblSignalRConnection
    {
        [Key]
        public long ID { get; set; }
        public string SignalRConnectionID { get; set; }
        public string UserName { get; set; }
        public string UserType { get; set; }
        public string UserID { get; set; }
        public string brwserInfo { get; set; }

    }
}
