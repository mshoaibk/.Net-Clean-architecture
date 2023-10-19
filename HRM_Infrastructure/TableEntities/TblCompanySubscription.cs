using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM_Infrastructure.TableEntities
{
    [Table("TblCompanySubscription")]
    public class TblCompanySubscription
    {
        [Key]
        public long SubscriptionId { get; set; }
        public long CompanyId { get; set; }
        public long PackageId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public bool? IsPaid { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public decimal? PiadAmount { get; set; }
        public long? TransectionId { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public string PaymentStatus { get; set; }
        public string SubscriptionStatus { get; set; }
        public bool IsDeleted { get; set; }


    }
}
