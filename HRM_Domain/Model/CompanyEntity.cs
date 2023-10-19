using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM_Domain.Model
{
    public class RegisterCompanyRequest
    {
        public long companyID { get; set; }
        public string companyName { get; set; }
        public string owner { get; set; }
        public string industry { get; set; }
        public string streetAddress { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string country { get; set; }
        public string zipCode { get; set; }
        public string emailAddress { get; set; }
        public string userName { get; set; }
        public string phoneNumber { get; set; }
        public string description { get; set; }
        public bool isDeleted { get; set; }
        public string createdBy { get; set; }
        public string action { get; set; }
        public IFormFile companyLogo { get; set; }
        public string companyLogoPath { get; set; }
        public string noOfEmployees { get; set; }
        public string companyAddress { get; set; }      
        public string website { get; set; }
        public string linkedIn { get; set; }
        public DateTime? registerDate { get; set; }
    }
    public class SearchCompanyDetailRequest
    {
        public string companyName { get; set; }
        public string industry { get; set; }
        public string city { get; set; }
        public string country { get; set; }
        public int pageIndex { get; set; }
        public int pageSize { get; set; }
    }
    public class GetCompanyDetailModel
    {
        public List<GetCompanyDetailResponse> CompanyList { get; set; }
        public Int32 TotalRecords { get; set; }
    }
    public class GetCompanyDetailResponse
    {
        public long companyID { get; set; }
        public string companyLogo { get; set; }
        public string companyName { get; set; }
        public string owner { get; set; }
        public string industry { get; set; }
        public string streetAddress { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string country { get; set; }
        public string zipCode { get; set; }
        public string emailAddress { get; set; }
        public string userID { get; set; }
        public string phoneNumber { get; set; }
        public string description { get; set; }
        public string createdBy { get; set; }
        public DateTime? createdDate { get; set; }
        public string modifiedBy { get; set; }
        public DateTime? modifiedDate { get; set; }
        public bool? IsActivated { get; set; }
        public long? employeeCount { get; set; }
    }
    public class GetCompanyResponse
    {
        public long companyID { get; set; }
        public string companyName { get; set; }
    }
    public class GetCompanyProfileResponse
    {
        public string userID { get; set; }
        public long companyID { get; set; }
        public string companyName { get; set; }
        public string companyLogo { get; set; }
        public string companyIndustry { get; set; }
        public string companyNumberOfEmployees { get; set; }
        public string companyEmail { get; set; }
        public string companyPhone { get; set; }
        public string description { get; set; }
        public string linkedIn { get; set; }
        public string website { get; set; }
        public string companyAddress { get; set; }
    }

    public class SaveCompanyAnnouncementRequestModel
    {
        public string eventTitle { get; set; }
        public DateTime eventDate { get; set; }
        public string eventTime { get; set; }
        public string description { get; set; }
        public long companyAnnouncementID { get; set; }
        public long companyId { get; set; }
        public string action { get; set; }
        public string createdBy { get; set; }
        public string modifiedBy { get; set; }
        public IFormFile announcementFeaturePhoto { get; set; }
        public string announcementFeaturePhotoName { get; set; }
    }

    public class SearchCompanyAnnouncement
    {
        public string evenTitle { get; set; }
        public long companyId { get; set; }
        public int pageIndex { get; set; }
        public int pageSize { get; set; }
    }

    public class GetCompanyAnnouncementModel
    {
        public List<GetCompanyAnnouncementResponse> CompanyAnnouncementResponse { get; set; }
        public Int32 TotalRecords { get; set; }
    }
    public class GetCompanyAnnouncementResponse
    {
        public string eventTitle { get; set; }
        public DateTime eventDate { get; set; }
        public string eventTime { get; set; }
        public string description { get; set; }
        public long companyAnnouncementID { get; set; }
        public long companyId { get; set; }
        public string action { get; set; }
        public string createdBy { get; set; }
        public string modifiedBy { get; set; }
        public string featurePhoto { get; set; }
    }
}
