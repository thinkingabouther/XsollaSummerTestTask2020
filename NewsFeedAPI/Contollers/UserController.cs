using NewsFeedAPI.Data;
using NewsFeedAPI.Models;
using NewsFeedAPI.UserManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace NewsFeedAPI.Contollers
{
    public class UserController : ApiController
    {
        private INewsFeedAPIContext db = new NewsFeedAPIContext();

        public UserController() { }

        public UserController(INewsFeedAPIContext context)
        {
            db = context;
        }
        /// <summary>
        /// Method to send a token for a user
        /// </summary>
        public IHttpActionResult GetToken()
        {
            return Ok(TokenProvider.GetToken(db));
        }
    }
}
