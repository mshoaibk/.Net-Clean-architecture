using HRM_Application.Interfaces;
using HRM_Application.Services;
using HRM_Domain.Model;
using iTextSharp.text.pdf;
using iTextSharp.text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Threading.Tasks;
using Org.BouncyCastle.Utilities;
using System.Net.Http.Headers;

namespace HRM_Core_WebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        #region Global

        private readonly IEmployeeServices _IEmployeeServices;
        private readonly IEmailServices _IEmailServices;
        

        public EmployeeController(IEmployeeServices _IEmployeeServices, IEmailServices _IEmailServices)
        {
            this._IEmployeeServices = _IEmployeeServices;
            this._IEmailServices = _IEmailServices;
           
        }
        #endregion

        #region Employee Controller API's
        /// <summary>
        /// Save employees
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("SaveEmployee")]
        public async Task<IActionResult> SaveEmployee([FromForm] EmployeeSaveRequest model)
        {
            try
            {
                if (model.PhotoType == "uploadedurl") {
                    var folderName = Path.Combine("Resources", "Images");
                    var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                    var fileName = ContentDispositionHeaderValue.Parse(model.employeePhoto.ContentDisposition).FileName.Trim('"');
                    var fullPath = Path.Combine(pathToSave, fileName);
                    var dbPath = Path.Combine(folderName, fileName);
                    using (var stream = new FileStream(fullPath, FileMode.Create)) {
                        model.employeePhoto.CopyTo(stream);
                    }
                    model.employeePhotoName = dbPath.ToString();
                }

                var _result = await _IEmployeeServices.SaveEmployee(model);
                return Ok(new { Status = true });
            }
            catch (Exception ex)
            {
                // Log the exception and send the email
                LogAndSendException(ex, "SaveEmployee Exception");
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        /// <summary>
        /// Get employee detail
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetEmployeeList")]
        public async Task<IActionResult> GetEmployeeList(SearchGetEmployeeListRequest model)
        {
            try
            {
                var _result = await _IEmployeeServices.GetEmployeeList(model);
                return Ok(new { Status = true, employeelist = _result.EmployeeList, totalRecords = _result.TotalRecords });
            }
            catch (Exception ex)
            {
                // Log the exception and send the email
                LogAndSendException(ex, "GetEmployeeList Exception");
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        /// <summary>
        /// Delete employee by id
        /// </summary>
        /// <param name="employeeID"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("DeleteEmployeeDetail/{employeeID}")]
        public async Task<IActionResult> DeleteEmployeeDetail(int employeeID)
        {
            try
            {
                var _result = await _IEmployeeServices.DeleteEmployeeDetail(employeeID);
                return Ok(new { Status = true });
            }
            catch (Exception ex)
            {
                // Log the exception and send the email
                LogAndSendException(ex, "DeleteEmployeeDetail Exception");
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        /// <summary>
        /// activate-deactivate employee detail
        /// </summary>
        /// <param name="employeeID"></param>
        /// <param name="isActivated"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("OnChangeEmployeeStatus/{employeeID}/{isActivated}")]
        public async Task<IActionResult> OnChangeEmployeeStatus(long employeeID, bool isActivated)
        {
            try
            {
                var _result = await _IEmployeeServices.OnChangeEmployeeStatus(employeeID, isActivated);
                return Ok(new { Status = true });
            }
            catch (Exception ex)
            {
                // Log the exception and send the email
                LogAndSendException(ex, "OnChangeEmployeeStatus Exception");
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        /// <summary>
        /// Get Employees basic detail for Company
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetEmployeeBasicDetailForCompany")]
        public async Task<IActionResult> GetEmployeeBasicDetailForCompany(SearchGetEmployeeBasicDetailRequest model)
        {
            try
            {
                var _result = await _IEmployeeServices.GetEmployeeBasicDetailForCompany(model);
                return Ok(new { Status = true, employeelist = _result.EmployeeList, totalRecords = _result.TotalRecords });
            }
            catch (Exception ex)
            {
                // Log the exception and send the email
                LogAndSendException(ex, "GetEmployeeBasicDetailForCompany Exception");
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }


        /// <summary>
        /// Get all Detail for Company
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("ShowCompleteEmployeeDetail")]
        public async Task<IActionResult> ShowCompleteEmployeeDetail(ShowCompleteEmployeeDetailRequest model)
        {
            try
            {
                var _result = await _IEmployeeServices.ShowCompleteEmployeeDetail(model);
                return Ok(new { Status = true, employeeDetail = _result });
            }
            catch (Exception ex)
            {
                // Log the exception and send the email
                LogAndSendException(ex, "ShowCompleteEmployeeDetail Exception");
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }
        #region
       
        /// <summary>
        /// Save employee salaries
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("SaveEmployeeSalary")]
        public async Task<IActionResult> SaveEmployeeSalary(SaveEmployeeSalaryRequestModel model)
        {
            try
            {
                var _result = await _IEmployeeServices.SaveEmployeeSalary(model);
                return Ok(new { Status = true });
            }
            catch (Exception ex)
            {
                // Log the exception and send the email
                LogAndSendException(ex, "SaveEmployeeSalary Exception");
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }
        /// <summary>
        /// Generate Employee Salary
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GenarateEmployeeSalary")]
        public async Task<IActionResult> GenerateEmployeeSalary(GenerateEmployeeSalaryRequestModel model)
        {
            try
            {
                var _result = await _IEmployeeServices.GenerateEmployeeSalary(model);
                return Ok(new { Status = _result });
            }
            catch (Exception ex)
            {
                // Log the exception and send the email
                LogAndSendException(ex, "SaveEmployeeSalary Exception");
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }
        /// <summary>
        /// Delete employee salaries
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("DeleteEmployeeSalary/{employeeSalaryID}")]
        public async Task<IActionResult> DeleteEmployeeSalary(int employeeSalaryID)
        {
            try
            {
                var _result = await _IEmployeeServices.DeleteEmployeeSalary(employeeSalaryID);
                return Ok(new { Status = true });
            }
            catch (Exception ex)
            {
                // Log the exception and send the email
                LogAndSendException(ex, "SaveEmployeeSalary Exception");
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }
        /// <summary>
        /// get salary by employee id
        /// </summary>
        /// <param name="employeeID"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetEmployeeSalaryById/{employeeID}")]
        public async Task<IActionResult> GetEmployeeSalaryById(long employeeID)
        {
            try
            {
                var _result = await _IEmployeeServices.GetEmployeeSalaryById(employeeID);
                return Ok(new { Status = true, employeeSalaryDetail= _result });
            }
            catch (Exception ex)
            {
                // Log the exception and send the email
                LogAndSendException(ex, "GetEmployeeSalaryById Exception");
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        /// <summary>
        /// Save employee salaries
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("SaveEmployeeSalarySlip")]
        public async Task<IActionResult> SaveEmployeeSalarySlip(SaveEmployeeSalarySlipRequestModel model)
        {
            try
            {
                var _result = await _IEmployeeServices.SaveEmployeeSalarySlip(model);
                return Ok(new { Status = true });
            }
            catch (Exception ex)
            {
                // Log the exception and send the email
                LogAndSendException(ex, "SaveEmployeeSalarySlip Exception");
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        /// <summary>
        /// get salary slip by employee id
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetEmployeeSalaryByEmployeeIdSlip")]
        public async Task<IActionResult> GetEmployeeSalaryByEmployeeIdSlip(GetEmployeeSalaryByEmployeeIdSlipModelRequest model)
        {
            try
            {
                var _result = await _IEmployeeServices.GetEmployeeSalaryByEmployeeIdSlip(model);
                return Ok(new 
                { 
                    Status = true, 
                    employeeSalarySlipDetail = _result.EmployeeSalaryByEmployeeIdSliplist,
                    totalRecord= _result.TotalRecords,
                    employeeName=_result.employeeName,
                    employeePicture = _result != null && _result.empPhotoType == "webcamurl" ? _result.employeePicture: await ConvertImagePathToDataURL(_result.employeePicture),
                    empPhotoType=_result.empPhotoType,
                    employeeDept=_result.employeeDept,
                    companyName=_result.companyName,
                    companyLogo= _result!=null ? await ConvertImagePathToDataURL(_result.companyLogo):"",
                    accountNum = _result.accountNum,
                    employeeDesignation= _result.employeeDesignation,
                });
            }
            catch (Exception ex)
            {
                // Log the exception and send the email
                LogAndSendException(ex, "GetEmployeeSalaryByEmployeeIdSlip Exception");
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        /// <summary>
        /// Save employee bank detail
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("SaveEmployeeBankDetail")]
        public async Task<IActionResult> SaveEmployeeBankDetail(SaveEmployeeBankDetailRequestModel model)
        {
            try
            {
                var _result = await _IEmployeeServices.SaveEmployeeBankDetail(model);
                return Ok(new { Status = true });
            }
            catch (Exception ex)
            {
                // Log the exception and send the email
                LogAndSendException(ex, "SaveEmployeeBankDetail Exception");
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        /// <summary>
        /// get bank detail by employee id
        /// </summary>
        /// <param name="employeeID"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetEmployeeBankDetail/{employeeID}")]
        public async Task<IActionResult> GetEmployeeBankDetail(long employeeID)
        {
            try
            {
                var _result = await _IEmployeeServices.GetEmployeeBankDetail(employeeID);
                return Ok(new { Status = true, employeeBankDetail = _result });
            }
            catch (Exception ex)
            {
                // Log the exception and send the email
                LogAndSendException(ex, "GetEmployeeBankDetail Exception");
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        /// <summary>
        /// show employees salary 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetEmployeeSalaries")]
        public async Task<IActionResult> GetEmployeeSalaries(GetEmployeeSalariesSearchRequest model)
        {
            try
            {
                var _result = await _IEmployeeServices.GetEmployeeSalaries(model);
                return Ok(new { Status = true, employeeSalarylist = _result.EmployeeSalaryList, totalRecords = _result.TotalRecords });
            }
            catch (Exception ex)
            {
                // Log the exception and send the email
                LogAndSendException(ex, "FillEmployeeDDLByCompany Exception");
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        /// <summary>
        /// show employees dropdownlist
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("FillEmployeeDDLByCompany/{companyId}/{requiredAll}")]
        public async Task<IActionResult> FillEmployeeDDLByCompany(long? companyId, string requiredAll)
        {
            try
            {
                var _result = await _IEmployeeServices.FillEmployeeDDLByCompany(companyId, requiredAll);
                return Ok(new { Status = true, employeeDDLlist = _result });
            }
            catch (Exception ex)
            {
                // Log the exception and send the email
                LogAndSendException(ex, "FillEmployeeDDLByCompany Exception");
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        /// <summary>
        /// get and check salary by employee id
        /// </summary>
        /// <param name="employeeID"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("CheckGetEmployeeSalaryById/{employeeID}")]
        public async Task<IActionResult> CheckGetEmployeeSalaryById(long employeeID)
        {
            try
            {
                var _result = await _IEmployeeServices.CheckGetEmployeeSalaryById(employeeID);
                return Ok(new { Status = true, employeeSalaryDetail = _result });
            }
            catch (Exception ex)
            {
                // Log the exception and send the email
                LogAndSendException(ex, "GetEmployeeSalaryById Exception");
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

        /// <summary>
        /// change salary status
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("ChangeSalarySlipDownloadStatus")]
        public async Task<IActionResult> ChangeSalarySlipDownloadStatus(ChangeSalarySlipDownloadStatusModelRequest model)
        {
            try
            {
                var _result = await _IEmployeeServices.ChangeSalarySlipDownloadStatus(model);
                return Ok(new { Status = true });
            }
            catch (Exception ex)
            {
                // Log the exception and send the email
                LogAndSendException(ex, "ChangeSalaryStatus Exception");
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        static async System.Threading.Tasks.Task<string> ConvertImagePathToDataURL(string imagePath)
        {
            try
            {
                if (imagePath != null)
                {
                    using (HttpClient client = new HttpClient())
                    {
                        string imageName = Path.GetFileName(imagePath);

                        imagePath = Path.Combine("http://localhost:44398/Resources/Images/", imageName);

                        // Fetch the image data
                        byte[] imageData = await client.GetByteArrayAsync(imagePath);

                        // Convert the image data to base64
                        string base64Data = Convert.ToBase64String(imageData);

                        // Create the data URL
                        string dataUrl = $"data:image/jpeg;base64,{base64Data}";

                        return dataUrl;
                    }
                }
                return "";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error converting image path to data URL: {ex.Message}");
                throw;
            }
        }
        #endregion
        /// <summary>
        /// check email and phone number for employee
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("CheckEmployeeRegisterValidation")]
        public async Task<IActionResult> CheckEmployeeRegisterValidation(CheckEmployeeRegisterValidationRequestModel model)
        {
            try
            {
                var _result = await _IEmployeeServices.CheckEmployeeRegisterValidation(model);
                return Ok(new { status = _result.status, msg = _result.message });
            }
            catch (Exception ex)
            {
                // Log the exception and send the email
                LogAndSendException(ex, "GetEmployeeSalaryById Exception");
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }
        /// <summary>
        /// check email and phone number for employee
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("FillEmployeeDDLForSalaries")]
        public async Task<IActionResult> FillEmployeeDDLForSalaries(FillEmployeeDDLForSalariesRequest model)
        {
            try
            {
                List<FillEmployeeSalariesDDLResponse> result = new List<FillEmployeeSalariesDDLResponse>();
                 result =await  _IEmployeeServices.FillEmployeeDDLForSalaries(model);
                return Ok(new { List = result });
            }
            catch (Exception ex)
            {
                // Log the exception and send the email
                LogAndSendException(ex, "GetEmployeeSalaryById Exception");
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        /// <summary>
        /// Get Employee Salary Slip List
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetEmployeeSalarySlipList")]
        public async Task<IActionResult> GetEmployeeSalarySlipList(GetEmployeeSalarySlipSearchRequest model)
        {
            try
            {
            var data = _IEmployeeServices.GetEmployeeSalarySlipList(model);
            return Ok(new { totalRecords= data.Result.totalRecords, SalarySlipList = data.Result.EmployeeSalarySlipList });
            }
            catch(Exception ex)
            {
                LogAndSendException(ex, "GetEmployeeSalarySlipList Exception");
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }
        /// <summary>
        /// Update /Add Salary Allowances
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("UpdateSalaryAllowances")]
        public async Task<IActionResult> UpdateSalaryAllowances(SalaryAllowancesRequest model)
        {
            try
            {
                var result = _IEmployeeServices.UpdateSalaryAllowances(model);
                Ok(result);
            }
            catch (Exception ex)
            {
                // Log the exception and send the email
                LogAndSendException(ex, "UpdateSalaryAllowances Exception");
                return StatusCode(500, $"Internal server error: {ex}");
            }
            return Ok("");
        }

        /// <summary>
        ///Salary Pay
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("SalaryPay/{EmployeeSalarySlipID}")]
        public async Task<IActionResult> SalaryPay(long EmployeeSalarySlipID)
        {
            try
            {
               var result = _IEmployeeServices.SalaryPay(EmployeeSalarySlipID);
               return Ok(result.Result);
            }
            catch (Exception ex)
            {
                // Log the exception and send the email
                LogAndSendException(ex, "SalaryPay Exception");
                return StatusCode(500, $"Internal server error: {ex}");
            }
           
        }
        #endregion
    }
}
