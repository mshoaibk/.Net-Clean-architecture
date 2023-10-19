using HRM_Application.Interfaces;
using HRM_Domain.Model;
using HRM_Infrastructure.HRMDataBaseContext;
using HRM_Infrastructure.TableEntities;
using iTextSharp.text;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data.Entity;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace HRM_Application.Services {
    public class EmployeeAttendanceServices : IEmployeeAttendanceServices {
        private readonly HRMContexts dbContextHRM;
        List<string> leaveValues = new List<string> { "full-day", "half-day", "short-leave" };
        public EmployeeAttendanceServices(HRMContexts context) {
            dbContextHRM = context;
        }
        public async Task<bool> AddAttendance(EmployeeAttendanceRequestModel model) {
            try {
                if (model.leaveDuration == "full-day" && model.leaveStartDate.HasValue && model.leaveEndDate.HasValue) {
                    DateTime startDate = model.leaveStartDate.Value;
                    DateTime endDate = model.leaveEndDate.Value;
                    for (DateTime date = startDate; date <= endDate; date = date.AddDays(1)) {
                        model.attendanceDate = date.ToString();
                        SaveAttendance(model);
                    }
                } else {
                    SaveAttendance(model);
                }
                return true;
            } catch (Exception ex) {
                return false;
            }
        }

        private void SaveAttendance(EmployeeAttendanceRequestModel model) {
            TblEmployeeAttendance tblEmployeeAttendanceObj = new TblEmployeeAttendance();
            if (model.action == "update") {
                tblEmployeeAttendanceObj = dbContextHRM.tblEmployeeAttendance.Where(atten => atten.EmployeeAttendanceID == model.employeeAttendanceID).FirstOrDefault();
            }
            tblEmployeeAttendanceObj.EmployeeID = model.employeeID;
            tblEmployeeAttendanceObj.CompanyID = model.companyID;

            model.attendanceDateTime = DateTime.Parse(model.attendanceDate);
            var employeeLeave = dbContextHRM.tblEmployeeLeaveRequest.
                Where(x => x.LeaveStartDate == model.attendanceDateTime).FirstOrDefault();
            if (employeeLeave != null) {
                dbContextHRM.Remove(employeeLeave);
            }
            tblEmployeeAttendanceObj.AttendanceDate = model.attendanceDateTime;
            tblEmployeeAttendanceObj.AttendanceYear = model.attendanceDateTime.Value.Year;
            tblEmployeeAttendanceObj.AttendanceMonth = model.attendanceDateTime.Value.Month;
            tblEmployeeAttendanceObj.AttendanceDateNum = model.attendanceDateTime.Value.Day;
            string checkedInTime = null;
            string checkedOutTime = null;
            if (!string.IsNullOrEmpty(model.checkedIn)) {
                checkedInTime = DateTime.ParseExact(model.checkedIn, "h:mm tt", CultureInfo.InvariantCulture).ToString("hh:mm tt");
            }
            if (!string.IsNullOrEmpty(model.checkedIn)) {
                checkedOutTime = !string.IsNullOrEmpty(model.checkedOut) ? DateTime.ParseExact(model.checkedOut, "h:mm tt", CultureInfo.InvariantCulture).ToString("hh:mm tt") : "";
            }
            tblEmployeeAttendanceObj.CheckedIn = checkedInTime;
            tblEmployeeAttendanceObj.CheckedOut = checkedOutTime;
            tblEmployeeAttendanceObj.TimeWorked = string.IsNullOrEmpty(checkedInTime) && string.IsNullOrEmpty(checkedOutTime) ? null : CalculateWorkedTime(checkedInTime, checkedOutTime).ToString();
            tblEmployeeAttendanceObj.AttendanceStatus = model.attendanceStatus;
            tblEmployeeAttendanceObj.ApprovalStatus = model.approvalStatus;
            tblEmployeeAttendanceObj.Comment = model.comment;
            tblEmployeeAttendanceObj.IsDeleted = false;
            // Checking if both start and end dates are not null

            if (model.action != "update") {
                tblEmployeeAttendanceObj.CreatedBy = model.createdBy;
                tblEmployeeAttendanceObj.CreatedDate = DateTime.Now;
            }
            if (model.action == "update") {
                tblEmployeeAttendanceObj.ModifiedBy = model.modifiedBy;
                tblEmployeeAttendanceObj.ModifiedDate = DateTime.Now;
            }
            if (model.action == "save") {
                dbContextHRM.tblEmployeeAttendance.Add(tblEmployeeAttendanceObj);
            } else {
                dbContextHRM.Update(tblEmployeeAttendanceObj);
            }
            dbContextHRM.SaveChanges();
        }

        public async Task<GetAttendanceCalendarViewResponseModel> GetAttendanceCalendarView(GetAttendanceCalendarViewRequest model) {
            var CompId = dbContextHRM.tblEmployee.Where(x => x.EmployeeID == model.employeeID).FirstOrDefault().CompanyID;
            GetAttendanceCalendarViewResponseModel objAtten = new GetAttendanceCalendarViewResponseModel();
            List<GetAttendanceCalendarViewResponse> response = new List<GetAttendanceCalendarViewResponse>();
            response = (from employeeAttendance in dbContextHRM.tblEmployeeAttendance
                        where employeeAttendance.IsDeleted == false &&
                        employeeAttendance.EmployeeID.Equals(model.employeeID)
                        && employeeAttendance.AttendanceMonth.Equals(model.searchedMonth)
                        && employeeAttendance.AttendanceYear.Equals(model.searchedYear)
                        select new GetAttendanceCalendarViewResponse {
                            employeeAttendanceID = employeeAttendance.EmployeeAttendanceID,
                            employeeID = employeeAttendance.EmployeeID,
                            companyID = employeeAttendance.CompanyID,
                            attendanceDate = employeeAttendance.AttendanceDate,
                            attendanceYear = employeeAttendance.AttendanceYear,
                            attendanceMonth = employeeAttendance.AttendanceMonth,
                            attendanceDateNum = employeeAttendance.AttendanceDateNum,
                            checkedIn = employeeAttendance.CheckedIn,
                            checkedOut = employeeAttendance.CheckedOut,
                            action = "update",
                            attendanceStatus = employeeAttendance.AttendanceStatus,
                            approvalStatus = employeeAttendance.ApprovalStatus,
                            comment = employeeAttendance.Comment
                        }).ToList();
            objAtten.GetAttendanceCalendarViewResponse = response;
            var emp = dbContextHRM.tblEmployee.Where(x => x.EmployeeID == model.employeeID).FirstOrDefault();
            objAtten.joiningDate = emp.HireDate;
            objAtten.employeeName = emp.FullName;
            objAtten.CalendarDay = GetMonthCalendar(model, emp.HireDate.Value, response, CompId);
            return objAtten;
        }

        public GetAttendanceCalendarViewResponse[][] GetMonthCalendar(GetAttendanceCalendarViewRequest model, DateTime hireDate, List<GetAttendanceCalendarViewResponse> resp,long CompId) {
            // Get the current year and month
            int year = (int)model.searchedYear;
            int month = (int)model.searchedMonth;

            // Determine the number of days in the current month
            int daysInMonth = DateTime.DaysInMonth(year, month);

            // Determine the first day of the month
            DateTime firstDayOfMonth = new DateTime(year, month, 1);

            // Determine the number of days to skip based on the hire date
            int daysToSkip = (hireDate > firstDayOfMonth) ? (hireDate - firstDayOfMonth).Days : 0;

            // Create the calendar array
            var calendar = new GetAttendanceCalendarViewResponse[6][];

            for (int i = 0; i < calendar.Length; i++) {
                calendar[i] = new GetAttendanceCalendarViewResponse[7];
            }

            // Populate the calendar with attendance data
            int day = 1;
            int row = 0;
            int col = (int)firstDayOfMonth.DayOfWeek;

            while (day <= daysInMonth) {
                // Skip the days before the hire date
                if (day <= daysToSkip) {
                    col++;
                    if (col == 7) {
                        col = 0;
                        row++;
                    }

                    day++;
                    continue;
                }

                var getAttendanceData = resp.Where(x => x.attendanceYear == year && x.attendanceMonth == month &&
                    x.attendanceDateNum == day).FirstOrDefault();
                if (getAttendanceData != null) {
                    calendar[row][col] = new GetAttendanceCalendarViewResponse {
                        attendanceDateNum = day,
                        employeeAttendanceID = getAttendanceData.employeeAttendanceID,
                        employeeID = getAttendanceData.employeeID,
                        companyID = getAttendanceData.companyID,
                        attendanceDate = getAttendanceData.attendanceDate,
                        attendanceYear = getAttendanceData.attendanceYear,
                        attendanceMonth = getAttendanceData.attendanceMonth,
                        checkedIn = getAttendanceData.checkedIn,
                        checkedOut = getAttendanceData.checkedOut,
                        isDeleted = getAttendanceData.isDeleted,
                        createdBy = getAttendanceData.createdBy,
                        createdDate = getAttendanceData.createdDate,
                        modifiedBy = getAttendanceData.modifiedBy,
                        modifiedDate = getAttendanceData.modifiedDate,
                        action = getAttendanceData.action,
                        attendanceStatus = getAttendanceData.attendanceStatus,
                        approvalStatus = getAttendanceData.approvalStatus,
                        comment = getAttendanceData.comment
                    };
                } else {
                    calendar[row][col] = new GetAttendanceCalendarViewResponse {
                        attendanceDateNum = day,
                        attendanceStatus = AttendanceDayStatus(year, month, day, CompId),

                    };
                }
                col++;
                if (col == 7) {
                    col = 0;
                    row++;
                }

                day++;
            }

            return calendar;
        }
        public  string AttendanceDayStatus(int year,int month,int day,long CompId)
        {
            //var attendanceStatus = "";
            DateTime date = new DateTime(year, month, day);
            var dayOfWeek = date.DayOfWeek.ToString();

            var checkforWeekDays = dbContextHRM.TblWeeklyHolidays.Where(x=>x.Holidays == dayOfWeek && x.CompanyId == CompId).Count();
            var checkForYearlyHolidays = dbContextHRM.TblYearlyHolidays.Where(x=>x.CompanyId== CompId).ToList();
                if (checkforWeekDays != 0)
                {
                    return  "WeeklyHoliday";
                }
                
            
            
            if(checkForYearlyHolidays != null)
            {
                foreach(var YHolidays in checkForYearlyHolidays)
                {
                   var FromDate=  YHolidays.FromDate;
                    var ToDate = YHolidays.ToDate;
                    if (date >= FromDate && date <= ToDate)
                    {
                        return YHolidays.YearlyHolidaysName;
                    }
                }
            }
            return  new DateTime(year, month, day).Date < DateTime.Now.Date ? "Absent" : null; ;
        }
        public async Task<bool> AddLeaveRequest(EmployeeLeaveRequestModel model)
        {
            TblEmployeeLeaveRequest tblEmployeeLeaveRequestObj = new TblEmployeeLeaveRequest();
            if (model.action == "update") {
                tblEmployeeLeaveRequestObj = dbContextHRM.tblEmployeeLeaveRequest.Where(leave => leave.EmployeeLeaveRequestID == model.employeeLeaveRequestID).FirstOrDefault();
            }
            tblEmployeeLeaveRequestObj.EmployeeID = model.employeeID;
            tblEmployeeLeaveRequestObj.CompanyID = model.companyID;

            DateTime dtStart = Convert.ToDateTime(model.leaveStartDate);
            tblEmployeeLeaveRequestObj.LeaveStartDate = model.leaveStartDate;
            tblEmployeeLeaveRequestObj.LeaveStartYear = dtStart.Year;
            tblEmployeeLeaveRequestObj.LeaveStartMonth = dtStart.Month;
            tblEmployeeLeaveRequestObj.LeaveStartDateNum = dtStart.Day;

            DateTime dtEnd = Convert.ToDateTime(model.leaveEndDate);
            tblEmployeeLeaveRequestObj.LeaveEndDate = model.leaveDuration == "full-day" ? model.leaveEndDate : null;
            tblEmployeeLeaveRequestObj.LeaveEndYear = model.leaveDuration == "full-day" ? dtEnd.Year : 0;
            tblEmployeeLeaveRequestObj.LeaveEndMonth = model.leaveDuration == "full-day" ? dtEnd.Month : 0;
            tblEmployeeLeaveRequestObj.LeaveEndDateNum = model.leaveDuration == "full-day" ? dtEnd.Day : 0;

            tblEmployeeLeaveRequestObj.LeaveDuration = model.leaveDuration;
            tblEmployeeLeaveRequestObj.LeaveType = model.leaveType;
            tblEmployeeLeaveRequestObj.LeaveStatus = "Pending";
            tblEmployeeLeaveRequestObj.LeaveReason = model.leaveReason;
            tblEmployeeLeaveRequestObj.LeaveTimeFrom = model.leaveTimeFrom;
            tblEmployeeLeaveRequestObj.LeaveTimeTo = model.leaveTimeTo;
            tblEmployeeLeaveRequestObj.IsDeleted = false;
            if (model.action != "update") {
                tblEmployeeLeaveRequestObj.CreatedBy = model.createdBy;
                tblEmployeeLeaveRequestObj.CreatedDate = DateTime.Now;
            }
            if (model.action == "update") {
                tblEmployeeLeaveRequestObj.ModifiedBy = model.modifiedBy;
                tblEmployeeLeaveRequestObj.ModifiedDate = DateTime.Now;
            }
            if (model.action == "save") {
                dbContextHRM.tblEmployeeLeaveRequest.Add(tblEmployeeLeaveRequestObj);
            } else {
                dbContextHRM.Update(tblEmployeeLeaveRequestObj);
            }
            dbContextHRM.SaveChanges();
            return true;
        }

        public async Task<List<GetEmployeeLeaveCalendarViewResponse>> GetEmployeeLeaveCalendarView(GetEmployeeLeaveCalendarViewRequest model) {
            List<GetEmployeeLeaveCalendarViewResponse> response = new List<GetEmployeeLeaveCalendarViewResponse>();
            response = (from employeeLeave in dbContextHRM.tblEmployeeAttendance
                        where employeeLeave.IsDeleted == false &&
                        employeeLeave.EmployeeID.Equals(model.employeeID)
                        select new GetEmployeeLeaveCalendarViewResponse {
                            employeeLeaveRequestID = employeeLeave.EmployeeAttendanceID,
                            employeeID = employeeLeave.EmployeeID,
                            companyID = employeeLeave.CompanyID,
                            leaveStartDate = employeeLeave.AttendanceDate,
                            leaveType = employeeLeave.AttendanceStatus,
                            leaveStatus = employeeLeave.ApprovalStatus,
                            leaveReason = employeeLeave.Comment,
                            leaveTimeFrom = employeeLeave.CheckedIn,
                            leaveTimeTo = employeeLeave.CheckedOut,
                            action = "update"
                        }).ToList();
            return response;
        }

        public async Task<GetEmployeeLeaveModel> GetEmployeeLeaveRequest(GetEmployeeLeaveSearchRequest model) {
            int skipCount = model.pageSize * model.pageIndex;
            int takeCount = model.pageSize;
            GetEmployeeLeaveModel obj = new GetEmployeeLeaveModel();
            IQueryable<GetEmployeeLeaveList> employeeLeaveResult;
            DateTime currentDate = DateTime.Now.Date; // Get the current date without the time component
            employeeLeaveResult = (from employee in dbContextHRM.tblEmployee
                                   join leaveReq in dbContextHRM.tblEmployeeAttendance
                                        on employee.EmployeeID equals leaveReq.EmployeeID
                                   where employee.IsDeleted == false
                                       && employee.CompanyID == model.companyId
                                       && (model.fullName == "" || employee.FullName.Contains(model.fullName))
                                       && leaveValues.Contains(leaveReq.AttendanceStatus)
                                       && ((model.filterStatus == "all"
                                             || model.filterStatus == "full-day" && leaveReq.AttendanceStatus.ToLower() == "full-day"
                                             || model.filterStatus == "half-day" && leaveReq.AttendanceStatus.ToLower() == "half-day"
                                             || model.filterStatus == "short-leave" && leaveReq.AttendanceStatus.ToLower() == "short-leave"
                                             || model.filterStatus == "pending" && leaveReq.ApprovalStatus.ToLower() == "pending"
                                             || model.filterStatus == "approved" && leaveReq.ApprovalStatus.ToLower() == "approved"
                                             || model.filterStatus == "rejected" && leaveReq.ApprovalStatus.ToLower() == "rejected"
                                            ))
                                   select new GetEmployeeLeaveList {
                                       employeeId = employee.EmployeeID,
                                       employeeName = employee.FullName,
                                       leaveDate = leaveReq.AttendanceDate,
                                       leaveTime = leaveReq.CheckedIn + " - " + leaveReq.CheckedOut,
                                       leaveType = leaveReq.AttendanceStatus,
                                       leaveDurationType = leaveReq.AttendanceStatus,
                                       employeeLeaveRequestId = leaveReq.EmployeeAttendanceID,
                                       status = leaveReq.ApprovalStatus,
                                       reason = leaveReq.Comment
                                   });
            obj.TotalRecords = employeeLeaveResult.Count();
            //Page size -1 is for all records
            if (model.pageSize == -1)
                obj.EmployeeLeaveList = employeeLeaveResult.ToList();
            else
                obj.EmployeeLeaveList = employeeLeaveResult.Skip(skipCount).Take(takeCount).ToList();

            return obj;
        }

        public async Task<GetEmployeeAttendanceModel> GetEmployeeAttendanceRecord(GetEmployeeAttendanceSearchRequest model) {
            int skipCount = model.pageSize * model.pageIndex;
            int takeCount = model.pageSize;
            GetEmployeeAttendanceModel obj = new GetEmployeeAttendanceModel();
            IQueryable<GetEmployeeAttendanceList> employeeAttendenceResult;
            DateTime currentDate = DateTime.Now.Date; // Get the current date without the time component
            employeeAttendenceResult = (from employee in dbContextHRM.tblEmployee
                                        join leaveRequest in dbContextHRM.tblEmployeeAttendance
                                        on new { EmployeeID = employee.EmployeeID, AttendanceDate = currentDate } equals new { EmployeeID = leaveRequest.EmployeeID, AttendanceDate = leaveRequest.AttendanceDate.Value.Date } into attendanceJoin
                                        from attendance in attendanceJoin.DefaultIfEmpty()
                                        where employee.IsDeleted == false
                                            && employee.CompanyID == model.companyId
                                            && (model.employmentType == "" || employee.EmploymentType.Contains(model.employmentType))
                                            && (model.location == "" || employee.Office.Contains(model.location))
                                            && (model.department == "" || employee.Department.Contains(model.department))
                                            && (model.position == "" || employee.Position.Contains(model.position))
                                            && (model.fullName == "" || employee.FullName.Contains(model.fullName))
                                            && (
                                            (model.filterStatus == "all"
                                             || model.filterStatus == "present" && attendance.AttendanceStatus.ToLower() == "present"
                                             || model.filterStatus == "absent" && attendance == null
                                             || model.filterStatus == "leave" && leaveValues.Contains(attendance.AttendanceStatus)
                                            ))
                                        //&& (attendance == null || attendance.AttendanceDate.Value.Date == currentDate)
                                        select new GetEmployeeAttendanceList {
                                            employeeId = employee.EmployeeID,
                                            employeeName = employee.FullName,
                                            attendanceDate = attendance == null ? currentDate : attendance.AttendanceDate,
                                            checkedIn = attendance == null ? null : attendance.CheckedIn,
                                            checkedOut = attendance == null ? null : attendance.CheckedOut,
                                            employeeAttendenceId = attendance == null ? 0 : attendance.EmployeeAttendanceID,
                                            status = attendance == null ? "Absent" : attendance.AttendanceStatus,
                                            approvalStatus = attendance.ApprovalStatus,
                                            timeWorked = attendance == null ? "" : attendance.TimeWorked,
                                            comment = attendance.Comment
                                        });
            obj.TotalRecords = employeeAttendenceResult.Count();
            //Page size -1 is for all records
            if (model.pageSize == -1)
                obj.EmployeeAttendenceList = employeeAttendenceResult.ToList();
            else
                obj.EmployeeAttendenceList = employeeAttendenceResult.Skip(skipCount).Take(takeCount).OrderBy(x => x.employeeName).ToList();

            return obj;
        }

        // Helper method to calculate the worked time duration
        TimeSpan CalculateWorkedTime(string checkedIn, string checkedOut) {
            DateTime checkInTime = new DateTime();
            if (!string.IsNullOrEmpty(checkedIn)) {
                checkInTime = DateTime.ParseExact(checkedIn, "hh:mm tt", CultureInfo.InvariantCulture);
            }
            TimeSpan Hours = TimeSpan.Zero;
            if (!string.IsNullOrEmpty(checkedOut)) {
                DateTime checkOutTime = DateTime.ParseExact(checkedOut, "hh:mm tt", CultureInfo.InvariantCulture);
                Hours = checkOutTime - checkInTime;
            }
            return Hours;
        }

        public async Task<GetEmployeeAttendanceModel> GetEmployeeAttendanceRecordByEmployeeId(GetEmployeeAttendanceRecordByEmployeeId model) {
            int skipCount = model.pageSize * model.pageIndex;
            int takeCount = model.pageSize;
            GetEmployeeAttendanceModel obj = new GetEmployeeAttendanceModel();
            List<GetEmployeeAttendanceList> attendanceList = new List<GetEmployeeAttendanceList>();
            DateTime? joiningDate = dbContextHRM.tblEmployee.
                Where(x => x.EmployeeID == model.employeeId).
                Select(x => x.HireDate).
                FirstOrDefault();
            DateTime? currentDate = dbContextHRM.tblEmployeeAttendance.
                Where(emp => emp.EmployeeID == model.employeeId).
                Select(emp => emp.AttendanceDate).
                Max();
            for (DateTime date = joiningDate.Value.Date; date <= currentDate; date = date.AddDays(1)) {
                var employeeAttendance = dbContextHRM.tblEmployeeAttendance.
                    Where(x => x.EmployeeID == model.employeeId && x.AttendanceDate == date).FirstOrDefault();
                if (employeeAttendance == null) {
                    GetEmployeeAttendanceList attendanceItem = new GetEmployeeAttendanceList {
                        employeeId = model.employeeId,
                        attendanceDate = date,
                        checkedIn = null,
                        checkedOut = null,
                        employeeAttendenceId = 0,
                        status = "Absent",
                        approvalStatus = "",
                        comment = "",
                        timeWorked = ""
                    };

                    attendanceList.Add(attendanceItem);
                } else {
                    GetEmployeeAttendanceList attendanceItem = new GetEmployeeAttendanceList {
                        employeeId = model.employeeId,
                        attendanceDate = date,
                        checkedIn = employeeAttendance.CheckedIn,
                        checkedOut = employeeAttendance.CheckedOut,
                        employeeAttendenceId = employeeAttendance.EmployeeAttendanceID,
                        status = employeeAttendance == null ? "Absent" : employeeAttendance.AttendanceStatus,
                        approvalStatus = employeeAttendance.ApprovalStatus,
                        timeWorked = employeeAttendance == null ? "" : employeeAttendance.TimeWorked,
                        comment = employeeAttendance.Comment,
                    };
                    attendanceList.Add(attendanceItem);
                }
            }
            IQueryable<GetEmployeeAttendanceList> employeeAttendenceResult = attendanceList.AsQueryable();
            obj.TotalRecords = employeeAttendenceResult.ToList().Count();
            //Page size -1 is for all records
            if (model.pageSize == -1)
                obj.EmployeeAttendenceList = employeeAttendenceResult.OrderByDescending(x => x.attendanceDate).ToList();
            else
                obj.EmployeeAttendenceList = employeeAttendenceResult.Skip(skipCount).Take(takeCount).OrderByDescending(x => x.attendanceDate).ToList();

            return obj;
        }

        public async Task<GetEmployeeLeaveHistoryModel> GetEmployeeLeaveRecordByEmployeeId(GetEmployeeLeaveRecordByEmployeeId model) {
            int skipCount = model.pageSize * model.pageIndex;
            int takeCount = model.pageSize;
            GetEmployeeLeaveHistoryModel obj = new GetEmployeeLeaveHistoryModel();
            List<GetEmployeeLeaveHistoryList> LeaveList = new List<GetEmployeeLeaveHistoryList>();
            DateTime? companyRegisterDate = dbContextHRM.tblCompanyDetail.Where(x => x.CompanyID == model.companyId).Select(x => x.CreatedDate).FirstOrDefault();
            DateTime? currentDate = DateTime.Now.Date;
            for (DateTime date = companyRegisterDate.Value.Date; date <= currentDate; date = date.AddDays(1)) {
                var employeeLeave = dbContextHRM.tblEmployeeLeaveRequest.
                    Where(x => x.EmployeeID == model.employeeId && x.CreatedDate.Value.Date == date.Date).FirstOrDefault();
                if (employeeLeave == null) {
                    GetEmployeeLeaveHistoryList LeaveItem = new GetEmployeeLeaveHistoryList {
                        employeeId = model.employeeId,
                        leaveDate = date.ToString(),
                        leaveTime = "",
                        leaveType = "",
                        leaveDurationType = "",
                        reason = "",
                        status = "",
                        employeeLeaveRequestId = 0,
                    };

                    LeaveList.Add(LeaveItem);
                } else {
                    GetEmployeeLeaveHistoryList attendanceItem = new GetEmployeeLeaveHistoryList {
                        employeeId = employeeLeave.EmployeeID,
                        leaveDate = employeeLeave == null ? currentDate.ToString() :
                                       (employeeLeave.LeaveStartDate.HasValue ?
                                       (employeeLeave.LeaveEndDate.HasValue ?
                                       employeeLeave.LeaveStartDate.Value.ToString("MMMM dd, yyyy") + " - " + employeeLeave.LeaveEndDate.Value.ToString("MMMM dd, yyyy") :
                                       employeeLeave.LeaveStartDate.Value.ToString("MMMM dd, yyyy")) :
                                       ""),
                        leaveTime = !string.IsNullOrEmpty(employeeLeave.LeaveTimeFrom) && !string.IsNullOrEmpty(employeeLeave.LeaveTimeTo) ? employeeLeave.LeaveTimeFrom + "-" + employeeLeave.LeaveTimeTo : "",
                        leaveType = employeeLeave == null ? null : employeeLeave.LeaveType,
                        leaveDurationType = employeeLeave == null ? null : employeeLeave.LeaveDuration,
                        employeeLeaveRequestId = employeeLeave == null ? 0 : employeeLeave.EmployeeLeaveRequestID,
                        status = employeeLeave == null ? "Pending" : employeeLeave.LeaveStatus,
                        reason = employeeLeave == null ? "" : employeeLeave.LeaveReason
                    };
                    LeaveList.Add(attendanceItem);
                }
            }
            IQueryable<GetEmployeeLeaveHistoryList> employeeLeaveResult = LeaveList.AsQueryable();
            obj.TotalRecords = employeeLeaveResult.ToList().Count();
            //Page size -1 is for all records
            if (model.pageSize == -1)
                obj.EmployeeLeaveList = employeeLeaveResult.OrderByDescending(x => x.leaveDate).ToList();
            else
                obj.EmployeeLeaveList = employeeLeaveResult.Skip(skipCount).Take(takeCount).OrderByDescending(x => x.leaveDate).ToList();

            return obj;
        }

        public async Task<bool> UpdateEmployeeLeaveStatus(UpdateEmployeeLeaveStatusRqst model) {
            var employeeLeave = dbContextHRM.tblEmployeeAttendance.
                Where(x => x.EmployeeAttendanceID == model.employeeLeaveRequestId).FirstOrDefault();
            if (employeeLeave != null) {
                if (model.status == "cancel") {
                    dbContextHRM.tblEmployeeAttendance.Remove(employeeLeave);
                } else {
                    employeeLeave.ApprovalStatus = model.status;
                }
                dbContextHRM.SaveChanges();
                return true;
            }
            return false;
        }

        public async Task<bool> DeleteAttendanceLeaveRecord(DeleteAttendanceLeaveRequest objReq) {
            var employeeLeave = dbContextHRM.tblEmployeeAttendance.
            Where(x => x.EmployeeAttendanceID == objReq.id).FirstOrDefault();
            if (employeeLeave != null) {
                dbContextHRM.Remove(employeeLeave);
                dbContextHRM.SaveChanges();
                return true;
            }
            return false;
        }

        public async Task<CompanyInfoByEmployeeResult> GetOfficeInfoByEmployee(long? employeeId) {
            CompanyInfoByEmployeeResult model = new CompanyInfoByEmployeeResult();
            var employeeLeave = (from employeeOffice in dbContextHRM.tblOfficeLocation
                                 join employee in dbContextHRM.tblEmployee
                                        on employeeOffice.OfficeId.ToString() equals employee.Office
                                 where employee.IsDeleted == false && employee.EmployeeID == employeeId
                                 select new CompanyInfoByEmployeeResult {
                                     latitude = employeeOffice.Latitude,
                                     longitude = employeeOffice.Longitude,
                                     officeId = employeeOffice.OfficeId,
                                     officeLocationName = employeeOffice.OfficeLocationName
                                 }).FirstOrDefault();
            model = employeeLeave;
            return model;
        }

        public async Task<TodayEmployeeAttendanceResult> GetTodayEmployeeAttendance(long? employeeId) {
            TodayEmployeeAttendanceResult model = new TodayEmployeeAttendanceResult();
            var employeeLeave = (from att in dbContextHRM.tblEmployeeAttendance
                                 where att.IsDeleted == false && att.EmployeeID == employeeId
                                 && att.AttendanceDate.Value.Date == DateTime.Now.Date
                                 select new TodayEmployeeAttendanceResult {
                                     checkedIn = string.IsNullOrEmpty(att.CheckedIn) ? "" : att.CheckedIn,
                                     checkedOut = string.IsNullOrEmpty(att.CheckedOut) ? "" : att.CheckedOut,
                                     employeeAttendanceID = att.EmployeeAttendanceID
                                 }).FirstOrDefault();
            model = employeeLeave;
            return model;
        }
        #region Weekly Holidays
        public async Task<SaveResponseMessage> SaveWeeklyHolidays(WeeklyHolidaysSaveRequest objReq) {
            SaveResponseMessage saveObject = new SaveResponseMessage();
            if (dbContextHRM.TblWeeklyHolidays.Where(holidays => holidays.CompanyId == objReq.companyId).Any()) {
                var tblWeeklyHolidaysObjRemove = dbContextHRM.TblWeeklyHolidays.ToList();
                dbContextHRM.TblWeeklyHolidays.RemoveRange(tblWeeklyHolidaysObjRemove);
                // Save changes to the database
                dbContextHRM.SaveChanges();
            }
            bool isSave = SaveToDatabaseWeeklyHolidays(objReq);
            if (isSave) {
                saveObject.msg = "Record saved successfully";
                saveObject.status = true;
            }
            return saveObject;
        }

        private bool SaveToDatabaseWeeklyHolidays(WeeklyHolidaysSaveRequest objReq) {
            try {
                foreach (var holiday in objReq.holidays) {
                    TblWeeklyHolidays tblWeeklyHolidaysObj = new TblWeeklyHolidays {
                        Holidays = holiday.holidays,
                        CompanyId = objReq.companyId,
                        IsDeleted = false,
                        CreatedBy = objReq.companyId.ToString(),
                        CreatedDate = DateTime.Now
                    };

                    dbContextHRM.TblWeeklyHolidays.Add(tblWeeklyHolidaysObj);
                    dbContextHRM.SaveChanges();
                }
                return true;
            } catch (Exception ex) {
                // Handle any exceptions that might occur during database operation
                // You may want to log the exception details for debugging.
                // Return false or throw an exception as needed.
                return false;
            }
        }

        public async Task<List<GetWeeklyHolidaysListRequest>> GetWeeklyHolidaysByCompId(long compId) {
            var Data = dbContextHRM.TblWeeklyHolidays.
                Where(wh => wh.CompanyId == compId).
                Select(wh => new GetWeeklyHolidaysListRequest {
                WeeklyholidaysId = wh.WeeklyholidaysId,
                Holidays = wh.Holidays,
                CompanyId = wh.CompanyId,
                IsDeleted = wh.IsDeleted,
                CreatedBy = wh.CreatedBy,
                CreatedDate = wh.CreatedDate,
                ModifiedBy = wh.ModifiedBy,
                ModifiedDate = wh.ModifiedDate,

            }).ToList();

            return Data;

        }
        public async Task<bool> DeleteWeeklyHolidayByCampId(long CompId) {
            var result = dbContextHRM.TblWeeklyHolidays.Where(x => x.WeeklyholidaysId == CompId).FirstOrDefault();
            if (result != null) {
                dbContextHRM.Remove(result);
                dbContextHRM.SaveChanges();
                return true;
            }
            return false;
        }
        #endregion
        #region Yearly Holidays
        public async Task<SaveResponseMessage> SaveYearlyHolidays(YearlyHolidaysRequest objReq) {
            SaveResponseMessage saveObject = new SaveResponseMessage();
            if ((dbContextHRM.TblYearlyHolidays.Any(holidays =>   holidays.YearlyHolidaysName == objReq.yearlyHolidaysName)) && (objReq.yearlyHolidaysId < 1))
            {
                saveObject.status = false;
                saveObject.msg = "holidays already exists.";
            } else {
                TblYearlyHolidays TblYearlyHolidaysObj = dbContextHRM.TblYearlyHolidays.FirstOrDefault(holidays => holidays.YearlyHolidaysId == objReq.yearlyHolidaysId);

                if (TblYearlyHolidaysObj == null) // if null than assign empty model to fill table
                {
                    TblYearlyHolidaysObj = new TblYearlyHolidays();
                }
                TblYearlyHolidaysObj.YearlyHolidaysName = objReq.yearlyHolidaysName;
                TblYearlyHolidaysObj.FromDate = objReq.fromDate;
                TblYearlyHolidaysObj.ToDate = objReq.toDate;
                TblYearlyHolidaysObj.NumberOfDays = objReq.numberOfDays;
                TblYearlyHolidaysObj.CompanyId = objReq.companyId;
                TblYearlyHolidaysObj.IsDeleted = false;
                //tblWeeklyHolidaysObj.IsDeleted = false;
                if (objReq.yearlyHolidaysId < 1) //if id not zero than add otherwise update
                {
                    TblYearlyHolidaysObj.IsDeleted = false;
                    TblYearlyHolidaysObj.CreatedBy = objReq.companyId.ToString();
                    TblYearlyHolidaysObj.CreatedDate = DateTime.Now;
                    dbContextHRM.TblYearlyHolidays.Add(TblYearlyHolidaysObj);
                    saveObject.msg = "Yearly Holidays Added successfully!";
                } else {
                    TblYearlyHolidaysObj.ModifiedBy = objReq.companyId.ToString();
                    TblYearlyHolidaysObj.ModifiedDate = DateTime.Now;
                    saveObject.msg = "Yearly Holidays Updated successfully!";
                }

                try {
                    dbContextHRM.SaveChanges();
                    saveObject.status = true;

                } catch (Exception ex) {
                    // Handle the exception and set appropriate status and message
                    saveObject.status = false;
                    saveObject.msg = "An error occurred while saving the Weekly Holidays.";
                    // You might also want to log the exception for debugging purposes
                }


            }
            return saveObject;
        }
        public async Task<GetYearlyHolidaysModel> GetYearlyHolidays(SearchYearlyHolidaysGetRequest model) {
            int skipCount = model.pageSize * model.pageIndex;
            int takeCount = model.pageSize;
            GetYearlyHolidaysModel obj = new GetYearlyHolidaysModel();
            IQueryable<GetYearlyHolidaysListRequest> YearlyHolidaysResponse;
            YearlyHolidaysResponse =  dbContextHRM.TblYearlyHolidays.Where(yh=>yh.CompanyId == model.companyId).Select(yh => new GetYearlyHolidaysListRequest
            {
                    yearlyHolidaysId = yh.YearlyHolidaysId,
                    yearlyHolidaysName = yh.YearlyHolidaysName,
                    fromDate = yh.FromDate,
                    toDate = yh.ToDate,
                    numberOfDays = yh.NumberOfDays,
                    companyId = yh.CompanyId,
                    isDeleted = yh.IsDeleted,
                    createdBy = yh.CreatedBy,
                    createdDate = yh.CreatedDate,
                    modifiedBy = yh.ModifiedBy,
                    modifiedDate = yh.ModifiedDate,
                    editableMode = false,

            }).AsQueryable();
            if (!string.IsNullOrEmpty(model.searchYearlyHolidays)) {
                YearlyHolidaysResponse.Where(x => model.searchYearlyHolidays.Contains(x.yearlyHolidaysName)).AsQueryable();
            }
            obj.totalRecords = YearlyHolidaysResponse.Count();
            if (model.pageSize == -1)
                obj.YearlyHolidaysList = YearlyHolidaysResponse.ToList();
            else
                obj.YearlyHolidaysList = YearlyHolidaysResponse.Skip(skipCount).Take(takeCount).ToList();

            return obj;

        }
        public async Task<bool> DeletYearlyHolidayByCampId(long CompId) {
            var result = dbContextHRM.TblYearlyHolidays.Where(x => x.YearlyHolidaysId == CompId).FirstOrDefault();
            if (result != null) {
                dbContextHRM.Remove(result);
                dbContextHRM.SaveChanges();
                return true;
            }
            return false;
        }
        #endregion
    }
}