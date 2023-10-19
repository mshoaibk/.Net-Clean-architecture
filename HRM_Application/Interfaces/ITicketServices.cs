using HRM_Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM_Application.Interfaces
{
    public interface ITicketServices
    {
        #region Tickets
        public Task<bool> SaveTicket(TicketRequest model);
        public  Task<GetTicketModel> ListTicket(TicketRequestModel model);
        public Task<TicketResponse> GetTicketById(long Id);
        public Task<bool> DeleteTicket(long ticketID);
        #endregion
        #region Tickets Comments
        public Task<bool> SaveTicketComment(Ticket_CommentRequest model);
        public Task<GetTicket_CommentListModel> GetTicketComment(Ticket_CommentRequestModel model);
        #endregion
    }
}
