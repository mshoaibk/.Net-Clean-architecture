using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM_Domain.Model
{
    #region Ticket
    public class TicketRequest
    {
        public long ticketID { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public long companyId { get; set; }
        public int status { get; set; }
        public DateTime? createdDate { get; set; }
        public DateTime? updatedDate { get; set; }
        public string createdBy { get; set; }
        public string serviceType { get; set; }
        public string action { get; set; }
    }
    public class TicketRequestModel
    {
        public long? ticketID { get; set; }
        public int pageIndex { get; set; }
        public int pageSize { get; set; }
    }
    public class TicketResponse
    {
        public long ticketID { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public long companyId { get; set; }
        public string companyName { get; set; }
        public string status { get; set; }
        public string serviceType { get; set; }
        public DateTime? createdDate { get; set; }
        public DateTime? updatedDate { get; set; }
        public string createdBy { get; set; }


    }
    public class GetTicketModel
    {
        public List<TicketResponse> ticketResponseList { get; set; }
        public Int32 totalRecords { get; set; }
    }
    #endregion

    #region Ticket Comments
    public class Ticket_CommentRequest
    {

        public long? commentID { get; set; }
        public long ticketId { get; set; }
        public string commentText { get; set; }
        public DateTime commentDate { get; set; }
        public long companyId { get; set; }
        public long userId { get; set; }
        public string photopath { get; set; }
        public string photoType { get; set; }
        public IFormFile employeePhoto { get; set; }
       // public string serviceType { get; set; }
        public string action { get; set; }

    }
    public class Ticket_CommentRequestModel
    {
        public long? ticketID { get; set; }
        public int pageIndex { get; set; }
        public int pageSize { get; set; }
    }

    public class Ticket_CommentResponse
    {
       
        public long commentID { get; set; }
        public long ticketId { get; set; }
        public string commentText { get; set; }
        public DateTime? commentDate { get; set; }
        public long? companyId { get; set; }
        public long userId { get; set; }
        public string photopath { get; set; }
       

    }
    public class GetTicket_CommentListModel
    {
        public List<Ticket_CommentResponse> TicketCommentList { get; set; }
        public Int32 totalRecords { get; set; }
    }
    #endregion
}
