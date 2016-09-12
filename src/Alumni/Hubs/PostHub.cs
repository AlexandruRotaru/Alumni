using Alumni.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace Alumni.Hubs
{
    public class PostHub: Hub
    {
        private string imgFolder = "/Images/profileimages/";
        private string defaultAvatar = "user.png";
        private readonly AlumniDataContext _context;

        public PostHub(AlumniDataContext context)
        {
            _context = context;
        }

        // GET api/WallPost
        public void GetPosts()
        {
            var ret = (from post in _context.Post.ToList()
                       orderby post.Timestamp descending
                       select new
                       {
                            Message = post.Message,
                            PostedBy = post.UserId,
                            PostedByName = _context.DBUser.Where(u => u.UserID == post.UserId).Select(u => u.lName + " " + u.fName).First(),
                            PostedByAvatar = imgFolder + (String.IsNullOrEmpty(_context.DBUser.Where(u => u.UserID == post.UserId).Select(u => u.AvatarExt).First()) ? 
                                                            defaultAvatar : post.UserId + "." + _context.DBUser.Where(u => u.UserID == post.UserId).Select(u => u.AvatarExt).First()),
                            PostedDate = post.Timestamp,
                            PostId = post.PostID,
                            PostComments = from comment in _context.Comment.Where(c => c.PostId == post.PostID).ToList()
                                            orderby comment.Timestamp
                                            select new
                                            {
                                                CommentedBy = comment.UserId,
                                                CommentedByName = _context.DBUser.Where(u => u.UserID == comment.UserId).Select(u => u.lName + " " + u.fName).First(),
                                                CommentedByAvatar = imgFolder + (String.IsNullOrEmpty(_context.DBUser.Where(u => u.UserID == comment.UserId).Select(u => u.AvatarExt).First()) ? 
                                                                                    defaultAvatar : comment.User + "." + _context.DBUser.Where(u => u.UserID == comment.UserId).Select(u => u.AvatarExt).First()),
                                                CommentedDate = comment.Timestamp,
                                                CommentId = comment.CommentID,
                                                Message = comment.Message,
                                                PostId = comment.PostId

                                            }
                        }).ToArray();
            Clients.All.loadPosts(ret);            
        }

        public void AddPost(Post post)
        {
            var name = Context.User.Identity.Name;
            var user = _context.DBUser.Where(u => u.Email == name).First();
            post.UserId = user.UserID;
            post.Timestamp = DateTime.UtcNow;
            _context.Post.Add(post);
            _context.SaveChanges();
            var usr = _context.DBUser.FirstOrDefault(x => x.UserID == post.UserId);
            var ret = new
            {
                Message = post.Message,
                PostedBy = post.UserId,
                PostedByName = usr.lName + " " + usr.fName,
                PostedByAvatar = imgFolder + (String.IsNullOrEmpty(usr.AvatarExt) ? defaultAvatar : post.UserId + "." + post.User.AvatarExt),
                PostedDate = post.Timestamp,
                PostId = post.PostID
            };

            Clients.Caller.addPost(ret);
            Clients.Others.newPost(ret);            
        }

        public dynamic AddComment(Comment postcomment)
        {
            var name = Context.User.Identity.Name;
            var user = _context.DBUser.Where(u => u.Email == name).First();
            postcomment.UserId = user.UserID;
            postcomment.Timestamp = DateTime.UtcNow;

            _context.Comment.Add(postcomment);
            _context.SaveChanges();
            var usr = _context.DBUser.FirstOrDefault(x => x.UserID == postcomment.UserId);
            var ret = new
            {
                CommentedBy = postcomment.UserId,
                CommentedByName = usr.lName + " " + usr.fName,
                CommentedByAvatar = imgFolder + (String.IsNullOrEmpty(usr.AvatarExt) ? defaultAvatar : postcomment.UserId + "." + postcomment.User.AvatarExt),
                CommentedDate = postcomment.Timestamp,
                CommentId = postcomment.CommentID,
                Message = postcomment.Message,
                PostId = postcomment.PostId
            };
            Clients.Others.newComment(ret, postcomment.PostId);
            return ret;            
        }
    }
}
