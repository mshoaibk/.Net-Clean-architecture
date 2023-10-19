using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM_Infrastructure.TableEntities
{
    [Table("TblOfficeLocation")]
    public class TblOfficeLocation
    {
        [Key]
        public long OfficeId { get; set; }
        public string OfficeLocationName { get; set; }
        public long CompanyId { get; set; }
        public bool? IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string Meter { get; set; }
    }
}
