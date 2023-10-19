using HRM_Application.Services;
using HRM_Common.EnumClasses;
using HRM_Domain.Model;
using HRM_Infrastructure.HRMDataBaseContext;
using HRM_Infrastructure.TableEntities;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser.clipper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data.Entity;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection.Metadata;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using static System.Net.Mime.MediaTypeNames;

namespace HRM_Application.Interfaces
{
    public class EmployeeServices : IEmployeeServices
    {
        private readonly HRMContexts dbContextHRM;
        private readonly IEmployeeAttendanceServices _IAttendanceServices;

        public EmployeeServices(HRMContexts context, IEmployeeAttendanceServices iAttendanceServices)
        {
            dbContextHRM = context;
            _IAttendanceServices = iAttendanceServices;
        }
        public async Task<bool> SaveEmployee(EmployeeSaveRequest model)
        {
            TblEmployees tblEmployeeObj = new TblEmployees();
            if (model.action == "update")
            {
                tblEmployeeObj = dbContextHRM.tblEmployee.Where(emp => emp.EmployeeID == model.employeeID).FirstOrDefault();
            }
            var userID = dbContextHRM.tblAspNetUsers.Where(x => x.Email == model.email).Select(x => x.Id).FirstOrDefault();
            tblEmployeeObj.CompanyID = model.companyID;
            tblEmployeeObj.FullName = model.fullName;
            tblEmployeeObj.Email = model.email;
            tblEmployeeObj.Gender = model.gender;
            tblEmployeeObj.EmploymentType = model.employmentType;
            tblEmployeeObj.Position = model.position;
            tblEmployeeObj.Team = model.team;
            tblEmployeeObj.Department = model.department;
            tblEmployeeObj.Office = model.office;
            tblEmployeeObj.Supervisor = "";
            tblEmployeeObj.HireDate = Convert.ToDateTime(model.hireDate);
            tblEmployeeObj.ProbationPeriod = model.probationPeriod;
            tblEmployeeObj.ContractEnd = Convert.ToDateTime(model.contractEnd);
            tblEmployeeObj.WorkType = model.workType;
            tblEmployeeObj.WorkingSchedule = model.workingSchedule;
            tblEmployeeObj.PaidVacation = model.paidVacation;
            tblEmployeeObj.IsDeleted = model.isDeleted;
            tblEmployeeObj.CreatedBy = model.createdBy;
            tblEmployeeObj.CreatedDate = DateTime.Now;
            tblEmployeeObj.ModifiedBy = "";
            tblEmployeeObj.ModifiedDate = DateTime.Now;
            tblEmployeeObj.UserID = userID;
            tblEmployeeObj.IsActivated = true;
            tblEmployeeObj.EmployeePhoto = model.employeePhotoName;
            tblEmployeeObj.PhotoType = model.PhotoType;
            tblEmployeeObj.PhoneNumber = model.phoneNumber;
            tblEmployeeObj.Qualification = model.qualification;
            tblEmployeeObj.Experience = model.experience;
            tblEmployeeObj.NumberOfLeavesAllowed = model.numberOfLeavesAllowed;
            tblEmployeeObj.ShiftId = model.shiftId;
            if (model.action == "save")
            {
                dbContextHRM.tblEmployee.Add(tblEmployeeObj);
                TblSignalR_User signalR_User = new TblSignalR_User();
                signalR_User.photoPath = tblEmployeeObj.EmployeePhoto;
                signalR_User.Name = tblEmployeeObj.FullName;
                signalR_User.ActualUserID = tblEmployeeObj.EmployeeID;
                signalR_User.Type = ((ChatType)Convert.ToInt64(2)).ToString();
                dbContextHRM.TblSignalR_User.Add(signalR_User);
            }
            else
            {
                dbContextHRM.Update(tblEmployeeObj);
            }
            dbContextHRM.SaveChanges();
            string[] supervisorList = !string.IsNullOrEmpty(model.supervisedBy) ? model.supervisedBy.Split(',') : new string[0];
            string[] teamMemberList = !string.IsNullOrEmpty(model.teamMember) ? model.teamMember.Split(',') : new string[0];
            var remove = dbContextHRM.tblHierarchyEmployees.Where(x => x.SelfEmployeeId == model.employeeID).ToList();
            if (remove != null) {
                dbContextHRM.tblHierarchyEmployees.RemoveRange(remove);
                await dbContextHRM.SaveChangesAsync();
            }
            foreach (var item in supervisorList)
            {
                long? employeeId = Convert.ToInt64(item);
                TblHierarchyEmployees tblHierarchyEmployees = new TblHierarchyEmployees();
                tblHierarchyEmployees.CompanyId = model.companyID;
                tblHierarchyEmployees.EmployeeId = employeeId;
                tblHierarchyEmployees.hierarchyStatus = "supervisor";
                tblHierarchyEmployees.SelfEmployeeId = tblEmployeeObj.EmployeeID;
                dbContextHRM.tblHierarchyEmployees.Add(tblHierarchyEmployees);
            }

            foreach (var item in teamMemberList)
            {
                long? employeeId = Convert.ToInt64(item);
                TblHierarchyEmployees tblHierarchyEmployees = new TblHierarchyEmployees();
                tblHierarchyEmployees.CompanyId = model.companyID;
                tblHierarchyEmployees.EmployeeId = employeeId;
                tblHierarchyEmployees.hierarchyStatus = "members";
                tblHierarchyEmployees.SelfEmployeeId = tblEmployeeObj.EmployeeID;
                dbContextHRM.tblHierarchyEmployees.Add(tblHierarchyEmployees);
            }
            dbContextHRM.SaveChanges();
            SaveEmployeeSalaryRequestModel obj = new SaveEmployeeSalaryRequestModel();
            if (model.action == "save") {
                obj.payType = model.payType;
                obj.monthlyPay = model.monthlyPay;
                obj.hourlyPay = model.hourlyPay;
                obj.hoursWorked = model.hoursWorked;
                obj.dailyPay = model.dailyPay;
                obj.weeklyPay = model.weeklyPay;
                obj.weeksWorked = model.weeksWorked;
                obj.currency = model.currency;
                obj.employeeID = tblEmployeeObj.EmployeeID;
                obj.appliedTaxPercentage = model.appliedTaxPercentage;
                obj.action = "save";
                await SaveEmployeeSalary(obj);
            } else {
                obj.payType = model.payType;
                obj.monthlyPay = model.monthlyPay;
                obj.hourlyPay = model.hourlyPay;
                obj.hoursWorked = model.hoursWorked;
                obj.dailyPay = model.dailyPay;
                obj.weeklyPay = model.weeklyPay;
                obj.weeksWorked = model.weeksWorked;
                obj.currency = model.currency;
                obj.employeeID = model.employeeID;
                obj.employeeSalaryID = model.employeeSalaryID;
                obj.appliedTaxPercentage = model.appliedTaxPercentage;
                obj.action = "update";
                await SaveEmployeeSalary(obj);
            }
            return true;
        }

