using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM_Infrastructure.TableEntities
{
    [Table("TblSubscriptionPackages")]
    public class TblSubscriptionPackages
    {
        [Key]
        public long PackageId { get; set; }
        public string PackageName { get; set; }
        public int? DurationInYears { get; set; }
        public int? DurationInMonths { get; set; }
        public int? DepartmentLimit{ get; set; }
        public int? OfficeLimit{ get; set; }
        public int? TeamLimit{ get; set; }
        public int? PositionLimit { get; set; }
        public int? EmployeeLimit { get; set; }
        public int? RoleLimit { get; set; }
        public int? LogsLimit { get; set; }
        public bool? IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public decimal? Price { get; set; }

    }
}
