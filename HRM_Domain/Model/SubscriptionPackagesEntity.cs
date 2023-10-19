using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM_Domain.Model
{
    #region Packages
   
    public class SubscriptionPackagesRequest
    {

        public long? packageId { get; set; }
        public string packageName { get; set; }
        public int? durationInYears { get; set; }
        public int? durationInMonths { get; set; }
        public int? departmentLimit { get; set; }
        public int? officeLimit { get; set; }
        public int? teamLimit { get; set; }
        public int? positionLimit { get; set; }
        public int? employeeLimit { get; set; }
        public int? roleLimit { get; set; }
        public int? logsLimit { get; set; }
        public decimal? price { get; set; }
        public string action { get; set; }
   

    }
    public class SubscriptionPackagesRequestModel
    {
        public long? packageId { get; set; }
        public int pageIndex { get; set; }
        public int pageSize { get; set; }
    }
    public class SubscriptionPackagesList
    {
        public long packageId { get; set; }
        public string packageName { get; set; }
        public int? durationInYears { get; set; }
        public int? durationInMonths { get; set; }
        public int? departmentLimit { get; set; }
        public int? officeLimit { get; set; }
        public int? teamLimit { get; set; }
        public int? positionLimit { get; set; }
        public int? employeeLimit { get; set; }
        public int? roleLimit { get; set; }
        public int? logsLimit { get; set; }
        public decimal? price { get; set; }


    }
    public class GetSubscriptionPackagesModel
    {
        public List<SubscriptionPackagesList> SubscriptionPackages { get; set; }
        public Int32 TotalRecords { get; set; }
    }
    #endregion

    #region Subscription
    public class CompanySubscriptionRequest
    {
      public long?  subscriptionId { get; set; }
        public long companyId { get; set; }
        public long packageId { get; set; }
       // public DateTime? createdDate { get; set; }
       // public string createdBy { get; set; }
        //public bool? isPaid { get; set; }
       // public DateTime? expiryDate { get; set; }
        public decimal? piadAmount { get; set; }
      //  public long? transectionId { get; set; }
      //  public DateTime? modifiedDate { get; set; }
       // public string modifiedBy { get; set; }
       // public int? paymentStatus { get; set; }
        public int? subscriptionStatus { get; set; }
        public string action { get; set; }

    }

    public class CompanySubscriptionRequestModel
    {
        public long? subscriptionId { get; set; }
        public int pageIndex { get; set; }
        public int pageSize { get; set; }
    }

    public class CompanySubscriptionListResponse
    {
        public long? subscriptionId { get; set; }
        public long? companyId { get; set; }
        public string companyName { get; set; }
        public long? packageId { get; set; }
        public string packageName { get; set; }
        public DateTime? createdDate { get; set; }
        public string createdBy { get; set; }
        public bool? isPaid { get; set; }
        public DateTime? expiryDate { get; set; }
        public decimal? piadAmount { get; set; }
        public long? transectionId { get; set; }
        public DateTime? modifiedDate { get; set; }
        public string modifiedBy { get; set; }
        public string paymentStatus { get; set; }
        public string subscriptionStatus { get; set; }


    }
    public class GetCompanySubscriptionModel
    {
        public List<CompanySubscriptionListResponse> CompanySubscriptionListResponse { get; set; }
        public Int32 TotalRecords { get; set; }
    }
    public class DDCompany
    {
        public long companyId { get; set; }
        public string companyName { get; set; }
    }
    public class DDpackage
    {
        public long packageId { get; set; }
        public string packageName { get; set; }
        public decimal? Amount { get; set; }
    }
    public class subscriptionStatusInfo
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }

    public class CompanySubscriptioDD
    {
        public List<DDCompany> DDCompanyList { get; set; }
        public List<DDpackage> DDpackageList { get; set; }
        public List<subscriptionStatusInfo> subscriptionStatusList { get; set; }

    }

    #endregion
}
