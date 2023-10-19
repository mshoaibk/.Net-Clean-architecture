using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM_Domain.Model
{
    #region Office Model Classes
    public class OfficeLocationSaveRequest
    {
        public long officeId { get; set; }
        public string officeLocationName { get; set; }
        public string latitude { get; set; }
        public string longitude { get; set; }
        public long companyId { get; set; }
        public string meter { get; set; }
    }

    public class OfficeLocationListResponse
    {
        public long officeId { get; set; }
        public string officeLocationName { get; set; }
        public long companyId { get; set; }
        public string createdBy { get; set; }
        public DateTime? createdDate { get; set; }
        public string modifiedBy { get; set; }
        public DateTime? modifiedDate { get; set; }
        public bool editableMode { get; set; }
        public string latitude { get; set; }
        public string longitude { get; set; }
    }

    public class SearchOfficeLocationGetRequest
    {
        public string searchOfficeLocationName { get; set; }
        public int pageIndex { get; set; }
        public int pageSize { get; set; }
        public long companyId { get; set; }
    }

    public class GetOfficeLocationDetailModel
    {
        public List<OfficeLocationListResponse> officeList { get; set; }
        public Int32 totalRecords { get; set; }
    }

    public class DeleteOfficeRequest
    {
        public long? id { get; set; }
    }
    #endregion

    #region Department Model Classes
    public class DepartmentSaveRequest
    {
        public long departmentId { get; set; }
        public string departmentName { get; set; }
        public long companyId { get; set; }
        public long officeLocationId { get; set; }
    }
    public class SearchDepartmentGetRequest
    {
        public string searchDepartmentName { get; set; }
        public int pageIndex { get; set; }
        public int pageSize { get; set; }
        public long companyId { get; set; }
    }
    public class GetDepartmentDetailModel
    {
        public List<DepartmentListResponse> departmentList { get; set; }
        public Int32 totalRecords { get; set; }
    }
    public class DepartmentListResponse
    {
        public long departmentId { get; set; }
        public string departmentName { get; set; }
        public string officeLocationName { get; set; }
        public long? companyId { get; set; }
        public string createdBy { get; set; }
        public DateTime? createdDate { get; set; }
        public string modifiedBy { get; set; }
        public DateTime? modifiedDate { get; set; }
        public bool editableMode { get; set; }
    }

    public class DeleteDepartmentRequest
    {
        public long? id { get; set; }
    }
    #endregion

    #region Team Model Classes
    public class TeamSaveRequest
    {
        public long teamId { get; set; }
        public string teamName { get; set; }
        public long companyId { get; set; }
        public long departmentId { get; set; }
        public long officeId { get; set; }
    }
    public class SearchTeamGetRequest
    {
        public string searchTeamName { get; set; }
        public int pageIndex { get; set; }
        public int pageSize { get; set; }
        public long companyId { get; set; }
    }
    public class GetTeamDetailModel
    {
        public List<TeamListResponse> teamList { get; set; }
        public Int32 totalRecords { get; set; }
    }
    public class TeamListResponse
    {
        public long teamId { get; set; }
        public string teamName { get; set; }
        public string departmentName { get; set; }
        public string officeName { get; set; }
        public long? companyId { get; set; }
        public string createdBy { get; set; }
        public DateTime? createdDate { get; set; }
        public string modifiedBy { get; set; }
        public DateTime? modifiedDate { get; set; }
        public bool editableMode { get; set; }
    }

    public class DeleteTeamRequest
    {
        public long? id { get; set; }
    }
    #endregion

    #region Position Model Classes
    public class PositionSaveRequest
    {
        public long positionId { get; set; }
        public string positionName { get; set; }
        public long departmentId { get; set; }
        public long teamId { get; set; }
        public long companyId { get; set; }
    }
    public class SearchPositionGetRequest
    {
        public string searchPositionName { get; set; }
        public int pageIndex { get; set; }
        public int pageSize { get; set; }
        public long companyId { get; set; }
    }
    public class GetPositionDetailModel
    {
        public List<PositionListResponse> positionList { get; set; }
        public Int32 totalRecords { get; set; }
    }
    public class PositionListResponse
    {
        public long positionId { get; set; }
        public string positionName { get; set; }
        public long companyId { get; set; }
        public string createdBy { get; set; }
        public DateTime? createdDate { get; set; }
        public string modifiedBy { get; set; }
        public DateTime? modifiedDate { get; set; }
        public string departmentName { get; set; }
        public string teamName { get; set; }
        public bool editableMode { get; set; }
    }

    public class DeletePositionRequest
    {
        public long? id { get; set; }
    }
    #endregion

    public class GetSetupLookUpDataRequestModel {
        public long companyId { get; set; }
        public string[] requiredDataList { get; set; }
    }
    public class GetSetupLookUpDataResponseModel {
        public List<OfficeLocationDDLResponse> officeLocationDDL { get; set; }
        public List<DepartmentDDLResponse> departmentDDL { get; set; }
        public List<TeamDDLResponse> teamDDL { get; set; }
        public List<PositionDDLResponse> positionDDL { get; set; }
        public List<EmploymentTypeDDLResponse> employmentTypeDDLResponse { get; set; }
        public List<GetShiftListDDLResponse> getShiftListDDLResponse { get; set; }
    }
    public class OfficeLocationDDLResponse {
        public long officeId { get; set; }
        public string officeLocationName { get; set; }
    }
    public class DepartmentDDLResponse {
        public long departmentId { get; set; }
        public string departmentName { get; set; }
    }
    public class TeamDDLResponse {
        public long teamId { get; set; }
        public string teamName { get; set; }
    }
    public class PositionDDLResponse {
        public long positionId { get; set; }
        public string positionName { get; set; }
    }
    public class PositionHierarchyEmployeesResponse
    {
        public List<SupervisorDDLResponse> supervisorDDLResponse { get; set; }
        public List<TeamMemberDDLResponse> teamMemberDDLResponse { get; set; }
    }
    public class SupervisorDDLResponse
    {
        public long supervisorId { get; set; }
        public string supervisorName { get; set; }
        public string email { get; set; }
        public bool selected { get; set; }
    }
    public class TeamMemberDDLResponse
    {
        public long teamMemberId { get; set; }
        public string teamMemberName { get; set; }
        public string email { get; set; }
        public bool selected { get; set; }
    }
    public class SaveResponseMessage
    {
        public bool status { get; set; }
        public string msg { get; set; }
    }

    public class EmploymentTypeSetupRequest
    {
        public long? employmentTypeId { get; set; }
        public List<string> employmentType { get; set; }
        public long companyId { get; set; }
    }

    public class SearchEmpTypeGetRequest
    {
        public string searchEmploymentType { get; set; }
        public int pageIndex { get; set; }
        public int pageSize { get; set; }
        public long companyId { get; set; }
    }
    public class GetEmploymentTypeModel
    {
        public List<EmploymentTypeListResponse> employmentTypeList { get; set; }
        public Int32 totalRecords { get; set; }
    }
    public class EmploymentTypeListResponse
    {
        public long EmploymentTypeId { get; set; }
        public string EmploymentType { get; set; }
        public long companyId { get; set; }
        public string createdBy { get; set; }
        public DateTime? createdDate { get; set; }
        public string modifiedBy { get; set; }
        public DateTime? modifiedDate { get; set; }
        public bool editableMode { get; set; }
    }

    public class DeleteEmploymentTypeRequest
    {
        public long? id { get; set; }
    }

    public class EmploymentTypeDDLResponse {
        public long employmentTypeId { get; set; }
        public string employmentTypeName { get; set; }
    }
    public class GetShiftListDDLResponse
    {
        public long shiftId { get; set; }
        public string shiftName { get; set; }
    }
        public class OfficeAddressSaveRequest
    {
        public string latitude { get; set; }
        public string longitude { get; set; }
        public long? companyId { get; set; }
        public long? officeId { get; set; }
    }

    public class GetLocationSearchModel
    {
        public long? companyId { get; set; }
        public long? officeId { get; set; }
    }

    public class GetLocationList
    {
        public string latitude { get; set; }
        public string longitude { get; set; }
        public long? companyId { get; set; }
        public long? officeId { get; set; }
        public long? OfficeMapAddressId { get; set; }
    }

    public class ShiftSaveRequest {
        public long shiftId { get; set; }
        public string shiftName { get; set; }
        public string hourFrom { get; set; }
        public string hourTo { get; set; }
        public long companyId { get; set; }
    }
    public class GetShiftListRequest
    {
        public long shiftId { get; set; }
        public string shiftName { get; set; }
        public string hourFrom { get; set; }
        public string hourTo { get; set; }
        public long companyId { get; set; }
        public string modifiedBy { get; set; }
        public DateTime? modifiedDate { get; set; }
        public bool editableMode { get; set; }
    }
    public class SearchShiftGetRequest
    {
        public string searchShift { get; set; }
        public int pageIndex { get; set; }
        public int pageSize { get; set; }
        public long companyId { get; set; }
    }
    public class GetShiftModel
    {
        public List<GetShiftListRequest> shiftList { get; set; }
        public Int32 totalRecords { get; set; }
    }
    public class SaveRoleReq {
        public long roleId { get; set; }
        public long? officeId { get; set; }
        public string officeName { get; set; }
        public long? department { get; set; }
        public string departmentName { get; set; }
        public long? team { get; set; }
        public string teamName { get; set; }
        public long? position { get; set; }
        public string positionName { get; set; }
        public string createdBy { get; set; }
        public long? companyId { get; set; }
        public List<ModulePermission> modulePermissions { get; set; }
    }
    public class ModulePermission {
        public string moduleName { get; set; }
        public int moduleId { get; set; }
        public bool? read { get; set; }
        public bool? write { get; set; }
        public new List<ModulePermission> subModules { get; set; }
    }

    public class GetRoleReq {
        public long? companyId { get; set; }
    }

    public class ModulePermissionList {
        public long RoleId { get; set; }
        public long? positionId { get; set; }
        public long? teamId { get; set; }
        public long? departmentId { get; set; }
        public long? officeId { get; set; }
        public string positionname { get; set; }
        public string teamName { get; set; }
        public string departmentName { get; set; }
        public string officeName { get; set; }
        public long? companyId { get; set; }
        public string moduleName { get; set; }
        public bool? read { get; set; }
        public bool? create { get; set; }
        public bool? edit { get; set; }
        public bool? delete { get; set; }
        public string createdBy { get; set; }
        public DateTime? createdDate { get; set; }
        public string modifiedBy { get; set; }
        public DateTime? modifiedDate { get; set; }
    }

    public class PositionHierarchyResponse {
        public long PositionHierarchyId { get; set; }
        public long PositionId { get; set; }
        public string PositionName { get; set; }
        public long CompanyId { get; set; }
        public long? OrderNumber { get; set; }
    }
    public class PositionHierarchyRequest {
        public long PositionId { get; set; }
        public string PositionName { get; set; }
        public long CompanyId { get; set; }
        public long? OrderNumber { get; set; }
    }

}
