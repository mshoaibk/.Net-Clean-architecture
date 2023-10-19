using HRM_Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM_Application.Interfaces
{
    public interface IchatService
    {
        //Conection Related Services...
        #region Connection
        public Task<bool> CreateConnection(string UserID, string ConnectionID, string type ,string UserName,string brwserInfo);
        public Task<bool> RemoveConnectionByUserId(string UserID, string type);
        public Task<bool> RemoveConnectionByConnectionID(string connectionID);
        public Task<string> GetConnectionById(string UserId,string type);
        public Task<connectionData> GetConnectionIdByConnectionId(string Conid);
        public Task<List<string>> GetAllConnectionOfThatUserID(string userId, string type);
        public Task<bool> RemoveAllConnectionOfThatUserID(string userId, string type, string brwserInfo);
        #endregion
        #region SignalRUser
        public Task<string> GetSignalRUserId(string UserActualID, string Type);
        public  Task<string> GetOrCreateSignalRUserId(string UserActualID, string Type, string Name);
        public Task<bool?> GetUserOnlineStatus(string UserActualId, string Type);
        public Task<SignalR_User> GetSignalRUserRecord(string UserActualId, string Type);
        public  Task<bool> TogleUserOnlineStatus(string SingalRUserId, string Type);
        public  Task<string> getUserNameById(string Id,string ReceptType);
        #endregion
        //Chat Related Services...
        #region chat
        public Task<string> GetChatId(string user1Id, string user2Id,string Type);
        public Task<List<GetAllMychatsWithNewMessage>> GetAllChatOfThatUserID(string UserId);
        public Task<getChatsResponse> UserChats(string UserId, string type);
        public Task<bool> AddMessage(string SenderID, string reciverId, string message, string chatId);
        public Task<List<MyMessages>> GetMessagesByChatID(string ChatId);
        #endregion
    }
}
