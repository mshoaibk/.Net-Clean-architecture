using HRM_Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM_Application.Interfaces
{
    public interface ICompanySetupServices
    {
        Task<SaveResponseMessage> CreateOffice(List<OfficeLocationSaveRequest> model);
        Task<GetOfficeLocationDetailModel> GetOfficeLocationList(SearchOfficeLocationGetRequest model);
        Task<bool> DeleteOfficeSetup(DeleteOfficeRequest model);
        Task<SaveResponseMessage> CreateDepartment(List<DepartmentSaveRequest> model);
        Task<GetDepartmentDetailModel> GetDepartmentList(SearchDepartmentGetRequest model);
        Task<bool> DeleteDepartmentSetup(DeleteDepartmentRequest model);
        Task<SaveResponseMessage> CreateTeam(List<TeamSaveRequest> model);
        Task<GetTeamDetailModel> GetTeamList(SearchTeamGetRequest model);
        Task<bool> DeleteTeamSetup(DeleteTeamRequest model);
        Task<SaveResponseMessage> CreatePosition(List<PositionSaveRequest> model);
        Task<GetPositionDetailModel> GetPositionList(SearchPositionGetRequest model);
        Task<bool> DeletePositionSetup(DeletePositionRequest model);
        Task<GetSetupLookUpDataResponseModel> GetSetupLookUpData(GetSetupLookUpDataRequestModel model);
        Task<List<DepartmentDDLResponse>> GetDepartmentByOfficeLocation(long? companyId, long? officeId);
        Task<List<TeamDDLResponse>> GetTeamByDepartment(long? companyId, long? departmentId);
        Task<List<PositionDDLResponse>> GetPositionByTeam(long? companyId, long? teamId);
        Task<PositionHierarchyEmployeesResponse> GetPositionHierarchyEmployees(long? companyId, long? positionId);
        Task<SaveResponseMessage> CreateEmploymentTypeSetup(EmploymentTypeSetupRequest model);
        Task<GetEmploymentTypeModel> GetEmploymentTypeList(SearchEmpTypeGetRequest model);
        Task<bool> DeleteEmploymentTypeSetup(DeleteEmploymentTypeRequest model);
        Task<SaveResponseMessage> SaveLocation(OfficeAddressSaveRequest model);
        Task<List<GetLocationList>> GetLocation(GetLocationSearchModel model);
        Task<SaveResponseMessage> CreateShift(ShiftSaveRequest model);
        Task<GetShiftModel> GetShiftList(SearchShiftGetRequest model);
        Task<bool> DeleteShift(int shiftId);
        Task<SaveResponseMessage> SaveRole(SaveRoleReq model);
        Task<List<ModulePermissionList>> GetRoles(GetRoleReq model);
        Task<SaveRoleReq> GetRolesById(long roleId, long office, long department, long team, long position, long companyID);
        Task<bool> DeleteEmployeeSetupRoles(long roleId);
        Task<List<PositionHierarchyResponse>> GetPositionHierarchy(long companyID);
        Task<SaveResponseMessage> UpdatePositionHierarchy(List<PositionHierarchyRequest> obj);
    }
}
