using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HRM_Application.Interfaces;
using HRM_Domain.Model;
using Serilog;

namespace HRM_Core_WebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobApplicationController : ControllerBase
    {
        #region Global

        private readonly IJobApplicationService _IJobApplicationService;

        public JobApplicationController(IJobApplicationService _IJobApplicationService)
        {
            this._IJobApplicationService = _IJobApplicationService;
        }
        #endregion

        #region Job Application Controller API's
        /// <summary>
        /// Save posted jobs
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("ApplyJob")]
        public async Task<IActionResult> ApplyJob(ApplyJobEntity model)
        {
            try
            {
                var _result = await _IJobApplicationService.ApplyJob(model);
                return Ok(new { Status = true });
            }
            catch (Exception ex)
            {
                // Log the exception and send the email
                LogAndSendException(ex, "ApplyJob Exception");
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        /// <summary>
        /// Show posted jobs
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("ShowAppliedApplication")]
        public async Task<IActionResult> ShowAppliedApplication(ShowAppliedApplicationRequestModel model)
        {
            try
            {
                var _result = await _IJobApplicationService.ShowAppliedApplication(model);
                return Ok(new { Status = true, showAppliedApplications = _result.ShowApplicationList, totalRecords = _result.TotalRecords });
            }
            catch (Exception ex)
            {
                // Log the exception and send the email
                LogAndSendException(ex, "ShowAppliedApplication Exception");
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        /// <summary>
        /// Show posted jobs
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("ShowCandidatesApplication")]
        public async Task<IActionResult> ShowCandidatesApplication(ShowCandidatesApplicationRequestModel model)
        {
            try
            {
                var _result = await _IJobApplicationService.ShowCandidatesApplication(model);
                return Ok(new { Status = true, showAppliedApplications = _result.showCandidatesApplicationList, totalRecords = _result.totalRecords, jobTitle = _result.jobTitle });
            }
            catch (Exception ex)
            {
                // Log the exception and send the email
                LogAndSendException(ex, "ShowCandidatesApplication Exception");
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        /// <summary>
        /// Show posted jobs
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("UpdateCandidatesApplication")]
        public async Task<IActionResult> UpdateCandidatesApplication(UpdateCandidatesApplicationRequestModel model)
        {
            try
            {
                var _result = await _IJobApplicationService.UpdateCandidatesApplication(model);
                return Ok(new { Status = true });
            }
            catch (Exception ex)
            {
                // Log the exception and send the email
                LogAndSendException(ex, "UpdateCandidatesApplication Exception");
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        /// <summary>
        /// Get posted job by id
        /// </summary>
        /// <param name="postJobId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetPostedJobById/{postJobId}")]
        public async Task<IActionResult> GetPostedJobById(long postJobId)
        {
            try
            {
                var _result = await _IJobApplicationService.GetPostedJobById(postJobId);
                return Ok(new { Status = true, postedJobResult=_result });
            }
            catch (Exception ex)
            {
                // Log the exception and send the email
                LogAndSendException(ex, "GetPostedJobById Exception");
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
