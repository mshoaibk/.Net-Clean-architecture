using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM_Domain.Model
{
    public class EmployeeSaveRequest
    {
        public long employeeID { get; set; }
        public long companyID { get; set; }
        public string fullName { get; set; }
        public string email { get; set; }
        public string gender { get; set; }
        public string employmentType { get; set; }
        public string position { get; set; }
        public string team { get; set; }
        public string department { get; set; }
        public string office { get; set; }
        public string supervisor { get; set; }
        public string hireDate { get; set; }
        public string probationPeriod { get; set; }
        public string contractEnd { get; set; }
        public string workType { get; set; }
        public string workingSchedule { get; set; }
        public string paidVacation { get; set; }
        public bool isDeleted { get; set; }
        public string createdBy { get; set; }
        public DateTime createdDate { get; set; }
        public string modifiedBy { get; set; }
        public DateTime modifiedDate { get; set; }
        public string action { get; set; }
        public string userName { get; set; }
        public IFormFile employeePhoto { get; set; }
        public string employeePhotoName { get; set; }
        public string PhotoType { get; set; }
        public string phoneNumber { get; set; }
        public string experience { get; set; }
        public string qualification { get; set; }
        public long employeeSalaryID { get; set; }
        public string payType { get; set; }
        public decimal? monthlyPay { get; set; }
        public decimal? hourlyPay { get; set; }
        public decimal? hoursWorked { get; set; }
        public decimal? dailyPay { get; set; }
        public decimal? weeklyPay { get; set; }
        public decimal? weeksWorked { get; set; }
        public string currency { get; set; }
        public int numberOfLeavesAllowed { get; set; }
        public long shiftId { get; set; }
        public string supervisedBy { get; set; }
        public string teamMember { get; set; }
        public decimal? appliedTaxPercentage { get; set; }
    }

    public class SearchGetEmployeeListRequest
    {
        public string fullName { get; set; }
        public string employmentType { get; set; }
        public string department { get; set; }
        public string team { get; set; }
        public int pageIndex { get; set; }
        public int pageSize { get; set; }
    }

    public class GetEmployeeListModel
    {
        public List<GetEmployeeResponse> EmployeeList { get; set; }
        public Int32 TotalRecords { get; set; }
    }
    public class GetEmployeeResponse
    {
        public long employeeID { get; set; }
        public long companyID { get; set; }
        public string fullName { get; set; }
        public string email { get; set; }
        public string gender { get; set; }
        public string employmentType { get; set; }
        public string position { get; set; }
        public string team { get; set; }
        public string department { get; set; }
        public string office { get; set; }
        public string supervisor { get; set; }
        public string hireDate { get; set; }
        public string probationPeriod { get; set; }
        public string contractEnd { get; set; }
        public string workType { get; set; }
        public string workingSchedule { get; set; }
        public string paidVacation { get; set; }
        public string createdBy { get; set; }
        public DateTime? createdDate { get; set; }
        public string modifiedBy { get; set; }
        public DateTime? modifiedDate { get; set; }
        public string UserID { get; set; }
        public string employeePhoto { get; set; }
        public string photoType { get; set; }
        public bool? IsActivated { get; set; }
    }

    public class SearchGetEmployeeBasicDetailRequest
    {
        public long companyID { get; set; }
        public string employmentType { get; set; }
        public string location { get; set; }
        public string department { get; set; }
        public string position { get; set; }
        public int pageIndex { get; set; }
        public int pageSize { get; set; }
        public string employeeName { get; set; }
        public string employmentStatus { get; set; }
    }

    public class GetEmployeeBasicDetailModel
    {
        public List<GetEmployeeBasicDetailResponse> EmployeeList { get; set; }
        public Int32 TotalRecords { get; set; }
    }
    public class GetEmployeeBasicDetailResponse
    {
        public long employeeID { get; set; }
        public long companyID { get; set; }
        public string fullName { get; set; }
        public string email { get; set; }
        public string gender { get; set; }
        public string employmentType { get; set; }
        public string position { get; set; }
        public string team { get; set; }
        public string department { get; set; }
        public string office { get; set; }
        public string supervisor { get; set; }
        public string hireDate { get; set; }
        public string probationPeriod { get; set; }
        public string contractEnd { get; set; }
        public string workType { get; set; }
        public string workingSchedule { get; set; }
        public string paidVacation { get; set; }
        public string createdBy { get; set; }
        public DateTime? createdDate { get; set; }
        public string modifiedBy { get; set; }
        public DateTime? modifiedDate { get; set; }
        public string UserID { get; set; }
        public string employeePhoto { get; set; }
        public string photoType { get; set; }
        public bool? IsActivated { get; set; }
    }

    public class ShowCompleteEmployeeDetailRequest
    {
        public long employeeID { get; set; }
        public string userID { get; set; }
        public ShowCompleteEmployeeDetailRequest()
        {
            this.userID = "";
            this.employeeID = 0;
        }
    }

    public class ShowCompleteEmployeeDetailRequestResponse
    {
        public long employeeID { get; set; }
        public long companyID { get; set; }
        public string fullName { get; set; }
        public string email { get; set; }
        public string gender { get; set; }
        public string employmentType { get; set; }
        public string position { get; set; }
        public string team { get; set; }
        public string department { get; set; }
        public string office { get; set; }
        public long? positionId { get; set; }
        public long? teamId { get; set; }
        public long? departmentId { get; set; }
        public long? officeId { get; set; }
        public string supervisor { get; set; }
        public long? supervisorId { get; set; }
        public string hireDate { get; set; }
        public string probationPeriod { get; set; }
        public string contractEnd { get; set; }
        public string workType { get; set; }
        public string workingSchedule { get; set; }
        public string paidVacation { get; set; }
        public string createdBy { get; set; }
        public DateTime? createdDate { get; set; }
        public string modifiedBy { get; set; }
        public DateTime? modifiedDate { get; set; }
        public string UserID { get; set; }
        public string employeePhoto { get; set; }
        public string photoType { get; set; }
        public bool? IsActivated { get; set; }
        public string experience { get; set; }
        public string qualification { get; set; }
        public string phoneNumber { get; set; }
        public int? numberOfLeavesAllowed { get; set; }
        public long? shiftId { get; set; }
        public List<long?> supervisorDDLResponse { get; set; }
        public List<long?> teamMemberDDLResponse { get; set; }
        public GetEmployeeSalariesList getEmployeeSalariesList { get; set; }
    }

    public class SaveEmployeeSalaryRequestModel
    {
        public long employeeSalaryID { get; set; }
        public long employeeID { get; set; }
        public string payType { get; set; }
        public decimal? monthlyPay { get; set; }
        public decimal? hourlyPay { get; set; }
        public decimal? hoursWorked { get; set; }
        public decimal? dailyPay { get; set; }
        public decimal? weeklyPay { get; set; }
        public decimal? weeksWorked { get; set; }
        public string currency { get; set; }
        public string createdBy { get; set; }
        public string modifiedBy { get; set; }
        public decimal? appliedTaxPercentage { get; set; }
        public string action { get; set; }
    }
     public class GenerateEmployeeSalaryRequestModel
    {
        public int companyId { get; set; }
        public int[] employeeIds { get; set; }
        public int searchedMonth { get; set; }
        public int searchedYear { get; set; }
    }
    public class SaveEmployeeSalarySlipRequestModel
    {
        public long EmployeeSalarySlipID { get; set; }
        public long employeeSalaryID { get; set; }
        public long employeeID { get; set; }
        public string month { get; set; }
        public DateTime paidOn { get; set; }
        public decimal? basicPay { get; set; }
        public decimal? allowances { get; set; }
        public decimal? deductions { get; set; }
        public decimal? netSalary { get; set; }
        public string createdBy { get; set; }
        public string modifiedBy { get; set; }
        public string action { get; set; }
        public int year { get; set; }
        public string currency { get; set; }
    }

    public class GetEmployeeSalaryByEmployeeIdSlipModelRequest
    {
        public long employeeID { get; set; }
        public string month { get; set; }
        public int year { get; set; }
        public int pageIndex { get; set; }
        public int pageSize { get; set; }
    }

    public class GetEmployeeSalaryByEmployeeIdSlipModel
    {
        public List<GetEmployeeSalaryByEmployeeIdSlipResponse> EmployeeSalaryByEmployeeIdSliplist { get; set; }
        public Int32 TotalRecords { get; set; }
        public string companyName { get; set; }
        public string employeeName { get; set; }
        public string employeePicture { get; set; }
        public string empPhotoType { get; set; }
        public string employeeDept { get; set; }
        public string employeeDesignation { get; set; }
        public string companyLogo { get; set; }
        public string accountNum { get; set; }
    }

    public class GetEmployeeSalaryByEmployeeIdSlipResponse
    {
        public long EmployeeSalarySlipID { get; set; }
        public long employeeSalaryID { get; set; }
        public long employeeID { get; set; }
        public string month { get; set; }
        public DateTime paidOn { get; set; }
        public string paidFullDate { get; set; }
        public decimal? basicPay { get; set; }
        public decimal? allowances { get; set; }
        public decimal? deductions { get; set; }
        public decimal? netSalary { get; set; }
        public string createdBy { get; set; }
        public string modifiedBy { get; set; }
        public string action { get; set; }
        public string requestStatus { get; set; }
        public  string currency { get; set; }
    }

    public class SaveEmployeeBankDetailRequestModel
    {
        public long EmployeeBankDetailId { get; set; }
        public long EmployeeID { get; set; }
        public string bankName { get; set; }
        public string bankBranch { get; set; }
        public string accountHolder { get; set; }
        public string accountNumber { get; set; }
        public string ifscCode { get; set; }
        public string createdBy { get; set; }
        public string modifiedBy { get; set; }
        public string action { get; set; }
    }

    public class GetEmployeeSalariesSearchRequest
    {
        public long companyId { get; set; }
        public string location { get; set; }
        public string fullName { get; set; }
        public string employmentType { get; set; }
        public string department { get; set; }
        public string position { get; set; }
        public int pageIndex { get; set; }
        public int pageSize { get; set; }
    }

    public class GetEmployeeSalariesModel
    {
        public List<GetEmployeeSalariesList> EmployeeSalaryList { get; set; }
        public Int32 TotalRecords { get; set; }
    }
    public class GetEmployeeSalariesList
    {
        public long employeeId { get; set; }
        public string employeeName { get; set; }
        public string position { get; set; }
        public string department { get; set; }
        public string employeePhoto { get; set; }
        public string photoType { get; set; }
        public decimal? monthlyPay { get; set; }
        public decimal? hourlyPay { get; set; }
        public decimal? hoursWorked { get; set; }
        public decimal? dailyPay { get; set; }
        public decimal? weeklyPay { get; set; }
        public decimal? weeksWorked { get; set; }
        public string currency { get; set; }
        public string payType { get; set; }
        public long employeeSalaryID { get; set; }
    }

    public class FillEmployeeDDLResponse
    {
        public long employeeId { get; set; }
        public string employeeName { get; set; }
        public string email { get; set; }
    }
    public class FillEmployeeSalariesDDLResponse
    {
        public long employeeId { get; set; }
        public string employeeName { get; set; }
        public string email { get; set; }
        public string employeePhoto { get; set; }
        public string photoType { get; set; }
        public string payType { get; set; }
        public string department { get; set; }
        public string Position { get; set; }
    }

    public class CheckGetEmployeeSalaryByIdRequestModel
    {
        public string fullName { get; set; }
        public string email { get; set; }
        public string employeePhoto { get; set; }
        public string photoType { get; set; }
        public string payType { get; set; }
        public decimal? monthlyPay { get; set; }
        public decimal? hourlyPay { get; set; }
        public decimal? hoursWorked { get; set; }
        public decimal? dailyPay { get; set; }
        public decimal? weeklyPay { get; set; }
        public decimal? weeksWorked { get; set; }
        public string currency { get; set; }
    }

    public class ChangeSalarySlipDownloadStatusModelRequest
    {
        public long EmployeeSalarySlipID { get; set; }
        public long employeeSalaryID { get; set; }
        public long employeeID { get; set; }
        public string month { get; set; }
        public DateTime paidOn { get; set; }
        public string paidFullDate { get; set; }
        public decimal? basicPay { get; set; }
        public decimal? allowances { get; set; }
        public decimal? deductions { get; set; }
        public decimal? netSalary { get; set; }
        public string statusClicked { get; set; }
    }

    public class CheckEmployeeRegisterValidationRequestModel
    {
        public string email { get; set; }
        public string phoneNumber { get; set; }
    }
    public class CheckEmployeeRegisterValidationReturnModel
    {
        public bool status { get; set; }
        public string message { get; set; }
    }
    public class FillEmployeeDDLForSalariesRequest
    {
        public long companyId { get; set; }
        public string payType { get; set; }
    }
    public class GetEmployeeSalarySlipSearchRequest
    {
        public long companyId { get; set; }
       // public string location { get; set; }
        public string fullName { get; set; }
       // public string employmentType { get; set; }
       // public string department { get; set; }
       // public string position { get; set; }
        public int pageIndex { get; set; }
        public int pageSize { get; set; }
    }
       public class GetEmployeeSalarySlipResponse
       {
        public long EmployeeSalarySlipID { get; set; }
        public long EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeePhoto{ get; set; }
        public long CompanyID { get; set; }
        public string Department { get; set; }
        public string Month { get; set; }
        public DateTime PaidOn { get; set; }
        public decimal? BasicPay { get; set; }
        public decimal? Allowances { get; set; }
        public decimal? Deductions { get; set; }
        public decimal? NetSalary { get; set; }
      
        public int Year { get; set; }
        public string Currency { get; set; }
        public string RequestStatus { get; set; }
        public string DeductionReason { get; set; }
        public decimal? DeductionPerHrs { get; set; }
        public decimal? NetSalaryPerHrs { get; set; }
        public string PayType { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; } 
        public bool? IsPaid { get; set; }
       
        public string position { get; set; }

        }
    public class GetEmployeeSalarySlipModel
    {
        public List<GetEmployeeSalarySlipResponse> EmployeeSalarySlipList { get; set; }
        public Int32 totalRecords { get; set; }
    }
    public class SalaryAllowancesRequest
    {
        public decimal? Allowances { get; set; }
        public long EmployeeSalarySlipID { get; set; }
    }
    
}
