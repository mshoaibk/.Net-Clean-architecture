using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM_Infrastructure.TableEntities
{
    [Table("TblPostJobRequiredSkills")]
    public class TblPostJobRequiredSkills
    {
        [Key]
        public long PostJobRequiredSkillsId { get; set; }
        public long? PostJobId { get; set; }
        public long? CompanayId { get; set; }
        public string RequiredSkills { get; set; }
        public bool IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}

