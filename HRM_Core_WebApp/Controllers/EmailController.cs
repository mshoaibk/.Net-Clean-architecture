using HRM_Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HRM_Core_WebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        #region Global

        private readonly IEmailServices _IEmailService;

        public EmailController(IEmailServices _IEmailServices)
        {
            this._IEmailService = _IEmailServices;
        }
        #endregion

    }
}
