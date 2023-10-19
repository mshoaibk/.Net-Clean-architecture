using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM_Infrastructure.TableEntities
{
    [Table("TblCandidateJobApplications")]
    public class TblCandidateJobApplications
    {
        [Key]
        public long CandidateJobApplicationId { get; set; }
        public long? PostJobId { get; set; }
        public long? CompanyId { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string CandidateEmail { get; set; }
        public string Resume { get; set; }
        public bool? IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool? IsActivated { get; set; }
        public string? statuses { get; set; }
    }
}
