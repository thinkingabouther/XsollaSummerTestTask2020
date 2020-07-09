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
        public IHttpActionResult GetToken()
        {
            return Ok(new { Token = TokenProvider.GetToken() });
        }
    }
}
