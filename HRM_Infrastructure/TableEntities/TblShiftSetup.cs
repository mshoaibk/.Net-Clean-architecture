using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM_Infrastructure.TableEntities {
    [Table("TblShiftSetup")]
    public class TblShiftSetup {
        [Key]
        public long ShiftId { get; set; }
        public string ShiftName { get; set; }
        public string TimeFrom { get; set; }
        public string TimeTo { get; set; }
        public long CompanyId { get; set; }
        public bool? IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
