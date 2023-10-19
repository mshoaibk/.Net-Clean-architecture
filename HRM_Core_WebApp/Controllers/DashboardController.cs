using HRM_Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRM_Core_WebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class DashboardController : ControllerBase
    {
        #region Global

        private readonly IDashboardServices _IDashboardServices;

        public DashboardController(IDashboardServices _IDashboardServices)
        {
            this._IDashboardServices = _IDashboardServices;
        }

        #endregion

        #region Dashboard Controller API's
        /// <summary>
        /// get dashboard admin data
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAdminDashboardData")]
        public async Task<IActionResult> GetAdminDashboardData()
        {
            try
            {
                var _result = await _IDashboardServices.GetAdminDashboardData();
                return Ok(new { result = _result, Status = true });
            }
            catch (Exception ex)
            {
                // Log the exception and send the email
                LogAndSendException(ex, "GetAdminDashboardData Exception");
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        /// <summary>
        /// get dashboard company data
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetCompanyDashboardData/{companyID}")]
        public async Task<IActionResult> GetCompanyDashboardData(long companyID)
        {
            try
            {
                var _result = await _IDashboardServices.GetCompanyDashboardData(companyID);
                return Ok(new { result = _result, Status = true });
            }
            catch (Exception ex)
            {
                // Log the exception and send the email
                LogAndSendException(ex, "GetCompanyDashboardData Exception");
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        /// <summary>
        /// Get company dashboard count by company id
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="requiredCount"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetCompanyDashboardCount/{companyId}/{requiredCount}")]
        public async Task<IActionResult> GetCompanyDashboardCount(long companyId, string requiredCount)
        {
            try
            {
                var _result = await _IDashboardServices.GetCompanyDashboardCount(companyId, requiredCount);
                return Ok(new { Status = true, dashboardCount = _result });
            }
            catch (Exception ex)
            {
                // Log the exception and send the email
                LogAndSendException(ex, "GetCompanyDashboardCount Exception");
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
