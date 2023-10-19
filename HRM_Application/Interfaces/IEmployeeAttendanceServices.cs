using HRM_Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM_Application.Interfaces
{
    public interface IEmployeeAttendanceServices
    {
        Task<bool> AddAttendance(EmployeeAttendanceRequestModel model);
        Task<GetAttendanceCalendarViewResponseModel> GetAttendanceCalendarView(GetAttendanceCalendarViewRequest model);
        Task<bool> AddLeaveRequest(EmployeeLeaveRequestModel model);
        Task<List<GetEmployeeLeaveCalendarViewResponse>> GetEmployeeLeaveCalendarView(GetEmployeeLeaveCalendarViewRequest model);
        Task<GetEmployeeLeaveModel> GetEmployeeLeaveRequest(GetEmployeeLeaveSearchRequest model);
        Task<GetEmployeeAttendanceModel> GetEmployeeAttendanceRecord(GetEmployeeAttendanceSearchRequest model);
        Task<GetEmployeeAttendanceModel> GetEmployeeAttendanceRecordByEmployeeId(GetEmployeeAttendanceRecordByEmployeeId model);
        Task<GetEmployeeLeaveHistoryModel> GetEmployeeLeaveRecordByEmployeeId(GetEmployeeLeaveRecordByEmployeeId model);
        Task<bool> UpdateEmployeeLeaveStatus(UpdateEmployeeLeaveStatusRqst model);
        Task<bool> DeleteAttendanceLeaveRecord(DeleteAttendanceLeaveRequest objReq);
        Task<CompanyInfoByEmployeeResult> GetOfficeInfoByEmployee(long? employeeId);
        Task<TodayEmployeeAttendanceResult> GetTodayEmployeeAttendance(long? employeeId);
        Task<SaveResponseMessage> SaveWeeklyHolidays(WeeklyHolidaysSaveRequest objReq);
        Task<List<GetWeeklyHolidaysListRequest>> GetWeeklyHolidaysByCompId(long compId);
        Task<bool> DeleteWeeklyHolidayByCampId(long CompId);
         
        Task<SaveResponseMessage> SaveYearlyHolidays(YearlyHolidaysRequest objReq);
        Task<GetYearlyHolidaysModel> GetYearlyHolidays(SearchYearlyHolidaysGetRequest model);
        Task<bool> DeletYearlyHolidayByCampId(long CompId);
    }
}
