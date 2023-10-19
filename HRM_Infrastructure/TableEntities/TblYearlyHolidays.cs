using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM_Infrastructure.TableEntities
{
    [Table("TblYearlyHolidays")]
    public class TblYearlyHolidays
    {
        [Key]
        public long YearlyHolidaysId { get; set; }
        public string YearlyHolidaysName { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set;}
        public int NumberOfDays { get; set; }
        public long? CompanyId { get; set; }
        public bool? IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
