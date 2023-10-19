using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM_Infrastructure.TableEntities
{
    [Table("TblEmployeeLeaveRequest")]
    public class TblEmployeeLeaveRequest
    {
        [Key]
        public long EmployeeLeaveRequestID { get; set; }
        public long EmployeeID { get; set; }
        public long CompanyID { get; set; }
        public DateTime? LeaveStartDate { get; set; }
        public int LeaveStartYear { get; set; }
        public int LeaveStartMonth { get; set; }
        public int LeaveStartDateNum { get; set; }
        public DateTime? LeaveEndDate { get; set; }
        public int LeaveEndYear { get; set; }
        public int LeaveEndMonth { get; set; }
        public int LeaveEndDateNum { get; set; }
        public string LeaveDuration { get; set; }
        public string LeaveType { get; set; }
        public string LeaveStatus { get; set; }
        public string LeaveReason { get; set; }
        public string LeaveTimeFrom { get; set; }
        public string LeaveTimeTo { get; set; }
        public bool? IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
