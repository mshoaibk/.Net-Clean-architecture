using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM_Infrastructure.TableEntities
{
    [Table("TblPosition")]
    public class TblPosition
    {
        [Key]
        public long PositionId { get; set; }
        public string PositionName { get; set; }
        public long CompanyId { get; set; }
        public bool? IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public long? DepartmentId { get; set; }
        public long? TeamId { get; set; }
    }
}
