using HRM_Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HRM_Application.Interfaces
{
    public interface IEmployeeServices
    {
        Task<bool> SaveEmployee(EmployeeSaveRequest model);
        Task<GetEmployeeListModel> GetEmployeeList(SearchGetEmployeeListRequest model);
        Task<bool> DeleteEmployeeDetail(long employeeID);
        Task<bool> OnChangeEmployeeStatus(long employeeID, bool isActivated);
        Task<GetEmployeeBasicDetailModel> GetEmployeeBasicDetailForCompany(SearchGetEmployeeBasicDetailRequest model);
        Task<ShowCompleteEmployeeDetailRequestResponse> ShowCompleteEmployeeDetail(ShowCompleteEmployeeDetailRequest model);
        Task<bool> SaveEmployeeSalary(SaveEmployeeSalaryRequestModel model);
        Task<bool> DeleteEmployeeSalary(int employeeSalaryID);
        Task<SaveEmployeeSalaryRequestModel> GetEmployeeSalaryById(long employeeID);
        Task<bool> SaveEmployeeSalarySlip(SaveEmployeeSalarySlipRequestModel model);
        Task<GetEmployeeSalaryByEmployeeIdSlipModel> GetEmployeeSalaryByEmployeeIdSlip(GetEmployeeSalaryByEmployeeIdSlipModelRequest model);
        Task<bool> SaveEmployeeBankDetail(SaveEmployeeBankDetailRequestModel model);
        Task<SaveEmployeeBankDetailRequestModel> GetEmployeeBankDetail(long employeeID);
        Task<GetEmployeeSalariesModel> GetEmployeeSalaries(GetEmployeeSalariesSearchRequest model);
        Task<List<FillEmployeeDDLResponse>> FillEmployeeDDLByCompany(long? companyId, string requiredAll);
        Task<CheckGetEmployeeSalaryByIdRequestModel> CheckGetEmployeeSalaryById(long employeeID);
        Task<bool> ChangeSalarySlipDownloadStatus(ChangeSalarySlipDownloadStatusModelRequest model);
        Task<CheckEmployeeRegisterValidationReturnModel> CheckEmployeeRegisterValidation(CheckEmployeeRegisterValidationRequestModel model);
        Task<bool> GenerateEmployeeSalary(GenerateEmployeeSalaryRequestModel model);
        Task<List<FillEmployeeSalariesDDLResponse>> FillEmployeeDDLForSalaries(FillEmployeeDDLForSalariesRequest model);

        Task<GetEmployeeSalarySlipModel> GetEmployeeSalarySlipList(GetEmployeeSalarySlipSearchRequest model);
        Task<bool> UpdateSalaryAllowances(SalaryAllowancesRequest model);
        Task<bool> SalaryPay(long EmployeeSalarySlipID);

    }
}
