using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM_Domain.Model {
    public class EmployeeDashboardResponse {
        public EmployeeProfileResponse employeeProfileResponse { get; set; }
        public EmployeeTotalLeaves employeeTotalLeaves { get; set; }
        public EmployeeCurrentStatus employeeCurrentStatus { get; set; }
        public List<EmployeeWeekAttendanceStatus> employeeWeekAttendanceStatus { get; set; }
    }
    public class EmployeeProfileResponse {
        public string employeeName { get; set; }
        public string designation { get; set; }
        public string email { get; set; }
        public string phoneNumber { get; set; }
        public string officeLocation { get; set; }
        public string departmentName { get; set; }
        public string teamName { get; set; }
        public string employeePicture { get; set; }
        public string empPhotoType { get; set; }
        public string workingPolicyTimeFrom { get; set; }
        public string workingPolicyTimeTo { get; set; }
    }
    public class EmployeeTotalLeaves {
        public int? totalleaves { get; set; }
        public int? consumedleaves { get; set; }
        public int? remainingleaves { get; set; }
    }
    public class EmployeeCurrentStatus
    {
        public string currentStatus { get; set; }
        public string checkInTime { get; set; }
        public string workingPolicy { get; set; }
        public string expectedEarnHour { get; set; }
    }

    public class EmployeeWeekAttendanceStatus
    {
        public string attendanceStatus { get; set; }
        public string attendanceTime { get; set; }
        public string day { get; set; }
    }
    public class EmployeeDashboardRequest {
        public long employeeId { get; set; }
        public long companyId { get; set; }
    }
}
