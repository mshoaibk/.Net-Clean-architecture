using HRM_Application.Interfaces;
using HRM_Domain.Model;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;
using System;
using Serilog;
using HRM_Core_WebApp.HubService;
using Microsoft.AspNetCore.SignalR;

namespace HRM_Core_WebApp.Controllers
{
    public class ChatController : Controller
    {
        #region Global

        //private readonly IHubContext<ChatHub> _hubContext; if u want to use signalR from controller side
        private readonly IchatService _IchatService;


        public ChatController(IchatService _IchatService)
        {
            this._IchatService = _IchatService;
        }
        #endregion
        /// <summary>
        /// Chet My Chats
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("getMyChats/{SignalRId}")]
        public async Task<IActionResult> getMyChats(string SignalRId)
        {
            try
            {
                var _result = await _IchatService.GetAllChatOfThatUserID(SignalRId);
                return Ok(new { Status = true,ChatList = _result });
            }
            catch (Exception ex)
            {
                // Log the exception and send the email
                LogAndSendException(ex, "SaveEmployee Exception");
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }
        /// <summary>
        /// Chet My Chats
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetMessages/{ChatID}")]
        public async Task<IActionResult> GetMessages (string ChatID)
        {
            try
            {
                var _result = await _IchatService.GetMessagesByChatID(ChatID);
                return Ok(new { Status = true, Mesagess = _result });
            }
            catch (Exception ex)
            {
                // Log the exception and send the email
                LogAndSendException(ex, "SaveEmployee Exception");
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
    }
}
