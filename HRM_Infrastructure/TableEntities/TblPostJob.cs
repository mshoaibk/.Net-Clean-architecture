using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM_Infrastructure.TableEntities
{
    [Table("TblPostJob")]
    public class TblPostJob
    {
        [Key]
        public long PostJobId { get; set; }
        public string JobAddedBy { get; set; }
        public long? CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string JobTitle { get; set; }
        public string country { get; set; }
        public string JobLocation { get; set; }
        public string JobTypeLoc { get; set; }
        public string JobType { get; set; }
        public string JobSalaryRangeFrom { get; set; }
        public string JobSalaryRangeTo { get; set; }
        public string JobDescription { get; set; }
        public bool IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool? IsActivated { get; set; }
        public int NumberOfHiring { get; set; }
        public string Qualification { get; set; }
        public string NumberOfExperience { get; set; }
        public string ExperienceLevel { get; set; }
        public string KeyResponsibility { get; set; }
        public string RequiredSkills { get; set; }
    }
}
