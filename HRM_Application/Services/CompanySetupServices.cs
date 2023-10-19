using HRM_Application.Interfaces;
using HRM_Domain.Model;
using HRM_Infrastructure.HRMDataBaseContext;
using HRM_Infrastructure.TableEntities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM_Application.Services {
    public class CompanySetupServices : ICompanySetupServices {
        public readonly HRMContexts dbContextHRM;
        public CompanySetupServices(HRMContexts dbContextHRM) {
            this.dbContextHRM = dbContextHRM;
        }

        public async Task<SaveResponseMessage> CreateOffice(List<OfficeLocationSaveRequest> modelRequest) {
            SaveResponseMessage saveObject = new SaveResponseMessage();

            foreach (var model in modelRequest) {
                if (await dbContextHRM.tblOfficeLocation.
                    AnyAsync(office => office.OfficeLocationName == model.officeLocationName && office.CompanyId == model.companyId)) {
                    saveObject.status = false;
                    saveObject.msg = "Office location already exists.";
                } else {
                    TblOfficeLocation tblOfficeObj = await dbContextHRM.tblOfficeLocation.FirstOrDefaultAsync(office => office.OfficeId == model.officeId);

                    if (tblOfficeObj == null) {
                        tblOfficeObj = new TblOfficeLocation();
                    }

                    tblOfficeObj.OfficeLocationName = model.officeLocationName;
                    tblOfficeObj.CompanyId = model.companyId;
                    tblOfficeObj.Latitude = model.latitude;
                    tblOfficeObj.Longitude = model.longitude;
                    tblOfficeObj.Meter = model.meter;

                    if (model.officeId < 1) {
                        tblOfficeObj.IsDeleted = false;
                        tblOfficeObj.CreatedBy = model.companyId.ToString();
                        tblOfficeObj.CreatedDate = DateTime.Now;
                        dbContextHRM.tblOfficeLocation.Add(tblOfficeObj);
                    } else {
                        tblOfficeObj.ModifiedBy = model.companyId.ToString();
                        tblOfficeObj.ModifiedDate = DateTime.Now;
                    }
                }
            }
            try {
                await dbContextHRM.SaveChangesAsync();
                saveObject.status = true;
                saveObject.msg = "Office setup successfully!";
            } catch (Exception ex) {
                // Handle the exception and set appropriate status and message
                saveObject.status = false;
                saveObject.msg = "An error occurred while saving the office.";
                // You might also want to log the exception for debugging purposes
            }

            return saveObject;
        }

        public async Task<GetOfficeLocationDetailModel> GetOfficeLocationList(SearchOfficeLocationGetRequest model) {
            int skipCount = model.pageSize * model.pageIndex;
            int takeCount = model.pageSize;
            GetOfficeLocationDetailModel obj = new GetOfficeLocationDetailModel();
            IQueryable<OfficeLocationListResponse> officeLocationListResponse;
            officeLocationListResponse = (from officeObj in dbContextHRM.tblOfficeLocation
                                          where officeObj.CompanyId == model.companyId &&
                                          officeObj.IsDeleted == false &&
                                          (model.searchOfficeLocationName == "" ||
                                          !string.IsNullOrEmpty(model.searchOfficeLocationName))
                                          select new OfficeLocationListResponse {
                                              companyId = officeObj.CompanyId,
                                              officeId = officeObj.OfficeId,
                                              officeLocationName = officeObj.OfficeLocationName,
                                              createdBy = officeObj.CreatedBy,
                                              createdDate = officeObj.CreatedDate,
                                              modifiedBy = officeObj.ModifiedBy,
                                              modifiedDate = officeObj.ModifiedDate,
                                              editableMode = false,
                                              longitude = officeObj.Longitude,
                                              latitude = officeObj.Latitude
                                          });
            obj.totalRecords = officeLocationListResponse.Count();
            //Page size -1 is for all records
            if (model.pageSize == -1)
                obj.officeList = officeLocationListResponse.ToList();
            else
                obj.officeList = officeLocationListResponse.Skip(skipCount).Take(takeCount).ToList();

            return obj;
        }

        public async Task<bool> DeleteOfficeSetup(DeleteOfficeRequest model) {
            var result = dbContextHRM.tblOfficeLocation.Where(x => x.OfficeId == model.id).FirstOrDefault();
            if (result != null) {
                dbContextHRM.Remove(result);
                dbContextHRM.SaveChanges();
                return true;
            }
            return false;
        }

        public async Task<SaveResponseMessage> CreateDepartment(List<DepartmentSaveRequest> modelReq) {
            SaveResponseMessage saveObject = new SaveResponseMessage();

            foreach (var model in modelReq) {
                if (await dbContextHRM.tblDepartment.
                    AnyAsync(dept => dept.DepartmentName == model.departmentName && dept.OfficeId == model.officeLocationId && dept.CompanyId == model.companyId)) {
                    saveObject.status = false;
                    saveObject.msg = "Department already exists.";
                } else {
                    TblDepartment tblDeptObj = await dbContextHRM.tblDepartment.FirstOrDefaultAsync(dept => dept.DepartmentId == model.departmentId);

                    if (tblDeptObj == null) {
                        tblDeptObj = new TblDepartment();
                    }

                    tblDeptObj.DepartmentName = model.departmentName;
                    tblDeptObj.CompanyId = model.companyId;

                    if (model.departmentId < 1) {
                        tblDeptObj.IsDeleted = false;
                        tblDeptObj.OfficeId = model.officeLocationId;
                        tblDeptObj.CreatedBy = model.companyId.ToString();
                        tblDeptObj.CreatedDate = DateTime.Now;
                        dbContextHRM.tblDepartment.Add(tblDeptObj);
                    } else {
                        tblDeptObj.ModifiedBy = model.companyId.ToString();
                        tblDeptObj.ModifiedDate = DateTime.Now;
                    }
                }
            }

            try {
                await dbContextHRM.SaveChangesAsync();
                saveObject.status = true;
                saveObject.msg = "Department setup successfully!";
            } catch (Exception ex) {
                // Handle the exception and set appropriate status and message
                saveObject.status = false;
                saveObject.msg = "An error occurred while saving the department.";
                // You might also want to log the exception for debugging purposes
            }

            return saveObject;
        }

        public async Task<GetDepartmentDetailModel> GetDepartmentList(SearchDepartmentGetRequest model) {
            int skipCount = model.pageSize * model.pageIndex;
            int takeCount = model.pageSize;
            GetDepartmentDetailModel obj = new GetDepartmentDetailModel();
            IQueryable<DepartmentListResponse> deptListResponse;
            deptListResponse = (from deptObj in dbContextHRM.tblDepartment
                                join officeObj in dbContextHRM.tblOfficeLocation
                                on deptObj.OfficeId equals officeObj.OfficeId into officeGroup
                                from officeObj in officeGroup.DefaultIfEmpty()
                                where deptObj.CompanyId == model.companyId &&
                                deptObj.IsDeleted == false &&
                                (model.searchDepartmentName == "" ||
                                !string.IsNullOrEmpty(model.searchDepartmentName))
                                select new DepartmentListResponse {
                                    companyId = deptObj.CompanyId,
                                    departmentId = deptObj.DepartmentId,
                                    departmentName = deptObj.DepartmentName,
                                    officeLocationName = officeObj.OfficeLocationName,
                                    createdBy = deptObj.CreatedBy,
                                    createdDate = deptObj.CreatedDate,
                                    modifiedBy = deptObj.ModifiedBy,
                                    modifiedDate = deptObj.ModifiedDate,
                                    editableMode = false
                                });
            obj.totalRecords = deptListResponse.Count();
            //Page size -1 is for all records
            if (model.pageSize == -1)
                obj.departmentList = deptListResponse.ToList();
            else
                obj.departmentList = deptListResponse.Skip(skipCount).Take(takeCount).ToList();

            return obj;
        }

        public async Task<bool> DeleteDepartmentSetup(DeleteDepartmentRequest model) {
            var result = dbContextHRM.tblDepartment.Where(x => x.DepartmentId == model.id).FirstOrDefault();
            if (result != null) {
                dbContextHRM.Remove(result);
                dbContextHRM.SaveChanges();
                return true;
            }
            return false;
        }

        public async Task<SaveResponseMessage> CreateTeam(List<TeamSaveRequest> modelReq) {
            SaveResponseMessage saveObject = new SaveResponseMessage();

            foreach (var model in modelReq) {
                if (await dbContextHRM.tblTeam.
                AnyAsync(team => team.TeamName == model.teamName && team.OfficeId == model.officeId
                && team.DepartmentId == model.departmentId && team.CompanyId == model.companyId)) {
                    saveObject.status = false;
                    saveObject.msg = "Team already exists.";
                } else {
                    TblTeam tblTeamObj = await dbContextHRM.tblTeam.FirstOrDefaultAsync(team => team.TeamId == model.teamId);

                    if (tblTeamObj == null) {
                        tblTeamObj = new TblTeam();
                    }

                    tblTeamObj.TeamName = model.teamName;
                    tblTeamObj.CompanyId = model.companyId;

                    if (model.teamId < 1) {
                        tblTeamObj.CreatedBy = model.companyId.ToString();
                        tblTeamObj.CreatedDate = DateTime.Now;
                        tblTeamObj.DepartmentId = model.departmentId;
                        tblTeamObj.OfficeId = model.officeId;
                        tblTeamObj.IsDeleted = false;
                        dbContextHRM.tblTeam.Add(tblTeamObj);
                    } else {
                        tblTeamObj.ModifiedBy = model.companyId.ToString();
                        tblTeamObj.ModifiedDate = DateTime.Now;
                    }
                }
            }
            try {
                await dbContextHRM.SaveChangesAsync();
                saveObject.status = true;
                saveObject.msg = "Team setup successfully!";
            } catch (Exception ex) {
                // Handle the exception and set appropriate status and message
                saveObject.status = false;
                saveObject.msg = "An error occurred while saving the team.";
                // You might also want to log the exception for debugging purposes
            }

            return saveObject;
        }

        public async Task<GetTeamDetailModel> GetTeamList(SearchTeamGetRequest model) {
            int skipCount = model.pageSize * model.pageIndex;
            int takeCount = model.pageSize;
            GetTeamDetailModel obj = new GetTeamDetailModel();
            IQueryable<TeamListResponse> teamListResponse;
            teamListResponse = (from teamObj in dbContextHRM.tblTeam
                                join deptObj in dbContextHRM.tblDepartment
                                on teamObj.DepartmentId equals deptObj.DepartmentId into deptGroup
                                from deptObj in deptGroup.DefaultIfEmpty()
                                join officeObj in dbContextHRM.tblOfficeLocation
                                on teamObj.OfficeId equals officeObj.OfficeId into officeGroup
                                from officeObj in officeGroup.DefaultIfEmpty()
                                where teamObj.CompanyId == model.companyId &&
                                teamObj.IsDeleted == false &&
                                (model.searchTeamName == "" ||
                                !string.IsNullOrEmpty(model.searchTeamName))
                                select new TeamListResponse {
                                    companyId = teamObj.CompanyId,
                                    teamId = teamObj.TeamId,
                                    teamName = teamObj.TeamName,
                                    departmentName = deptObj.DepartmentName,
                                    officeName = officeObj.OfficeLocationName,
                                    createdBy = teamObj.CreatedBy,
                                    createdDate = teamObj.CreatedDate,
                                    modifiedBy = teamObj.ModifiedBy,
                                    modifiedDate = teamObj.ModifiedDate,
                                    editableMode = false
                                });
            obj.totalRecords = teamListResponse.Count();
            //Page size -1 is for all records
            if (model.pageSize == -1)
                obj.teamList = teamListResponse.ToList();
            else
                obj.teamList = teamListResponse.Skip(skipCount).Take(takeCount).ToList();

            return obj;
        }

        public async Task<bool> DeleteTeamSetup(DeleteTeamRequest model) {
            var result = dbContextHRM.tblTeam.Where(x => x.TeamId == model.id).FirstOrDefault();
            if (result != null) {
                dbContextHRM.Remove(result);
                dbContextHRM.SaveChanges();
                return true;
            }
            return false;
        }

        public async Task<SaveResponseMessage> CreatePosition(List<PositionSaveRequest> modelReq) {
            SaveResponseMessage saveObject = new SaveResponseMessage();

            foreach (var model in modelReq) {
                if (await dbContextHRM.tblPosition.
                AnyAsync(position => position.PositionName == model.positionName && position.DepartmentId == model.departmentId
                 && position.TeamId == model.teamId && position.CompanyId == model.companyId)) {
                    saveObject.status = false;
                    saveObject.msg = "Position already exists.";
                } else {
                    TblPosition tblPositionObj = await dbContextHRM.tblPosition.FirstOrDefaultAsync(position => position.PositionId == model.positionId);

                    if (tblPositionObj == null) {
                        tblPositionObj = new TblPosition();
                    }

                    tblPositionObj.PositionName = model.positionName;
                    tblPositionObj.CompanyId = model.companyId;

                    if (model.positionId < 1) {
                        tblPositionObj.DepartmentId = model.departmentId;
                        tblPositionObj.TeamId = model.teamId;
                        tblPositionObj.CreatedBy = model.companyId.ToString();
                        tblPositionObj.CreatedDate = DateTime.Now;
                        tblPositionObj.IsDeleted = false;
                        dbContextHRM.tblPosition.Add(tblPositionObj);
                    } else {
                        tblPositionObj.ModifiedBy = model.companyId.ToString();
                        tblPositionObj.ModifiedDate = DateTime.Now;
                    }
                }
            }

            try {
                await dbContextHRM.SaveChangesAsync();
                saveObject.status = true;
                saveObject.msg = "Position setup successfully!";
            } catch (Exception ex) {
                // Handle the exception and set appropriate status and message
                saveObject.status = false;
                saveObject.msg = "An error occurred while saving the position.";
                // You might also want to log the exception for debugging purposes
            }

            return saveObject;
        }

        public async Task<GetPositionDetailModel> GetPositionList(SearchPositionGetRequest model) {
            int skipCount = model.pageSize * model.pageIndex;
            int takeCount = model.pageSize;
            GetPositionDetailModel obj = new GetPositionDetailModel();
            IQueryable<PositionListResponse> positionListResponse;
            positionListResponse = (from positionObj in dbContextHRM.tblPosition
                                    join deptObj in dbContextHRM.tblDepartment
                                    on positionObj.DepartmentId equals deptObj.DepartmentId into deptGroup
                                    from deptObj in deptGroup.DefaultIfEmpty()
                                    join teamObj in dbContextHRM.tblTeam
                                    on positionObj.TeamId equals teamObj.TeamId into teamGroup
                                    from teamObj in teamGroup.DefaultIfEmpty()
                                    where positionObj.CompanyId == model.companyId &&
                                    positionObj.IsDeleted == false &&
                                    (model.searchPositionName == "" ||
                                    !string.IsNullOrEmpty(model.searchPositionName))
                                    select new PositionListResponse {
                                        companyId = positionObj.CompanyId,
                                        positionId = positionObj.PositionId,
                                        positionName = positionObj.PositionName,
                                        createdBy = positionObj.CreatedBy,
                                        createdDate = positionObj.CreatedDate,
                                        modifiedBy = positionObj.ModifiedBy,
                                        modifiedDate = positionObj.ModifiedDate,
                                        editableMode = false,
                                        departmentName = deptObj.DepartmentName,
                                        teamName = teamObj.TeamName
                                    });
            obj.totalRecords = positionListResponse.Count();
            //Page size -1 is for all records
            if (model.pageSize == -1)
                obj.positionList = positionListResponse.ToList();
            else
                obj.positionList = positionListResponse.Skip(skipCount).Take(takeCount).ToList();

            return obj;
        }

        public async Task<bool> DeletePositionSetup(DeletePositionRequest model) {
            var result = dbContextHRM.tblPosition.Where(x => x.PositionId == model.id).FirstOrDefault();
            if (result != null) {
                dbContextHRM.Remove(result);
                dbContextHRM.SaveChanges();
                return true;
            }
            return false;
        }
        public async Task<GetSetupLookUpDataResponseModel> GetSetupLookUpData(GetSetupLookUpDataRequestModel model) {
            GetSetupLookUpDataResponseModel obj = new GetSetupLookUpDataResponseModel();
            if (model.requiredDataList.Contains("office")) {
                obj.officeLocationDDL = dbContextHRM.tblOfficeLocation.
                    Where(o => o.CompanyId == model.companyId).
                    Select(office => new OfficeLocationDDLResponse() { officeId = office.OfficeId, officeLocationName = office.OfficeLocationName }).ToList();
            }
            if (model.requiredDataList.Contains("department")) {
                obj.departmentDDL = dbContextHRM.tblDepartment.
                    Where(d => d.CompanyId == model.companyId).
                    Select(dept => new DepartmentDDLResponse() { departmentId = dept.DepartmentId, departmentName = dept.DepartmentName }).ToList();
            }
            if (model.requiredDataList.Contains("team")) {
                obj.teamDDL = dbContextHRM.tblTeam.
                    Where(t => t.CompanyId == model.companyId).
                    Select(team => new TeamDDLResponse() { teamId = team.TeamId, teamName = team.TeamName }).ToList();
            }
            if (model.requiredDataList.Contains("position")) {
                obj.positionDDL = dbContextHRM.tblPosition.
                    Where(p => p.CompanyId == model.companyId).
                    Select(position => new PositionDDLResponse() { positionId = position.PositionId, positionName = position.PositionName }).ToList();
            }
            if (model.requiredDataList.Contains("et")) {
                obj.employmentTypeDDLResponse = dbContextHRM.tblEmploymentTypeSetup.
                    Where(p => p.CompanyId == model.companyId).
                    Select(et => new EmploymentTypeDDLResponse() { employmentTypeId = et.EmploymentTypeId, employmentTypeName = et.EmploymentType }).ToList();
            }
            if (model.requiredDataList.Contains("shift")) {
                obj.getShiftListDDLResponse = dbContextHRM.tblShiftSetup.
                    Where(p => p.CompanyId == model.companyId).
                    Select(et => new GetShiftListDDLResponse() { shiftId = et.ShiftId, shiftName = et.ShiftName }).ToList();
            }
            return obj;
        }

        public async Task<List<DepartmentDDLResponse>> GetDepartmentByOfficeLocation(long? companyId, long? officeId) {
            List<DepartmentDDLResponse> lst = dbContextHRM.tblDepartment.
                Where(d => d.CompanyId == companyId && d.OfficeId == officeId).
                Select(dept => new DepartmentDDLResponse() { departmentId = dept.DepartmentId, departmentName = dept.DepartmentName }).ToList();
            return lst;
        }

        public async Task<List<TeamDDLResponse>> GetTeamByDepartment(long? companyId, long? departmentId) {
            List<TeamDDLResponse> lst = dbContextHRM.tblTeam.
                    Where(t => t.CompanyId == companyId && t.DepartmentId == departmentId).
                    Select(team => new TeamDDLResponse() { teamId = team.TeamId, teamName = team.TeamName }).ToList();
            return lst;
        }

        public async Task<List<PositionDDLResponse>> GetPositionByTeam(long? companyId, long? teamId) {
            List<PositionDDLResponse> lst = dbContextHRM.tblPosition.
                     Where(p => p.CompanyId == companyId && p.TeamId == teamId).
                     Select(position => new PositionDDLResponse() { positionId = position.PositionId, positionName = position.PositionName }).ToList();
            return lst;
        }

        public async Task<PositionHierarchyEmployeesResponse> GetPositionHierarchyEmployees(long? companyId, long? positionId)
        {
            PositionHierarchyEmployeesResponse obj = new PositionHierarchyEmployeesResponse();
            try {
                var orderedHierarchy = dbContextHRM
                    .tblPositionHierarchy
                    .Where(x => x.CompanyId == companyId)
                    .OrderBy(item => item.OrderNumber)
                    .ToList();
                if (positionId == 0) {
                    obj.supervisorDDLResponse = new List<SupervisorDDLResponse>();
                    obj.teamMemberDDLResponse = new List<TeamMemberDDLResponse>();
                    return obj;
                }
                int index = orderedHierarchy.Select((data, index) => new { data, index }).First(x => x.data.PositionId == positionId).index;
                string upperResultPositionId = "0";
                string lowerResultPositionId = "0";
                if (index > 0)
                    upperResultPositionId = orderedHierarchy[index - 1].PositionId.ToString();
                lowerResultPositionId = orderedHierarchy[index + 1].PositionId.ToString();

                List<SupervisorDDLResponse> result1 = dbContextHRM.tblEmployee
                    .Where(p => p.CompanyID == companyId && p.Position == upperResultPositionId && !p.IsDeleted && p.IsActivated == true)
                    .Select(emp => new SupervisorDDLResponse() { supervisorId = emp.EmployeeID, supervisorName = emp.FullName, email = emp.Email, selected = false })
                    .ToList();

                List<TeamMemberDDLResponse> result2 = dbContextHRM.tblEmployee.
                    Where(p => p.CompanyID == companyId && p.Position == lowerResultPositionId && !p.IsDeleted && p.IsActivated == true).
                    Select(emp => new TeamMemberDDLResponse() { teamMemberId = emp.EmployeeID, teamMemberName = emp.FullName, email = emp.Email, selected = false }).ToList();

                obj.supervisorDDLResponse = result1;
                obj.teamMemberDDLResponse = result2;
                return obj;
            }
            catch(Exception ex) {
                return obj;
            }
        }

        public async Task<SaveResponseMessage> CreateEmploymentTypeSetup(EmploymentTypeSetupRequest model) {
            SaveResponseMessage saveObject = new SaveResponseMessage();
            foreach (var empType in model.employmentType) {
                if (await dbContextHRM.tblEmploymentTypeSetup.
                    AnyAsync(t => t.EmploymentType == empType && t.CompanyId == model.companyId)) {
                    saveObject.status = false;
                    saveObject.msg = "Employment type already exists.";
                    return saveObject;
                }

                TblEmploymentTypeSetup tblEmploymentTypeObj = await dbContextHRM.tblEmploymentTypeSetup.FirstOrDefaultAsync(t => t.EmploymentTypeId == model.employmentTypeId);

                if (tblEmploymentTypeObj == null) {
                    tblEmploymentTypeObj = new TblEmploymentTypeSetup();
                }

                tblEmploymentTypeObj.EmploymentType = empType;
                tblEmploymentTypeObj.CompanyId = model.companyId;

                if (model.employmentTypeId == null || model.employmentTypeId < 1) {
                    tblEmploymentTypeObj.CreatedBy = model.companyId.ToString();
                    tblEmploymentTypeObj.CreatedDate = DateTime.Now;
                    tblEmploymentTypeObj.IsDeleted = false;
                    dbContextHRM.tblEmploymentTypeSetup.Add(tblEmploymentTypeObj);
                } else {
                    tblEmploymentTypeObj.ModifiedBy = model.companyId.ToString();
                    tblEmploymentTypeObj.ModifiedDate = DateTime.Now;
                }
            }
            try {
                await dbContextHRM.SaveChangesAsync();
                saveObject.status = true;
                saveObject.msg = "Employment type setup successfully!";
            } catch (Exception ex) {
                // Handle the exception and set appropriate status and message
                saveObject.status = false;
                saveObject.msg = "An error occurred while saving the position.";
                // You might also want to log the exception for debugging purposes
            }

            return saveObject;
        }

        public async Task<GetEmploymentTypeModel> GetEmploymentTypeList(SearchEmpTypeGetRequest model) {
            int skipCount = model.pageSize * model.pageIndex;
            int takeCount = model.pageSize;
            GetEmploymentTypeModel obj = new GetEmploymentTypeModel();
            IQueryable<EmploymentTypeListResponse> employmentTypeListResponse;
            employmentTypeListResponse = (from empObj in dbContextHRM.tblEmploymentTypeSetup
                                          where empObj.CompanyId == model.companyId &&
                                          empObj.IsDeleted == false &&
                                          (model.searchEmploymentType == "" ||
                                          !string.IsNullOrEmpty(model.searchEmploymentType)) // ?
                                          select new EmploymentTypeListResponse {
                                              companyId = empObj.CompanyId,
                                              EmploymentTypeId = empObj.EmploymentTypeId,
                                              EmploymentType = empObj.EmploymentType,
                                              createdBy = empObj.CreatedBy,
                                              createdDate = empObj.CreatedDate,
                                              modifiedBy = empObj.ModifiedBy,
                                              modifiedDate = empObj.ModifiedDate,
                                              editableMode = false
                                          });
            obj.totalRecords = employmentTypeListResponse.Count();
            //Page size -1 is for all records
            if (model.pageSize == -1)
                obj.employmentTypeList = employmentTypeListResponse.ToList();
            else
                obj.employmentTypeList = employmentTypeListResponse.Skip(skipCount).Take(takeCount).ToList();

            return obj;
        }

        public async Task<bool> DeleteEmploymentTypeSetup(DeleteEmploymentTypeRequest model) {
            var result = dbContextHRM.tblEmploymentTypeSetup.Where(x => x.EmploymentTypeId == model.id).FirstOrDefault();
            if (result != null) {
                dbContextHRM.Remove(result);
                dbContextHRM.SaveChanges();
                return true;
            }
            return false;
        }

        public async Task<SaveResponseMessage> SaveLocation(OfficeAddressSaveRequest model) {
            SaveResponseMessage saveObject = new SaveResponseMessage();

            TblOfficeMapAddress tblOfficeMapAddressObj = await dbContextHRM.tblOfficeMapAddress.
                FirstOrDefaultAsync(t => t.CompanyId == model.companyId && t.OfficeId == model.officeId);

            if (tblOfficeMapAddressObj == null) {
                tblOfficeMapAddressObj = new TblOfficeMapAddress();
            }

            tblOfficeMapAddressObj.Latitude = model.latitude;
            tblOfficeMapAddressObj.Longitude = model.latitude;
            tblOfficeMapAddressObj.CompanyId = model.companyId;
            tblOfficeMapAddressObj.OfficeId = model.officeId;
            if (tblOfficeMapAddressObj.OfficeMapAddressId <= 0) {
                tblOfficeMapAddressObj.CreatedBy = model.companyId.ToString();
                tblOfficeMapAddressObj.CreatedDate = DateTime.Now;
                tblOfficeMapAddressObj.IsDeleted = false;
                dbContextHRM.tblOfficeMapAddress.Add(tblOfficeMapAddressObj);
            } else {
                tblOfficeMapAddressObj.ModifiedBy = model.companyId.ToString();
                tblOfficeMapAddressObj.ModifiedDate = DateTime.Now;
            }
            try {
                await dbContextHRM.SaveChangesAsync();
                saveObject.status = true;
                saveObject.msg = "Location on map has been set successfully!";
            } catch (Exception ex) {
                // Handle the exception and set appropriate status and message
                saveObject.status = false;
                saveObject.msg = "An error occurred while saving the position.";
                // You might also want to log the exception for debugging purposes
            }

            return saveObject;
        }

        public async Task<List<GetLocationList>> GetLocation(GetLocationSearchModel model) {
            var result = dbContextHRM.tblOfficeMapAddress
                .Where(x => x.CompanyId == model.companyId && x.OfficeId == model.officeId)
                .Select(x => new GetLocationList { companyId = x.CompanyId, OfficeMapAddressId = x.OfficeMapAddressId, latitude = x.Latitude, longitude = x.Longitude, officeId = x.OfficeId })
                .ToList();
            return result;
        }

        public async Task<SaveResponseMessage> CreateShift(ShiftSaveRequest model) {
            SaveResponseMessage saveObject = new SaveResponseMessage();
            if (await dbContextHRM.tblShiftSetup.
                AnyAsync(shift => shift.ShiftName == model.shiftName && shift.CompanyId == model.companyId) && model.shiftId <= 0) //check if already Exist
            {
                saveObject.status = false;
                saveObject.msg = "Shift already exists.";
            } else  // otherwise get record
              {
                TblShiftSetup tblShiftObj = await dbContextHRM.tblShiftSetup.FirstOrDefaultAsync(shift => shift.ShiftId == model.shiftId);

                if (tblShiftObj == null) // if null than assign empty model to fill table
                {
                    tblShiftObj = new TblShiftSetup();
                }
                //assing data from view-model to table model
                tblShiftObj.ShiftName = model.shiftName;
                tblShiftObj.CompanyId = model.companyId;
                tblShiftObj.TimeFrom = model.hourFrom;
                tblShiftObj.TimeTo = model.hourTo;

                if (model.shiftId < 1) //if id not zero than add otherwise update
                {
                    tblShiftObj.IsDeleted = false;
                    tblShiftObj.CreatedBy = model.companyId.ToString();
                    tblShiftObj.CreatedDate = DateTime.Now;
                    dbContextHRM.tblShiftSetup.Add(tblShiftObj);
                } else {
                    tblShiftObj.ModifiedBy = model.companyId.ToString();
                    tblShiftObj.ModifiedDate = DateTime.Now;
                }

            }
            try {
                await dbContextHRM.SaveChangesAsync();
                saveObject.status = true;
                saveObject.msg = "Shift setup successfully!";
            } catch (Exception ex) {
                // Handle the exception and set appropriate status and message
                saveObject.status = false;
                saveObject.msg = "An error occurred while saving the office.";
                // You might also want to log the exception for debugging purposes
            }
            return saveObject;
        }

        public async Task<GetShiftModel> GetShiftList(SearchShiftGetRequest model) {
            int skipCount = model.pageSize * model.pageIndex;
            int takeCount = model.pageSize;
            GetShiftModel obj = new GetShiftModel();
            IQueryable<GetShiftListRequest> shiftListListResponse;
            shiftListListResponse = (from empObj in dbContextHRM.tblShiftSetup
                                     where empObj.CompanyId == model.companyId &&
                                     empObj.IsDeleted == false
                                     &&
                                     (model.searchShift == "" ||
                                     !string.IsNullOrEmpty(model.searchShift)) // ?
                                     select new GetShiftListRequest {
                                         shiftId = empObj.ShiftId,
                                         shiftName = empObj.ShiftName,
                                         companyId = empObj.CompanyId,
                                         hourFrom = empObj.TimeFrom,
                                         hourTo = empObj.TimeTo,
                                         modifiedBy = empObj.ModifiedBy,
                                         modifiedDate = empObj.ModifiedDate,
                                         editableMode = false
                                     });
            obj.totalRecords = shiftListListResponse.Count();
            //Page size -1 is for all records
            if (model.pageSize == -1)
                obj.shiftList = await shiftListListResponse.ToListAsync();
            else
                obj.shiftList = await shiftListListResponse.Skip(skipCount).Take(takeCount).ToListAsync();

            return obj;
        }
        public async Task<bool> DeleteShift(int shiftId) {
            var result = dbContextHRM.tblShiftSetup.Where(x => x.ShiftId == shiftId).FirstOrDefault();
            if (result != null) {
                dbContextHRM.Remove(result);
                dbContextHRM.SaveChanges();
                return true;
            }
            return false;
        }

        public async Task<SaveResponseMessage> SaveRole(SaveRoleReq model) {
            SaveResponseMessage saveObject = new SaveResponseMessage();
            if (await dbContextHRM.TblEmployeeRole.
                AnyAsync(role => role.CompanyId == model.companyId &&
                role.OfficeId == model.officeId &&
                role.DepartmentId == model.department &&
                role.TeamId == model.team &&
                role.PositionId == model.position) && model.roleId <= 0) //check if already Exist
            {
                saveObject.status = false;
                saveObject.msg = "Employee role already exists against this data.";
                return saveObject;
            } else if (model.roleId > 0) {
                var roleRemove = dbContextHRM.TblEmployeeRole.Where(x => x.RoleId == model.roleId).ToList();
                dbContextHRM.TblEmployeeRole.RemoveRange(roleRemove);
                await dbContextHRM.SaveChangesAsync();
            }

            TblRole tblRole = new TblRole();
            if (model.roleId <= 0) {
                tblRole.OfficeId = model.officeId;
                tblRole.DepartmentId = model.department;
                tblRole.TeamId = model.team;
                tblRole.PositionId = model.position;
                tblRole.CompanyId = model.companyId;
                tblRole.CreatedBy = model.createdBy;
                tblRole.CreatedDate = DateTime.Now;
                tblRole.IsDeleted = false;
                dbContextHRM.TblRole.Add(tblRole);
                await dbContextHRM.SaveChangesAsync();
            }
            foreach (var role in model.modulePermissions) {
                    TblEmployeeRoles tblEmployeeRole = new TblEmployeeRoles();
                    tblEmployeeRole.RoleId = model.roleId > 0 ? model.roleId : tblRole.RoleId;
                    tblEmployeeRole.OfficeId = model.officeId;
                    tblEmployeeRole.DepartmentId = model.department;
                    tblEmployeeRole.TeamId = model.team;
                    tblEmployeeRole.PositionId = model.position;
                    tblEmployeeRole.ModuleName = role.moduleName;
                    tblEmployeeRole.CompanyId = model.companyId;
                    tblEmployeeRole.IsRead = role.read;
                    tblEmployeeRole.IsWrite = role.write;
                    tblEmployeeRole.CreatedBy = model.createdBy;
                    tblEmployeeRole.CreatedDate = DateTime.Now;
                    tblEmployeeRole.IsDeleted = false;
                    tblEmployeeRole.ModuleId = role.moduleId;
                    dbContextHRM.TblEmployeeRole.Add(tblEmployeeRole);

                if (role.subModules.Count > 0)
                {
                    foreach (var subModule in role.subModules)
                    {
                        TblEmployeeRoles tblEmployeeSubModule = new TblEmployeeRoles();
                        tblEmployeeSubModule.RoleId = model.roleId > 0 ? model.roleId : tblRole.RoleId;
                        tblEmployeeSubModule.OfficeId = model.officeId;
                        tblEmployeeSubModule.DepartmentId = model.department;
                        tblEmployeeSubModule.TeamId = model.team;
                        tblEmployeeSubModule.PositionId = model.position;
                        tblEmployeeSubModule.ModuleName = subModule.moduleName;
                        tblEmployeeSubModule.CompanyId = model.companyId;
                        tblEmployeeSubModule.IsRead = subModule.read;
                        tblEmployeeSubModule.IsWrite = subModule.write;
                        tblEmployeeSubModule.CreatedBy = model.createdBy;
                        tblEmployeeSubModule.CreatedDate = DateTime.Now;
                        tblEmployeeSubModule.IsDeleted = false;
                        tblEmployeeSubModule.ModuleId = subModule.moduleId;
                        dbContextHRM.TblEmployeeRole.Add(tblEmployeeSubModule);
                    }
                }
            }
            try {
                await dbContextHRM.SaveChangesAsync();
                saveObject.status = true;
                saveObject.msg = "Role saved successfully!";
            } catch (Exception ex) {
                // Handle the exception and set appropriate status and message
                saveObject.status = false;
                saveObject.msg = "An error occurred while saving the role.";
                // You might also want to log the exception for debugging purposes
            }
            return saveObject;
        }


        public async Task<List<ModulePermissionList>> GetRoles(GetRoleReq model) {
            List<ModulePermissionList> result = (from empRole in dbContextHRM.TblRole
                                                 join officeObj in dbContextHRM.tblOfficeLocation
                                                 on empRole.OfficeId equals officeObj.OfficeId
                                                 join deptObj in dbContextHRM.tblDepartment
                                                 on empRole.DepartmentId equals deptObj.DepartmentId
                                                 join teamObj in dbContextHRM.tblTeam
                                                 on empRole.TeamId equals teamObj.TeamId
                                                 join posObj in dbContextHRM.tblPosition
                                                 on empRole.PositionId equals posObj.PositionId
                                                 where empRole.CompanyId == model.companyId &&
                                                 empRole.IsDeleted == false
                                                 select new ModulePermissionList {
                                                     RoleId = empRole.RoleId,
                                                     positionId = empRole.PositionId,
                                                     teamId = empRole.TeamId,
                                                     departmentId = empRole.DepartmentId,
                                                     officeId = empRole.OfficeId,
                                                     departmentName = deptObj.DepartmentName,
                                                     positionname = posObj.PositionName,
                                                     teamName = teamObj.TeamName,
                                                     officeName = officeObj.OfficeLocationName,
                                                     companyId = empRole.CompanyId,
                                                     createdBy = empRole.CreatedBy,
                                                     createdDate = empRole.CreatedDate,
                                                     modifiedBy = empRole.ModifiedBy,
                                                     
                                                 })
                                     .ToList();

            return result;
        }

        public async Task<SaveRoleReq> GetRolesById(long roleId, long officeId, long department, long teamId, long position, long companyID) {
            SaveRoleReq obj = new SaveRoleReq();
            obj.modulePermissions = (from empRole in dbContextHRM.TblEmployeeRole
                                     where
                                     empRole.CompanyId == companyID &&
                                     empRole.IsDeleted == false && empRole.RoleId == roleId
                                     select new ModulePermission {
                                         write = empRole.IsWrite,
                                         moduleName = empRole.ModuleName,
                                         read = empRole.IsRead,
                                         moduleId = empRole.ModuleId,
                                     })
                                     .ToList();
            obj.officeName = dbContextHRM.tblOfficeLocation.Where(office => office.OfficeId == officeId).Select(x => x.OfficeLocationName).FirstOrDefault();
            obj.officeId = officeId;
            obj.departmentName = dbContextHRM.tblDepartment.Where(dept => dept.DepartmentId == department).Select(x => x.DepartmentName).FirstOrDefault();
            obj.department = department;
            obj.teamName = dbContextHRM.tblTeam.Where(team => team.TeamId == teamId).Select(x => x.TeamName).FirstOrDefault();
            obj.team = teamId;
            obj.positionName = dbContextHRM.tblPosition.Where(post => post.PositionId == position).Select(x => x.PositionName).FirstOrDefault();
            obj.position = position;
            return obj;
        }

        public async Task<bool> DeleteEmployeeSetupRoles(long roleId)
        {
            try
            {
                var deleteRole = dbContextHRM.TblRole.Where(x => x.RoleId == roleId).ToList();
                var deleteEmployeeRole = dbContextHRM.TblEmployeeRole.Where(x => x.RoleId == roleId).ToList();
                if (deleteRole != null && deleteRole.Count > 0)
                {
                    dbContextHRM.TblRole.RemoveRange(deleteRole);
                }
                if (deleteEmployeeRole != null && deleteEmployeeRole.Count > 0)
                {
                    dbContextHRM.TblEmployeeRole.RemoveRange(deleteEmployeeRole);
                }
                dbContextHRM.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<List<PositionHierarchyResponse>> GetPositionHierarchy(long companyId) {
            List<PositionHierarchyResponse> obj = new List<PositionHierarchyResponse>();
            try {
                obj = (from p in dbContextHRM.tblPosition
                       join ph in dbContextHRM.tblPositionHierarchy
                       on p.PositionId equals ph.PositionId into leftJoinGroup
                       from ph in leftJoinGroup.DefaultIfEmpty()
                       where p.CompanyId == companyId
                       select new PositionHierarchyResponse {
                           CompanyId = companyId,
                           OrderNumber = ph != null ? ph.OrderNumber : 0,
                           PositionHierarchyId = ph != null ? ph.PositionHierarchyId : 0,
                           PositionId = p.PositionId,
                           PositionName = p.PositionName
                       }).OrderBy(x=>x.OrderNumber).ToList();
                return obj;

            } catch (Exception ex) {
                return obj;
            }
        }

        public async Task<SaveResponseMessage> UpdatePositionHierarchy(List<PositionHierarchyRequest> model) {
            SaveResponseMessage obj = new SaveResponseMessage();
            try {
                long companyId = model.Select(x => x.CompanyId).FirstOrDefault();
                var result = dbContextHRM.tblPositionHierarchy.Where(x => x.CompanyId == companyId).ToList();
                if (result != null && result.Count>0) {
                    dbContextHRM.RemoveRange(result);
                    dbContextHRM.SaveChanges();
                }

                foreach (var rst in model) {
                    TblPositionHierarchy tbl = new TblPositionHierarchy();
                    tbl.PositionId = rst.PositionId;
                    tbl.OrderNumber = rst.OrderNumber;
                    tbl.CompanyId = rst.CompanyId;
                    dbContextHRM.tblPositionHierarchy.Add(tbl);
                }
                dbContextHRM.SaveChanges();
                obj.status = true;
                obj.msg = "Updated successfully.";
                return obj;
            } catch (Exception ex) {
                return obj;
            }
        }

    }
}
