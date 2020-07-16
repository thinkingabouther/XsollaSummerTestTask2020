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
        /// <summary>
        /// Method to send a token for a user
        /// </summary>
        /// <returns>
        /// Returns a JSON object with a new token
        /// </returns>
        public IHttpActionResult GetToken()
        {
            return Ok(new { Token = TokenProvider.GetToken() });
        }
    }
}
