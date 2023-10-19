using HRM_Common.EnumClasses;
using HRM_Domain.Model;
using HRM_Infrastructure.HRMDataBaseContext;
using HRM_Infrastructure.TableEntities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace HRM_Application.Interfaces
{
    public class CompanyRegistrationServices : ICompanyRegistrationServices
    {
        private readonly HRMContexts dbContextHRM;
        public CompanyRegistrationServices(HRMContexts context)
        {
            dbContextHRM = context;
        }
        public async Task<bool> RegisterCompanyDetail(RegisterCompanyRequest model)
        {
            TblCompanyDetail tblCompanyObj = new TblCompanyDetail();
            if (model.action == "update")
            {
                tblCompanyObj = dbContextHRM.tblCompanyDetail.Where(company => company.CompanyID == model.companyID).FirstOrDefault();
            }
            if (tblCompanyObj == null)
            {
                return false;
            }
            var userID = dbContextHRM.tblAspNetUsers.Where(x => x.Email == model.emailAddress).Select(x => x.Id).FirstOrDefault();
            tblCompanyObj.CompanyName = model.companyName;
            tblCompanyObj.Owner = model.owner;
            tblCompanyObj.Industry = model.industry;
            tblCompanyObj.StreetAddress = model.streetAddress;
            tblCompanyObj.City = model.city;
            tblCompanyObj.State = model.state;
            tblCompanyObj.Country = model.country;
            tblCompanyObj.ZipCode = model.zipCode;
            tblCompanyObj.EmailAddress = model.emailAddress;
            tblCompanyObj.PhoneNumber = model.phoneNumber;
            tblCompanyObj.Description = model.description;
            tblCompanyObj.UserID = userID;
            tblCompanyObj.IsDeleted = model.isDeleted;
            tblCompanyObj.CreatedBy = model.createdBy;
            tblCompanyObj.CreatedDate = DateTime.Now;
            tblCompanyObj.ModifiedBy = "";
            tblCompanyObj.ModifiedDate = DateTime.Now;
            tblCompanyObj.IsActivated = true;
            tblCompanyObj.CompanyLogo = model.companyLogoPath;
            tblCompanyObj.NoOfEmployees = model.noOfEmployees;
            tblCompanyObj.companyAddress = model.companyAddress;
            tblCompanyObj.Website = model.website;
            tblCompanyObj.LinkedIn = model.linkedIn;
            tblCompanyObj.JoinedTime = model.registerDate;
            if (model.action == "save")
            {
                dbContextHRM.tblCompanyDetail.Add(tblCompanyObj);
                TblSignalR_User signalR_User = new TblSignalR_User();
                signalR_User.photoPath = tblCompanyObj.CompanyLogo;
                signalR_User.Name = tblCompanyObj.CompanyName;
                signalR_User.ActualUserID = tblCompanyObj.CompanyID;
                signalR_User.Type = ((ChatType)Convert.ToInt64(1)).ToString();
                dbContextHRM.TblSignalR_User.Add(signalR_User); 
            }

            else
            {
                dbContextHRM.Update(tblCompanyObj);
            }
            dbContextHRM.SaveChanges();
            return true;
        }
        public async Task<GetCompanyDetailModel> GetCompanyDetail(SearchCompanyDetailRequest model)
        {
            int skipCount = model.pageSize * model.pageIndex;
            int takeCount = model.pageSize;
            GetCompanyDetailModel obj = new GetCompanyDetailModel();
            IQueryable<GetCompanyDetailResponse> companyDetailResult;
            companyDetailResult = (from companyObj in dbContextHRM.tblCompanyDetail
                                   where companyObj.IsDeleted == false &&
                                   (model.companyName == "" ||
                                   !string.IsNullOrEmpty(model.companyName) &&
                                   (companyObj.CompanyName.Contains(model.companyName)))
                                   &&
                                   (model.industry == "" ||
                                   !string.IsNullOrEmpty(model.industry) &&
                                   (companyObj.Industry.Contains(model.industry)))
                                   &&
                                   (model.country == "" ||
                                   !string.IsNullOrEmpty(model.country) &&
                                   (companyObj.Country.Contains(model.country)))
                                   &&
                                   (model.city == "" ||
                                   !string.IsNullOrEmpty(model.city) &&
                                   (companyObj.City.Contains(model.city)))
                                   select new GetCompanyDetailResponse
                                   {
                                       companyID = companyObj.CompanyID,
                                       companyLogo = companyObj.CompanyLogo,
                                       companyName = companyObj.CompanyName,
                                       owner = companyObj.Owner,
                                       industry = companyObj.Industry,
                                       streetAddress = companyObj.StreetAddress,
                                       city = companyObj.City,
                                       state = companyObj.State,
                                       country = companyObj.Country,
                                       zipCode = companyObj.ZipCode,
                                       emailAddress = companyObj.EmailAddress,
                                       phoneNumber = companyObj.PhoneNumber,
                                       userID = companyObj.UserID,
                                       description = companyObj.Description,
                                       createdBy = companyObj.CreatedBy,
                                       createdDate = companyObj.CreatedDate,
                                       modifiedBy = companyObj.ModifiedBy,
                                       modifiedDate = companyObj.ModifiedDate,
                                       IsActivated = companyObj.IsActivated == null ? false : companyObj.IsActivated,
                                       employeeCount = (dbContextHRM.tblEmployee.
                                       Where(x => x.CompanyID == companyObj.CompanyID &&
                                       x.IsActivated == true && x.IsDeleted == false).Count()
                                       )
                                   });
            obj.TotalRecords = companyDetailResult.Count();
            //Page size -1 is for all records
            if (model.pageSize == -1)
                obj.CompanyList = companyDetailResult.ToList();
            else
                obj.CompanyList = companyDetailResult.Skip(skipCount).Take(takeCount).ToList();

            return obj;
        }

        public async Task<bool> DeleteCompanyDetail(long companyID)
        {
            var companyObject = dbContextHRM.tblCompanyDetail.Where(x => x.CompanyID == companyID).FirstOrDefault();
            if (companyObject == null)
            {
                return false;
            }
            companyObject.IsDeleted = true;
            dbContextHRM.SaveChanges();
            return true;
        }

        public async Task<bool> OnChangeCompanyStatus(long companyID, bool isActivated)
        {
            var companyObject = dbContextHRM.tblCompanyDetail.Where(x => x.CompanyID == companyID).FirstOrDefault();
            if (companyObject == null)
            {
                return false;
            }
            companyObject.IsActivated = isActivated;
            dbContextHRM.SaveChanges();
            return true;
        }

        public async Task<List<GetCompanyResponse>> GetCompaniesName()
        {
            List<GetCompanyResponse> companyList = dbContextHRM.tblCompanyDetail.
                Select(x => new GetCompanyResponse
                {
                    companyName = x.CompanyName,
                    companyID = x.CompanyID
                }).ToList();
            if (companyList == null || companyList.Count == 0)
            {
                return new List<GetCompanyResponse>();
            }
            return companyList;
        }

        public async Task<GetCompanyProfileResponse> GetCompanyProfile(string companyId)
        {
            GetCompanyProfileResponse companyList = new GetCompanyProfileResponse();
            companyList = dbContextHRM.tblCompanyDetail
                .Where(x => x.UserID == companyId)
                .Select(x => new GetCompanyProfileResponse
                {
                    userID = x.UserID,
                    companyID = x.CompanyID,
                    companyName = x.CompanyName,
                    companyLogo = x.CompanyLogo,
                    companyIndustry = x.Industry,
                    companyNumberOfEmployees = x.NoOfEmployees,
                    companyEmail = x.EmailAddress,
                    companyPhone = x.PhoneNumber,
                    description = x.Description,
                    linkedIn = x.LinkedIn,
                    website = x.Website,
                    companyAddress = x.companyAddress
                }).FirstOrDefault();
            if (companyList == null)
            {
                companyList = new GetCompanyProfileResponse();
                return companyList;
            }
            return companyList;
        }

        public async Task<bool> SaveCompanyAnnouncement(SaveCompanyAnnouncementRequestModel model)
        {
            TblCompanyAnnouncement tblCompanyAnnouncementObj = new TblCompanyAnnouncement();
            if (model.action == "update")
            {
                tblCompanyAnnouncementObj = dbContextHRM.tblCompanyAnnouncement.Where(company => company.CompanyAnnouncementID == model.companyAnnouncementID).FirstOrDefault();
            }
            if (tblCompanyAnnouncementObj == null)
            {
                return false;
            }
            tblCompanyAnnouncementObj.CompanyID = model.companyId;
            tblCompanyAnnouncementObj.EventTitle = model.eventTitle;
            tblCompanyAnnouncementObj.EventDate = model.eventDate;
            tblCompanyAnnouncementObj.EventTime = model.eventTime;
            tblCompanyAnnouncementObj.EventDescription = model.description;
            tblCompanyAnnouncementObj.FeaturePhoto=model.announcementFeaturePhotoName;
            tblCompanyAnnouncementObj.IsDeleted = false;
            if (model.action == "update")
            {
                tblCompanyAnnouncementObj.ModifiedBy = model.modifiedBy;
                tblCompanyAnnouncementObj.ModifiedDate = DateTime.Now;
            }
            if (model.action == "save")
            {
                tblCompanyAnnouncementObj.CreatedBy = model.createdBy;
                tblCompanyAnnouncementObj.CreatedDate = DateTime.Now;
                dbContextHRM.tblCompanyAnnouncement.Add(tblCompanyAnnouncementObj);
            }
            else
            {
                dbContextHRM.Update(tblCompanyAnnouncementObj);
            }
            dbContextHRM.SaveChanges();
            return true;
        }
        public async Task<GetCompanyAnnouncementModel> GetCompanyAnnouncementList(SearchCompanyAnnouncement model)
        {
            int skipCount = model.pageSize * model.pageIndex;
            int takeCount = model.pageSize;
            GetCompanyAnnouncementModel obj = new GetCompanyAnnouncementModel();
            IQueryable<GetCompanyAnnouncementResponse> companyAnnouncementResponse;
            companyAnnouncementResponse = (from companyAnnouncementObj in dbContextHRM.tblCompanyAnnouncement
                                           where companyAnnouncementObj.IsDeleted == false &&
                                           companyAnnouncementObj.CompanyID == model.companyId &&
                                           (model.evenTitle == "" ||
                                           !string.IsNullOrEmpty(model.evenTitle) &&
                                           (companyAnnouncementObj.EventTitle.Contains(model.evenTitle)))
                                           orderby companyAnnouncementObj.CompanyAnnouncementID
                                           select new GetCompanyAnnouncementResponse
                                           {
                                               companyAnnouncementID = companyAnnouncementObj.CompanyAnnouncementID,
                                               companyId = companyAnnouncementObj.CompanyID,
                                               eventTitle = companyAnnouncementObj.EventTitle,
                                               eventDate = companyAnnouncementObj.EventDate,
                                               eventTime = companyAnnouncementObj.EventTime,
                                               description = companyAnnouncementObj.EventDescription,
                                               createdBy = companyAnnouncementObj.CreatedBy,
                                               modifiedBy = companyAnnouncementObj.ModifiedBy,
                                               featurePhoto= companyAnnouncementObj.FeaturePhoto,
                                               action = "update"
                                           });
            obj.TotalRecords = companyAnnouncementResponse.Count();
            //Page size -1 is for all records
            if (model.pageSize == -1)
                obj.CompanyAnnouncementResponse = companyAnnouncementResponse.ToList();
            else
                obj.CompanyAnnouncementResponse = companyAnnouncementResponse.Skip(skipCount).Take(takeCount).ToList();

            return obj;
        }

        public async Task<bool> DeleteCompanyAnnouncement(long? companyAnnouncementID) {
            var companyAnnounceObject = dbContextHRM.tblCompanyAnnouncement.Where(x => x.CompanyAnnouncementID == companyAnnouncementID).FirstOrDefault();
            if (companyAnnounceObject == null) {
                return false;
            }
            companyAnnounceObject.IsDeleted = true;
            dbContextHRM.SaveChanges();
            return true;
        }
    }
}
