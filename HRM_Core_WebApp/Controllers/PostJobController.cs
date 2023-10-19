using HRM_Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using HRM_Domain.Model;
using System.Threading.Tasks;
using System.Collections.Generic;
using HRM_Common;
using Serilog;

namespace HRM_Core_WebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostJobController : ControllerBase
    {
        #region Global

        private readonly IPostJobServices _IPostJobService;

        public PostJobController(IPostJobServices _IPostJobService)
        {
            this._IPostJobService = _IPostJobService;
        }
        #endregion

        #region PostJob Controller API's
        /// <summary>
        /// Save posted jobs
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("SavePostJob")]
        public async Task<IActionResult> SavePostJob(SavePostJobRequest model)
        {
            try
            {
                var _result = await _IPostJobService.SavePostJob(model);
                return Ok(new { Status = true });
            }
            catch (Exception ex)
            {
                // Log the exception and send the email
                LogAndSendException(ex, "SavePostJob Exception");
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        /// <summary>
        /// Get posted jobs
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetPostedJob")]
        public async Task<IActionResult> GetPostedJob(SearchPostedJobRequest model)
        {
            try
            {
                var _result = await _IPostJobService.GetPostedJob(model);
                return Ok(new { Status = true, postedJobList = _result.PostedJobList, totalRecords = _result.TotalRecords });
            }
            catch (Exception ex)
            {
                // Log the exception and send the email
                LogAndSendException(ex, "GetPostedJob Exception");
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        /// <summary>
        /// Delete posted jobs
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("DeletePostedJob/{postJobId}")]
        public async Task<IActionResult> DeletePostedJob(int postJobId)
        {
            try
            {
                var _result = await _IPostJobService.DeletePostedJob(postJobId);
                return Ok(new { Status = true });
            }
            catch (Exception ex)
            {
                // Log the exception and send the email
                LogAndSendException(ex, "DeletePostedJob Exception");
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        /// <summary>
        /// activate-deactivate specific job detail
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("OnChangeJobActivationStatus/{postJobId}/{isActivated}")]
        public async Task<IActionResult> OnChangeJobActivationStatus(long postJobId, bool isActivated)
        {
            try
            {
                var _result = await _IPostJobService.OnChangeJobActivationStatus(postJobId, isActivated);
                return Ok(new { Status = true });
            }
            catch (Exception ex)
            {
                // Log the exception and send the email
                LogAndSendException(ex, "OnChangeJobActivationStatus Exception");
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        /// <summary>
        /// Get carrer jobs
        /// </summary>
        /// <param name="EncodedModel"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetCarrerByComapny/{EncodedModel}")]
        public async Task<IActionResult> GetCarrerByComapny(string EncodedModel)
        {
            try
            {
                SearchCarrerByComapnyRequest model = Methods.GetDecodedModel<SearchCarrerByComapnyRequest>(EncodedModel);
                var _result = await _IPostJobService.GetCarrerByComapny(model);
                return Ok(new { Status = true, postedJobList = _result.PostedJobList, totalRecords = _result.TotalRecords });
            }
            catch (Exception ex)
            {
                // Log the exception and send the email
                LogAndSendException(ex, "GetCarrerByComapny Exception");
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