        public async Task<GetEmployeeListModel> GetEmployeeList(SearchGetEmployeeListRequest model)
        {
            int skipCount = model.pageSize * model.pageIndex;
            int takeCount = model.pageSize;
            GetEmployeeListModel obj = new GetEmployeeListModel();
            IQueryable<GetEmployeeResponse> employeeDetailResult;
            employeeDetailResult = (from employeeObj in dbContextHRM.tblEmployee
                                    join officeObj in dbContextHRM.tblOfficeLocation
                                    on employeeObj.Office equals officeObj.OfficeId.ToString() into officeGroup
                                    from officeObj in officeGroup.DefaultIfEmpty()
                                    join deptObj in dbContextHRM.tblDepartment
                                    on employeeObj.Department equals deptObj.DepartmentId.ToString() into deptGroup
                                    from deptObj in deptGroup.DefaultIfEmpty()
                                    join teamObj in dbContextHRM.tblTeam
                                    on employeeObj.Team equals teamObj.TeamId.ToString() into teamGroup
                                    from teamObj in teamGroup.DefaultIfEmpty()
                                    join positionObj in dbContextHRM.tblPosition
                                    on employeeObj.Position equals positionObj.PositionId.ToString() into posGroup
                                    from positionObj in posGroup.DefaultIfEmpty()
                                    where employeeObj.IsDeleted == false &&
                                    (model.fullName == "" ||
                                    !string.IsNullOrEmpty(model.fullName) &&
                                    (employeeObj.FullName.Contains(model.fullName)))
                                    &&
                                    (model.employmentType == "" ||
                                    !string.IsNullOrEmpty(model.employmentType) &&
                                    (employeeObj.EmploymentType.Contains(model.employmentType)))
                                    &&
                                    (model.department == "" ||
                                    !string.IsNullOrEmpty(model.department) &&
                                    (employeeObj.Department.Contains(model.department)))
                                    &&
                                    (model.team == "" ||
                                    !string.IsNullOrEmpty(model.team) &&
                                    (employeeObj.Team.Contains(model.team)))
                                    select new GetEmployeeResponse
                                    {
                                        employeeID = employeeObj.EmployeeID,
                                        companyID = employeeObj.CompanyID,
                                        fullName = employeeObj.FullName,
                                        email = employeeObj.Email,
                                        gender = employeeObj.Gender,
                                        employmentType = employeeObj.EmploymentType,
                                        position = positionObj.PositionName,
                                        team = teamObj.TeamName,
                                        department = deptObj.DepartmentName,
                                        office = officeObj.OfficeLocationName,
                                        supervisor = dbContextHRM.tblEmployee.Where(x=>x.EmployeeID.ToString()==employeeObj.Supervisor).Select(x=>x.Supervisor).FirstOrDefault(),
                                        hireDate = employeeObj.HireDate.ToString(),
                                        probationPeriod = employeeObj.ProbationPeriod,
                                        contractEnd = employeeObj.ContractEnd.ToString(),
                                        workType = employeeObj.WorkType,
                                        workingSchedule = employeeObj.WorkingSchedule,
                                        paidVacation = employeeObj.PaidVacation,
                                        createdBy = employeeObj.CreatedBy,
                                        createdDate = employeeObj.CreatedDate,
                                        modifiedBy = employeeObj.ModifiedBy,
                                        modifiedDate = employeeObj.ModifiedDate,
                                        IsActivated = employeeObj.IsActivated == null ? false : employeeObj.IsActivated,
                                        UserID = employeeObj.UserID,
                                        employeePhoto = employeeObj.EmployeePhoto,
                                        photoType = employeeObj.PhotoType
                                    });
            obj.TotalRecords = employeeDetailResult.Count();
            //Page size -1 is for all records
            if (model.pageSize == -1)
                obj.EmployeeList = employeeDetailResult.ToList();
            else
                obj.EmployeeList = employeeDetailResult.Skip(skipCount).Take(takeCount).ToList();

            return obj;
        }

        public async Task<bool> DeleteEmployeeDetail(long employeeID)
        {
            var employeeObject = dbContextHRM.tblEmployee.Where(x => x.EmployeeID == employeeID).FirstOrDefault();
            if (employeeObject == null)
            {
                return false;
            }
            employeeObject.IsDeleted = true;
            dbContextHRM.SaveChanges();
            return true;
        }

        public async Task<bool> OnChangeEmployeeStatus(long employeeID, bool isActivated)
        {
            var employeeObject = dbContextHRM.tblEmployee.Where(x => x.EmployeeID == employeeID).FirstOrDefault();
            if (employeeObject == null)
            {
                return false;
            }
            employeeObject.IsActivated = isActivated;
            dbContextHRM.SaveChanges();
            return true;
        }

        public async Task<GetEmployeeBasicDetailModel> GetEmployeeBasicDetailForCompany(SearchGetEmployeeBasicDetailRequest model)
        {
            int skipCount = model.pageSize * model.pageIndex;
            int takeCount = model.pageSize;
            GetEmployeeBasicDetailModel obj = new GetEmployeeBasicDetailModel();
            IQueryable<GetEmployeeBasicDetailResponse> employeeDetailResult;
            employeeDetailResult = (from employeeObj in dbContextHRM.tblEmployee
                                    join officeObj in dbContextHRM.tblOfficeLocation
                                    on employeeObj.Office equals officeObj.OfficeId.ToString() into officeGroup
                                    from officeObj in officeGroup.DefaultIfEmpty()
                                    join deptObj in dbContextHRM.tblDepartment
                                    on employeeObj.Department equals deptObj.DepartmentId.ToString() into deptGroup
                                    from deptObj in deptGroup.DefaultIfEmpty()
                                    join teamObj in dbContextHRM.tblTeam
                                    on employeeObj.Team equals teamObj.TeamId.ToString() into teamGroup
                                    from teamObj in teamGroup.DefaultIfEmpty()
                                    join positionObj in dbContextHRM.tblPosition
                                    on employeeObj.Position equals positionObj.PositionId.ToString() into posGroup
                                    from positionObj in posGroup.DefaultIfEmpty()
                                    join empTypeObj in dbContextHRM.tblEmploymentTypeSetup
                                    on employeeObj.EmploymentType equals empTypeObj.EmploymentTypeId.ToString() into typeGroup
                                    from empTypeObj in typeGroup.DefaultIfEmpty()
                                    where employeeObj.IsDeleted == false
                                    && employeeObj.CompanyID == model.companyID
                                    &&
                                    (model.employmentType == "" ||
                                    !string.IsNullOrEmpty(model.employmentType) &&
                                    (employeeObj.EmploymentType.Contains(model.employmentType)))
                                    &&
                                    (model.location == "" ||
                                    !string.IsNullOrEmpty(model.location) &&
                                    (employeeObj.Office.Contains(model.location)))
                                    &&
                                    (model.department == "" ||
                                    !string.IsNullOrEmpty(model.department) &&
                                    (employeeObj.Department.Contains(model.department)))
                                    &&
                                    (model.position == "" ||
                                    !string.IsNullOrEmpty(model.position) &&
                                    (employeeObj.Position.Contains(model.position)))
                                    &&
                                    (model.employeeName == "" ||
                                    !string.IsNullOrEmpty(model.employeeName) &&
                                    (employeeObj.FullName.Contains(model.employeeName)))
                                    &&
                                    (model.employmentStatus == "" || model.employmentStatus == "all" ||
                                    (model.employmentStatus == "active" && employeeObj.IsActivated==true) ||
                                    (model.employmentStatus == "deactive" && employeeObj.IsActivated==false))
                                    select new GetEmployeeBasicDetailResponse
                                    {
                                        employeeID = employeeObj.EmployeeID,
                                        companyID = employeeObj.CompanyID,
                                        fullName = employeeObj.FullName,
                                        email = employeeObj.Email,
                                        employmentType = employeeObj.EmploymentType,
                                        position = positionObj.PositionName,
                                        department = deptObj.DepartmentName,
                                        office = officeObj.OfficeLocationName,
                                        employeePhoto = employeeObj.EmployeePhoto,
                                        photoType = employeeObj.PhotoType,
                                        UserID = employeeObj.UserID,
                                        workType=employeeObj.WorkType,
                                        IsActivated=employeeObj.IsActivated
                                    });
            obj.TotalRecords = employeeDetailResult.Count();
            //Page size -1 is for all records
            if (model.pageSize == -1)
                obj.EmployeeList = employeeDetailResult.ToList();
            else
                obj.EmployeeList = employeeDetailResult.Skip(skipCount).Take(takeCount).ToList();

            return obj;
        }

