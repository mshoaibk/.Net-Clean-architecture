using HRM_Application.Interfaces;
using HRM_Common;
using HRM_Domain.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace HRM_Core_WebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyRegistrationController : ControllerBase
    {
        #region Global

        private readonly ICompanyRegistrationServices _ICompanyRegistrationServices;

        public CompanyRegistrationController(ICompanyRegistrationServices _ICompanyRegistrationServices)
        {
            this._ICompanyRegistrationServices = _ICompanyRegistrationServices;
        }
        #endregion

        #region Company Controller API's
        /// <summary>
        /// Register Company 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("RegisterCompanyDetail")]
        public async Task<IActionResult> RegisterCompanyDetail([FromForm] RegisterCompanyRequest model)
        {
            try
            {
                var folderName = Path.Combine("Resources", "Images");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                var fileName = ContentDispositionHeaderValue.Parse(model.companyLogo.ContentDisposition).FileName.Trim('"');
                var fullPath = Path.Combine(pathToSave, fileName);
                var dbPath = Path.Combine(folderName, fileName);
                using (var stream = new FileStream(fullPath, FileMode.Create)) {
                    model.companyLogo.CopyTo(stream);
                }
                model.companyLogoPath = dbPath.ToString();
                var _result = await _ICompanyRegistrationServices.RegisterCompanyDetail(model);
                return Ok(new { Status = true });
            }
            catch (Exception ex)
            {
                // Log the exception and send the email
                LogAndSendException(ex, "RegisterCompanyDetail Exception");
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        /// <summary>
        /// Get Company Detail
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetCompanyDetail")]
        public async Task<IActionResult> GetCompanyDetail(SearchCompanyDetailRequest model)
        {
            try
            {
                var _result = await _ICompanyRegistrationServices.GetCompanyDetail(model);
                return Ok(new { Status = true, postedCompanylist = _result.CompanyList, totalRecords = _result.TotalRecords });
            }
            catch (Exception ex)
            {
                // Log the exception and send the email
                LogAndSendException(ex, "GetCompanyDetail Exception");
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        /// <summary>
        /// Delete company detail
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("DeleteCompanyDetail/{companyID}")]
        public async Task<IActionResult> DeleteCompanyDetail(long companyID)
        {
            try
            {
                var _result = await _ICompanyRegistrationServices.DeleteCompanyDetail(companyID);
                return Ok(new { Status = true });
            }
            catch (Exception ex)
            {
                // Log the exception and send the email
                LogAndSendException(ex, "DeleteCompanyDetail Exception");
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        /// <summary>
        /// activate-deactivate company detail
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("OnChangeCompanyStatus/{companyID}/{isActivated}")]
        public async Task<IActionResult> OnChangeCompanyStatus(long companyID,bool isActivated)
        {
            try
            {
                var _result = await _ICompanyRegistrationServices.OnChangeCompanyStatus(companyID, isActivated);
                return Ok(new { Status = true });
            }
            catch (Exception ex)
            {
                // Log the exception and send the email
                LogAndSendException(ex, "OnChangeCompanyStatus Exception");
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        /// <summary>
        /// Get company name,id
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetCompaniesName")]
        public async Task<IActionResult> GetCompaniesName()
        {
            try
            {
                var _result = await _ICompanyRegistrationServices.GetCompaniesName();
                return Ok(new { Status = true, companyList = _result });
            }
            catch (Exception ex)
            {
                // Log the exception and send the email
                LogAndSendException(ex, "GetCompaniesName Exception");
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }


        /// <summary>
        /// Get Company Profile Detail
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetCompanyProfile/{companyID}")]
        public async Task<IActionResult> GetCompanyProfile(string companyId)
        {
            try
            {
                GetCompanyProfileResponse profileObject = new GetCompanyProfileResponse();
                profileObject = await _ICompanyRegistrationServices.GetCompanyProfile(companyId);
                return Ok(new { Status = true, companyList = profileObject, message = "Company profile have a data." });
            }
            catch (Exception ex)
            {
                // Log the exception and send the email
                LogAndSendException(ex, "GetCompanyProfile Exception");
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        /// <summary>
        /// Add announcement
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("SaveCompanyAnnouncement")]
        public async Task<IActionResult> SaveCompanyAnnouncement([FromForm] SaveCompanyAnnouncementRequestModel model)
        {
            try
            {
                var folderName = Path.Combine("Resources", "Images");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                var fileName = ContentDispositionHeaderValue.Parse(model.announcementFeaturePhoto.ContentDisposition).FileName.Trim('"');
                var fullPath = Path.Combine(pathToSave, fileName);
                var dbPath = Path.Combine(folderName, fileName);
                using (var stream = new FileStream(fullPath, FileMode.Create)) {
                    model.announcementFeaturePhoto.CopyTo(stream);
                }
                model.announcementFeaturePhotoName = dbPath.ToString();
                var _result = await _ICompanyRegistrationServices.SaveCompanyAnnouncement(model);
                return Ok(new { Status = true });
            }
            catch (Exception ex)
            {
                // Log the exception and send the email
                LogAndSendException(ex, "SaveCompanyAnnouncement Exception");
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        /// <summary>
        /// Add announcement
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetCompanyAnnouncementList")]
        public async Task<IActionResult> GetCompanyAnnouncementList(SearchCompanyAnnouncement model)
        {
            try
            {
                var _result = await _ICompanyRegistrationServices.GetCompanyAnnouncementList(model);
                return Ok(new { Status = true, GetCompanyAnnouncement=_result.CompanyAnnouncementResponse, totalRecords =_result.TotalRecords});
            }
            catch (Exception ex)
            {
                // Log the exception and send the email
                LogAndSendException(ex, "GetCompanyAnnouncementList Exception");
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        /// <summary>
        /// Delete company annoncement
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("DeleteCompanyAnnouncement/{companyAnnouncementID}")]
        public async Task<IActionResult> DeleteCompanyAnnouncement(long? companyAnnouncementID) {
            try {
                var _result = await _ICompanyRegistrationServices.DeleteCompanyAnnouncement(companyAnnouncementID);
                return Ok(new { Status = true });
            } catch (Exception ex) {
                // Log the exception and send the email
                LogAndSendException(ex, "DeleteCompanyAnnouncement Exception");
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
    }
}
