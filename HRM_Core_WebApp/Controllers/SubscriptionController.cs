using HRM_Application.Interfaces;
using HRM_Domain.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using Serilog;
using System.Net.WebSockets;
using System.Web.Http.Results;

namespace HRM_Core_WebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubscriptionController : ControllerBase
    {
        #region Global

        private readonly ISubscription _ISubscription;

        public SubscriptionController(ISubscription _ISubscription)
        {
            this._ISubscription = _ISubscription;
        }

        #endregion
        /// <summary>
        /// Save Packages
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// 

        #region Packages
        [HttpPost]
        [Route("SavePackages")]
        public async Task<IActionResult> SavePackages(SubscriptionPackagesRequest model)
        {
            try
            {
                var _result = await _ISubscription.SavePackages(model);
                return Ok(new { Status = _result, });
               
            }
            catch (Exception ex)
            {
                // Log the exception and send the email
                LogAndSendException(ex, "SavePackages Exception");
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        /// <summary>
        /// List Packages
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("ListPackages")]
        public async Task<IActionResult> ListPackages(SubscriptionPackagesRequestModel model)
        {
            try
            {
                var _result = await _ISubscription.ListPackages(model);
                return Ok(new { Status = true, Packageslist = _result.SubscriptionPackages, totalRecords = _result.TotalRecords });
            }
            catch (Exception ex)
            {
                // Log the exception and send the email
                LogAndSendException(ex, "ListPackages Exception");
                return StatusCode(500, $"Internal server error: {ex}");
            } }
  

        /// <summary>
        /// Get packageBy Id
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetpackageById/{id}")]
        public async Task<IActionResult> GetpackageById(long id)
        {
            try
            {


                var _result = await _ISubscription.GetpackageById(id);
                return Ok(new { Status = true, PackagesData = _result });

            }
            catch (Exception ex)
            {
                // Log the exception and send the email
                LogAndSendException(ex, "ListPackages Exception");
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }
        /// <summary>
        /// Delete Packages
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("DeletePackages/{packageId}")]
        public async Task<IActionResult> DeletePackages(long packageId)
        {
            try
            {


                var _result = await _ISubscription.DeletePackages(packageId);
                return Ok(new { Status = _result });

            }
            catch (Exception ex)
            {
                // Log the exception and send the email
                LogAndSendException(ex, "ListPackages Exception");
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }
        #endregion

        #region Company Subscription

        /// <summary>
        /// Save Company Subscription Packages APi
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// 
        [HttpPost]
        [Route("SaveCompanySubscriptionPackages")]
        public async Task<IActionResult> SaveCompanySubscriptionPackages(CompanySubscriptionRequest model)
        {
            try
            {
                var _result = await _ISubscription.SaveCompanySubscriptionPackages(model);
                return Ok(new { Status = _result, });

            }
            catch (Exception ex)
            {
                // Log the exception and send the email
                LogAndSendException(ex, "SavePackages Exception");
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        /// <summary>
        /// Company Subscriptio nPackages List
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("CompanySubscriptionPackagesList")]
        public async Task<IActionResult> CompanySubscriptionPackagesList(CompanySubscriptionRequestModel model)
        {
            try
            {


                var _result = await _ISubscription.CompanySubscriptionPackagesList(model);
                return Ok(new { Status = true, Packageslist = _result.CompanySubscriptionListResponse, totalRecords = _result.TotalRecords });

            }
            catch (Exception ex)
            {
                // Log the exception and send the email
                LogAndSendException(ex, "ListPackages Exception");
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }
        /// <summary>
        /// Get Company Subscription PackagesData For Drop Down
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetCompanySubscriptionPackagesDataForDD")]
        public async Task<IActionResult> GetCompanySubscriptionPackagesDataForDD()
        {
            try
            {
               var _result  = _ISubscription.GetCompanySubscriptionPackagesDataForDD();
                return Ok(new { DDCompanyList = _result.Result.DDCompanyList, DDpackageList = _result.Result.DDpackageList, subscriptionStatusList = _result.Result.subscriptionStatusList });
            }
            catch(Exception ex)
            {
                // Log the exception and send the email
                LogAndSendException(ex, "SavePackages Exception");
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }
        
        #endregion
        private void LogAndSendException(Exception ex, string msg)
        {
            // Log the exception
            Log.Error(ex, msg);
            // Send the exception email
            //_IEmailServices.SendExceptionEmail("athariqbal294@gmail.com", msg, ex.ToString());
        }
    }
}
