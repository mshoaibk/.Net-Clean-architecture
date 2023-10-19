using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM_Infrastructure.TableEntities
{
    [Table("TblCompanyDetail")]
    public class TblCompanyDetail
    {
        [Key]
        public long CompanyID { get; set; }
        public string CompanyName { get; set; }
        public string Owner { get; set; }
        public string Industry { get; set; }
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string ZipCode { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public string Description { get; set; }
        public bool IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string UserID { get; set; }
        public bool? IsActivated { get; set; }
        public string CompanyLogo { get; set; }
        public string NoOfEmployees { get; set; }
        public string companyAddress { get; set; }
        public DateTime? JoinedTime { get; set; }
        public string Website { get; set; }
        public string LinkedIn { get; set; }
    }
}
