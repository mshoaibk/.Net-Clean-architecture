using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM_Infrastructure.TableEntities
{
    [Table("TblEmployees")]
    public class TblEmployees
    {
        [Key]
        public long EmployeeID { get; set; }
        public long CompanyID { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
        public string EmploymentType { get; set; }
        public string Position { get; set; }
        public string Team { get; set; }
        public string Department { get; set; }
        public string Office { get; set; }
        public string Supervisor { get; set; }
        public DateTime? HireDate { get; set; }
        public string ProbationPeriod { get; set; }
        public DateTime? ContractEnd { get; set; }
        public string WorkType { get; set; }
        public string WorkingSchedule { get; set; }
        public string PaidVacation { get; set; }
        public bool IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string UserID { get; set; }
        public bool? IsActivated { get; set; }
        public string EmployeePhoto { get; set; }
        public string PhotoType { get; set; }
        public string PhoneNumber { get; set; }
        public string Experience { get; set; }
        public string Qualification { get; set; }
        public int? NumberOfLeavesAllowed { get; set; }
        public long? ShiftId { get; set; }
    }
}
