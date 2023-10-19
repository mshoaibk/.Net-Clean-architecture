using HRM_Application.Interfaces;
using HRM_Application.Services;
using HRM_Domain.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRM_Core_WebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanySetupController : ControllerBase
    {
        #region Global
        private readonly ICompanySetupServices _ICompanySetupServices;
        public CompanySetupController(ICompanySetupServices _ICompanySetupServices)
        {
            this._ICompanySetupServices = _ICompanySetupServices;
        }
        #endregion

        #region Company setup controller api's

        /// <summary>
        /// This is for create or set office for individual companies
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("CreateOffice")]
        public async Task<IActionResult> CreateOffice(List<OfficeLocationSaveRequest> model)
        {
            try
            {
                var officeCreation = _ICompanySetupServices.CreateOffice(model);
                return Ok(new { status = officeCreation.Result.status, msg = officeCreation.Result.msg });
            }
            catch (Exception ex)
            {
                // Log the exception and send the email
                LogAndSendException(ex, "CreateOffice Exception");
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        /// <summary>
        /// This is used to get office for individual companies
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("GetOfficeLocationList")]
        public async Task<IActionResult> GetOfficeLocationList(SearchOfficeLocationGetRequest model)
        {
            try
            {
                var officeSetupList = _ICompanySetupServices.GetOfficeLocationList(model);
                return Ok(new { Status = true, officeList = officeSetupList.Result.officeList });
            }
            catch (Exception ex)
            {
                // Log the exception and send the email
                LogAndSendException(ex, "GetOfficeLocationList Exception");
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        /// <summary>
        /// Delete office setup
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("DeleteOfficeSetup")]
        public async Task<IActionResult> DeleteOfficeSetup(DeleteOfficeRequest objReq)
        {
            try
            {
                var _result = await _ICompanySetupServices.DeleteOfficeSetup(objReq);
                return Ok(new { Status = true });
            }
            catch (Exception ex)
            {
                // Log the exception and send the email
                LogAndSendException(ex, "DeleteAttendanceLeaveRecord Exception");
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        /// <summary>
        /// This is for create or set department for individual companies
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("CreateDepartment")]
        public async Task<IActionResult> CreateDepartment(List<DepartmentSaveRequest> model)
        {
            try
            {
                var departmentCreation = _ICompanySetupServices.CreateDepartment(model);
                return Ok(new { status = departmentCreation.Result.status, msg = departmentCreation.Result.msg });
            }
            catch (Exception ex)
            {
                // Log the exception and send the email
                LogAndSendException(ex, "CreateDepartment Exception");
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        /// <summary>
        /// This is used for to get department for individual companies
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("GetDepartmentList")]
        public async Task<IActionResult> GetDepartmentList(SearchDepartmentGetRequest model)
        {
            try
            {
                var departmentList = _ICompanySetupServices.GetDepartmentList(model);
                return Ok(new { Status = true, departmentList = departmentList.Result.departmentList });
            }
            catch (Exception ex)
            {
                // Log the exception and send the email
                LogAndSendException(ex, "GetDepartmentList Exception");
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        /// <summary>
        /// Delete department detail
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("DeleteDepartmentSetup")]
        public async Task<IActionResult> DeleteDepartmentSetup(DeleteDepartmentRequest objReq)
        {
            try
            {
                var _result = await _ICompanySetupServices.DeleteDepartmentSetup(objReq);
                return Ok(new { Status = true });
            }
            catch (Exception ex)
            {
                // Log the exception and send the email
                LogAndSendException(ex, "DeleteDepartmentSetup Exception");
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        /// <summary>
        /// This is used for create team for individual companies
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("CreateTeam")]
        public async Task<IActionResult> CreateTeam(List<TeamSaveRequest> model)
        {
            try
            {
                var teamCreation = _ICompanySetupServices.CreateTeam(model);
                return Ok(new { status = teamCreation.Result.status, msg = teamCreation.Result.msg });
            }
            catch (Exception ex)
            {
                // Log the exception and send the email
                LogAndSendException(ex, "CreateTeam Exception");
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        /// <summary>
        /// This is used for to get Team for individual companies
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("GetTeamList")]
        public async Task<IActionResult> GetTeamList(SearchTeamGetRequest model)
        {
            try
            {
                var teamSetupList = _ICompanySetupServices.GetTeamList(model);
                return Ok(new { Status = true, teamList = teamSetupList.Result.teamList });
            }
            catch (Exception ex)
            {
                // Log the exception and send the email
                LogAndSendException(ex, "GetTeamList Exception");
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        /// <summary>
        /// Delete Team detail
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("DeleteTeamSetup")]
        public async Task<IActionResult> DeleteTeamSetup(DeleteTeamRequest objReq)
        {
            try
            {
                var _result = await _ICompanySetupServices.DeleteTeamSetup(objReq);
                return Ok(new { Status = true });
            }
            catch (Exception ex)
            {
                // Log the exception and send the email
                LogAndSendException(ex, "DeleteTeamSetup Exception");
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        /// <summary>
        /// This is used for Position for individual companies
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("CreatePosition")]
        public async Task<IActionResult> CreatePosition(List<PositionSaveRequest> model)
        {
            try
            {
                var positionCreation = _ICompanySetupServices.CreatePosition(model);
                return Ok(new { status = positionCreation.Result.status, msg = positionCreation.Result.msg });
            }
            catch (Exception ex)
            {
                // Log the exception and send the email
                LogAndSendException(ex, "CreatePosition Exception");
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        /// <summary>
        /// This is used for to get Position for individual companies
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("GetPositionList")]
        public async Task<IActionResult> GetPositionList(SearchPositionGetRequest model)
        {
            try
            {
                var positionSetupList = _ICompanySetupServices.GetPositionList(model);
                return Ok(new { Status = true, positionList = positionSetupList.Result.positionList });
            }
            catch (Exception ex)
            {
                // Log the exception and send the email
                LogAndSendException(ex, "GetPositionList Exception");
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        /// <summary>
        /// Delete Position detail
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("DeletePositionSetup")]
        public async Task<IActionResult> DeletePositionSetup(DeletePositionRequest objReq)
        {
            try
            {
                var _result = await _ICompanySetupServices.DeletePositionSetup(objReq);
                return Ok(new { Status = true });
            }
            catch (Exception ex)
            {
                // Log the exception and send the email
                LogAndSendException(ex, "DeletePositionSetup Exception");
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        /// <summary>
        /// This is used for to get Position for individual companies
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("GetSetupLookUpData")]
        public async Task<IActionResult> GetSetupLookUpData(GetSetupLookUpDataRequestModel model)
        {
            try
            {
                var lst = _ICompanySetupServices.GetSetupLookUpData(model);
                return Ok(new { Status = true, lookUpList = lst.Result });
            }
            catch (Exception ex)
            {
                // Log the exception and send the email
                LogAndSendException(ex, "GetSetupLookUpData Exception");
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        /// <summary>
        /// This is used for to get department against office location id
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetDepartmentByOfficeLocation/{companyId}/{officeId}")]
        public async Task<IActionResult> GetDepartmentByOfficeLocation(long? companyId,long? officeId)
        {
            try
            {
                var lst = _ICompanySetupServices.GetDepartmentByOfficeLocation(companyId,officeId);
                return Ok(new { Status = true, lookUpList = lst.Result });
            }
            catch (Exception ex)
            {
                // Log the exception and send the email
                LogAndSendException(ex, "GetDepartmentByOfficeLocation Exception");
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        /// <summary>
        /// This is used for to get team against department
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetTeamByDepartment/{companyId}/{departmentId}")]
        public async Task<IActionResult> GetTeamByDepartment(long? companyId, long? departmentId)
        {
            try
            {
                var lst = _ICompanySetupServices.GetTeamByDepartment(companyId,departmentId);
                return Ok(new { Status = true, lookUpList = lst.Result });
            }
            catch (Exception ex)
            {
                // Log the exception and send the email
                LogAndSendException(ex, "GetTeamByDepartment Exception");
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        /// <summary>
        /// This is used for to get position against team
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetPositionByTeam/{companyId}/{teamId}")]
        public async Task<IActionResult> GetPositionByTeam(long? companyId, long? teamId)
        {
            try
            {
                var lst = _ICompanySetupServices.GetPositionByTeam(companyId,teamId);
                return Ok(new { Status = true, lookUpList = lst.Result });
            }
            catch (Exception ex)
            {
                // Log the exception and send the email
                LogAndSendException(ex, "GetPositionByTeam Exception");
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        /// <summary>
        /// This is used for to get supervisor against team
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetPositionHierarchyEmployees/{companyId}/{positionId}")]
        public async Task<IActionResult> GetPositionHierarchyEmployees(long? companyId, long? positionId)
        {
            try
            {
                var lst = _ICompanySetupServices.GetPositionHierarchyEmployees(companyId, positionId);
                return Ok(new { Status = true, lookUpList = lst.Result });
            }
            catch (Exception ex)
            {
                // Log the exception and send the email
                LogAndSendException(ex, "GetPositionHierarchyEmployees Exception");
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        /// <summary>
        /// This is used for EmploymentTypeSetup for individual companies
        /// </summary>
        /// <returns></returns>
        ///
       
        [HttpPost]
        [Route("CreateEmploymentTypeSetup")]
        public async Task<IActionResult> CreateEmploymentTypeSetup(EmploymentTypeSetupRequest model)
        {
            try
            {
                var creation = _ICompanySetupServices.CreateEmploymentTypeSetup(model);
                return Ok(new { status = creation.Result.status, msg = creation.Result.msg });
            }
            catch (Exception ex)
            {
                // Log the exception and send the email
                LogAndSendException(ex, "CreateEmploymentTypeSetup Exception");
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        /// <summary>
        /// This is used for to get employment type
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("GetEmploymentTypeList")]
        public async Task<IActionResult> GetEmploymentTypeList(SearchEmpTypeGetRequest model)
        {
            try
            {
                var setupList = _ICompanySetupServices.GetEmploymentTypeList(model);
                return Ok(new { Status = true, employmentTypeList = setupList.Result.employmentTypeList });
            }
            catch (Exception ex)
            {
                // Log the exception and send the email
                LogAndSendException(ex, "GetEmploymentTypeList Exception");
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        /// <summary>
        /// Delete Position detail
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("DeleteEmploymentTypeSetup")]
        public async Task<IActionResult> DeleteEmploymentTypeSetup(DeleteEmploymentTypeRequest objReq)
        {
            try
            {
                var _result = await _ICompanySetupServices.DeleteEmploymentTypeSetup(objReq);
                return Ok(new { Status = true });
            }
            catch (Exception ex)
            {
                // Log the exception and send the email
                LogAndSendException(ex, "DeleteEmploymentTypeSetup Exception");
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        /// <summary>
        /// Save office location with latitude and longitude detail
        /// </summary>
        /// <param name="objReq"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("SaveLocation")]
        public async Task<IActionResult> SaveLocation(OfficeAddressSaveRequest objReq)
        {
            try
            {
                var _result = await _ICompanySetupServices.SaveLocation(objReq);
                return Ok(new { status = _result.status, msg = _result.msg });
            }
            catch (Exception ex)
            {
                // Log the exception and send the email
                LogAndSendException(ex, "SaveLocation Exception");
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        /// <summary>
        /// Save office location with latitude and longitude detail
        /// </summary>
        /// <param name="objReq"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetLocation")]
        public async Task<IActionResult> GetLocation(GetLocationSearchModel objReq)
        {
            try
            {
                var _result = await _ICompanySetupServices.GetLocation(objReq);
                return Ok(new { status = true, locationList = _result });
            }
            catch (Exception ex)
            {
                // Log the exception and send the email
                LogAndSendException(ex, "GetLocation Exception");
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        /// <summary>
        /// This is for create or set shift for individual companies
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("CreateShift")]
        public async Task<IActionResult> CreateShift(ShiftSaveRequest model) {
            try {
                var shiftCreation = _ICompanySetupServices.CreateShift(model);
                return Ok(new { status = shiftCreation.Result.status, msg = shiftCreation.Result.msg });
            } catch (Exception ex) {
                // Log the exception and send the email
                LogAndSendException(ex, "CreateShift Exception");
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }
        /// <summary>
        /// This is used for to get Shift List
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("GetShiftList")]
        public async Task<IActionResult> GetShiftList(SearchShiftGetRequest model)
        {
            try
            {
                var setupList =  _ICompanySetupServices.GetShiftList(model);
                return Ok(new { Status = true, shiftList = setupList.Result.shiftList });
            }
            catch (Exception ex)
            {
                // Log the exception and send the email
                LogAndSendException(ex, "Get Shift List Exception");
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }
        /// <summary>
        /// This is used for to Delete Shift
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("DeleteShift/{shiftId}")]
        public async Task<IActionResult> DeleteShift(int shiftId)
        {
            try
            {
                var _result = await _ICompanySetupServices.DeleteShift(shiftId);
                return Ok(new { Status = true });
            }
            catch (Exception ex)
            {
                // Log the exception and send the email
                LogAndSendException(ex, "DeleteShift Exception");
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        /// <summary>
        /// This is for to create roles for employees
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("SaveRole")]
        public async Task<IActionResult> SaveRole(SaveRoleReq model) {
            try {
                var saveRole = _ICompanySetupServices.SaveRole(model);
                return Ok(new { status = saveRole.Result.status, msg = saveRole.Result.msg });
            } catch (Exception ex) {
                // Log the exception and send the email
                LogAndSendException(ex, "SaveRole Exception");
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        /// <summary>
        /// This is for to create roles for employees
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("GetRoles")]
        public async Task<IActionResult> GetRoles(GetRoleReq model) {
            try {
                var getRole = _ICompanySetupServices.GetRoles(model);
                return Ok(new { status = true, lstRoles = getRole.Result });
            } catch (Exception ex) {
                // Log the exception and send the email
                LogAndSendException(ex, "GetRoles Exception");
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        /// <summary>
        /// This is for to create roles for employees by id
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetRolesById/{roleId}/{office}/{department}/{team}/{position}/{companyID}")]
        public async Task<IActionResult> GetRolesById(long roleId, long office, long department, long team, long position, long companyID) {
            try {
                var getRole = _ICompanySetupServices.GetRolesById(roleId,office,department,team,position, companyID);
                return Ok(new { status = true, lstRoles = getRole.Result });
            } catch (Exception ex) {
                // Log the exception and send the email
                LogAndSendException(ex, "GetRolesById Exception");
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        /// <summary>
        /// This is for to create roles for employees by id
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("DeleteEmployeeSetupRoles/{roleId}")]
        public async Task<IActionResult> DeleteEmployeeSetupRoles(long roleId)
        {
            try
            {
                var roleResult = _ICompanySetupServices.DeleteEmployeeSetupRoles(roleId);
                return Ok(new { status = roleResult });
            }
            catch (Exception ex)
            {
                // Log the exception and send the email
                LogAndSendException(ex, "GetRolesById Exception");
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        /// <summary>
        /// This is for to GetPositionHierarchy id companyid
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetPositionHierarchy/{companyId}")]
        public async Task<IActionResult> GetPositionHierarchy(long companyId) {
            try {
                var result = _ICompanySetupServices.GetPositionHierarchy(companyId);
                return Ok(new { lst = result.Result });
            } catch (Exception ex) {
                // Log the exception and send the email
                LogAndSendException(ex, "GetPositionHierarchy Exception");
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        /// <summary>
        /// This is for to create roles for employees by id
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("UpdatePositionHierarchy")]
        public async Task<IActionResult> UpdatePositionHierarchy(List<PositionHierarchyRequest> obj) {
            try {
                var result = _ICompanySetupServices.UpdatePositionHierarchy(obj);
                return Ok(new { status = result.Result.status,msg=result.Result.msg });
            } catch (Exception ex) {
                // Log the exception and send the email
                LogAndSendException(ex, "UpdatePositionHierarchy Exception");
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        private void LogAndSendException(Exception ex, string msg)
        {
            // Log the exception
            Log.Error(ex, msg);
            // Send the exception email
            //_IEmailServices.SendExceptionEmail("athariqbal294@gmail.com", msg, ex.ToString());
        }

        #endregion

    }
}
