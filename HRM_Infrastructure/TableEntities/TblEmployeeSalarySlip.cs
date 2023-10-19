using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM_Infrastructure.TableEntities
{
    [Table("TblEmployeeSalarySlip")]
    public class TblEmployeeSalarySlip
    {
        [Key]
        public long EmployeeSalarySlipID { get; set; }
        public long EmployeeID { get; set; }
        public long CompanyID { get; set; }
        public long EmployeeSalaryID { get; set; }
        public string Month { get; set; }
        public DateTime PaidOn { get; set; }
        public decimal? BasicPay { get; set; }
        public decimal? Allowances { get; set; }
        public decimal? Deductions { get; set; }
        public decimal? NetSalary { get; set; }
        public bool? IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int Year { get; set; }
        public string Currency { get; set; }
        public string RequestStatus { get; set; }
        public string DeductionReason { get; set; }
        public decimal? DeductionPerHrs { get; set; }
        public decimal? TaxDeduction { get; set; }
        public decimal? NetSalaryPerHrs { get; set; }
        public string PayType { get; set; }
        public bool? IsPaid { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}
