using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM_Infrastructure.TableEntities
{
    [Table("TblPrivateMessages")]
    public class TblPrivateMessages
    {
        [Key]
        public long PrivateMessageId { get; set; }
        public long PrivateChatId { get; set; }
        public string SenderUserId { get; set; }
        public string ReceiverUserId { get; set; }
        public string MessageText { get; set; }
        public bool IsSeen { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? Timestamp { get; set; }

    }
}
