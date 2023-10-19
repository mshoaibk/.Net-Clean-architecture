using HRM_Application.Interfaces;
using HRM_Domain.Model;
using HRM_Infrastructure.HRMDataBaseContext;
using HRM_Infrastructure.TableEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM_Application.Services
{
    public class JobApplicationService : IJobApplicationService
    {
        private readonly HRMContexts dbContextHRM;

        public JobApplicationService(HRMContexts context)
        {
            dbContextHRM = context;
        }
        public async Task<bool> ApplyJob(ApplyJobEntity model)
        {
            int isAlreadyExist = dbContextHRM.tblCandidateJobApplications.Where(candidate => candidate.PostJobId == model.postJobId
              && candidate.CandidateEmail == model.email).Count();
            if (isAlreadyExist <= 0)
            {
                TblCandidateJobApplications tblCandidateJobApplicationsObj = new TblCandidateJobApplications();
                tblCandidateJobApplicationsObj.PostJobId = model.postJobId;
                tblCandidateJobApplicationsObj.CompanyId = model.companyId;
                tblCandidateJobApplicationsObj.FullName = model.fullName;
                tblCandidateJobApplicationsObj.PhoneNumber = model.phoneNumber;
                tblCandidateJobApplicationsObj.CandidateEmail = model.email;
                tblCandidateJobApplicationsObj.Resume = model.resumeFile;
                tblCandidateJobApplicationsObj.IsDeleted = false;
                tblCandidateJobApplicationsObj.CreatedBy = model.fullName;
                tblCandidateJobApplicationsObj.CreatedDate = DateTime.Now;
                tblCandidateJobApplicationsObj.ModifiedBy = "";
                tblCandidateJobApplicationsObj.ModifiedDate = DateTime.Now;
                tblCandidateJobApplicationsObj.IsActivated = true;
                tblCandidateJobApplicationsObj.statuses = "Recieved";
                dbContextHRM.tblCandidateJobApplications.Add(tblCandidateJobApplicationsObj);
                dbContextHRM.SaveChanges();
            }
            return true;
        }

        public async Task<ShowAppliedApplicationReturnModel> ShowAppliedApplication(ShowAppliedApplicationRequestModel model)
        {
            int skipCount = model.pageSize * model.pageIndex;
            int takeCount = model.pageSize;
            ShowAppliedApplicationReturnModel obj = new ShowAppliedApplicationReturnModel();
            IQueryable<ShowAppliedApplicationList> showApplications;
            showApplications = (from postJob in dbContextHRM.tblPostJob
                                where postJob.IsDeleted == false && postJob.CompanyId == model.companyId &&
                                (model.jobTitle == "" ||
                                !string.IsNullOrEmpty(model.jobTitle) &&
                                (postJob.JobTitle.Contains(model.jobTitle)))
                                &&
                                (model.jobType == "" ||
                                !string.IsNullOrEmpty(model.jobType) &&
                                (postJob.JobType.Contains(model.jobType)))
                                &&
                                (model.location == "" ||
                                !string.IsNullOrEmpty(model.location) &&
                                (postJob.country.Contains(model.location)))
                                select new ShowAppliedApplicationList
                                {
                                    postJobId = postJob.PostJobId,
                                    jobTitle = postJob.JobTitle,
                                    jobType = postJob.JobType,
                                    NoOfHiring = postJob.NumberOfHiring,
                                    NoOfApplication = dbContextHRM.tblCandidateJobApplications.
                                    Where(x => x.PostJobId == postJob.PostJobId && x.IsDeleted == false).Count()
                                });
            obj.TotalRecords = showApplications.Count();
            //Page size -1 is for all records
            if (model.pageSize == -1)
                obj.ShowApplicationList = showApplications.ToList();
            else
                obj.ShowApplicationList = showApplications.Skip(skipCount).Take(takeCount).ToList();

            return obj;
        }

        public async Task<ShowCandidatesApplicationReturnModel> ShowCandidatesApplication(ShowCandidatesApplicationRequestModel model)
        {
            int skipCount = model.pageSize * model.pageIndex;
            int takeCount = model.pageSize;
            ShowCandidatesApplicationReturnModel obj = new ShowCandidatesApplicationReturnModel();
            IQueryable<ShowCandidatesApplicationList> showApplications;
            showApplications = (from candidateJob in dbContextHRM.tblCandidateJobApplications
                                where candidateJob.IsDeleted == false && candidateJob.PostJobId == model.postedJobId
                                select new ShowCandidatesApplicationList
                                {
                                    candidateJobApplicationId = candidateJob.CandidateJobApplicationId,
                                    candidateName = candidateJob.FullName,
                                    candidateEmail = candidateJob.CandidateEmail,
                                    candidatePhnNo = candidateJob.PhoneNumber,
                                    status = candidateJob.statuses,
                                    resume = candidateJob.Resume
                                });
            obj.totalRecords = showApplications.Count();
            obj.jobTitle = dbContextHRM.tblPostJob.Where(x => x.PostJobId == model.postedJobId).Select(x => x.JobTitle).FirstOrDefault();
            //Page size -1 is for all records
            if (model.pageSize == -1)
                obj.showCandidatesApplicationList = showApplications.ToList();
            else
                obj.showCandidatesApplicationList = showApplications.Skip(skipCount).Take(takeCount).ToList();

            return obj;
        }

        public async Task<bool> UpdateCandidatesApplication(UpdateCandidatesApplicationRequestModel model)
        {
            var getCandidateApplication = dbContextHRM.tblCandidateJobApplications.Where(x => x.CandidateJobApplicationId == model.candidateJobApplicationId).FirstOrDefault();
            getCandidateApplication.statuses = model.statuses;
            dbContextHRM.SaveChanges();
            return true;
        }

        public async Task<GetPostedJobByIdResponse> GetPostedJobById(long postJobId)
        {
            GetPostedJobByIdResponse postedJobResult = new GetPostedJobByIdResponse();
            postedJobResult = (from postJob in dbContextHRM.tblPostJob
                                where postJob.IsDeleted == false && postJob.PostJobId==postJobId
                                select new GetPostedJobByIdResponse
                                {

                                    PostJobId = postJob.PostJobId,
                                    jobAddedBy = postJob.JobAddedBy,
                                    companyId = postJob.CompanyId,
                                    company = postJob.CompanyName,
                                    jobTitle = postJob.JobTitle,
                                    jobLocation = postJob.JobLocation,
                                    jobTypeLoc = postJob.JobTypeLoc,
                                    jobType = postJob.JobType,
                                    salaryRangeFrom = postJob.JobSalaryRangeFrom,
                                    salaryRangeTo = postJob.JobSalaryRangeTo,
                                    description = postJob.JobDescription,
                                    experienceLevel = postJob.ExperienceLevel,
                                    keyResponsibility = postJob.KeyResponsibility,
                                    createdBy = postJob.CreatedBy,
                                    createdDate = postJob.CreatedDate.ToString(),
                                    modifiedBy = postJob.ModifiedBy,
                                    IsActivated = postJob.IsActivated == null ? false : postJob.IsActivated,
                                    modifiedDate = postJob.ModifiedDate.ToString(),
                                    selectedCountry = postJob.country,
                                    noOfHiring = postJob.NumberOfHiring,
                                    noOfExperience = postJob.NumberOfExperience,
                                    qualification = postJob.Qualification,
                                    keyResponsibilities=postJob.KeyResponsibility,
                                    jobBenefit = (from benefits in dbContextHRM.tblPostJobBenefit
                                                  where benefits.IsDeleted == false &&
                                                  benefits.PostJobId == postJob.PostJobId
                                                  select new GetPostedJobBenefitsResponse
                                                  {
                                                      PostJobBenefitId = benefits.PostJobBenefitId,
                                                      PostJobId = benefits.PostJobId,
                                                      BenefitId = benefits.BenefitId,
                                                      BenefitTitle = benefits.BenefitTitle,
                                                      CreatedBy = benefits.CreatedBy,
                                                      CreatedDate = benefits.CreatedDate,
                                                      ModifiedBy = benefits.ModifiedBy,
                                                      ModifiedDate = benefits.ModifiedDate,
                                                  }).ToList(),
                                    requiredSkills = (from skills in dbContextHRM.tblPostJobRequiredSkills
                                                      where skills.IsDeleted == false &&
                                                      skills.PostJobId == postJob.PostJobId
                                                      select new GetRequiredSkills
                                                      {
                                                          postJobRequiredSkillsId = skills.PostJobRequiredSkillsId,
                                                          PostJobId = skills.PostJobId,
                                                          CompanayId = skills.CompanayId,
                                                          requiredSkill = skills.RequiredSkills
                                                      }).ToList()
                                }).FirstOrDefault();

            return postedJobResult;
        }       
    }
}
