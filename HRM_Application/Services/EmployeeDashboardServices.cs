using HRM_Application.Interfaces;
using HRM_Domain.Model;
using HRM_Infrastructure.HRMDataBaseContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM_Application.Services {
    public class EmployeeDashboardServices : IEmployeeDashboardServices
        {
        private readonly HRMContexts dbContextHRM;
        //private readonly IEmployeeDashboardServices _IEmployeeDashboardServices;

        public EmployeeDashboardServices(HRMContexts context ) {
            dbContextHRM = context;
            //_IEmployeeDashboardServices = iEmployeeDashboardServices;
        }
        public async Task<EmployeeDashboardResponse> GetEmployeeDashboardData(EmployeeDashboardRequest model) {
            EmployeeDashboardResponse employeeDashboardResponse = new EmployeeDashboardResponse();
            employeeDashboardResponse.employeeProfileResponse = (from employeeProfile in dbContextHRM.tblEmployee
                                                                 join officeObj in dbContextHRM.tblOfficeLocation
                                                                 on employeeProfile.Office equals officeObj.OfficeId.ToString() into officeGroup
                                                                 from officeObj in officeGroup.DefaultIfEmpty()
                                                                 join deptObj in dbContextHRM.tblDepartment
                                                                 on employeeProfile.Department equals deptObj.DepartmentId.ToString() into deptGroup
                                                                 from deptObj in deptGroup.DefaultIfEmpty()
                                                                 join teamObj in dbContextHRM.tblTeam
                                                                 on employeeProfile.Team equals teamObj.TeamId.ToString() into teamGroup
                                                                 from teamObj in teamGroup.DefaultIfEmpty()
                                                                 join positionObj in dbContextHRM.tblPosition
                                                                 on employeeProfile.Position equals positionObj.PositionId.ToString() into posGroup
                                                                 from positionObj in posGroup.DefaultIfEmpty()
                                                                 join shiftObj in dbContextHRM.tblShiftSetup
                                                                 on employeeProfile.ShiftId equals shiftObj.ShiftId into shiftGroup
                                                                 from shiftObj in shiftGroup.DefaultIfEmpty()
                                                                 where employeeProfile.IsDeleted == false
                                                                 && employeeProfile.EmployeeID == model.employeeId
                                                                 select new EmployeeProfileResponse {
                                                                     employeeName=employeeProfile.FullName,
                                                                     email=employeeProfile.Email,
                                                                     designation=positionObj.PositionName,
                                                                     officeLocation=officeObj.OfficeLocationName,
                                                                     phoneNumber=employeeProfile.PhoneNumber,
                                                                     departmentName=deptObj.DepartmentName,
                                                                     teamName=teamObj.TeamName,
                                                                     employeePicture=employeeProfile.EmployeePhoto,
                                                                     empPhotoType = employeeProfile.PhotoType,
                                                                     workingPolicyTimeFrom = shiftObj.TimeFrom,
                                                                     workingPolicyTimeTo = shiftObj.TimeTo,
                                                                 }).FirstOrDefault();
            string workingPolicyTimeFrom = employeeDashboardResponse.employeeProfileResponse.workingPolicyTimeFrom;
            string workingPolicyTimeTo = employeeDashboardResponse.employeeProfileResponse.workingPolicyTimeTo;
            EmployeeTotalLeaves employeeTotalLeaves =new EmployeeTotalLeaves();
            employeeTotalLeaves.totalleaves = 0;
            employeeTotalLeaves.remainingleaves = 0;
            employeeTotalLeaves.consumedleaves = 0;

            employeeTotalLeaves.totalleaves = dbContextHRM.tblEmployee.
                Where(x => x.IsDeleted == false && x.EmployeeID == model.employeeId).Select(x => x.NumberOfLeavesAllowed).
                FirstOrDefault();
            
            employeeTotalLeaves.consumedleaves = dbContextHRM.tblEmployeeAttendance.
                Where(x => x.IsDeleted == false && x.EmployeeID == model.employeeId
                && x.AttendanceStatus.ToLower() != "present" && x.ApprovalStatus.ToLower() != "Approved").
                Count();

            employeeTotalLeaves.remainingleaves = employeeTotalLeaves.totalleaves - employeeTotalLeaves.consumedleaves;

            employeeDashboardResponse.employeeTotalLeaves = employeeTotalLeaves;

            EmployeeCurrentStatus objStatus = new EmployeeCurrentStatus();
            
            objStatus.currentStatus = "red";
            if (dbContextHRM.tblEmployeeAttendance.
                Any(att => att.EmployeeID == model.employeeId && att.AttendanceDate.Value.Date == DateTime.Now.Date && att.CheckedIn != null))
                objStatus.currentStatus = "green";
            
            var currentAttendanceResult = dbContextHRM.tblEmployeeAttendance.
                Where(att => att.EmployeeID == model.employeeId && att.AttendanceDate.Value.Date == DateTime.Now.Date).Select(x => x.CheckedIn).
                FirstOrDefault();
            objStatus.checkInTime = currentAttendanceResult != null ? currentAttendanceResult : "";

            objStatus.workingPolicy = workingPolicyTimeFrom + "-" + workingPolicyTimeTo;

            // Parse time strings to DateTime objects
            DateTime startTime = DateTime.Parse(workingPolicyTimeFrom);
            DateTime endTime = DateTime.Parse(workingPolicyTimeTo);

            // Calculate the time difference
            TimeSpan timeDifference = endTime - startTime;
            string totalExpectedHours = "";

            // Check if the difference is less than 1 hour
            if (timeDifference.TotalMinutes < 60)
            {
                totalExpectedHours = timeDifference.TotalMinutes.ToString() +" m";
            }
            else
            {
                // Display in HH:mm format if it's 1 hour or more
                totalExpectedHours = timeDifference.ToString(@"hh\:mm") + " h";
            }

            string earnedHour = "0";
            if (currentAttendanceResult != null)
            {
                DateTime earnedDateTime = DateTime.Parse(currentAttendanceResult);
                DateTime currentTime = DateTime.Now;

                TimeSpan earnedTimeDifference = currentTime - earnedDateTime;
                if (earnedTimeDifference.TotalMinutes < 60)
                {
                    earnedHour = earnedTimeDifference.TotalMinutes.ToString() + " m";
                }
                else
                {
                    // Display in HH:mm format if it's 1 hour or more
                    earnedHour = earnedTimeDifference.ToString(@"hh\:mm") + " h";
                }

            }
            objStatus.expectedEarnHour = earnedHour + "/" + totalExpectedHours;
            employeeDashboardResponse.employeeCurrentStatus = objStatus;

            List<EmployeeWeekAttendanceStatus> lstWeekStatus = new List<EmployeeWeekAttendanceStatus>();
            // Calculate start and end dates for the current week
            DateTime currentDate = DateTime.Now;
            DateTime startOfWeek = currentDate.AddDays(-(int)currentDate.DayOfWeek);
            DateTime endOfWeek = startOfWeek.AddDays(6);

            // Iterate through each day of the week
            for (DateTime date = startOfWeek; date <= endOfWeek; date = date.AddDays(1))
            {
                EmployeeWeekAttendanceStatus objWeekStatus = new EmployeeWeekAttendanceStatus();
                var rst= dbContextHRM.tblEmployeeAttendance.FirstOrDefault(record => record.AttendanceDate.Value.Date == date.Date);
                var checkforWeekDays = dbContextHRM.TblWeeklyHolidays.Where(x => x.Holidays == date.DayOfWeek.ToString() && x.CompanyId == model.companyId).Count();
                objWeekStatus.attendanceStatus = "";
                if (checkforWeekDays <= 0 && rst==null)
                {
                    objWeekStatus.attendanceStatus = "Absent";
                }
                objWeekStatus.day = date.DayOfWeek.ToString();
                objWeekStatus.attendanceTime = "";
                if (rst != null)
                {
                    string attendanceStatus = (rst != null) ? rst.AttendanceStatus : "Absent";
                    objWeekStatus.attendanceStatus = attendanceStatus;
                    objWeekStatus.day = date.DayOfWeek.ToString();
                    objWeekStatus.attendanceTime = rst.CheckedIn + "-" + rst.CheckedOut;
                }
                lstWeekStatus.Add(objWeekStatus);
            }
            employeeDashboardResponse.employeeWeekAttendanceStatus = lstWeekStatus;

            return employeeDashboardResponse;
        }
    }
}
