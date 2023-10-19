using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM_Infrastructure.TableEntities {
    [Table("TblPositionHierarchy")]
    public class TblPositionHierarchy {
        [Key]
        public long PositionHierarchyId { get; set; }
        public long PositionId { get; set; }
        public long CompanyId { get; set; }
        public long? OrderNumber { get; set; }
    }
}
