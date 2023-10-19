using HRM_Application.Interfaces;
using HRM_Common.EnumClasses;
using HRM_Domain.Model;
using HRM_Infrastructure.HRMDataBaseContext;
using HRM_Infrastructure.TableEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM_Application.Services
{
    public class SubscriptionServices : ISubscription
    {
        private readonly HRMContexts dbContextHRM;
        public SubscriptionServices(HRMContexts context)
        {
            dbContextHRM = context;

        }

        #region Packages
        public async Task<bool> SavePackages(SubscriptionPackagesRequest model)
        {
            TblSubscriptionPackages tblPackagesObj = new TblSubscriptionPackages();
            if (model.action == "update")
            {
                tblPackagesObj = dbContextHRM.TblSubscriptionPackages.Where(emp => emp.PackageId == model.packageId).FirstOrDefault();
            }
            //
            // tblPackagesObj. PackageId
            tblPackagesObj.PackageName = model.packageName;
            tblPackagesObj.DurationInYears = model.durationInYears;
            tblPackagesObj.DurationInMonths = model.durationInMonths;
            tblPackagesObj.DepartmentLimit = model.departmentLimit;
            tblPackagesObj.OfficeLimit = model.officeLimit;
            tblPackagesObj.TeamLimit = model.teamLimit;
            tblPackagesObj.PositionLimit = model.positionLimit;
            tblPackagesObj.EmployeeLimit = model.employeeLimit;
            tblPackagesObj.RoleLimit = model.roleLimit;
            tblPackagesObj.LogsLimit = model.logsLimit;
            tblPackagesObj.Price = model.price;
            if (model.action == "save")
            {
                tblPackagesObj.IsDeleted = false;
                tblPackagesObj.CreatedBy = "SAdmin";
                tblPackagesObj.CreatedDate = DateTime.Now; ;
                dbContextHRM.TblSubscriptionPackages.Add(tblPackagesObj);
            }
            else
            {
                dbContextHRM.Update(tblPackagesObj);
            }
            dbContextHRM.SaveChanges();

            return true;
        }
        public async Task<GetSubscriptionPackagesModel> ListPackages(SubscriptionPackagesRequestModel model)
        {
            int skipCount = model.pageSize * model.pageIndex;
            int takeCount = model.pageSize;
            GetSubscriptionPackagesModel obj = new GetSubscriptionPackagesModel();
            IQueryable<SubscriptionPackagesList> subscriptionPackagesResult;
            //
            subscriptionPackagesResult = dbContextHRM.TblSubscriptionPackages.Where(x => x.IsDeleted == false).Select(x => new SubscriptionPackagesList
            {
                packageId = x.PackageId,
                packageName = x.PackageName,
                durationInYears = x.DurationInYears,
                durationInMonths = x.DurationInMonths,
                departmentLimit = x.DepartmentLimit,
                officeLimit = x.OfficeLimit,
                teamLimit = x.TeamLimit,
                positionLimit = x.PositionLimit,
                employeeLimit = x.EmployeeLimit,
                roleLimit = x.RoleLimit,
                logsLimit = x.LogsLimit,
                price = x.Price,
            }).AsQueryable();



            obj.TotalRecords = subscriptionPackagesResult.Count();
            //Page size -1 is for all records
            if (model.pageSize == -1)
                obj.SubscriptionPackages = subscriptionPackagesResult.ToList();
            else
                obj.SubscriptionPackages = subscriptionPackagesResult.Skip(skipCount).Take(takeCount).ToList();

            return obj;
        }
        public async Task<SubscriptionPackagesList> GetpackageById(long Id)
        {
            var Data = dbContextHRM.TblSubscriptionPackages.Where(x=>x.PackageId == Id).Select(x=> new SubscriptionPackagesList
            {
                packageId=x.PackageId,
                packageName=x.PackageName,
                durationInYears = x.DurationInYears,
                durationInMonths = x.DurationInMonths,
                departmentLimit = x.DepartmentLimit,
                    officeLimit = x.OfficeLimit,
                    teamLimit = x.TeamLimit,
                    positionLimit = x.PositionLimit,
                    employeeLimit = x.EmployeeLimit,
                    roleLimit = x.RoleLimit,
                    logsLimit = x.LogsLimit,
                    price = x.Price,

            }).FirstOrDefault();
            return Data;
        }
        public async Task<bool> DeletePackages(long packageId)
        {
            var data = dbContextHRM.TblSubscriptionPackages.Where(x => x.PackageId == packageId).FirstOrDefault();
            data.IsDeleted = true;
            dbContextHRM.Update(data);
            dbContextHRM.SaveChanges();
            return true;
        }
        #endregion
        #region Company Subscription
        public async Task<bool> SaveCompanySubscriptionPackages(CompanySubscriptionRequest model)
        {

            TblCompanySubscription tblCompanySubscriptionObj = new TblCompanySubscription();
            if (model.action == "update")
            {
                tblCompanySubscriptionObj = dbContextHRM.TblCompanySubscription.Where(emp => emp.SubscriptionId == model.subscriptionId).FirstOrDefault();
            }
            //
           tblCompanySubscriptionObj.CompanyId = model.companyId;
            tblCompanySubscriptionObj.PackageId = model.packageId;
            
            tblCompanySubscriptionObj.IsPaid = false;
            tblCompanySubscriptionObj.PiadAmount = model.piadAmount;
           // tblCompanySubscriptionObj.TransectionId = ;
            tblCompanySubscriptionObj.PaymentStatus = ((SubscriptionStatus)3).ToString();
            tblCompanySubscriptionObj.SubscriptionStatus = ((PaymentStatus)model.subscriptionStatus).ToString();
           

            // tblPackagesObj. PackageId

            if (model.action == "save")
            {
                tblCompanySubscriptionObj.IsDeleted = false;
                tblCompanySubscriptionObj.CreatedDate = DateTime.Now;
                tblCompanySubscriptionObj.CreatedBy = "S-Admin"; 
                dbContextHRM.TblCompanySubscription.Add(tblCompanySubscriptionObj);
            }
            else
            {
                tblCompanySubscriptionObj.ModifiedDate = DateTime.Now;
                tblCompanySubscriptionObj.ModifiedBy = "S-Admin";
                dbContextHRM.Update(tblCompanySubscriptionObj);
            }
            dbContextHRM.SaveChanges();

            return true;
        }
        public async Task<GetCompanySubscriptionModel> CompanySubscriptionPackagesList(CompanySubscriptionRequestModel model)
        {
            int skipCount = model.pageSize * model.pageIndex;
            int takeCount = model.pageSize;
            GetCompanySubscriptionModel obj = new GetCompanySubscriptionModel();
            IQueryable<CompanySubscriptionListResponse> CompanySubscriptionResult;
            //
            CompanySubscriptionResult =(from Csub in dbContextHRM.TblCompanySubscription 
                                        join pkg in dbContextHRM.TblSubscriptionPackages on Csub.PackageId equals pkg.PackageId
                                        join comp in dbContextHRM.tblCompanyDetail on Csub.CompanyId equals comp.CompanyID
                                        where comp.IsDeleted == false select new CompanySubscriptionListResponse
                                        {
                                            subscriptionId = Csub.SubscriptionId,
                                            companyId = Csub.CompanyId,
                                            companyName = comp.CompanyName,
                                            packageId = Csub.PackageId,
                                            packageName = pkg.PackageName,
                                            createdDate = comp.CreatedDate,
                                            isPaid = Csub.IsPaid,
                                            piadAmount = Csub.PiadAmount,
                                            paymentStatus = Csub.PaymentStatus,
                                            subscriptionStatus = Csub.SubscriptionStatus,
                                            expiryDate = Csub.ExpiryDate,
                                            transectionId = Csub.TransectionId, 
                                        }).AsQueryable();



            obj.TotalRecords = CompanySubscriptionResult.Count();
            //Page size -1 is for all records
            if (model.pageSize == -1)
                obj.CompanySubscriptionListResponse = CompanySubscriptionResult.ToList();
            else
                obj.CompanySubscriptionListResponse = CompanySubscriptionResult.Skip(skipCount).Take(takeCount).ToList();

            return obj;
        }
        public async Task<CompanySubscriptioDD> GetCompanySubscriptionPackagesDataForDD()
        {
            CompanySubscriptioDD obj = new CompanySubscriptioDD();
            obj.DDCompanyList = dbContextHRM.tblCompanyDetail
                                .Where(x=>x.IsDeleted == false).
                                Select(x=> new DDCompany {
                                    companyId = x.CompanyID,
                                    companyName = x.CompanyName 
                                }).ToList();
                                
            obj.DDpackageList = dbContextHRM.TblSubscriptionPackages
                                .Where(x => x.IsDeleted == false)
                                .Select(x => new DDpackage
                                { packageId = x.PackageId,
                                    packageName = x.PackageName,
                                    Amount = x.Price
                                }).ToList();
                                
            obj.subscriptionStatusList = Enum.GetValues(typeof(TicketStatus))
                                        .Cast<TicketStatus>()
                                        .Select(status => new subscriptionStatusInfo
                                        {
                                            ID = (int)status,
                                            Name = status.ToString()
                                        }).ToList();
                                        
            return obj;
        }
        #endregion
    }
}
