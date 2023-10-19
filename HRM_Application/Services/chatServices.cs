using HRM_Application.Interfaces;
using HRM_Common.EnumClasses;
using HRM_Domain.Model;
using HRM_Infrastructure.HRMDataBaseContext;
using HRM_Infrastructure.TableEntities;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace HRM_Application.Services
{
    public class chatServices : IchatService
    {
        private readonly HRMContexts dbContextHRM;
        public chatServices(HRMContexts context)
        {
            dbContextHRM = context;
        }

        #region SignalR Connections
        public async Task<bool> CreateConnection(string UserID, string ConnectionID, string type, string UserName, string brwserInfo)
        {
            if (string.IsNullOrEmpty(UserID) || string.IsNullOrEmpty(ConnectionID)) { return false; }
                
            else  
            {
                //just check the dublication 
                if(!dbContextHRM.TblSignalRConnection.Where(x=>x.UserID == UserID && x.SignalRConnectionID== ConnectionID && x.UserType == type).Any())
                {
                    //create connection
                    TblSignalRConnection obj = new TblSignalRConnection();
                    obj.UserID = UserID;
                    obj.brwserInfo = brwserInfo;
                    obj.SignalRConnectionID = ConnectionID;
                    obj.UserType = type;
                    obj.UserName = UserName;
                    dbContextHRM.TblSignalRConnection.Add(obj);
                    dbContextHRM.SaveChanges();
                    return true;

                }
                return false;
            }
           


        }
        public async Task<bool> RemoveConnectionByUserId(string UserID,string type)
        {
            if ((!string.IsNullOrEmpty(UserID)) && dbContextHRM.TblSignalRConnection.Where(x => x.UserID == UserID && x.UserType == type).Any()) {
                var obj = dbContextHRM.TblSignalRConnection.Where(x => x.UserID == UserID ).FirstOrDefault();
                dbContextHRM.TblSignalRConnection.Remove(obj);
                dbContextHRM.SaveChanges();
                return true;
            }
            return false;
        }
        public async Task<bool> RemoveConnectionByConnectionID(string connectionID)
        {

            var obj = dbContextHRM.TblSignalRConnection.Where(x => x.SignalRConnectionID == connectionID).FirstOrDefault();
            if(obj != null)
            {
            dbContextHRM.TblSignalRConnection.Remove(obj);
            dbContextHRM.SaveChanges();
            return true;
            }
            else { return false; }


        }

        public async Task<List<string>> GetAllConnectionOfThatUserID(string userId,string type)
        {
            var ConnectionList = dbContextHRM.TblSignalRConnection.Where(x=>x.UserID == userId && x.UserType == type).Select(x=>x.SignalRConnectionID).ToList();
            return ConnectionList;
        }
        public async Task<bool> RemoveAllConnectionOfThatUserID(string userId,string type, string brwserInfo)
        {
            var ConnectionList = dbContextHRM.TblSignalRConnection.Where(x=>x.UserID == userId && x.UserType == type && x.brwserInfo==brwserInfo).ToList();
            dbContextHRM.TblSignalRConnection.RemoveRange(ConnectionList);
            dbContextHRM.SaveChanges();
            return true;
        }
        public async Task<string> GetConnectionById(string UserId, string type)
        {
            var connectID = dbContextHRM.TblSignalRConnection.Where(x => x.UserID == UserId && x.UserType== type).Select(x => x.SignalRConnectionID).FirstOrDefault();
            return connectID;
        }
        public async Task<connectionData> GetConnectionIdByConnectionId(string Conid)
        {
            connectionData obj = new connectionData();

            obj = dbContextHRM.TblSignalRConnection.Where(x => x.SignalRConnectionID == Conid).Select(x => new connectionData
            {
                ConnectionId = x.SignalRConnectionID,
                userId = x.UserID,
            }).FirstOrDefault();
            return obj;
        }
        #endregion

        #region SignalR User
        public async Task<string> GetSignalRUserId(string UserActualID,string Type)
        {
            var user = Convert.ToInt64(UserActualID);
            var Id= dbContextHRM.TblSignalR_User.Where(x=>x.ActualUserID == user && x.Type == Type).FirstOrDefault()
                .SignalRUserID
                .ToString();

          
           
            return Id;
        }
        public async Task<string> GetOrCreateSignalRUserId(string UserActualID, string Type, string Name)
        {
           
            var user = Convert.ToInt64(UserActualID);
            if (dbContextHRM.TblSignalR_User.Where(x => x.ActualUserID == user && x.Type == Type).Any())
            {
                return dbContextHRM.TblSignalR_User.Where(x => x.ActualUserID == user && x.Type == Type).FirstOrDefault()
                    .SignalRUserID
                    .ToString();

            }
            else
            {
                TblSignalR_User signalR_User = new TblSignalR_User();
                //getting images

                signalR_User.photoPath = Type == "company" ?
                    dbContextHRM.tblCompanyDetail.Where(x => x.CompanyID == Convert.ToInt64(UserActualID)).FirstOrDefault().CompanyLogo 
                    : dbContextHRM.tblEmployee.Where(x => x.EmployeeID == Convert.ToInt64(UserActualID)).FirstOrDefault().EmployeePhoto;
                //
                signalR_User.Name = Name;
                signalR_User.ActualUserID = Convert.ToInt64(UserActualID);
                signalR_User.Type = Type;
                dbContextHRM.TblSignalR_User.Add(signalR_User);
                dbContextHRM.SaveChanges();
                return   signalR_User.SignalRUserID.ToString();
            }
            
        }
        public async Task<bool?> GetUserOnlineStatus(string UserActualId,string Type)
        {
            var user = Convert.ToInt64(UserActualId);
            var Status = dbContextHRM.TblSignalR_User.Where(x => x.ActualUserID == user && x.Type == Type).FirstOrDefault().isOnline;
            Status = Status== null ? false : Status;
            return Status;
        }
        public async Task<SignalR_User> GetSignalRUserRecord(string UserActualId, string Type)
        {
            var user = Convert.ToInt64(UserActualId);
            var Data = dbContextHRM.TblSignalR_User.Where(x => x.ActualUserID == user && x.Type == Type).Select(x=> new SignalR_User
            {
                SignalRUserID = x.SignalRUserID,
                ActualUserID = x.ActualUserID,
                Type = Type,
                isOnline= x.isOnline,
                LastOnlineDate = x.LastOnlineDate,
                Name = x.Name,
               
            }).FirstOrDefault();
           
            return Data;
        }

        public async Task<bool> TogleUserOnlineStatus(string SingalRUserId, string Type)
        {
            bool result = false;
            var User = dbContextHRM.TblSignalR_User.Where(x => x.SignalRUserID == Convert.ToInt64(SingalRUserId)).FirstOrDefault();
            if (User != null)
            {
            var check = dbContextHRM.TblSignalRConnection.Where(x=>x.UserID== SingalRUserId).Any();
             User.isOnline = check;
             dbContextHRM.SaveChanges();
                result = check;
            }
            //check if there is any Connection Exsit for that user 
            return result;
        }

        public async Task<string> getUserNameById(string Id, string ReceptType)
        {

            return ReceptType =="2"?  dbContextHRM.tblEmployee.Where(x=>x.EmployeeID==Convert.ToInt64(Id)).FirstOrDefault().FullName: dbContextHRM.tblCompanyDetail.Where(x => x.CompanyID == Convert.ToInt64(Id)).FirstOrDefault().CompanyName;
        }
        #endregion

        #region Private Chat
        public async Task<string> GetChatId(string user1Id, string user2Id ,string Type) //user1 is sender
        {
         
            var User1Name =  dbContextHRM.TblSignalR_User.Where(x=>x.SignalRUserID == Convert.ToInt64(user1Id)).FirstOrDefault().Name;
            var User2Name = dbContextHRM.TblSignalR_User.Where(x => x.SignalRUserID == Convert.ToInt64(user2Id)).FirstOrDefault().Name;


            if (dbContextHRM.TblPrivateChats.Any(c => (c.User1Id == user1Id && c.User2Id == user2Id) || (c.User1Id == user2Id && c.User2Id == user1Id)) )
            {
                var chat = dbContextHRM.TblPrivateChats.SingleOrDefault(c => (c.User1Id == user1Id && c.User2Id == user2Id) || (c.User1Id == user2Id && c.User2Id == user1Id));

                // If a chat is found, return the ChatId; otherwise, return null
                return chat?.PrivateChatId.ToString();
            }
            else
            {
                var newChat = new TblPrivateChats
                {
                    User1Id = user1Id,
                    User2Id = user2Id,
                    user1Name = User1Name, //send from
                    user2Name = User2Name, //send send to
                    ChatType = Type,   // Set the appropriate chat type
                };

                dbContextHRM.TblPrivateChats.Add(newChat);
                dbContextHRM.SaveChanges();

                return newChat?.PrivateChatId.ToString();
            }

            // For this example, we'll assume that there is no existing chat


        }

        public async Task<List<GetAllMychatsWithNewMessage>> GetAllChatOfThatUserID(string UserId)
        {
            List<GetAllMychatsWithNewMessage> List = new List<GetAllMychatsWithNewMessage>();
           var ChatList =  dbContextHRM.TblPrivateChats.Where(x=>x.User1Id==UserId||x.User2Id == UserId).ToList();

            //get new new Or Last message
            foreach(var i in ChatList.Where(x=>x.User2Id != x.User1Id))
            {
                GetAllMychatsWithNewMessage obj = new GetAllMychatsWithNewMessage();
                obj.chatId = i.PrivateChatId;
                obj.SenderSignalRId = UserId == i.User1Id?i.User2Id:i.User1Id;
                obj.ActualUserID = dbContextHRM.TblSignalR_User.Where(x=>x.SignalRUserID== Convert.ToInt64(obj.SenderSignalRId)).FirstOrDefault().ActualUserID.ToString();
                obj.UserName = UserId == i.User1Id ? i.user2Name : i.user1Name;
                obj.fisrtMessage = dbContextHRM.TblPrivateMessages.Where(x=>x.ReceiverUserId == obj.SenderSignalRId || x.SenderUserId == obj.SenderSignalRId).OrderByDescending(x=>x.PrivateMessageId).FirstOrDefault().MessageText;
                obj.date = dbContextHRM.TblPrivateMessages.Where(x => x.ReceiverUserId == obj.SenderSignalRId || x.SenderUserId == obj.SenderSignalRId).OrderByDescending(x => x.PrivateMessageId).FirstOrDefault().Timestamp;
                List.Add(obj);
            }
            return List.OrderByDescending(x=>x.date).ToList();

        }
        public async Task<getChatsResponse> UserChats(string UserId, string type)
        {
            getChatsResponse obj = new getChatsResponse();
            obj.MyChatsList =  dbContextHRM.TblPrivateChats.Where(x=>(x.User1Id == UserId || x.User2Id == UserId)&& x.ChatType== type).Select(x=> new MyChats
            {
                ChatId = x.PrivateChatId,
                UserId = x.User1Id==UserId?x.User2Id:x.User1Id,
                Username = x.ChatType,

            }).ToList();
            obj.totalChats = obj.MyChatsList.Count;
            return obj;
        }
        #endregion

        #region private Message
        public async Task<bool> AddMessage(string SenderID, string reciverId, string message, string chatId)
        {
            TblPrivateMessages obj = new TblPrivateMessages() {
                MessageText = message,
                ReceiverUserId = reciverId,
                SenderUserId = SenderID,
                IsDeleted = false,
                IsSeen = false,
                Timestamp = DateTime.Now,
                PrivateChatId = Convert.ToInt64(chatId),

            };
            dbContextHRM.TblPrivateMessages.Add(obj);
            dbContextHRM.SaveChanges();
            return true;
        }
        public async Task<List<MyMessages>> GetMessagesByChatID(string ChatId) {
            List<MyMessages> obj = new List<MyMessages>();
            obj = dbContextHRM.TblPrivateMessages.Where(x=>x.PrivateChatId == Convert.ToInt64(ChatId)).Select(x=>  new MyMessages
            {
                chatId = x.PrivateChatId,
                messageID = x.PrivateMessageId,
                message  = x.MessageText,
                senderID = x.SenderUserId,
                date = x.Timestamp

            }).OrderBy(x=>x.messageID).ToList();

            return obj;

        }


        #endregion

    }
    public class SignalR_User
    {
        public long SignalRUserID { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public bool? isOnline { get; set; }
        public DateTime? LastOnlineDate { get; set; }
        public long ActualUserID { get; set; }
    }
    public class connectionData
    {
        public string ConnectionId { get; set; }
        public string userId { get; set; }
    }
    public class getChatsResponse
    {
      public int totalChats { get; set; }
      public List<MyChats> MyChatsList { get; set; }
    }
    public class MyChats
    {
        public long ChatId { get; set; }
        public string UserId { get; set; }
        public string Username { get; set; }
    }
    public class MyMessages
    {
       public long messageID { get; set; }
       public string message { get; set; }
       public long chatId { get; set; }
        public string senderID { get; set; }
       public DateTime? date { get; set; }
    }

    public class GetAllMychatsWithNewMessage
    {
        public string photoPath { get; set; }
        public long chatId { get; set;} 
        public string UserName { get; set; }
        public string SenderSignalRId { get; set; }
        public string fisrtMessage { get; set; }
        public string ActualUserID { get; set; }
        public DateTime? date {  get; set; }

    }

}
