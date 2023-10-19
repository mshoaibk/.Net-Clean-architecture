using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM_Infrastructure.TableEntities
{
    [Table("TblPrivateChats")]
    public class TblPrivateChats
    {
        [Key]
        public long PrivateChatId { get; set; }
        public string User1Id { get; set; }
        public string User2Id { get; set; }
        public string ChatType { get; set;} 
        public string user1Name { get; set;}
        public string user2Name { get; set;}

    }
}