        public async Task<ShowCompleteEmployeeDetailRequestResponse> ShowCompleteEmployeeDetail(ShowCompleteEmployeeDetailRequest model)
        {
            GetEmployeeBasicDetailModel obj = new GetEmployeeBasicDetailModel();
            ShowCompleteEmployeeDetailRequestResponse employeeDetailResult;
            employeeDetailResult = (from employeeObj in dbContextHRM.tblEmployee
                                    join officeObj in dbContextHRM.tblOfficeLocation
                                    on employeeObj.Office equals officeObj.OfficeId.ToString() into officeGroup
                                    from officeObj in officeGroup.DefaultIfEmpty()
                                    join deptObj in dbContextHRM.tblDepartment
                                    on employeeObj.Department equals deptObj.DepartmentId.ToString() into deptGroup
                                    from deptObj in deptGroup.DefaultIfEmpty()
                                    join teamObj in dbContextHRM.tblTeam
                                    on employeeObj.Team equals teamObj.TeamId.ToString() into teamGroup
                                    from teamObj in teamGroup.DefaultIfEmpty()
                                    join positionObj in dbContextHRM.tblPosition
                                    on employeeObj.Position equals positionObj.PositionId.ToString() into posGroup
                                    from positionObj in posGroup.DefaultIfEmpty()
                                    join empTypeObj in dbContextHRM.tblEmploymentTypeSetup
                                    on employeeObj.EmploymentType equals empTypeObj.EmploymentTypeId.ToString() into typeGroup
                                    from empTypeObj in typeGroup.DefaultIfEmpty()
                                    where employeeObj.IsDeleted == false
                                    && (model.employeeID == 0 ||
                                    model.employeeID != 0 &&
                                    (employeeObj.EmployeeID.Equals(model.employeeID)))
                                    && (model.userID == "" ||
                                    !string.IsNullOrEmpty(model.userID) &&
                                    (employeeObj.UserID.Equals(model.userID)))
                                    select new ShowCompleteEmployeeDetailRequestResponse
                                    {
                                        employeeID = employeeObj.EmployeeID,
                                        companyID = employeeObj.CompanyID,
                                        fullName = employeeObj.FullName,
                                        email = employeeObj.Email,
                                        gender = employeeObj.Gender,
                                        employmentType = empTypeObj.EmploymentTypeId.ToString(),
                                        position = positionObj.PositionName,
                                        team = teamObj.TeamName,
                                        department = deptObj.DepartmentName,
                                        office = officeObj.OfficeLocationName,
                                        positionId = positionObj.PositionId,
                                        teamId = teamObj.TeamId,
                                        departmentId = deptObj.DepartmentId,
                                        officeId = officeObj.OfficeId,
                                        supervisor = "",
                                        supervisorId = 0,
                                        hireDate = employeeObj.HireDate.ToString(),
                                        probationPeriod = employeeObj.ProbationPeriod,
                                        contractEnd = employeeObj.ContractEnd.ToString(),
                                        workType = employeeObj.WorkType,
                                        workingSchedule = employeeObj.WorkingSchedule,
                                        paidVacation = employeeObj.PaidVacation,
                                        createdBy = employeeObj.CreatedBy,
                                        createdDate = employeeObj.CreatedDate,
                                        modifiedBy = employeeObj.ModifiedBy,
                                        modifiedDate = employeeObj.ModifiedDate,
                                        IsActivated = employeeObj.IsActivated == null ? false : employeeObj.IsActivated,
                                        UserID = employeeObj.UserID,
                                        employeePhoto = employeeObj.EmployeePhoto,
                                        photoType = employeeObj.PhotoType,
                                        experience = employeeObj.Experience,
                                        qualification = employeeObj.Qualification,
                                        phoneNumber = employeeObj.PhoneNumber,
                                        numberOfLeavesAllowed = employeeObj.NumberOfLeavesAllowed ==null?0: employeeObj.NumberOfLeavesAllowed,
                                        shiftId = employeeObj.ShiftId ==null?0 : employeeObj.ShiftId
                                    }).FirstOrDefault();

            var hierarchyResult = dbContextHRM.tblHierarchyEmployees
                                        .Where(p => p.CompanyId == employeeDetailResult.companyID && p.SelfEmployeeId == model.employeeID)
                                        .ToList();
            employeeDetailResult.supervisorDDLResponse = hierarchyResult.Where(x => x.hierarchyStatus == "supervisor").Select(c => c.EmployeeId).ToList();
            employeeDetailResult.teamMemberDDLResponse = hierarchyResult.Where(x => x.hierarchyStatus == "members").Select(c => c.EmployeeId).ToList();
            employeeDetailResult.getEmployeeSalariesList = (from employeeSalaryObj in dbContextHRM.tblEmployeeSalary
                                                            where employeeSalaryObj.IsDeleted == false
                                                            && employeeSalaryObj.EmployeeID == model.employeeID
                                                            select new GetEmployeeSalariesList {
                                                                employeeSalaryID = employeeSalaryObj.EmployeeSalaryID,
                                                                payType=employeeSalaryObj.PayType,
                                                                currency=employeeSalaryObj.Currency,
                                                                monthlyPay= employeeSalaryObj.MonthlyPay,
                                                                hourlyPay=employeeSalaryObj.HourlyPay,
                                                                hoursWorked= employeeSalaryObj.HoursWorked,
                                                                dailyPay= employeeSalaryObj.DailyPay,
                                                                weeklyPay= employeeSalaryObj.WeeklyPay,
                                                                weeksWorked= employeeSalaryObj.WeeksWorked
                                                            }).FirstOrDefault();

            return employeeDetailResult;
        }

        public async Task<bool> SaveEmployeeSalary(SaveEmployeeSalaryRequestModel model)
        {
            TblEmployeeSalary tblEmployeeSalaryObj = new TblEmployeeSalary();
            if (model.action == "update")
            {
                tblEmployeeSalaryObj = dbContextHRM.tblEmployeeSalary.Where(emp => emp.EmployeeSalaryID == model.employeeSalaryID).FirstOrDefault();
            }
            var companyId = dbContextHRM.tblEmployee.Where(x => x.EmployeeID == model.employeeID).Select(x => x.CompanyID).FirstOrDefault();
            tblEmployeeSalaryObj.CompanyID = companyId;
            tblEmployeeSalaryObj.EmployeeID = model.employeeID;
            tblEmployeeSalaryObj.PayType = model.payType;
            tblEmployeeSalaryObj.Currency = model.currency;
            tblEmployeeSalaryObj.MonthlyPay = model.monthlyPay;
            tblEmployeeSalaryObj.HourlyPay = model.hourlyPay;
            tblEmployeeSalaryObj.HoursWorked = model.hoursWorked;
            tblEmployeeSalaryObj.DailyPay = model.dailyPay;
            tblEmployeeSalaryObj.WeeklyPay = model.weeklyPay;
            tblEmployeeSalaryObj.WeeksWorked = model.weeksWorked;
            tblEmployeeSalaryObj.AppliedTaxPercentage = model.appliedTaxPercentage;
            tblEmployeeSalaryObj.IsDeleted = false;
            if (model.action != "update")
            {
                tblEmployeeSalaryObj.CreatedBy = model.createdBy;
                tblEmployeeSalaryObj.CreatedDate = DateTime.Now;
            }
            if (model.action == "update")
            {
                tblEmployeeSalaryObj.ModifiedBy = model.modifiedBy;
                tblEmployeeSalaryObj.ModifiedDate = DateTime.Now;
            }
            if (model.action == "save")
            {
                dbContextHRM.tblEmployeeSalary.Add(tblEmployeeSalaryObj);
            }
            else
            {
                dbContextHRM.Update(tblEmployeeSalaryObj);
            }
            dbContextHRM.SaveChanges();
            return true;
        }
        public async Task<bool> DeleteEmployeeSalary(int employeeSalaryID)
        {
            var employeeObject = dbContextHRM.tblEmployeeSalary.Where(x => x.EmployeeSalaryID == employeeSalaryID).FirstOrDefault();
            if (employeeObject == null)
            {
                return false;
            }
            employeeObject.IsDeleted = true;
            dbContextHRM.SaveChanges();
            return true;
        }
        public async Task<SaveEmployeeSalaryRequestModel> GetEmployeeSalaryById(long employeeID)
        {
            SaveEmployeeSalaryRequestModel employeeSalaryDetailResult;
            employeeSalaryDetailResult = (from employeeSalaryObj in dbContextHRM.tblEmployeeSalary
                                          where employeeSalaryObj.IsDeleted == false
                                          && (employeeID == 0 ||
                                          employeeID != 0 &&
                                          (employeeSalaryObj.EmployeeID.Equals(employeeID)))
                                          select new SaveEmployeeSalaryRequestModel
                                          {
                                              employeeSalaryID = employeeSalaryObj.EmployeeSalaryID,
                                              employeeID = employeeSalaryObj.EmployeeID,
                                              payType = employeeSalaryObj.PayType,
                                              monthlyPay = employeeSalaryObj.MonthlyPay,
                                              hourlyPay = employeeSalaryObj.HourlyPay,
                                              hoursWorked = employeeSalaryObj.HoursWorked,
                                              dailyPay = employeeSalaryObj.DailyPay,
                                              weeklyPay = employeeSalaryObj.WeeklyPay,
                                              weeksWorked = employeeSalaryObj.WeeksWorked,
                                              currency = employeeSalaryObj.Currency,
                                              action = "update"
                                          }).FirstOrDefault();
            return employeeSalaryDetailResult;
        }

