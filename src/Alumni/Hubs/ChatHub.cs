using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Alumni.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.SignalR.Hubs;
using Microsoft.AspNetCore.Http;

namespace Alumni.Hubs
{
    public class ChatHub : Hub
    {
        private readonly AlumniDataContext _context;

        public ChatHub(AlumniDataContext context)
        {
            _context = context;
        }

        public void Send(string name, string message)
        {
            if (Clients != null)
            {
                DBChatMessage mess = new DBChatMessage();

                string[] userName = name.Split(new char[] { ' ' }, 2);
                string fn = userName[1];
                string ln = userName[0];
                var userId = _context.DBUser.Where(u => u.fName == fn && u.lName == ln).Select(u => u.UserID).FirstOrDefault();

                mess.Text = message;
                mess.UserId = userId;
                mess.Timestamp = DateTime.Now;
                _context.DBChatMessage.AddRange(mess);

                _context.SaveChanges();

                // Call the addMessage method on all clients
                Clients.All.addNewMessage(name, message);
            }
        }

        public override Task OnConnected()
        {
            var connectionId = Context.ConnectionId;
            //string firstName = Context.Request.HttpContext.Session.GetString("Nume");
            //string lastName = Context.Request.HttpContext.Session.GetString("Prenume");
            var name = Context.User.Identity.Name;
            var user = _context.DBUser.Where(u => u.Email == name).First();

            OnLineUser.AddUser(connectionId, user, user.UserID);

            var loggedUser = _context.DBLoggedInUser.Where(u => u.UserId == user.UserID).FirstOrDefault();
            if (loggedUser != null)
            {
                loggedUser.ConnectionId = connectionId;
                _context.DBLoggedInUser.Update(loggedUser);
            }
            else
            {
                DBLoggedInUser newLUser = new DBLoggedInUser();
                newLUser.ConnectionId = connectionId;
                newLUser.UserId = user.UserID;
                _context.DBLoggedInUser.AddRange(newLUser);
            }
          
            var userName = user.lName + " " + user.fName;     
            Clients.Caller.onConnected(user.UserID,connectionId, userName, OnLineUser.onLineUserList);
            Clients.AllExcept(connectionId).newUserConnected(user.UserID, userName, OnLineUser.onLineUserList);

            _context.SaveChanges();
            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {

            var connectionId = Context.ConnectionId;
            var name = Context.User.Identity.Name;
            var userName = _context.DBUser.Where(u => u.Email == name).Select(u => u.lName + " " + u.fName).First();

            var user = _context.DBLoggedInUser.Where(u => u.ConnectionId == connectionId).FirstOrDefault();
            if (user != null)
            {
                user.ConnectionId = null;
                _context.DBLoggedInUser.Update(user);
                _context.SaveChanges();
                OnLineUser.RemoveUser(connectionId, user.UserId);

                Clients.AllExcept(connectionId).onUserDisconnected(user.UserId, userName, OnLineUser.onLineUserList);
            }
            
            return base.OnDisconnected(stopCalled);
        }

        public void CreateGroup(int currentUserId, int toConnectTo)
        {
            string strGroupName = GetUniqueGroupName(currentUserId, toConnectTo);
            string connectionId_To = OnLineUser.onLineUserList.Where(item => item.UserId == toConnectTo).Select(item => item.ConnectionId).SingleOrDefault();
            string name = OnLineUser.onLineUserList.Where(item => item.UserId == toConnectTo).Select(item => item.User.lName + " " + item.User.fName).First();
            if (!string.IsNullOrEmpty(connectionId_To))
            {
                Groups.Add(Context.ConnectionId, strGroupName);
                Groups.Add(connectionId_To, strGroupName);
                Clients.Caller.setChatWindow(strGroupName, toConnectTo, name);
            }
        }

        private string GetUniqueGroupName(int currentUserId, int toConnectTo)
        {
            return (currentUserId.GetHashCode() ^ toConnectTo.GetHashCode()).ToString();
        }

        public void SendPrivateMessage(string message, string groupName, int fromUserId, int toUser)
        {
            if (Clients != null)
            {
                DBPrivateMessage privateMessage = new DBPrivateMessage();
                privateMessage.Text = message;               
                privateMessage.UserId = fromUserId;
                privateMessage.ToUserId = toUser;
                privateMessage.TimeStamp = DateTime.Now;
                _context.DBPrivateMessage.AddRange(privateMessage);

                _context.SaveChanges();

                var name = _context.DBUser.Where(u => u.UserID == fromUserId).Select(u => u.lName + " " + u.fName).First();
                Clients.Group(groupName).addMessage(message, groupName, fromUserId, toUser, name);
            }
        }
    }    
}

