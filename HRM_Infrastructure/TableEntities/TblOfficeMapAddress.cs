using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM_Infrastructure.TableEntities
{
    [Table("TblOfficeMapAddress")]
    public class TblOfficeMapAddress
    {
        [Key]
        public long OfficeMapAddressId { get; set; }
        public long? OfficeId { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public long? CompanyId { get; set; }
        public bool? IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