        public async Task<bool> SaveEmployeeSalarySlip(SaveEmployeeSalarySlipRequestModel model)
        {
            TblEmployeeSalarySlip tblEmployeeSalarySlipObj = new TblEmployeeSalarySlip();
            if (model.action == "update")
            {
                tblEmployeeSalarySlipObj = dbContextHRM.tblEmployeeSalarySlip
                    .Where(emp => emp.EmployeeSalarySlipID == model.EmployeeSalarySlipID).FirstOrDefault();
            }
            var companyId = dbContextHRM.tblEmployee.Where(x => x.EmployeeID == model.employeeID).Select(x => x.CompanyID).FirstOrDefault();
            tblEmployeeSalarySlipObj.CompanyID = companyId;
            tblEmployeeSalarySlipObj.EmployeeID = model.employeeID;
            tblEmployeeSalarySlipObj.EmployeeSalaryID = model.employeeSalaryID;
            tblEmployeeSalarySlipObj.Month = model.month;
            tblEmployeeSalarySlipObj.PaidOn = model.paidOn;
            tblEmployeeSalarySlipObj.BasicPay = model.basicPay;
            tblEmployeeSalarySlipObj.Allowances = model.allowances;
            tblEmployeeSalarySlipObj.Deductions = model.deductions;
            tblEmployeeSalarySlipObj.NetSalary = model.netSalary;
            tblEmployeeSalarySlipObj.IsDeleted = false;
            tblEmployeeSalarySlipObj.Year = model.year;
            tblEmployeeSalarySlipObj.Currency = model.currency;
            if (model.action == "update")
            {
                tblEmployeeSalarySlipObj.ModifiedBy = model.modifiedBy;
                tblEmployeeSalarySlipObj.ModifiedDate = DateTime.Now;
                dbContextHRM.Update(tblEmployeeSalarySlipObj);
            }
            else
            {
                tblEmployeeSalarySlipObj.CreatedBy = model.createdBy;
                tblEmployeeSalarySlipObj.CreatedDate = DateTime.Now;
                dbContextHRM.tblEmployeeSalarySlip.Add(tblEmployeeSalarySlipObj);
            }
            dbContextHRM.SaveChanges();
            return true;
        }

        public async Task<GetEmployeeSalaryByEmployeeIdSlipModel> GetEmployeeSalaryByEmployeeIdSlip(GetEmployeeSalaryByEmployeeIdSlipModelRequest model)
        {
            int skipCount = model.pageSize * model.pageIndex;
            int takeCount = model.pageSize;
            GetEmployeeSalaryByEmployeeIdSlipModel obj = new GetEmployeeSalaryByEmployeeIdSlipModel();
            IQueryable<GetEmployeeSalaryByEmployeeIdSlipResponse> GetEmployeeSalaryByEmployeeIdSlipResponse;
            GetEmployeeSalaryByEmployeeIdSlipResponse = (from employeeSalaryObj in dbContextHRM.tblEmployeeSalarySlip
                                                         where employeeSalaryObj.IsDeleted == false && employeeSalaryObj.EmployeeID == model.employeeID &&
                                                         (model.month == "" ||
                                                         !string.IsNullOrEmpty(model.month) &&
                                                         (employeeSalaryObj.Month.Equals(model.month)))
                                                         &&
                                                         (model.year == 0 ||
                                                         model.year != 0 &&
                                                         (employeeSalaryObj.PaidOn.Year.Equals(model.year)))
                                                         select new GetEmployeeSalaryByEmployeeIdSlipResponse
                                                         {
                                                             EmployeeSalarySlipID = employeeSalaryObj.EmployeeSalarySlipID,
                                                             employeeSalaryID = employeeSalaryObj.EmployeeSalaryID,
                                                             employeeID = employeeSalaryObj.EmployeeID,
                                                             month = employeeSalaryObj.Month,
                                                             paidOn = employeeSalaryObj.PaidOn,
                                                             paidFullDate = employeeSalaryObj.PaidOn.Day + " " + employeeSalaryObj.PaidOn.ToString("MMMM") + " " + employeeSalaryObj.PaidOn.Year,
                                                             basicPay = employeeSalaryObj.BasicPay,
                                                             allowances = employeeSalaryObj.Allowances,
                                                             deductions = employeeSalaryObj.Deductions,
                                                             netSalary = employeeSalaryObj.NetSalary,
                                                             createdBy = employeeSalaryObj.CreatedBy,
                                                             modifiedBy = employeeSalaryObj.ModifiedBy,
                                                             action = "update",
                                                             requestStatus= employeeSalaryObj.RequestStatus,
                                                             currency= employeeSalaryObj.Currency,
                                                         });
            obj.TotalRecords = GetEmployeeSalaryByEmployeeIdSlipResponse.Count();
            var employee = dbContextHRM.tblEmployee.
                Where(emp => emp.EmployeeID == model.employeeID).FirstOrDefault();
            obj.employeeName = employee.FullName;
            obj.employeePicture = employee.EmployeePhoto;
            obj.empPhotoType = employee.PhotoType;
            if (!string.IsNullOrEmpty(employee.Department))
            {
                long? deptId = Convert.ToInt64(employee.Department);
                obj.employeeDept = dbContextHRM.tblDepartment.Where(dept => dept.DepartmentId == deptId).Select(x => x.DepartmentName).FirstOrDefault();
                obj.employeeDept = string.IsNullOrEmpty(obj.employeeDept) ? "" : obj.employeeDept;
            }
            if (!string.IsNullOrEmpty(employee.Position))
            {
                long? postition = Convert.ToInt64(employee.Position);
                obj.employeeDesignation = dbContextHRM.tblPosition.Where(pos => pos.PositionId == postition).Select(x => x.PositionName).FirstOrDefault();
                obj.employeeDesignation = string.IsNullOrEmpty(obj.employeeDesignation) ? "" : obj.employeeDesignation;
            }
            var company = dbContextHRM.tblCompanyDetail.
                Where(comp=>comp.CompanyID==employee.CompanyID).FirstOrDefault();
            obj.companyName = company.CompanyName;
            obj.companyLogo = company.CompanyLogo;
            string employeeAcc = dbContextHRM.tblEmployeeBankDetail.
                Where(empBank => empBank.EmployeeID== model.employeeID).Select(x=>x.AccountNumber).FirstOrDefault();
            obj.accountNum = employeeAcc;
            obj.companyLogo = company.CompanyLogo;
            //Page size -1 is for all records
            if (model.pageSize == -1)
                obj.EmployeeSalaryByEmployeeIdSliplist = GetEmployeeSalaryByEmployeeIdSlipResponse.ToList();
            else
                obj.EmployeeSalaryByEmployeeIdSliplist = GetEmployeeSalaryByEmployeeIdSlipResponse.Skip(skipCount).Take(takeCount).ToList();

            return obj;
        }

