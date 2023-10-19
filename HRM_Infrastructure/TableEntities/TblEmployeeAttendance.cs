using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM_Infrastructure.TableEntities
{
    [Table("TblEmployeeAttendance")]
    public class TblEmployeeAttendance
    {
        [Key]
        public long EmployeeAttendanceID { get; set; }
        public long EmployeeID { get; set; }
        public long CompanyID { get; set; }
        public DateTime? AttendanceDate { get; set; }
        public int AttendanceYear { get; set; }
        public int AttendanceMonth { get; set; }
        public int AttendanceDateNum { get; set; }
        public string CheckedIn { get; set; }
        public string CheckedOut { get; set; }
        public bool? IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string TimeWorked { get; set; }
        public string AttendanceStatus { get; set; }
        public string ApprovalStatus { get; set; }
        public string Comment { get; set; }
        public bool? IsSalaryGenerated { get; set; }
    }
}
