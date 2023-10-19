using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM_Domain.Model
{
    public class AdminDashboardResponse
    {
        public long? totalCompanyCount { get; set; }
        public long? totalActiveCompanyCount { get; set; }
        public long? totalInActiveCompanyCount { get; set; }
    }

    public class CompanyDashboardResponse
    {
        public long? totalEmployeesCount { get; set; }
        public long? totalActiveEmployeesCount { get; set; }
        public long? totalInActiveEmployeesCount { get; set; }
        public long? totalJobCount { get; set; }
        public long? totalActiveJobCount { get; set; }
        public long? totalInActiveJobCount { get; set; }
    }
}