        public async Task<bool> SaveEmployeeBankDetail(SaveEmployeeBankDetailRequestModel model)
        {
            TblEmployeeBankDetail tblEmployeeBankDetailObj = new TblEmployeeBankDetail();
            if (model.action == "update")
            {
                tblEmployeeBankDetailObj = dbContextHRM.tblEmployeeBankDetail
                    .Where(emp => emp.EmployeeBankDetailId == model.EmployeeBankDetailId).FirstOrDefault();
            }
            var companyId = dbContextHRM
                .tblEmployee
                .Where(x => x.EmployeeID == model.EmployeeID)
                .Select(x => x.CompanyID).FirstOrDefault();

            tblEmployeeBankDetailObj.CompanyID = companyId;
            tblEmployeeBankDetailObj.EmployeeID = model.EmployeeID;
            tblEmployeeBankDetailObj.BankName = model.bankName;
            tblEmployeeBankDetailObj.BankBranch = model.bankBranch;
            tblEmployeeBankDetailObj.AccountHolderName = model.accountHolder;
            tblEmployeeBankDetailObj.AccountNumber = model.accountNumber;
            tblEmployeeBankDetailObj.IFSCCode = model.ifscCode;
            tblEmployeeBankDetailObj.IsDeleted = false;
            if (model.action != "update")
            {
                tblEmployeeBankDetailObj.CreatedBy = model.createdBy;
                tblEmployeeBankDetailObj.CreatedDate = DateTime.Now;
            }
            if (model.action == "update")
            {
                tblEmployeeBankDetailObj.ModifiedBy = model.modifiedBy;
                tblEmployeeBankDetailObj.ModifiedDate = DateTime.Now;
            }
            if (model.action == "save")
            {
                dbContextHRM.tblEmployeeBankDetail.Add(tblEmployeeBankDetailObj);
            }
            else
            {
                dbContextHRM.Update(tblEmployeeBankDetailObj);
            }
            dbContextHRM.SaveChanges();
            return true;
        }

        public async Task<SaveEmployeeBankDetailRequestModel> GetEmployeeBankDetail(long employeeID)
        {
            SaveEmployeeBankDetailRequestModel employeeBankDetailResponetModel;
            employeeBankDetailResponetModel = (from employeeBankDetailObj in dbContextHRM.tblEmployeeBankDetail
                                               where employeeBankDetailObj.IsDeleted == false
                                               && (employeeID == 0 ||
                                               employeeID != 0 &&
                                               (employeeBankDetailObj.EmployeeID.Equals(employeeID)))
                                               select new SaveEmployeeBankDetailRequestModel
                                               {
                                                   EmployeeBankDetailId = employeeBankDetailObj.EmployeeBankDetailId,
                                                   EmployeeID = employeeBankDetailObj.EmployeeID,
                                                   bankName = employeeBankDetailObj.BankName,
                                                   bankBranch = employeeBankDetailObj.BankBranch,
                                                   accountHolder = employeeBankDetailObj.AccountHolderName,
                                                   accountNumber = employeeBankDetailObj.AccountNumber,
                                                   ifscCode = employeeBankDetailObj.IFSCCode,
                                                   createdBy = employeeBankDetailObj.CreatedBy,
                                                   modifiedBy = employeeBankDetailObj.ModifiedBy,
                                                   action = "update"
                                               }).FirstOrDefault();
            return employeeBankDetailResponetModel;
        }

        public async Task<GetEmployeeSalariesModel> GetEmployeeSalaries(GetEmployeeSalariesSearchRequest model)
        {
            int skipCount = model.pageSize * model.pageIndex;
            int takeCount = model.pageSize;
            GetEmployeeSalariesModel obj = new GetEmployeeSalariesModel();
            IQueryable<GetEmployeeSalariesList> employeeSalariesResult;
            employeeSalariesResult = (from employeeSalary in dbContextHRM.tblEmployeeSalary
                                      join employee in dbContextHRM.tblEmployee
                                      on employeeSalary.EmployeeID equals employee.EmployeeID
                                      join deptObj in dbContextHRM.tblDepartment
                                      on employee.Department equals deptObj.DepartmentId.ToString() into deptGroup
                                      from deptObj in deptGroup.DefaultIfEmpty()
                                      join positionObj in dbContextHRM.tblPosition
                                      on employee.Position equals positionObj.PositionId.ToString() into posGroup
                                      from positionObj in posGroup.DefaultIfEmpty()
                                      where employee.IsDeleted == false && employeeSalary.IsDeleted == false
                                      && employee.CompanyID == model.companyId
                                      && (model.employmentType == ""
                                      || !string.IsNullOrEmpty(model.employmentType)
                                      && (employee.EmploymentType.Contains(model.employmentType)))
                                      && (model.employmentType == "" ||
                                      !string.IsNullOrEmpty(model.employmentType) &&
                                      (employee.EmploymentType.Contains(model.employmentType)))
                                      && (model.location == "" ||
                                      !string.IsNullOrEmpty(model.location) &&
                                      (employee.Office.Contains(model.location)))
                                      && (model.department == "" ||
                                      !string.IsNullOrEmpty(model.department) &&
                                      (employee.Department.Contains(model.department)))
                                      && (model.position == "" ||
                                      !string.IsNullOrEmpty(model.position) &&
                                      (employee.Position.Contains(model.position)))
                                      && (model.fullName == "" ||
                                      !string.IsNullOrEmpty(model.fullName) &&
                                      (employee.FullName.Contains(model.fullName)))
                                      select new GetEmployeeSalariesList
                                      {
                                          employeeId = employee.EmployeeID,
                                          employeeName = employee.FullName,
                                          department = deptObj.DepartmentName,
                                          position = positionObj.PositionName,
                                          dailyPay = employeeSalary.DailyPay,
                                          hourlyPay=employeeSalary.HourlyPay,
                                          hoursWorked = employeeSalary.HoursWorked,
                                          monthlyPay = employeeSalary.MonthlyPay,
                                          weeklyPay = employeeSalary.WeeklyPay,
                                          weeksWorked = employeeSalary.WeeksWorked,
                                          currency = employeeSalary.Currency,
                                          photoType = employee.PhotoType,
                                          employeePhoto = employee.EmployeePhoto,
                                          payType = employeeSalary.PayType,
                                          employeeSalaryID= employeeSalary.EmployeeSalaryID
                                      });
            obj.TotalRecords = employeeSalariesResult.Count();
            //Page size -1 is for all records
            if (model.pageSize == -1)
                obj.EmployeeSalaryList = employeeSalariesResult.ToList();
            else
                obj.EmployeeSalaryList = employeeSalariesResult.Skip(skipCount).Take(takeCount).ToList();

            return obj;
        }

        public async Task<List<FillEmployeeDDLResponse>> FillEmployeeDDLByCompany(long? companyId, string requiredAll)
        {
            List<FillEmployeeDDLResponse> result = new List<FillEmployeeDDLResponse>();
            if (requiredAll == "salary")
            {
                result = (from emp in dbContextHRM.tblEmployee
                          where emp.CompanyID == companyId
                          && emp.IsDeleted == false
                          select new FillEmployeeDDLResponse
                          {
                              employeeId = emp.EmployeeID,
                              employeeName = emp.FullName,
                              email = emp.Email
                          }).ToList();
            }
            else if (requiredAll == "employees")
            {
                result = (from emp in dbContextHRM.tblEmployee
                          where emp.CompanyID == companyId
                          && emp.IsDeleted == false
                          select new FillEmployeeDDLResponse
                          {
                              employeeId = emp.EmployeeID,
                              employeeName = emp.FullName,
                              email = emp.Email
                          }).ToList();
            }
            return result;
        }

        public async Task<CheckGetEmployeeSalaryByIdRequestModel> CheckGetEmployeeSalaryById(long employeeID)
        {
            CheckGetEmployeeSalaryByIdRequestModel employeeSalaryDetailResult=new CheckGetEmployeeSalaryByIdRequestModel();
            employeeSalaryDetailResult = (from employeeSalaryObj in dbContextHRM.tblEmployeeSalary
                                          join emp in dbContextHRM.tblEmployee 
                                          on employeeSalaryObj.EmployeeID equals emp.EmployeeID
                                          where employeeSalaryObj.IsDeleted == false
                                          && (employeeID == 0 ||
                                          employeeID != 0 &&
                                          (employeeSalaryObj.EmployeeID.Equals(employeeID)))
                                          select new CheckGetEmployeeSalaryByIdRequestModel
                                          {
                                              email = emp.Email,
                                              employeePhoto=emp.EmployeePhoto,
                                              photoType = emp.PhotoType,
                                              fullName=emp.FullName,
                                              payType = employeeSalaryObj.PayType,
                                              monthlyPay = employeeSalaryObj.MonthlyPay,
                                              hourlyPay = employeeSalaryObj.HourlyPay,
                                              hoursWorked = employeeSalaryObj.HoursWorked,
                                              dailyPay = employeeSalaryObj.DailyPay,
                                              weeklyPay = employeeSalaryObj.WeeklyPay,
                                              weeksWorked = employeeSalaryObj.WeeksWorked,
                                              currency = employeeSalaryObj.Currency
                                          }).FirstOrDefault();
            return employeeSalaryDetailResult;
        }

