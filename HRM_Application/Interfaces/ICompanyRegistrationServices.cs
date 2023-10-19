using HRM_Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM_Application.Interfaces
{
    public interface ICompanyRegistrationServices
    {
        Task<bool> RegisterCompanyDetail(RegisterCompanyRequest model);
        Task<GetCompanyDetailModel> GetCompanyDetail(SearchCompanyDetailRequest model);
        Task<bool> DeleteCompanyDetail(long companyID);
        Task<bool> OnChangeCompanyStatus(long companyID, bool isActivated);
        Task<List<GetCompanyResponse>> GetCompaniesName();
        Task<GetCompanyProfileResponse> GetCompanyProfile(string companyId);
        Task<bool> SaveCompanyAnnouncement(SaveCompanyAnnouncementRequestModel model);
        Task<GetCompanyAnnouncementModel> GetCompanyAnnouncementList(SearchCompanyAnnouncement model);
        Task<bool> DeleteCompanyAnnouncement(long? companyAnnouncementID);
    }
}
