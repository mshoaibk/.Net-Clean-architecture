using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM_Domain.Model
{
    public class SavePostJobRequest
    {
        public long PostJobId { get; set; }
        public string jobAddedBy { get; set; }
        public long? companyId { get; set; }
        public string companyName { get; set; }
        public string jobTitle { get; set; }
        public string country { get; set; }
        public string jobTypeLoc { get; set; }
        public string jobLocation { get; set; }
        public int noOfHiring { get; set; }
        public string qualification { get; set; }
        public string noOfExperience { get; set; }
        public string experienceLevel { get; set; }
        public string jobType { get; set; }
        public List<string> jobBenefit { get; set; }
        public string keyResponsibilities { get; set; }
        public List<string> requiredSkill { get; set; }
        public string jobSalaryRangeFrom { get; set; }
        public string jobSalaryRangeTo { get; set; }
        public string jobDescription { get; set; }
        public bool isDeleted { get; set; }
        public string createdBy { get; set; }
        public string action { get; set; }
    }
    public class SearchPostedJobRequest
    {
        public string companyName { get; set; }
        public string jobTitle { get; set; }
        public string jobLocation { get; set; }
        public int pageIndex { get; set; }
        public int pageSize { get; set; }

    }
    public class GetPostedJobModel
    {
        public List<GetPostedJobResponse> PostedJobList { get; set; }
        public Int32 TotalRecords { get; set; }
    }
    public class GetPostedJobResponse
    {
        public long PostJobId { get; set; }
        public string jobAddedBy { get; set; }
        public long? companyId { get; set; }
        public string company { get; set; }
        public string jobTitle { get; set; }
        public string jobLocation { get; set; }
        public string jobTypeLoc { get; set; }
        public string jobType { get; set; }
        public string salaryRangeFrom { get; set; }
        public string salaryRangeTo { get; set; }
        public List<GetPostedJobBenefitsResponse> jobBenefit { get; set; }
        public string description { get; set; }
        public string keyResponsibility { get; set; }
        public List<GetRequiredSkills> requiredSkills { get; set; }
        public string createdBy { get; set; }
        public string createdDate { get; set; }
        public string modifiedBy { get; set; }
        public string modifiedDate { get; set; }
        public bool? IsActivated { get; set; }
        public string experienceLevel { get; set; }
        public string noOfEmployees { get; set; }
        public DateTime? joinedDate { get; set; }
        public string companyLocation { get; set; }
    }
    public class GetRequiredSkills
    {
        public long postJobRequiredSkillsId { get; set; }
        public long? PostJobId { get; set; }
        public long? CompanayId { get; set; }
        public string requiredSkill { get; set; }
    }
    public class GetPostedJobBenefitsResponse
    {
        public long PostJobBenefitId { get; set; }
        public long? PostJobId { get; set; }
        public long? BenefitId { get; set; }
        public string BenefitTitle { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }

    public class SearchCarrerByComapnyRequest
    {
        public long companyId { get; set; }
        public string jobTitle { get; set; }
        public string jobLocation { get; set; }
        public int pageIndex { get; set; }
        public int pageSize { get; set; }

    }
}