        public async Task<bool> ChangeSalarySlipDownloadStatus(ChangeSalarySlipDownloadStatusModelRequest model)
        {
            var salaryData=dbContextHRM.tblEmployeeSalarySlip.
                Where(salary=>salary.EmployeeSalarySlipID==model.EmployeeSalarySlipID).
                FirstOrDefault();
            salaryData.RequestStatus = model.statusClicked;
            dbContextHRM.SaveChanges();
            return true;
        }

        public async Task<CheckEmployeeRegisterValidationReturnModel> CheckEmployeeRegisterValidation(CheckEmployeeRegisterValidationRequestModel model)
        {
            CheckEmployeeRegisterValidationReturnModel obj = new CheckEmployeeRegisterValidationReturnModel
            {
                status = true,
                message = ""
            };

            bool emailExists = dbContextHRM.tblEmployee.Any(emp => emp.Email == model.email) ||
                               dbContextHRM.tblCompanyDetail.Any(company => company.EmailAddress == model.email) ||
                               dbContextHRM.tblAspNetUsers.Any(user => user.Email == model.email);

            bool phoneNumberExists = dbContextHRM.tblEmployee.Any(emp => emp.PhoneNumber == model.phoneNumber) ||
                                     dbContextHRM.tblCompanyDetail.Any(company => company.PhoneNumber == model.phoneNumber) ||
                                     dbContextHRM.tblAspNetUsers.Any(user => user.PhoneNumber == model.phoneNumber);

            if (emailExists)
            {
                obj.message = "Email already exists in our database!";
                obj.status = false;
            }
            else if (phoneNumberExists)
            {
                obj.message = "Phone Number already exists in our database!";
                obj.status = false;
            }

            return obj;
        }
        public async Task<List<FillEmployeeSalariesDDLResponse>> FillEmployeeDDLForSalaries(FillEmployeeDDLForSalariesRequest model)
        {
            List<FillEmployeeSalariesDDLResponse> result = new List<FillEmployeeSalariesDDLResponse>();
            
            return   result = (from emp in dbContextHRM.tblEmployee
                                join sa in dbContextHRM.tblEmployeeSalary
                                on emp.EmployeeID equals sa.EmployeeID
                               join po in dbContextHRM.tblPosition
                               on  emp.Position equals po.PositionId.ToString()
                               join dep in dbContextHRM.tblDepartment
                               on emp.Department equals dep.DepartmentId.ToString()
                               where emp.CompanyID == model.companyId
                          && emp.IsDeleted == false
                          && sa.PayType == model.payType
                               select new FillEmployeeSalariesDDLResponse
                          {
                              employeeId = emp.EmployeeID,
                              employeeName = emp.FullName,
                              email = emp.Email,
                              payType = sa.PayType,
                              employeePhoto = emp.EmployeePhoto,
                              department = dep.DepartmentName,
                              Position = po.PositionName,

                               }).ToList();
            
        }
            public async Task<bool> GenerateEmployeeSalary(GenerateEmployeeSalaryRequestModel model) //companyid is not added yet
        {   //getting All Employee Ids
            var EmpIds = model.employeeIds;
            int absnet = 0;
            int WeekHolidays = 0;
            int YearHolidays = 0;
            string monthName = DateTimeFormatInfo.CurrentInfo.GetMonthName(model.searchedMonth);
            List<TblEmployeeSalarySlip> SalarySlips_Listobj = new List<TblEmployeeSalarySlip>();
            DateTime firstDayOfMonth = new DateTime(model.searchedYear, model.searchedMonth, 1);
            DateTime lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

            foreach (var emp in EmpIds)
            {
                //Checking Employee Salary Slip
                var EmpDetails = dbContextHRM.tblEmployee.Where(ED=> ED.EmployeeID == emp).FirstOrDefault();
              var salaryslipdata =  dbContextHRM.tblEmployeeSalarySlip.Where(x => x.EmployeeID == emp && (x.CreatedDate>= firstDayOfMonth &&x.CreatedDate <= lastDayOfMonth)).ToList();
                var EmpSalaryInfo = dbContextHRM.tblEmployeeSalary.Where(info => info.EmployeeID == emp).FirstOrDefault();
               
                if(salaryslipdata.Count != 0 && EmpSalaryInfo.PayType == "monthly"){continue;} // if monthly salary is Created Skip processing for this employee and move to the next one

                //get employee related data

                var EmpAttanInfo = dbContextHRM.tblEmployeeAttendance
                    .Where(empA => empA.EmployeeID == emp &&
                                   //empA.ApprovalStatus == "Approved"&&
                                   (empA.IsSalaryGenerated == false || empA.IsSalaryGenerated==null)&&
                                   empA.AttendanceDate >= firstDayOfMonth &&
                                   empA.AttendanceDate <= lastDayOfMonth)
                    .ToList();

                if (EmpAttanInfo.Count == 0){continue;}// if there is No Attendance Skip processing for this employee and move to the next one

                var Empshift = dbContextHRM.tblShiftSetup.Where(x => x.ShiftId == EmpDetails.ShiftId).FirstOrDefault();
                
                TblEmployeeSalarySlip SalarySlip = new TblEmployeeSalarySlip();
                //----------Field   

                SalarySlip.EmployeeID = emp;
                SalarySlip.CompanyID = model.companyId;
                SalarySlip.EmployeeSalaryID = EmpSalaryInfo.EmployeeSalaryID;
                SalarySlip.Month = monthName;
                SalarySlip.PaidOn = DateTime.Now;
                SalarySlip.BasicPay = EmpSalaryInfo.PayType == "monthly" ? EmpSalaryInfo.MonthlyPay : EmpSalaryInfo.PayType == "hourly" ? EmpSalaryInfo.HourlyPay : EmpSalaryInfo.PayType == "daily" ? EmpSalaryInfo.DailyPay : EmpSalaryInfo.PayType == "weekly" ? EmpSalaryInfo.WeeklyPay : 0;//YOu can more
                SalarySlip.Allowances = 0;
                SalarySlip.IsDeleted = false;
                SalarySlip.CreatedBy = "";
                SalarySlip.CreatedDate = DateTime.Now;
                SalarySlip.ModifiedBy = "";
                SalarySlip.ModifiedDate = DateTime.Now;
                SalarySlip.Year = model.searchedYear;
                SalarySlip.Currency = EmpSalaryInfo.Currency;
                SalarySlip.RequestStatus = "Not Paid";
                SalarySlip.PayType = EmpSalaryInfo.PayType;
                SalarySlip.IsPaid = false;
                
                SalarySlip.TaxDeduction = 0;
                //SalarySlip.DeductionReason
                //SalarySlip.Deductions
                //SalarySlip.NetSalary
                //SalarySlip.NetSalaryPerHrs
                //SalarySlip.DeductionPerHrs


                DateTime startTime = DateTime.Parse(Empshift.TimeFrom);
                DateTime endTime = DateTime.Parse(Empshift.TimeTo);
                // Calculate the time difference as a TimeSpan
                TimeSpan timeDifference = endTime - startTime;
                decimal ShiftHours =Convert.ToDecimal(timeDifference.TotalHours);// shift hr of emp per day 

                GetAttendanceCalendarViewRequest getAttendanceCalendarViewRequest = new GetAttendanceCalendarViewRequest
                {
                    employeeID = emp,
                    searchedMonth = model.searchedMonth,
                    searchedYear = model.searchedYear,
                };
                //getting attendance for each employee
                var _result = await _IAttendanceServices.GetAttendanceCalendarView(getAttendanceCalendarViewRequest);
                
                // genarate selary for Each employee based on Attendance
                if (_result != null)
                {
                    foreach (var outerArray in _result.CalendarDay)
                    {
                        foreach (var record in outerArray)
                        {
                            if (record != null)
                            {
                                var attendanceStatusProperty = record.GetType().GetProperty("attendanceStatus").GetValue(record);
                                if (attendanceStatusProperty != null)
                                {
                                    // Access 'absentStatus' field value
                                    string absentStatus = record.GetType().GetProperty("attendanceStatus").GetValue(record).ToString();
                                    if (absentStatus == "Absent")
                                    {
                                        //Absent Increment by 1\
                                        absnet++;

                                    }
                                    if (absentStatus == "WeeklyHoliday")
                                    {
                                        WeekHolidays++;
                                    }
                                    if(absentStatus != "Present"&& absentStatus != "full-day" && absentStatus != "short-leave"&& absentStatus != "half-day"&& absentStatus != "Absent" && absentStatus != null)
                                    {
                                        YearHolidays++;
                                    }
                                }
                            }

                        }
                    }

                }


                // var attendanceRecords = dbContextHRM.tblEmployeeAttendance.Where(attn=>attn.EmployeeID == emp).ToList(); 

                //calculating total working Hours
                TimeSpan totalWorkedTime = TimeSpan.Zero;
                foreach (var record in EmpAttanInfo.Where(x=>x.ApprovalStatus == "Approved"))
                {
                    if (TimeSpan.TryParse(record.TimeWorked, out TimeSpan timeWorked))
                    {
                        totalWorkedTime = totalWorkedTime.Add(timeWorked);
                    }
                }
                var totalabsnet = absnet;
                decimal totalWorkingHrs = Convert.ToDecimal( totalWorkedTime.TotalHours);
               //calculate for Month
               if(EmpSalaryInfo.PayType == "monthly")
                {

                    var PayPerDay = EmpSalaryInfo.MonthlyPay / 30;
                    //total absent
                    var AbsentDeduction = PayPerDay * totalabsnet;
                    var NotAprrovedOrPenddingLeave = PayPerDay * EmpAttanInfo.Where(x => x.ApprovalStatus != "Approved").Count();
                    var Deduction = AbsentDeduction + NotAprrovedOrPenddingLeave;
                    var salaryForMonth = EmpSalaryInfo.MonthlyPay - Deduction;

                    //Adding values to db Fields
                    SalarySlip.Deductions = Deduction>0?Deduction:0;
                    SalarySlip.NetSalary = salaryForMonth>0?salaryForMonth:0;
                    SalarySlip.DeductionReason = Deduction!=0? "Monthly Deduction Aplied": "No Deduction Aplied";

                    //salary per hr

                    decimal totalMonthlyHr = ShiftHours * 30;
                    var salaryParHr = EmpSalaryInfo.MonthlyPay / totalMonthlyHr;
                    // - absent hours

                    decimal HolidaysHrs = (WeekHolidays+YearHolidays)*ShiftHours;
                    decimal absentHr =ShiftHours * (EmpAttanInfo.Where(x => x.ApprovalStatus != "Approved").Count()+ totalabsnet);
                    decimal presentHr = HolidaysHrs + totalWorkingHrs;

                    var totalSalaryPrHrs = presentHr * salaryParHr;
                    var DeductionPrHours = absentHr * salaryParHr;
                    
                    //Adding Values to db Fields
                    SalarySlip.NetSalaryPerHrs =totalSalaryPrHrs>0? totalSalaryPrHrs:0;
                    SalarySlip.DeductionPerHrs = DeductionPrHours > 0 ? DeductionPrHours : 0;
                   
                }
               if(EmpSalaryInfo.PayType == "hourly")
                {
                    var totalSalaryPerWorkingHr = Convert.ToDecimal(totalWorkingHrs) * EmpSalaryInfo.HourlyPay;
                    SalarySlip.Deductions = 0;
                    SalarySlip.NetSalary = totalSalaryPerWorkingHr;
                    SalarySlip.NetSalaryPerHrs = totalSalaryPerWorkingHr;
                    SalarySlip.DeductionPerHrs = 0;
                    SalarySlip.DeductionReason = "NO Deduction Aplied";
                }
               if (EmpSalaryInfo.PayType == "daily")
                  {
                  var TotalPrensnetDays =   EmpAttanInfo.Where(x => x.ApprovalStatus == "Approved").Count();
                    var totalSalary = TotalPrensnetDays * EmpSalaryInfo.DailyPay;

                    var salaryPerHr =  EmpSalaryInfo.DailyPay/ShiftHours;
                    var totalSalaryPrHr = salaryPerHr * totalWorkingHrs;
                    var deductionPrHr = totalSalary - totalSalaryPrHr;
                    SalarySlip.Deductions = 0;
                    SalarySlip.NetSalary = totalSalary;
                    SalarySlip.NetSalaryPerHrs = totalSalaryPrHr > 0 ? totalSalaryPrHr : 0;
                    SalarySlip.DeductionPerHrs = deductionPrHr >0? deductionPrHr :0;
                    SalarySlip.DeductionReason = "NO Deduction Aplied";

                }
               if (EmpSalaryInfo.PayType == "weekly")
                {

                    var salaryperday = EmpSalaryInfo.WeeklyPay / 7;
                    // get all current week days
                    DateTime currentDate = DateTime.Now.AddDays(9);

                    // Calculate the start date of the previous week (previous Sunday)
                    DateTime startOfPreviousWeek1 = currentDate.AddDays(-((int)currentDate.DayOfWeek + 7) % 7);
                    startOfPreviousWeek1 = startOfPreviousWeek1.AddDays(-7);
                    DateTime endOfPreviousWeek = startOfPreviousWeek1.AddDays(6);

                    //gose two week back
                    var startOfPreviousWeek2 =  startOfPreviousWeek1.AddDays(-14);
                    // Calculate the end date of the previous week (previous Saturday)

                    // Create a list to store the dates for the previous week
                    List<DateTime> previousWeekDates = new List<DateTime>();

                    // Loop through the dates in the previous week and add them to the list
                    for (DateTime date = startOfPreviousWeek2; date <= endOfPreviousWeek; date = date.AddDays(1))
                    {
                        previousWeekDates.Add(date);
                    }




                    // check presnt in week
                    int present = 0;
                    int weekend = 0;
                    int yearlyHolidays = 0;
                    foreach (DateTime date in previousWeekDates)
                    {
                        EmpAttanInfo.Where(x => x.AttendanceDate == date.Date).Count();

                        if (EmpAttanInfo.Where(x => x.AttendanceDate == date.Date && x.ApprovalStatus == "Approved").Any())
                        {
                            present++;

                        }
                        //check holidays in that week
                       else if (dbContextHRM.TblWeeklyHolidays.Where(x=>x.Holidays == date.DayOfWeek.ToString() && x.CompanyId ==model.companyId).Any())
                        {
                            weekend++;
                        }
                       else if(dbContextHRM.TblYearlyHolidays.Where(x => x.FromDate <= date.Date && x.ToDate >= date.Date && x.CompanyId == model.companyId).Any())
                        {
                            yearlyHolidays++;
                        }

                    }
                    

                    //totalprensntdays =  holidays + presnt
                    var totalPrentDays = present + weekend + yearlyHolidays;

                    //NetweeklySalary = salarPerday * totalPresentDays
                    var salary = totalPrentDays * salaryperday;
                    // deduction = basicweeksalary - NetweeklySalary
                    var deduction = EmpSalaryInfo.WeeklyPay - salary;

                    SalarySlip.Deductions = deduction;
                    SalarySlip.NetSalary = salary;
                   
                    SalarySlip.DeductionReason = deduction != 0? "Weekly Deduction Applied " : "NO Deduction Applied";





                    //for hrs totalprensntdays  multiplay with shift hours
                    var totalhrs = ShiftHours * 7;
                    var salaryprhr = EmpSalaryInfo.WeeklyPay / totalhrs;
                    var weenendhrs= ShiftHours * weekend;
                    var yearholidayHr = ShiftHours * yearlyHolidays;
                    var TotalprasentHr =  (totalWorkingHrs+ yearholidayHr + yearholidayHr);
                    var NetsalaryprHr = TotalprasentHr * salaryprhr;
                    var deductionprHr = EmpSalaryInfo.WeeklyPay - NetsalaryprHr;


                    SalarySlip.NetSalaryPerHrs = NetsalaryprHr > 0 ? NetsalaryprHr : 0;
                    SalarySlip.DeductionPerHrs = deductionprHr > 0 ? deductionprHr : 0;
                }

               //check for Applied Tax Deduction
               if(EmpSalaryInfo.AppliedTaxPercentage!=null && EmpSalaryInfo.AppliedTaxPercentage != 0)
                {
                   var taxPercentage = EmpSalaryInfo.AppliedTaxPercentage / 100;
                    var taxAmount = SalarySlip.NetSalary * taxPercentage;
                    var netSalary = SalarySlip.NetSalary - taxAmount;
                    SalarySlip.TaxDeduction = taxAmount;
                    SalarySlip.NetSalary = netSalary;
                }

                SalarySlips_Listobj.Add(SalarySlip);
            }

            if(SalarySlips_Listobj.Count == 0){return false;}// if there is no record to SalarySlips_Listob

            using (var transaction = new TransactionScope())
            {
                try
                {
                    // Add the new salary slips
                    dbContextHRM.tblEmployeeSalarySlip.AddRange(SalarySlips_Listobj);

                    // Update attendance records
                    foreach (var emp in SalarySlips_Listobj)
                    {
                         var recordsToUpdate = dbContextHRM.tblEmployeeAttendance
                                                .Where(record => (record.IsSalaryGenerated == false || record.IsSalaryGenerated == null)&& record.EmployeeID == emp.EmployeeID)
                                                .ToList();

                         foreach (var record in recordsToUpdate)
                           {
                             record.IsSalaryGenerated = true;
                           }
                    }
                   

                    // Save all changes within the same transaction
                    dbContextHRM.SaveChanges();

                    // Commit the transaction if everything was successful
                    transaction.Complete();
                }
                catch (Exception ex)
                {
                    return false;
                    // Handle exceptions or log errors here
                    // The transaction will automatically be rolled back if an exception occurs
                }
            }
            
            return true;
        }
        public async Task<GetEmployeeSalarySlipModel> GetEmployeeSalarySlipList(GetEmployeeSalarySlipSearchRequest model)
        {
            int skipCount = model.pageSize * model.pageIndex;
            int takeCount = model.pageSize;
            GetEmployeeSalarySlipModel obj = new GetEmployeeSalarySlipModel();
            IQueryable<GetEmployeeSalarySlipResponse> EmployeeSalarySlip;

             EmployeeSalarySlip = (from slip in dbContextHRM.tblEmployeeSalarySlip
             join employee in dbContextHRM.tblEmployee on slip.EmployeeID equals employee.EmployeeID
             join salaryinfo in dbContextHRM.tblEmployeeSalary on slip.EmployeeID equals salaryinfo.EmployeeID
             join Department in dbContextHRM.tblDepartment on employee.Department equals Department.DepartmentId.ToString()
             join position in dbContextHRM.tblPosition on employee.Position equals position.PositionId.ToString()
                                   // Where (Conditions)
                                   where slip.IsDeleted == false
             select new GetEmployeeSalarySlipResponse
             {
                 EmployeeSalarySlipID = slip.EmployeeSalarySlipID,
                 EmployeeId = slip.EmployeeID,
                 EmployeeName = employee.FullName,
                 EmployeePhoto = employee.EmployeePhoto,
                 Department = Department.DepartmentName,
                 position = position.PositionName,
                 CompanyID = slip.CompanyID,
                 Month = slip.Month,
                 PaidOn = slip.PaidOn,
                 BasicPay = slip.BasicPay,
                 Allowances = slip.Allowances,
                 Deductions = slip.Deductions,
                 NetSalary = slip.NetSalary,
                 //IsDeleted= x.IsDeleted,
                 Year = slip.Year,
                 Currency = slip.Currency,
                 RequestStatus = slip.RequestStatus,
                 DeductionReason =      slip.DeductionReason,
                 DeductionPerHrs = slip.DeductionPerHrs,
                 NetSalaryPerHrs = slip.NetSalaryPerHrs,
                 PayType = slip.PayType,
                 FromDate = slip.FromDate,
                 ToDate = slip.ToDate,
                 IsPaid =slip.IsPaid
             });


            
            obj.totalRecords = EmployeeSalarySlip.Count();
            //Page size -1 is for all records
            if (model.pageSize == -1)
                obj.EmployeeSalarySlipList =  EmployeeSalarySlip.ToList();
            else
                obj.EmployeeSalarySlipList =  EmployeeSalarySlip.Skip(skipCount).Take(takeCount).ToList();

            return obj;
        }
       public async Task<bool> UpdateSalaryAllowances(SalaryAllowancesRequest model)
        {
           
          var data=  dbContextHRM.tblEmployeeSalarySlip.Where(x=>x.EmployeeSalarySlipID == model.EmployeeSalarySlipID).FirstOrDefault();
           //remove previouse allownce from net salary
            data.NetSalary = data.NetSalary - data.Allowances;

            data.Allowances = model.Allowances;
            data.NetSalary = data.NetSalary + model.Allowances;
            dbContextHRM.SaveChanges();
            return true;
        }
        public async Task<bool> SalaryPay(long EmployeeSalarySlipID)
        {
           
            var data  = dbContextHRM.tblEmployeeSalarySlip.Where(x => x.EmployeeSalarySlipID == EmployeeSalarySlipID &&( x.IsPaid == false || x.IsPaid == null)).FirstOrDefault();
            //check may transaction Already x.IsPaid == false 
            if(data == null){return false;}
            //update to paid
            data.IsPaid = true;
            data.RequestStatus = "Paid";
            //get salary data
            var EmployeeBankInfo = dbContextHRM.tblEmployeeBankDetail.Where(x => x.EmployeeID == data.EmployeeID).FirstOrDefault();
            //add to transection Record
            if (EmployeeBankInfo != null)
            {
            TblTransaction obj = new TblTransaction();
                obj.ToAcountName = "Bank:"+EmployeeBankInfo.BankName + ",Branch: " + EmployeeBankInfo.BankBranch;
                obj.ToAcountNo = EmployeeBankInfo.AccountNumber;
                obj.UserId = data.EmployeeID;
                obj.TransactionType = "Salary";
                obj.Amount = data.NetSalary;
                obj.FromAcountName = "Company"; // here we will Company selected Acount
                obj.FromAcountNo = "Company";
                obj.CompanyId  = data.CompanyID;
                obj.IsDeleted = false;
                obj.CreatedDate = DateTime.Now;
                obj.ModifiedDate = DateTime.Now;
                obj.CreatedBy = "Company";
                dbContextHRM.TblTransaction.Add(obj);
            }
            else
            {
                TblTransaction obj = new TblTransaction();
                obj.ToAcountName = dbContextHRM.tblEmployee.Where(x=>x.EmployeeID== data.EmployeeID).Select(x=>x.FullName).FirstOrDefault();
                obj.ToAcountNo = "";
                obj.UserId = data.EmployeeID;
                obj.TransactionType = "Salary";
                obj.Amount = data.NetSalary;
                obj.FromAcountName = "Company"; // here we will Company selected Acount
                obj.FromAcountNo = "Company";
                obj.CompanyId = data.CompanyID;
                obj.IsDeleted = false;
                obj.CreatedDate = DateTime.Now;
                obj.ModifiedDate = DateTime.Now;
                obj.CreatedBy = "Company";
                dbContextHRM.TblTransaction.Add(obj);
            }

            dbContextHRM.SaveChanges();


            return true;
        }
    }
}
