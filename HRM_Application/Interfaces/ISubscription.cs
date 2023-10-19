using HRM_Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM_Application.Interfaces
{
    public interface ISubscription
    {
        #region CreatePackages
        Task<bool> SavePackages(SubscriptionPackagesRequest model);
        Task<GetSubscriptionPackagesModel> ListPackages(SubscriptionPackagesRequestModel model);
        Task<SubscriptionPackagesList> GetpackageById(long Id);
        Task<bool> DeletePackages(long packageId);
        #endregion

        #region Packages Subscription
        Task<bool> SaveCompanySubscriptionPackages(CompanySubscriptionRequest model);
        Task<GetCompanySubscriptionModel> CompanySubscriptionPackagesList(CompanySubscriptionRequestModel model);
        public  Task<CompanySubscriptioDD> GetCompanySubscriptionPackagesDataForDD();
        #endregion
    }
}
