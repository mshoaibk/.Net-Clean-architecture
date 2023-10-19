using HRM_Application.Interfaces;
using HRM_Application.Services;
using HRM_Domain.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRM_Core_WebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeAttendanceController : ControllerBase
    {
        #region Global

        private readonly IEmployeeAttendanceServices _IEmployeeAttendanceServices;

        public EmployeeAttendanceController(IEmployeeAttendanceServices _IEmployeeAttendanceServices)
        {
            this._IEmployeeAttendanceServices = _IEmployeeAttendanceServices;
        }
        #endregion

        #region Employee Attendance Controller API's
        /// <summary>
        /// Add employee attendance
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("AddAttendance")]
        public async Task<IActionResult> AddAttendance(EmployeeAttendanceRequestModel model)
        {
            try
            {
                var _result = await _IEmployeeAttendanceServices.AddAttendance(model);
                return Ok(new { Status = true });
            }
            catch (Exception ex)
            {
                // Log the exception and send the email
                LogAndSendException(ex, "AddAttendance Exception");
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        /// <summary>
        /// show employee attendance in full calendar view
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetAttendanceCalendarView")]
        public async Task<IActionResult> GetAttendanceCalendarView(GetAttendanceCalendarViewRequest model)
        {
            try
            {
                var _result = await _IEmployeeAttendanceServices.GetAttendanceCalendarView(model);
                return Ok(new { Status = true, showCalendarView = _result });
            }
            catch (Exception ex)
            {
                // Log the exception and send the email
                LogAndSendException(ex, "GetAttendanceCalendarView Exception");
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        /// <summary>
        /// Add employee leave request
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("AddLeaveRequest")]
        public async Task<IActionResult> AddLeaveRequest(EmployeeLeaveRequestModel model)
        {
            try
            {
                var _result = await _IEmployeeAttendanceServices.AddLeaveRequest(model);
                return Ok(new { Status = true });
            }
            catch (Exception ex)
            {
                // Log the exception and send the email
                LogAndSendException(ex, "AddLeaveRequest Exception");
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        /// <summary>
        /// show employee leave in full calendar view
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetEmployeeLeaveCalendarView")]
        public async Task<IActionResult> GetEmployeeLeaveCalendarView(GetEmployeeLeaveCalendarViewRequest model)
        {
            try
            {
                var _result = await _IEmployeeAttendanceServices.GetEmployeeLeaveCalendarView(model);
                return Ok(new { Status = true, showLeaveCalendarView = _result });
            }
            catch (Exception ex)
            {
                // Log the exception and send the email
                LogAndSendException(ex, "GetEmployeeLeaveCalendarView Exception");
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        /// <summary>
        /// show employee leave in full calendar view
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetEmployeeLeaveRequest")]
        public async Task<IActionResult> GetEmployeeLeaveRequest(GetEmployeeLeaveSearchRequest model)
        {
            try
            {
                var _result = await _IEmployeeAttendanceServices.GetEmployeeLeaveRequest(model);
                return Ok(new { Status = true, employeelist = _result.EmployeeLeaveList, totalRecords = _result.TotalRecords });
            }
            catch (Exception ex)
            {
                // Log the exception and send the email
                LogAndSendException(ex, "GetEmployeeLeaveRequest Exception");
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        /// <summary>
        /// show employee attendance system
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetEmployeeAttendanceRecord")]
        public async Task<IActionResult> GetEmployeeAttendanceRecord(GetEmployeeAttendanceSearchRequest model)
        {
            try
            {
                var _result = await _IEmployeeAttendanceServices.GetEmployeeAttendanceRecord(model);
                return Ok(new { Status = true, employeelist = _result.EmployeeAttendenceList, totalRecords = _result.TotalRecords });
            }
            catch (Exception ex)
            {
                // Log the exception and send the email
                LogAndSendException(ex, "GetEmployeeAttendanceRecord Exception");
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        /// <summary>
        /// show employee attendance system
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetEmployeeAttendanceRecordByEmployeeId")]
        public async Task<IActionResult> GetEmployeeAttendanceRecordByEmployeeId(GetEmployeeAttendanceRecordByEmployeeId model)
        {
            try
            {
                var _result = await _IEmployeeAttendanceServices.GetEmployeeAttendanceRecordByEmployeeId(model);
                return Ok(new { Status = true, employeelist = _result.EmployeeAttendenceList, totalRecords = _result.TotalRecords });
            }
            catch (Exception ex)
            {
                // Log the exception and send the email
                LogAndSendException(ex, "GetEmployeeAttendanceRecordByEmployeeId Exception");
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        /// <summary>
        /// show employee Leave system history
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetEmployeeLeaveRecordByEmployeeId")]
        public async Task<IActionResult> GetEmployeeLeaveRecordByEmployeeId(GetEmployeeLeaveRecordByEmployeeId model)
        {
            try
            {
                var _result = await _IEmployeeAttendanceServices.GetEmployeeLeaveRecordByEmployeeId(model);
                return Ok(new { Status = true, employeelist = _result.EmployeeLeaveList, totalRecords = _result.TotalRecords });
            }
            catch (Exception ex)
            {
                // Log the exception and send the email
                LogAndSendException(ex, "GetEmployeeLeaveRecordByEmployeeId Exception");
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        /// <summary>
        /// show employee Leave system history
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("UpdateEmployeeLeaveStatus")]
        public async Task<IActionResult> UpdateEmployeeLeaveStatus(UpdateEmployeeLeaveStatusRqst model)
        {
            try
            {
                var _result = await _IEmployeeAttendanceServices.UpdateEmployeeLeaveStatus(model);
                return Ok(new { Status = _result});
            }
            catch (Exception ex)
            {
                // Log the exception and send the email
                LogAndSendException(ex, "UpdateEmployeeLeaveStatus Exception");
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        /// <summary>
        /// Delete attendance detail
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("DeleteAttendanceLeaveRecord")]
        public async Task<IActionResult> DeleteAttendanceLeaveRecord(DeleteAttendanceLeaveRequest objReq)
        {
            try
            {
                var _result = await _IEmployeeAttendanceServices.DeleteAttendanceLeaveRecord(objReq);
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
        /// This is used for to get company location by employee id
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetOfficeInfoByEmployee/{employeeId}")]
        public async Task<IActionResult> GetOfficeInfoByEmployee(long? employeeId) {
            try {
                var lst = _IEmployeeAttendanceServices.GetOfficeInfoByEmployee(employeeId);
                return Ok(new { Status = true, officeLocation = lst.Result });
            } catch (Exception ex) {
                // Log the exception and send the email
                LogAndSendException(ex, "GetOfficeInfoByEmployee Exception");
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        /// <summary>
        /// This is used for to get company location by employee id
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetTodayEmployeeAttendance/{employeeId}")]
        public async Task<IActionResult> GetTodayEmployeeAttendance(long? employeeId) {
            try {
                var lst = _IEmployeeAttendanceServices.GetTodayEmployeeAttendance(employeeId);
                return Ok(new { Status = true, attendanceRecord = lst.Result });
            } catch (Exception ex) {
                // Log the exception and send the email
                LogAndSendException(ex, "GetTodayEmployeeAttendance Exception");
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

        #region Holidays Api's Action
        /// <summary>
        /// This is for create or set Weekly Holidays
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("SaveWeeklyHolidays")]
        public async Task<IActionResult> SaveWeeklyHolidays(WeeklyHolidaysSaveRequest model)
        {
            try
            {
                var SaveWeeklyHoliday = _IEmployeeAttendanceServices.SaveWeeklyHolidays(model);
                return Ok(new { status = SaveWeeklyHoliday.Result.status, msg = SaveWeeklyHoliday.Result.msg });
            }
            catch (Exception ex)
            {
                // Log the exception and send the email
                LogAndSendException(ex, "CreateShift Exception");
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }
        /// <summary>
        /// This is used for to get Weekly Holidays
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetWeeklyHolidays/{companyID}")]
        public async Task<IActionResult> GetWeeklyHolidays(long companyID)
        {
            try
            {
                var HolidaysList = _IEmployeeAttendanceServices.GetWeeklyHolidaysByCompId(companyID);
                return Ok(new { Status = true, holidaysList = HolidaysList });
            }
            catch (Exception ex)
            {
                // Log the exception and send the email
                LogAndSendException(ex, "Get Weekly Holidays Exception");
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }
        /// <summary>
        /// This is used  to  Delete Weekly Holiday
        /// </summary>
        /// <returns></returns>
        [HttpDelete]
        [Route("DeleteWeeklyHoliday/{compId}")]
        public async Task<IActionResult> DeleteWeeklyHoliday(long compId)
        {
            try
            {
                var result = _IEmployeeAttendanceServices.DeleteWeeklyHolidayByCampId(compId);
                if(result.Result ==true)
                {
                    return Ok(new { Status = true, message = "Weekly Holidays has been deleted successfully" });
                }
                else
                {
                    return Ok(new { Status = false, message = "Failed" });
                }
            }
            catch (Exception ex)
            {
                // Log the exception and send the email
                LogAndSendException(ex, "Get Weekly Holidays Exception");
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        /// <summary>
        /// This is for create or set Save Yearly Holidays
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("SaveYearlyHolidays")]
        public async Task<IActionResult> SaveYearlyHolidays(YearlyHolidaysRequest model)
        {
            try
            {
                var SaveWeeklyHoliday = _IEmployeeAttendanceServices.SaveYearlyHolidays(model);
                return Ok(new { status = SaveWeeklyHoliday.Result.status, msg = SaveWeeklyHoliday.Result.msg });
            }
            catch (Exception ex)
            {
                // Log the exception and send the email
                LogAndSendException(ex, "Save Yearly Holidays Exception");
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }
        /// <summary>
        /// This is used for to get early Holidays List
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("GetYearlyHolidays")]
        public async Task<IActionResult> GetYearlyHolidays(SearchYearlyHolidaysGetRequest model)
        {
            try
            {
               // GetYearlyHolidaysModel obj = new GetYearlyHolidaysModel();
               var obj = _IEmployeeAttendanceServices.GetYearlyHolidays(model);
                
                return Ok(new { Status = true, holidaysList = obj.Result.YearlyHolidaysList, totalRecords = obj.Result.totalRecords });
            }
            catch (Exception ex)
            {
                // Log the exception and send the email
                LogAndSendException(ex, "Get Weekly Holidays Exception");
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }
        /// <summary>
        /// This is used  to  Delete Yearly Holiday
        /// </summary>
        /// <returns></returns>
        [HttpDelete]
        [Route("DeleteYearlyHolidays/{compId}")]
        public async Task<IActionResult> DeleteYearlyHolidays(long compId)
        {
            try
            {
                var result = _IEmployeeAttendanceServices.DeletYearlyHolidayByCampId(compId);
                if (result.Result == true)
                {
                    return Ok(new { Status = true, message = "Yearly Holidays has been deleted successfully" });
                }
                else
                {
                    return Ok(new { Status = false, message = "Failed" });
                }
            }
            catch (Exception ex)
            {
                // Log the exception and send the email
                LogAndSendException(ex, "Get Weekly Holidays Exception");
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }
        #endregion
    }
}
