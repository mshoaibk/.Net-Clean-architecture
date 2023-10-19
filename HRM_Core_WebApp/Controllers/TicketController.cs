using HRM_Application.Interfaces;
using HRM_Domain.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using Serilog;
using System.IO;
using System.Net.Http.Headers;

namespace HRM_Core_WebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        private readonly ITicketServices _ITicketServices;

        public TicketController(ITicketServices ITicketServices)
        {
            _ITicketServices = ITicketServices;
        }
        #region Tickets

        /// <summary>
        /// Save Ticket
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("SaveTicket")]
        public async Task<IActionResult> SaveTicket(TicketRequest model)
        {
            try
            {
                var _result = await _ITicketServices.SaveTicket(model);
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
        /// List Ticket
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("ListTicket")]
        public async Task<IActionResult> ListTicket(TicketRequestModel model)
        {
            try
            {
                var _result = await _ITicketServices.ListTicket(model);
                return Ok(new { Status = true, Packageslist = _result.ticketResponseList, totalRecords = _result.totalRecords });
            }
            catch (Exception ex)
            {
                // Log the exception and send the email
                LogAndSendException(ex, "ListPackages Exception");
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        /// <summary>
        /// Get TicketBy Id
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetTicketById/{id}")]
        public async Task<IActionResult> GetTicketById(long id)
        {
            try
            {
                var _result = await _ITicketServices.GetTicketById(id);
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
        /// Delete Ticket
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("DeleteTicket/{id}")]
        public async Task<IActionResult> DeleteTicket(long id)
        {
            try
            {
                var _result = await _ITicketServices.DeleteTicket(id);
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

        #region Ticket Comments

        /// <summary>
        /// Save Ticket Comment
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("SaveTicketComment")]
        public async Task<IActionResult> SaveTicketComment([FromForm]  Ticket_CommentRequest model)
        {
            try
            {
                if (model.photoType == "uploadedurl")
                {
                    var folderName = Path.Combine("Resources", "Images");
                    var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                    var fileName = ContentDispositionHeaderValue.Parse(model.employeePhoto.ContentDisposition).FileName.Trim('"');
                    var fullPath = Path.Combine(pathToSave, fileName);
                    var dbPath = Path.Combine(folderName, fileName);
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        model.employeePhoto.CopyTo(stream);
                    }
                    model.photopath = dbPath.ToString();
                }


                var _result = await _ITicketServices.SaveTicketComment(model);
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
        /// Get Ticket Comment
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetTicketComment")]
        public async Task<IActionResult> GetTicketComment(Ticket_CommentRequestModel model)
        {
            try
            {
                var _result = await _ITicketServices.GetTicketComment(model);
                return Ok(new { Status = true ,TotalComments = _result.totalRecords,Comments = _result.TicketCommentList });
            }
            catch (Exception ex)
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
