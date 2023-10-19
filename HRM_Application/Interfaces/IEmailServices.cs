using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM_Application.Interfaces
{
    public interface IEmailServices
    {
        void SendExceptionEmail(string to, string subject, string body);
    }
}
