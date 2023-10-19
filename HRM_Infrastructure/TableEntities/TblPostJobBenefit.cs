using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM_Infrastructure.TableEntities
{
    [Table("TblPostJobBenefit")]
    public class TblPostJobBenefit
    {
        [Key]
        public long PostJobBenefitId { get; set; }
        public long? PostJobId { get; set; }
        public long? CompanayId { get; set; }
        public long? BenefitId { get; set; }
        public string BenefitTitle { get; set; }
        public bool IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
