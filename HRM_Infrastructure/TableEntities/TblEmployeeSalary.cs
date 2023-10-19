using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM_Infrastructure.TableEntities
{
    [Table("TblEmployeeSalary")]
    public class TblEmployeeSalary
    {
        [Key]
        public long EmployeeSalaryID { get; set; }
        public long EmployeeID { get; set; }
        public long CompanyID { get; set; }
        public string PayType { get; set; }
        public decimal? MonthlyPay { get; set; }
        public decimal? HourlyPay { get; set; }
        public decimal? HoursWorked { get; set; }
        public decimal? DailyPay { get; set; }
        public decimal? WeeklyPay { get; set; }
        public decimal? WeeksWorked { get; set; }
        public bool? IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string Currency { get; set; }
        public decimal? AppliedTaxPercentage { get; set; }
    }
}
