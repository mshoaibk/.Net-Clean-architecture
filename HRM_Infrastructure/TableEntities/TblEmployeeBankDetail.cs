using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM_Infrastructure.TableEntities
{
    [Table("TblEmployeeBankDetail")]
    public class TblEmployeeBankDetail
    {
        [Key]
        public long EmployeeBankDetailId { get; set; }
        public long EmployeeID { get; set; }
        public long CompanyID { get; set; }
        public string BankName { get; set; }
        public string BankBranch { get; set; }
        public string AccountHolderName { get; set; }
        public string AccountNumber { get; set; }
        public string IFSCCode { get; set; }
        public bool IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
