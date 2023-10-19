using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM_Infrastructure.TableEntities {
    [Table("TblRole")]
    public class TblRole {
        [Key]
        public long RoleId { get; set; }
        public long? CompanyId { get; set; }
        public long? OfficeId { get; set; }
        public long? DepartmentId { get; set; }
        public long? TeamId { get; set; }
        public long? PositionId { get; set; }
        public bool IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
