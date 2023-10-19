using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM_Infrastructure.TableEntities
{
    [Table("TblTicketComments")]
    public class TblTicketComments
    {
        [Key]
        public long CommentID { get; set; }
        public string Photopath { get; set; }
        public long TicketId { get; set; }
        public string CommentText { get; set; }
        public DateTime? CommentDate { get; set; }
        public long CompanyId { get; set; }
        public long UserId { get; set; }
       // public string ServiceType { get; set; }

    }
}
