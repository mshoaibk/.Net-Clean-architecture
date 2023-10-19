using HRM_Application.Interfaces;
using HRM_Domain.Model;
using HRM_Infrastructure.HRMDataBaseContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM_Application.Services
{
    public class DashboardServices: IDashboardServices
    {
        private readonly HRMContexts dbContextHRM;

        public DashboardServices(HRMContexts context)
        {
            dbContextHRM = context;
        }

        public async Task<AdminDashboardResponse> GetAdminDashboardData()
        {
            AdminDashboardResponse obj = new AdminDashboardResponse();
            obj.totalCompanyCount = dbContextHRM.tblCompanyDetail.Count();
            obj.totalActiveCompanyCount = dbContextHRM.tblCompanyDetail.Where(x=>x.IsActivated==true).Count();
            obj.totalInActiveCompanyCount = dbContextHRM.tblCompanyDetail.Where(x => x.IsActivated == false).Count();
            return obj;
        }

        public async Task<CompanyDashboardResponse> GetCompanyDashboardData(long companyID)
        {
            CompanyDashboardResponse obj = new CompanyDashboardResponse();
            obj.totalEmployeesCount = dbContextHRM.tblEmployee.Count();
            obj.totalActiveEmployeesCount = dbContextHRM.tblEmployee.Where(x => x.IsActivated == true).Count();
            obj.totalInActiveEmployeesCount = dbContextHRM.tblEmployee.Where(x => x.IsActivated == false).Count();
            obj.totalJobCount = dbContextHRM.tblPostJob.Count();
            //obj.totalActiveJobCount = dbContextHRM.tblPostJob.Where(x => x.IsActivated == true).Count();
            //obj.totalInActiveJobCount = dbContextHRM.tblPostJob.Where(x => x.IsActivated == false).Count();
            return obj;
        }

        public async Task<GetCountByCompanyIdResponse> GetCompanyDashboardCount(long companyId, string requiredCount)
        {
            GetCountByCompanyIdResponse obj = new GetCountByCompanyIdResponse();
            if (requiredCount == "dashboardcount")
            {
                obj.jobApplicationCount = dbContextHRM.tblCandidateJobApplications
                    .Where(candidateJob => candidateJob.IsDeleted == false && candidateJob.CompanyId == companyId)
                    .Count();
            }
            return obj;
        }
    }
}
