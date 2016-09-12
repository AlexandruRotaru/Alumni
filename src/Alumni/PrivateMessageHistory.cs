using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Alumni.Models;

namespace Alumni
{
    public class PrivateMessageHistory
    {
        public class UserInfo
        {
            private string name;
            private string message;

            public string Name { get; set; }
            public string Message { get; set; }
           
        }

        private AlumniDataContext _context;

        private List<UserInfo> users = new List<UserInfo>();

        private int _currentUserId, _toConnectTo;
        public PrivateMessageHistory(AlumniDataContext context, int  currentUserId, int toConnectTo)
        {
            _context = context;
            _currentUserId = currentUserId;
            _toConnectTo = toConnectTo;
        }

        public void AddMessage(string userName, string message)
        {
            UserInfo user = new UserInfo();

            user.Name = userName;
            user.Message = message;
            users.Add(user);
        }

        public List<UserInfo> GetMessages()
        {
            var messages = _context.DBPrivateMessage.Where(p => (p.UserId == _currentUserId && p.ToUserId == _toConnectTo) || (p.UserId == _toConnectTo && p.ToUserId == _currentUserId)).ToList();
            foreach (var message in messages)
            {
                AddMessage(_context.DBUser.Where(u => u.UserID == message.UserId).Select(u => u.lName + " " + u.fName).First(), message.Text);
            }
            return users;
        }
    }
}
