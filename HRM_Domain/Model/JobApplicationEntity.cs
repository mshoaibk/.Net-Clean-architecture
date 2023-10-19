using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM_Domain.Model
{
    public class ApplyJobEntity
    {
        public long? postJobId { get; set; }
        public long? companyId { get; set; }
        public string fullName { get; set; }
        public string phoneNumber { get; set; }
        public string email { get; set; }
        public string resumeFile { get; set; }
    }

    public class ShowAppliedApplicationRequestModel
    {
        public long? companyId { get; set; }
        public string jobTitle { get; set; }
        public string location { get; set; }
        public string jobType { get; set; }
        public int pageIndex { get; set; }
        public int pageSize { get; set; }
    }

    public class ShowAppliedApplicationReturnModel
    {
        public Int32 TotalRecords { get; set; }
        public List<ShowAppliedApplicationList> ShowApplicationList { get; set; }
    }

    public class ShowAppliedApplicationList
    {
        public long postJobId { get; set; }
        public string jobTitle { get; set; }
        public string jobType { get; set; }
        public int NoOfApplication { get; set; }
        public int NoOfHiring { get; set; }
    }

    public class ShowCandidatesApplicationRequestModel
    {
        public long? postedJobId { get; set; }
        public int pageIndex { get; set; }
        public int pageSize { get; set; }
    }

    public class ShowCandidatesApplicationReturnModel
    {
        public Int32 totalRecords { get; set; }
        public string jobTitle { get; set; }
        public List<ShowCandidatesApplicationList> showCandidatesApplicationList { get; set; }
    }

    public class ShowCandidatesApplicationList
    {
        public long? candidateJobApplicationId { get; set; }
        public string candidateName { get; set; }
        public string candidateEmail { get; set; }
        public string candidatePhnNo { get; set; }
        public string status { get; set; }
        public string resume { get; set; }
    }

    public class UpdateCandidatesApplicationRequestModel
    {
        public long? candidateJobApplicationId { get; set; }
        public string statuses { get; set; }
    }

    public class GetPostedJobByIdResponse
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
        public string selectedCountry { get; set; }
        public int noOfHiring { get; set; }
        public string qualification { get; set; }
        public string noOfExperience { get; set; }
        public string keyResponsibilities { get; set; }
    }

    public class GetCountByCompanyIdResponse
    {
        public long? jobApplicationCount { get; set; }
    }
}
