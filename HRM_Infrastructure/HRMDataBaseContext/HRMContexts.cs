using HRM_Domain.Model;
using HRM_Infrastructure.TableEntities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM_Infrastructure.HRMDataBaseContext
{
    public class HRMContexts : DbContext
    {
        private readonly DbContextOptions _options;

        public HRMContexts(DbContextOptions<HRMContexts> options) : base(options)
        {
            _options = options;
        }
        protected override void OnModelCreating(Microsoft.EntityFrameworkCore.ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TblPostJob>().Property(e => e.PostJobId).IsRequired().UsePropertyAccessMode(PropertyAccessMode.Field);
            modelBuilder.Entity<TblPostJobBenefit>().Property(e => e.PostJobBenefitId).IsRequired().UsePropertyAccessMode(PropertyAccessMode.Field);
            modelBuilder.Entity<TblCompanyDetail>().Property(e => e.CompanyID).IsRequired().UsePropertyAccessMode(PropertyAccessMode.Field);
            modelBuilder.Entity<TblEmployees>().Property(e => e.EmployeeID).IsRequired().UsePropertyAccessMode(PropertyAccessMode.Field);
            modelBuilder.Entity<AspNetUsers>().Property(e => e.Id).IsRequired().UsePropertyAccessMode(PropertyAccessMode.Field);
            modelBuilder.Entity<TblPostJobRequiredSkills>().Property(e => e.PostJobRequiredSkillsId).IsRequired().UsePropertyAccessMode(PropertyAccessMode.Field);
            modelBuilder.Entity<TblCandidateJobApplications>().Property(e => e.CandidateJobApplicationId).IsRequired().UsePropertyAccessMode(PropertyAccessMode.Field);
            modelBuilder.Entity<TblEmployeeSalary>().Property(e => e.EmployeeSalaryID).IsRequired().UsePropertyAccessMode(PropertyAccessMode.Field);
            modelBuilder.Entity<TblEmployeeSalarySlip>().Property(e => e.EmployeeSalarySlipID).IsRequired().UsePropertyAccessMode(PropertyAccessMode.Field);
            modelBuilder.Entity<TblEmployeeBankDetail>().Property(e => e.EmployeeBankDetailId).IsRequired().UsePropertyAccessMode(PropertyAccessMode.Field);
            modelBuilder.Entity<TblCompanyAnnouncement>().Property(e => e.CompanyAnnouncementID).IsRequired().UsePropertyAccessMode(PropertyAccessMode.Field);
            modelBuilder.Entity<TblEmployeeAttendance>().Property(e => e.EmployeeAttendanceID).IsRequired().UsePropertyAccessMode(PropertyAccessMode.Field);
            modelBuilder.Entity<TblEmployeeLeaveRequest>().Property(e => e.EmployeeLeaveRequestID).IsRequired().UsePropertyAccessMode(PropertyAccessMode.Field);
            modelBuilder.Entity<TblOfficeLocation>().Property(e => e.OfficeId).IsRequired().UsePropertyAccessMode(PropertyAccessMode.Field);
            modelBuilder.Entity<TblDepartment>().Property(e => e.DepartmentId).IsRequired().UsePropertyAccessMode(PropertyAccessMode.Field);
            modelBuilder.Entity<TblTeam>().Property(e => e.TeamId).IsRequired().UsePropertyAccessMode(PropertyAccessMode.Field);
            modelBuilder.Entity<TblPosition>().Property(e => e.PositionId).IsRequired().UsePropertyAccessMode(PropertyAccessMode.Field);
            modelBuilder.Entity<TblEmploymentTypeSetup>().Property(e => e.EmploymentTypeId).IsRequired().UsePropertyAccessMode(PropertyAccessMode.Field);
            modelBuilder.Entity<TblOnBoarding>().Property(e => e.OnBoardingId).IsRequired().UsePropertyAccessMode(PropertyAccessMode.Field);
            modelBuilder.Entity<TblProbationPeriod>().Property(e => e.ProbationPeriodId).IsRequired().UsePropertyAccessMode(PropertyAccessMode.Field);
            modelBuilder.Entity<TblOfficeMapAddress>().Property(e => e.OfficeMapAddressId).IsRequired().UsePropertyAccessMode(PropertyAccessMode.Field);
            modelBuilder.Entity<TblShiftSetup>().Property(e => e.ShiftId).IsRequired().UsePropertyAccessMode(PropertyAccessMode.Field);
            modelBuilder.Entity<TblWeeklyHolidays>().Property(e => e.WeeklyholidaysId).IsRequired().UsePropertyAccessMode(PropertyAccessMode.Field); 
            modelBuilder.Entity<TblYearlyHolidays>().Property(e => e.YearlyHolidaysId).IsRequired().UsePropertyAccessMode(PropertyAccessMode.Field);
            modelBuilder.Entity<TblEmployeeRoles>().Property(e => e.EmployeeRoleId).IsRequired().UsePropertyAccessMode(PropertyAccessMode.Field);
            modelBuilder.Entity<TblRole>().Property(e => e.RoleId).IsRequired().UsePropertyAccessMode(PropertyAccessMode.Field);
            modelBuilder.Entity<TblTransaction>().Property(e => e.TransactionId).IsRequired().UsePropertyAccessMode(PropertyAccessMode.Field);
            modelBuilder.Entity<TblSubscriptionPackages>().Property(e => e.PackageId).IsRequired().UsePropertyAccessMode(PropertyAccessMode.Field);
            modelBuilder.Entity<TblCompanySubscription>().Property(e => e.SubscriptionId).IsRequired().UsePropertyAccessMode(PropertyAccessMode.Field);
            modelBuilder.Entity<TblTickets>().Property(e => e.TicketID).IsRequired().UsePropertyAccessMode(PropertyAccessMode.Field);
            modelBuilder.Entity<TblTicketComments>().Property(e => e.CompanyId).IsRequired().UsePropertyAccessMode(PropertyAccessMode.Field);
            modelBuilder.Entity<TblSignalRConnection>().Property(e => e.ID).IsRequired().UsePropertyAccessMode(PropertyAccessMode.Field);
            modelBuilder.Entity<TblPrivateMessages>().Property(e => e.PrivateMessageId).IsRequired().UsePropertyAccessMode(PropertyAccessMode.Field);
            modelBuilder.Entity<TblPrivateChats>().Property(e => e.PrivateChatId).IsRequired().UsePropertyAccessMode(PropertyAccessMode.Field);

            modelBuilder.Entity<TblPositionHierarchy>().Property(e => e.PositionHierarchyId).IsRequired().UsePropertyAccessMode(PropertyAccessMode.Field);
            modelBuilder.Entity<TblHierarchyEmployees>().Property(e => e.HierarchyEmployeesId).IsRequired().UsePropertyAccessMode(PropertyAccessMode.Field);
            modelBuilder.Entity<TblSignalR_User>().Property(e => e.SignalRUserID).IsRequired().UsePropertyAccessMode(PropertyAccessMode.Field);
        }
        public DbSet<AspNetUsers> tblAspNetUsers { get; set; }
        public DbSet<TblPostJob> tblPostJob { get; set; }
        public DbSet<TblPostJobBenefit> tblPostJobBenefit { get; set; }
        public DbSet<TblPostJobRequiredSkills> tblPostJobRequiredSkills { get; set; }
        public DbSet<TblCompanyDetail> tblCompanyDetail { get; set; }
        public DbSet<TblEmployees> tblEmployee { get; set; }
        public DbSet<TblCandidateJobApplications> tblCandidateJobApplications { get; set; }
        public DbSet<TblEmployeeSalary> tblEmployeeSalary { get; set; }
        public DbSet<TblEmployeeSalarySlip> tblEmployeeSalarySlip { get; set; }
        public DbSet<TblEmployeeBankDetail> tblEmployeeBankDetail { get; set; }
        public DbSet<TblCompanyAnnouncement> tblCompanyAnnouncement { get; set; }
        public DbSet<TblEmployeeAttendance> tblEmployeeAttendance { get; set; }
        public DbSet<TblEmployeeLeaveRequest> tblEmployeeLeaveRequest { get; set; }
        public DbSet<TblOfficeLocation> tblOfficeLocation { get; set; }
        public DbSet<TblDepartment> tblDepartment { get; set; }
        public DbSet<TblTeam> tblTeam { get; set; }
        public DbSet<TblPosition> tblPosition { get; set; }
        public DbSet<TblEmploymentTypeSetup> tblEmploymentTypeSetup { get; set; }
        public DbSet<TblOnBoarding> tblOnBoarding { get; set; }
        public DbSet<TblProbationPeriod> tblProbationPeriod { get; set; }
        public DbSet<TblOfficeMapAddress> tblOfficeMapAddress { get; set; }
        public DbSet<TblShiftSetup> tblShiftSetup { get; set; }
        public DbSet<TblWeeklyHolidays> TblWeeklyHolidays { get; set; }
        public DbSet<TblYearlyHolidays> TblYearlyHolidays { get; set; }
        public DbSet<TblEmployeeRoles> TblEmployeeRole { get; set; }
        public DbSet<TblRole> TblRole { get; set; }
        public DbSet<TblTransaction> TblTransaction { get; set; } 
        public DbSet<TblSubscriptionPackages> TblSubscriptionPackages { get; set; }
        public DbSet<TblCompanySubscription> TblCompanySubscription { get; set; }
        public DbSet<TblTickets> TblTickets { get; set; }
        public DbSet<TblTicketComments> TblTicketComments { get; set; }
        public DbSet<TblSignalRConnection> TblSignalRConnection { get; set; }
        public DbSet<TblPrivateMessages> TblPrivateMessages { get; set; }
        public DbSet<TblPrivateChats> TblPrivateChats { get; set; }

        public DbSet<TblPositionHierarchy> tblPositionHierarchy { get; set; }
        public DbSet<TblHierarchyEmployees> tblHierarchyEmployees { get; set; }
        public DbSet<TblSignalR_User> TblSignalR_User { get; set; }
    }
}
