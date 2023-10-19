using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM_Infrastructure.TableEntities
{
    [Table("TblCompanyAnnouncement")]
    public class TblCompanyAnnouncement
    {
        [Key]
        public long CompanyAnnouncementID { get; set; }
        public long CompanyID { get; set; }
        public string EventTitle { get; set; }
        public DateTime EventDate { get; set; }
        public string EventTime { get; set; }
        public string EventDescription { get; set; }
        public bool IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string FeaturePhoto { get; set; }
    }
}
