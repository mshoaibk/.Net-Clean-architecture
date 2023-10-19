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
    public class TicketServices: ITicketServices
    {
        private readonly HRMContexts dbContextHRM;
        public TicketServices(HRMContexts context)
        {
            dbContextHRM = context;

        }

        #region Tickets
        public async Task<bool> SaveTicket(TicketRequest model)
        {
            TblTickets tblTicketsObj = new TblTickets();
            if (model.action == "update")
            {
                tblTicketsObj = dbContextHRM.TblTickets.Where(emp => emp.TicketID == model.ticketID).FirstOrDefault();
            }
            //
            tblTicketsObj.Title = model.title;
            tblTicketsObj.Description = model.description;
            tblTicketsObj.CompanyId = model.companyId;
            tblTicketsObj.Status= ((TicketStatus)2).ToString();
            tblTicketsObj.ServiceType = model.serviceType;

            // tblPackagesObj. PackageId
           
            if (model.action == "save")
            {
                tblTicketsObj.IsDeleted = false;
                tblTicketsObj.CreatedBy = "SAdmin";
                tblTicketsObj.CreatedDate = DateTime.Now; ;
                dbContextHRM.TblTickets.Add(tblTicketsObj);
            }
            else
            {
                dbContextHRM.Update(tblTicketsObj);
            }
            dbContextHRM.SaveChanges();

            return true;
        }
        public async Task<GetTicketModel> ListTicket(TicketRequestModel model)
        {
            int skipCount = model.pageSize * model.pageIndex;
            int takeCount = model.pageSize;
            GetTicketModel obj = new GetTicketModel();
            IQueryable<TicketResponse> TicketResponseResult;
            //
            TicketResponseResult = (from x in dbContextHRM.TblTickets 
                                    join comp in dbContextHRM.tblCompanyDetail on x.CompanyId equals comp.CompanyID
                                    where x.IsDeleted == false
                                    select 
                                    new TicketResponse
            {
                ticketID = x.TicketID,
                title = x.Title,
                companyName = comp.CompanyName,
                description = x.Description,
                companyId = x.CompanyId,
                status = x.Status,
                createdDate = x.CreatedDate,
                updatedDate = x.UpdatedDate,
                createdBy = x.CreatedBy,
                serviceType = x.ServiceType
               
            }).AsQueryable();



            obj.totalRecords = TicketResponseResult.Count();
            //Page size -1 is for all records
            if (model.pageSize == -1)
                obj.ticketResponseList = TicketResponseResult.ToList();
            else
                obj.ticketResponseList = TicketResponseResult.Skip(skipCount).Take(takeCount).ToList();

            return obj;
        }
        public async Task<TicketResponse> GetTicketById(long Id)
        {
            var Data = (from x in dbContextHRM.TblTickets
                        join comp in dbContextHRM.tblCompanyDetail on x.CompanyId equals comp.CompanyID
                        where x.TicketID == Id
                        select
                        new TicketResponse
                        {
                            ticketID = x.TicketID,
                            title = x.Title,
                            companyName = comp.CompanyName,
                            description = x.Description,
                            companyId = x.CompanyId,
                            status = x.Status,
                            createdDate = x.CreatedDate,
                            updatedDate = x.UpdatedDate,
                            createdBy = x.CreatedBy,
                            serviceType = x.ServiceType,

                        }).AsQueryable();
            return Data.FirstOrDefault();
        }
        public async Task<bool> DeleteTicket(long ticketID)
        {
            var data = dbContextHRM.TblTickets.Where(x => x.TicketID == ticketID).FirstOrDefault();
            data.IsDeleted = true;
            dbContextHRM.Update(data);
            dbContextHRM.SaveChanges();
            return true;
        }
        #endregion

        #region Tickets Comments
        public async Task<bool> SaveTicketComment(Ticket_CommentRequest model)
        {
            TblTicketComments tblCommentObj = new TblTicketComments();
            if (model.action == "update")
            {
                tblCommentObj = dbContextHRM.TblTicketComments.Where(emp => emp.CompanyId == model.commentID).FirstOrDefault();
            }
            //
            tblCommentObj.TicketId = model.ticketId;
            tblCommentObj.Photopath = model.photopath;
            tblCommentObj.CommentText = model.commentText;
            tblCommentObj.CompanyId = model.companyId;
            tblCommentObj.UserId = model.companyId;
           
            // tblPackagesObj. PackageId

            if (model.action == "save")
            {
                
                tblCommentObj.CommentDate = DateTime.Now; ;
                dbContextHRM.TblTicketComments.Add(tblCommentObj);
            }
            else
            {
                dbContextHRM.Update(tblCommentObj);
            }
            dbContextHRM.SaveChanges();


            return true;
        }
        public async Task <GetTicket_CommentListModel> GetTicketComment(Ticket_CommentRequestModel model)
        {
            int skipCount = model.pageSize * model.pageIndex;
            int takeCount = model.pageSize;
            GetTicket_CommentListModel obj = new GetTicket_CommentListModel();
            IQueryable<Ticket_CommentResponse> Ticket_CommentResponseResult;
            Ticket_CommentResponseResult = dbContextHRM.TblTicketComments.Where(x=>x.TicketId == model.ticketID)
                .Select(x=> new Ticket_CommentResponse
                    {
                        commentID = x.CommentID,
                        commentText = x.CommentText,
                        companyId = x.CompanyId,
                        userId = x.UserId,
                        ticketId = x.TicketId,
                        photopath = x.Photopath,
                        commentDate = x.CommentDate,
                    }
            ).AsQueryable();
            obj.totalRecords = Ticket_CommentResponseResult.Count();
            //Page size -1 is for all records
            if (model.pageSize == -1)
                obj.TicketCommentList = Ticket_CommentResponseResult.ToList();
            else
                obj.TicketCommentList = Ticket_CommentResponseResult.Skip(skipCount).Take(takeCount).ToList();

            return obj;
        }
        
        #endregion
    }
}
