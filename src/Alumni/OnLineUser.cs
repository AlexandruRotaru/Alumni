using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Alumni.Models;

namespace Alumni
{
    public static class OnLineUser
    {
        public static List<DBLoggedInUser> onLineUserList = new List<DBLoggedInUser>();

        public static void AddUser(string connectionId, DBUser currentUser, int userId)
        {
            DBLoggedInUser user = new DBLoggedInUser();
            user.ConnectionId = connectionId;
            user.UserId = userId;
            user.User = currentUser;

            onLineUserList.Add(user);
        }

        public static void RemoveUser(string connectionId, int? userId)
        {
            var user = onLineUserList.Where(u => u.UserId == userId && u.ConnectionId == connectionId).FirstOrDefault();
            if (user != null) onLineUserList.Remove(user);
        }
    }
}
