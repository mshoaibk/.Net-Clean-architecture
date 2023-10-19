using HRM_Domain.Model;
using HRM_Infrastructure.HRMDataBaseContext;
using HRM_Infrastructure.TableEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM_Application.Interfaces
{
    public class PostJobServices : IPostJobServices
    {
        private readonly HRMContexts dbContextHRM;

        public PostJobServices(HRMContexts context)
        {
            dbContextHRM = context;
        }
        public async Task<bool> SavePostJob(SavePostJobRequest model)
        {
            TblPostJob tblPostJobObj = new TblPostJob();
            if (model.action == "update")
            {
                tblPostJobObj = dbContextHRM.tblPostJob.Where(post => post.PostJobId == model.PostJobId).FirstOrDefault();
                if (tblPostJobObj == null)
                {
                    tblPostJobObj = new TblPostJob();
                }
            }
            tblPostJobObj.JobAddedBy = model.jobAddedBy;
            tblPostJobObj.CompanyId = model.companyId;
            tblPostJobObj.CompanyName = model.companyName;
            tblPostJobObj.JobTitle = model.jobTitle;
            tblPostJobObj.JobLocation = model.jobLocation;
            tblPostJobObj.JobTypeLoc = model.jobTypeLoc;
            tblPostJobObj.JobType = model.jobType;
            tblPostJobObj.JobSalaryRangeFrom = model.jobSalaryRangeFrom;
            tblPostJobObj.JobSalaryRangeTo = model.jobSalaryRangeTo;
            tblPostJobObj.JobDescription = model.jobDescription;
            tblPostJobObj.IsDeleted = model.isDeleted;
            tblPostJobObj.CreatedBy = model.createdBy;
            tblPostJobObj.CreatedDate = DateTime.Now;
            tblPostJobObj.ModifiedBy = "";
            tblPostJobObj.ModifiedDate = DateTime.Now;
            tblPostJobObj.country = model.country;
            tblPostJobObj.NumberOfHiring = model.noOfHiring;
            tblPostJobObj.Qualification = model.qualification;
            tblPostJobObj.NumberOfExperience = model.noOfExperience;
            tblPostJobObj.ExperienceLevel = model.experienceLevel;
            tblPostJobObj.KeyResponsibility = model.keyResponsibilities;
            if (model.action == "save")
            {
                dbContextHRM.tblPostJob.Add(tblPostJobObj);
            }
            else
            {
                dbContextHRM.Update(tblPostJobObj);
            }
            if (model.action == "update")
            {
                IEnumerable<TblPostJobBenefit> removeBenefits = dbContextHRM.tblPostJobBenefit.Where(x => x.PostJobId == model.PostJobId).ToList();
                dbContextHRM.tblPostJobBenefit.RemoveRange(removeBenefits);
            }
            dbContextHRM.SaveChanges();
            foreach (var jobBenefitModel in model.jobBenefit)
            {
                TblPostJobBenefit tblPostJobBenefitObj = new TblPostJobBenefit();
                tblPostJobBenefitObj.PostJobId = tblPostJobObj.PostJobId;
                tblPostJobBenefitObj.CompanayId = model.companyId;
                tblPostJobBenefitObj.BenefitId = 0;
                tblPostJobBenefitObj.BenefitTitle = jobBenefitModel;
                tblPostJobBenefitObj.IsDeleted = model.isDeleted;
                tblPostJobBenefitObj.CreatedBy = model.createdBy;
                tblPostJobBenefitObj.CreatedDate = DateTime.Now;
                tblPostJobBenefitObj.ModifiedBy = "";
                tblPostJobBenefitObj.ModifiedDate = DateTime.Now;
                dbContextHRM.tblPostJobBenefit.Add(tblPostJobBenefitObj);
            }
            if (model.action == "update")
            {
                IEnumerable<TblPostJobRequiredSkills> removeSkills = dbContextHRM.tblPostJobRequiredSkills.Where(x => x.PostJobId == model.PostJobId).ToList();
                dbContextHRM.tblPostJobRequiredSkills.RemoveRange(removeSkills);
            }
            dbContextHRM.SaveChanges();
            if (model.requiredSkill != null)
            {
                foreach (var jobRequiredSkills in model.requiredSkill)
                {
                    TblPostJobRequiredSkills tblPostJobReqSkillObj = new TblPostJobRequiredSkills();
                    tblPostJobReqSkillObj.PostJobId = tblPostJobObj.PostJobId;
                    tblPostJobReqSkillObj.CompanayId = model.companyId;
                    tblPostJobReqSkillObj.RequiredSkills = jobRequiredSkills;
                    tblPostJobReqSkillObj.IsDeleted = model.isDeleted;
                    tblPostJobReqSkillObj.CreatedBy = model.createdBy;
                    tblPostJobReqSkillObj.CreatedDate = DateTime.Now;
                    tblPostJobReqSkillObj.ModifiedBy = "";
                    tblPostJobReqSkillObj.ModifiedDate = DateTime.Now;
                    dbContextHRM.tblPostJobRequiredSkills.Add(tblPostJobReqSkillObj);
                }
            }
            dbContextHRM.SaveChanges();
            return true;
        }

        public async Task<GetPostedJobModel> GetPostedJob(SearchPostedJobRequest model)
        {
            int skipCount = model.pageSize * model.pageIndex;
            int takeCount = model.pageSize;
            GetPostedJobModel obj = new GetPostedJobModel();
            IQueryable<GetPostedJobResponse> postedJobsResult;
            postedJobsResult = (from companyDetail in dbContextHRM.tblCompanyDetail
                                join postJob in dbContextHRM.tblPostJob 
                                on companyDetail.CompanyID equals postJob.CompanyId
                                where postJob.IsDeleted == false &&
                                (model.companyName == "" ||
                                !string.IsNullOrEmpty(model.companyName) &&
                                (postJob.CompanyName.Contains(model.companyName)))
                                &&
                                (model.jobTitle == "" ||
                                !string.IsNullOrEmpty(model.jobTitle) &&
                                (postJob.JobTitle.Contains(model.jobTitle)))
                                &&
                                (model.jobLocation == "" ||
                                !string.IsNullOrEmpty(model.jobLocation) &&
                                (postJob.JobLocation.Contains(model.jobLocation)))
                                select new GetPostedJobResponse
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
                                    experienceLevel=postJob.ExperienceLevel,
                                    keyResponsibility=postJob.KeyResponsibility,
                                    createdBy = postJob.CreatedBy,
                                    createdDate = postJob.CreatedDate.ToString(),
                                    modifiedBy = postJob.ModifiedBy,
                                    IsActivated= postJob.IsActivated == null ? false : postJob.IsActivated,
                                    modifiedDate = postJob.ModifiedDate.ToString(),
                                    joinedDate=companyDetail.JoinedTime,
                                    noOfEmployees=companyDetail.NoOfEmployees,
                                    companyLocation=companyDetail.companyAddress,
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
                                });
            obj.TotalRecords = postedJobsResult.Count();
            //Page size -1 is for all records
            if (model.pageSize == -1)
                obj.PostedJobList = postedJobsResult.ToList();
            else
                obj.PostedJobList = postedJobsResult.Skip(skipCount).Take(takeCount).ToList();

            return obj;
        }

        public async Task<bool> DeletePostedJob(long postJobId)
        {
            var postedJobObject = dbContextHRM.tblPostJob.Where(x => x.PostJobId == postJobId).FirstOrDefault();
            if (postedJobObject == null)
            {
                return false;
            }
            postedJobObject.IsDeleted = true;
            dbContextHRM.SaveChanges();
            var postedJobBenefitObject = dbContextHRM.tblPostJobBenefit.Where(x => x.PostJobId == postJobId).
                ToList();
            dbContextHRM.RemoveRange(postedJobBenefitObject);
            dbContextHRM.SaveChanges();
            return true;
        }

        public async Task<bool> OnChangeJobActivationStatus(long postJobId, bool isActivated)
        {
            var jobObject = dbContextHRM.tblPostJob.Where(x => x.PostJobId == postJobId).FirstOrDefault();
            if (jobObject == null)
            {
                return false;
            }
            jobObject.IsActivated = isActivated;
            dbContextHRM.SaveChanges();
            return true;
        }

        public async Task<GetPostedJobModel> GetCarrerByComapny(SearchCarrerByComapnyRequest model)
        {
            int skipCount = model.pageSize * model.pageIndex;
            int takeCount = model.pageSize;
            GetPostedJobModel obj = new GetPostedJobModel();
            IQueryable<GetPostedJobResponse> postedJobsResult;
            postedJobsResult = (from postJob in dbContextHRM.tblPostJob
                                where postJob.IsDeleted == false && postJob.CompanyId == model.companyId
                                &&
                                (model.jobTitle == "" ||
                                !string.IsNullOrEmpty(model.jobTitle) &&
                                (postJob.JobTitle.Contains(model.jobTitle)))
                                &&
                                (model.jobLocation == "" ||
                                !string.IsNullOrEmpty(model.jobLocation) &&
                                (postJob.JobLocation.Contains(model.jobLocation)))
                                select new GetPostedJobResponse
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
                                });
            obj.TotalRecords = postedJobsResult.Count();
            //Page size -1 is for all records
            if (model.pageSize == -1)
                obj.PostedJobList = postedJobsResult.ToList();
            else
                obj.PostedJobList = postedJobsResult.Skip(skipCount).Take(takeCount).ToList();

            return obj;
        }
    }
}
