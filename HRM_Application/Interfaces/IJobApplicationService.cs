using HRM_Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM_Application.Interfaces
{
    public interface IJobApplicationService
    {
        Task<bool> ApplyJob(ApplyJobEntity model);
        Task<ShowAppliedApplicationReturnModel> ShowAppliedApplication(ShowAppliedApplicationRequestModel model);
        Task<ShowCandidatesApplicationReturnModel> ShowCandidatesApplication(ShowCandidatesApplicationRequestModel model);
        Task<bool> UpdateCandidatesApplication(UpdateCandidatesApplicationRequestModel model);
        Task<GetPostedJobByIdResponse> GetPostedJobById(long postJobId);       
    }
}
