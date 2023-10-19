using HRM_Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM_Application.Interfaces {
    public interface IEmployeeDashboardServices {
        Task<EmployeeDashboardResponse> GetEmployeeDashboardData(EmployeeDashboardRequest model);
    }
}
