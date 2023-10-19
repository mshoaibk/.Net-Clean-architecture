using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM_Infrastructure.TableEntities
{
    [Table("TblTransaction")]
    public class TblTransaction
    {
        [Key]
        public long TransactionId { get; set; }

        public long UserId { get; set; }
        public string FromAcountName { get; set; }
        public string FromAcountNo{ get; set; }
        public string ToAcountName{ get; set; }
        public string TransactionType{ get; set; }
        public decimal? Amount { get; set; }
        public bool? IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string ToAcountNo { get; set; }
        public long CompanyId { get; set; }

    }
}
