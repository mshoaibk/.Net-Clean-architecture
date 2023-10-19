using HRM_Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM_Application.Interfaces
{
    public interface IPostJobServices
    {
        Task<bool> SavePostJob(SavePostJobRequest model);
        Task<GetPostedJobModel> GetPostedJob(SearchPostedJobRequest model);
        Task<bool> DeletePostedJob(long postJobId);
        Task<bool> OnChangeJobActivationStatus(long postJobId, bool isActivated);
        Task<GetPostedJobModel> GetCarrerByComapny(SearchCarrerByComapnyRequest model);
    }
}
