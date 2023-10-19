using HRM_Application.Interfaces;
using HRM_Common.EnumClasses;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRM_Core_WebApp.HubService
{
    public class ChatHub : Hub
    {
        private readonly IchatService _IchatService;
       public ChatHub(IchatService ichat)
        {
            _IchatService = ichat;

        }
        //private readonly Dictionary<string, List<string>> groupConnections = new Dictionary<string, List<string>>();
        public async Task OpenNewPage(string currentUserId,string userName,string type,string brwserInfo)
        {

            string UserSignalRId = _IchatService.GetOrCreateSignalRUserId(currentUserId, ((ChatType)Convert.ToInt64(type)).ToString(), userName).Result; //getting curent SignalR User 
            await _IchatService.CreateConnection(UserSignalRId, Context.ConnectionId, ((ChatType)Convert.ToInt64(type)).ToString(), userName, brwserInfo);
            //check connection id
            await _IchatService.TogleUserOnlineStatus(UserSignalRId, type);
            await Groups.AddToGroupAsync(Context.ConnectionId, UserSignalRId);
            await Clients.Caller.SendAsync("SetsingalRId", UserSignalRId);
            await Clients.Caller.SendAsync("Connected", Context.ConnectionId);
            //every time User Open new page Create New ConnectionID has been created




        }
        //Remove ConnectionIDs
        public async Task LeavePage(string currentUserId,string name, string type,string brwserInfo)
        {
            string UserSignalRId = _IchatService.GetOrCreateSignalRUserId(currentUserId, ((ChatType)Convert.ToInt64(type)).ToString(), name).Result;
            await _IchatService.RemoveConnectionByConnectionID(Context.ConnectionId);
            await _IchatService.TogleUserOnlineStatus(UserSignalRId, type);
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, UserSignalRId);   
        }
        //when user logout
        public async Task LeaveApplication(string currentUserId,string type,string brwserInfo)
        {
            //get All Connections Of that User
            string UserSignalRId = _IchatService.GetSignalRUserId(currentUserId, ((ChatType)Convert.ToInt64(type)).ToString()).Result;
            var ConnectionIDs = _IchatService.GetAllConnectionOfThatUserID(UserSignalRId, ((ChatType)Convert.ToInt64(type)).ToString()).Result ;
            //Loop Remove All one By one
            foreach(var COn in ConnectionIDs)
            { 
             if(!string.IsNullOrEmpty(COn))
            await Groups.RemoveFromGroupAsync(COn, UserSignalRId);
            }
            //remove Connection From Db also
            await _IchatService.RemoveAllConnectionOfThatUserID(UserSignalRId, ((ChatType)Convert.ToInt64(type)).ToString(), brwserInfo);
            await _IchatService.TogleUserOnlineStatus(UserSignalRId, type);
        }
           

        //Send text
        public async Task SendPrivateMessage(string currentUserId, string recipientUserId,string Type,string ReceptType, string message)
        {
            
            var recipientuserName = _IchatService.getUserNameById(recipientUserId, ReceptType).Result;
            var currentUserName = _IchatService.getUserNameById(currentUserId, Type).Result;
            var CurrentUserSignalRId = _IchatService.GetOrCreateSignalRUserId(currentUserId, ((ChatType)Convert.ToInt64(Type)).ToString(), currentUserName).Result;

            string recipientSignalRId = _IchatService.GetOrCreateSignalRUserId(recipientUserId, ((ChatType)Convert.ToInt64(ReceptType)).ToString(), recipientuserName).Result; 
            var checStatus = _IchatService.GetUserOnlineStatus(recipientUserId, ((ChatType)Convert.ToInt64(ReceptType)).ToString()).Result;
           
            var PrivatechatId = _IchatService.GetChatId(CurrentUserSignalRId, recipientSignalRId, ((ChatType)Convert.ToInt64(Type)).ToString()).Result; 
            // Ensure that the sender and recipient are in the same private chat
            if (!string.IsNullOrEmpty(PrivatechatId))
            {
               if( _IchatService.AddMessage(CurrentUserSignalRId, recipientSignalRId, message, PrivatechatId).Result)
                {
                    if(checStatus ==true) //mean if this person is online
                    {
                        // get All Connection Ids Of senders
                      var ConIds =   _IchatService.GetAllConnectionOfThatUserID(recipientSignalRId, ((ChatType)Convert.ToInt64(ReceptType)).ToString()).Result;
                        if(ConIds.Count > 0)
                        {
                        foreach(var Con in ConIds)
                        {
                        await Clients.Client(Con).SendAsync("ReceivePrivateMessage", PrivatechatId, "from:"+ currentUserName + " message:" +message);
                        await Clients.Client(Con).SendAsync("NotifayMe", "you have New Message :"+message);
                        }
                       
                        }
                    }

                    await Clients.Caller.SendAsync("SendMeasseNotifayMe", "Message has been Sent", PrivatechatId);

                }
            }
            else
            {
                // Handle invalid chat access
                // You can log an error, send an error message, etc.
            }
        }

        //private bool IsInSameChat(string user1, string user2, string chatId)
        //{
        //    // Implement logic to check if user1 and user2 are in the same private chat (e.g., from your chat service)
        //    // Return true if they are in the same chat, otherwise false
        //    return true;
        //}
    }
}
