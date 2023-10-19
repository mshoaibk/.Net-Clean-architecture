using HRM_Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using Serilog;
using HRM_Domain.Model;

namespace HRM_Core_WebApp.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeDashboardController : ControllerBase {
        #region Global

        private readonly IEmployeeDashboardServices _IEmployeeDashboardServices;

        public EmployeeDashboardController(IEmployeeDashboardServices IEmployeeDashboardServices) {
            this._IEmployeeDashboardServices = IEmployeeDashboardServices;
        }

        #endregion

        #region Employee Dashboard Controller API's
        /// <summary>
        /// get dashboard data
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("GetEmployeeDashboardData")]
        public async Task<IActionResult> GetEmployeeDashboardData(EmployeeDashboardRequest model) {
            try {
                var _result = await _IEmployeeDashboardServices.GetEmployeeDashboardData(model);
                return Ok(new { employeeProfileResponse = _result.employeeProfileResponse, Status = true,
                    totalleaves =_result.employeeTotalLeaves.totalleaves,
                    consumedleaves= _result.employeeTotalLeaves.consumedleaves,
                    remainingleaves = _result.employeeTotalLeaves.remainingleaves,
                    currentStatus = _result.employeeCurrentStatus.currentStatus,
                    checkInTime = _result.employeeCurrentStatus.checkInTime,
                    workingPolicy = _result.employeeCurrentStatus.workingPolicy,
                    expectedEarnHour = _result.employeeCurrentStatus.expectedEarnHour,
                    employeeWeekAttendanceStatus=_result.employeeWeekAttendanceStatus
                });
            } catch (Exception ex) {
                // Log the exception and send the email
                LogAndSendException(ex, "GetEmployeeDashboardData Exception");
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        private void LogAndSendException(Exception ex, string msg) {
            // Log the exception
            Log.Error(ex, msg);
            // Send the exception email
            //_IEmailServices.SendExceptionEmail("athariqbal294@gmail.com", msg, ex.ToString());
        }
        #endregion
    }
}
