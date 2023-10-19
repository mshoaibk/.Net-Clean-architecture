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
    [Table("TblHierarchyEmployees")]
    public class TblHierarchyEmployees
    {
        [Key]
        public long HierarchyEmployeesId { get; set; }
        public string hierarchyStatus { get; set; }
        public long? CompanyId { get; set; }
        public long? EmployeeId { get; set; }
        public long? SelfEmployeeId { get; set; }
    }
}
