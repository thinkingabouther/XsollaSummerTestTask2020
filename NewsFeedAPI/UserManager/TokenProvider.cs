using NewsFeedAPI.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NewsFeedAPI.Models;

namespace NewsFeedAPI.UserManager
{
    public class TokenProvider
    {
        const int tokenLength = 10;
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        static Random random = new Random();
        public static string GetToken()
        {
            using (var db = new NewsFeedAPIContext())
            {
                while (true) 
                {
                    var token = new string(Enumerable.Repeat(chars, tokenLength).Select(s => s[random.Next(s.Length)]).ToArray());
                    if ((from userRate in db.UserRates where userRate.Token == token select userRate).Count() == 0)
                    {
                        db.UserRates.Add(new UserRate { Token = token, NewsInstanceID = -1, Rating = -1 });
                        db.SaveChanges();
                        return token;
                    }
                }
            }
        }
    }
}