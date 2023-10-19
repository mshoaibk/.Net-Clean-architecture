using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM_Domain.Model
{
    #region Attendance
    public class EmployeeAttendanceRequestModel
    {
        public long employeeAttendanceID { get; set; }
        public long employeeID { get; set; }
        public long companyID { get; set; }
        public DateTime? attendanceDateTime { get; set; }
        public string attendanceDate { get; set; }
        public string checkedIn { get; set; }
        public string checkedOut { get; set; }
        public bool? isDeleted { get; set; }
        public string createdBy { get; set; }
        public DateTime? createdDate { get; set; }
        public string modifiedBy { get; set; }
        public DateTime? modifiedDate { get; set; }
        public string action { get; set; }
        public string approvalStatus { get; set; }
        public string attendanceStatus { get; set; }
        public string comment { get; set; }
        public DateTime? leaveStartDate { get; set; }
        public DateTime? leaveEndDate { get; set; }
        public string leaveDuration { get; set; }
        public string leaveType { get; set; }
    }

    public class GetAttendanceCalendarViewRequest
    {
        public long? employeeID { get; set; }
        public int? searchedMonth { get; set; }
        public int? searchedYear { get; set; }
    }

    public class GetAttendanceCalendarViewResponseModel
    {
        public List<GetAttendanceCalendarViewResponse> GetAttendanceCalendarViewResponse { get; set; }
        public GetAttendanceCalendarViewResponse[][] CalendarDay { get; set; }
        public DateTime? joiningDate { get; set; }
        public string employeeName { get; set; }
    }

    public class GetAttendanceCalendarViewResponse
    {
        public long employeeAttendanceID { get; set; }
        public long employeeID { get; set; }
        public long companyID { get; set; }
        public DateTime? attendanceDate { get; set; }
        public int attendanceYear { get; set; }
        public int attendanceMonth { get; set; }
        public int attendanceDateNum { get; set; }
        public string checkedIn { get; set; }
        public string checkedOut { get; set; }
        public bool? isDeleted { get; set; }
        public string createdBy { get; set; }
        public DateTime? createdDate { get; set; }
        public string modifiedBy { get; set; }
        public DateTime? modifiedDate { get; set; }
        public string action { get; set; }
        public string attendanceStatus { get; set; }
        public string approvalStatus { get; set; }
        public string comment { get; set; }
    }

    public class EmployeeLeaveRequestModel
    {
        public long employeeLeaveRequestID { get; set; }
        public long employeeID { get; set; }
        public long companyID { get; set; }
        public DateTime? leaveStartDate { get; set; }
        public DateTime? leaveEndDate { get; set; }
        public string leaveDuration { get; set; }
        public string leaveType { get; set; }
        public string leaveStatus { get; set; }
        public string leaveReason { get; set; }
        public string leaveTimeFrom { get; set; }
        public string leaveTimeTo { get; set; }
        public string createdBy { get; set; }
        public string modifiedBy { get; set; }
        public string action { get; set; }
    }

    public class GetEmployeeLeaveCalendarViewRequest
    {
        public long employeeID { get; set; }
    }

    public class GetEmployeeLeaveCalendarViewResponse
    {
        public long employeeLeaveRequestID { get; set; }
        public long employeeID { get; set; }
        public long companyID { get; set; }
        public DateTime? leaveStartDate { get; set; }
        public int leaveStartYear { get; set; }
        public int leaveStartMonth { get; set; }
        public int leaveStartDateNum { get; set; }
        public DateTime? leaveEndDate { get; set; }
        public int leaveEndYear { get; set; }
        public int leaveEndMonth { get; set; }
        public int leaveEndDateNum { get; set; }
        public string leaveDuration { get; set; }
        public string leaveType { get; set; }
        public string leaveStatus { get; set; }
        public string leaveReason { get; set; }
        public string leaveTimeFrom { get; set; }
        public string leaveTimeTo { get; set; }
        public string createdBy { get; set; }
        public DateTime? createdDate { get; set; }
        public string modifiedBy { get; set; }
        public DateTime? modifiedDate { get; set; }
        public string action { get; set; }
    }

    public class GetEmployeeLeaveSearchRequest
    {
        public long companyId { get; set; }
        public string fullName { get; set; }
        public DateTime? searchDate { get; set; }
        public int pageIndex { get; set; }
        public int pageSize { get; set; }
        public string filterStatus { get; set; }
    }
    public class GetEmployeeLeaveModel
    {
        public List<GetEmployeeLeaveList> EmployeeLeaveList { get; set; }
        public Int32 TotalRecords { get; set; }
    }
    public class GetEmployeeLeaveList
    {
        public long employeeId { get; set; }
        public string employeeName { get; set; }
        public DateTime? leaveDate { get; set; }
        public string leaveTime { get; set; }
        public string leaveType { get; set; }
        public string leaveDurationType { get; set; }
        public string reason { get; set; }
        public string status { get; set; }
        public long employeeLeaveRequestId { get; set; }
    }
    public class GetEmployeeAttendanceSearchRequest
    {
        public long companyId { get; set; }
        public string location { get; set; }
        public string fullName { get; set; }
        public string employmentType { get; set; }
        public string department { get; set; }
        public string position { get; set; }
        public string team { get; set; }
        public string filterStatus { get; set; }
        public DateTime? searchDate { get; set; }
        public int pageIndex { get; set; }
        public int pageSize { get; set; }
    }
    public class GetEmployeeAttendanceModel
    {
        public List<GetEmployeeAttendanceList> EmployeeAttendenceList { get; set; }
        public Int32 TotalRecords { get; set; }
    }
    public class GetEmployeeAttendanceList
    {
        public long employeeId { get; set; }
        public string employeeName { get; set; }
        public string status { get; set; }
        public long employeeAttendenceId { get; set; }
        public DateTime? attendanceDate { get; set; }
        public string checkedIn { get; set; }
        public string checkedOut { get; set; }
        public string timeWorked { get; set; }
        public string approvalStatus { get; set; }
        public string comment { get; set; }
    }

    public class GetEmployeeAttendanceRecordByEmployeeId
    {
        public long companyId { get; set; }
        public long employeeId { get; set; }
        public DateTime? searchDate { get; set; }
        public int pageIndex { get; set; }
        public int pageSize { get; set; }
    }

    public class GetEmployeeLeaveRecordByEmployeeId
    {
        public long companyId { get; set; }
        public long employeeId { get; set; }
        public DateTime? searchDate { get; set; }
        public int pageIndex { get; set; }
        public int pageSize { get; set; }
    }

    public class GetEmployeeLeaveHistoryModel
    {
        public List<GetEmployeeLeaveHistoryList  > EmployeeLeaveList { get; set; }
        public Int32 TotalRecords { get; set; }
    }
    public class GetEmployeeLeaveHistoryList
    {
        public long employeeId { get; set; }
        public string employeeName { get; set; }
        public string leaveDate { get; set; }
        public string leaveTime { get; set; }
        public string leaveType { get; set; }
        public string leaveDurationType { get; set; }
        public string reason { get; set; }
        public string status { get; set; }
        public long employeeLeaveRequestId { get; set; }
    }

    public class UpdateEmployeeLeaveStatusRqst
    {
        public long employeeLeaveRequestId { get; set; }
        public string status { get; set; }
    }

    public class DeleteAttendanceLeaveRequest
    {
        public long? id { get; set; }
    }

    public class ReturnMessageAlreadyExistAttend
    {
        public string msg { get; set; }
        public bool status { get; set; }
    }

    public class CompanyInfoByEmployeeResult {
        public long officeId { get; set; }
        public string officeLocationName { get; set; }
        public string latitude { get; set; }
        public string longitude { get; set; }
    }

    public class TodayEmployeeAttendanceResult {
        public string checkedIn { get; set; }
        public string checkedOut { get; set; }
        public long? employeeAttendanceID { get; set; }
    }
    #endregion
    #region Weekly Holidays
    public class WeeklyHolidaysSaveRequest   
    {
        public long weeklyholidaysId { get; set; }
        public long companyId { get; set; }
        public List<WeeklyHolidaysListRequest> holidays { get; set; }
    }
    public class WeeklyHolidaysListRequest {
        public string holidays { get; set; }
    }
    public class GetWeeklyHolidaysListRequest
    {
        public long WeeklyholidaysId { get; set; }
         public string Holidays { get; set; }
        public long? CompanyId { get; set; }
        public bool? IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        

    }
    #endregion

    #region Yearly Holidays
    public class YearlyHolidaysRequest
    {
        public long yearlyHolidaysId { get; set; }
        public string yearlyHolidaysName { get; set; }
        public DateTime? fromDate { get; set; }
        public DateTime? toDate { get; set; }
        public int numberOfDays { get; set; }
        public long? companyId { get; set; }
    }
    public class GetYearlyHolidaysListRequest
    {
        public long yearlyHolidaysId { get; set; }
        public string yearlyHolidaysName { get; set; }
        public DateTime? fromDate { get; set; }
        public DateTime? toDate { get; set; }
        public int numberOfDays { get; set; }
        public long? companyId { get; set; }
        public bool? isDeleted { get; set; }
        public string createdBy { get; set; }
        public DateTime? createdDate { get; set; }
        public string modifiedBy { get; set; }
        public DateTime? modifiedDate { get; set; }
        public bool editableMode { get; set; }


    }
    public class SearchYearlyHolidaysGetRequest
    {
        public string searchYearlyHolidays { get; set; }
        public int pageIndex { get; set; }
        public int pageSize { get; set; }
        public long companyId { get; set; }
    }
    public class GetYearlyHolidaysModel
    {
        public List<GetYearlyHolidaysListRequest> YearlyHolidaysList { get; set; }
        public Int32 totalRecords { get; set; }
    }
    #endregion
}
