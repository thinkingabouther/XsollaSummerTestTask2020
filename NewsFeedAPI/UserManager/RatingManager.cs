using NewsFeedAPI.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NewsFeedAPI.Models;
using System.Data.Entity;

namespace NewsFeedAPI.UserManager
{
    public class RatingManager
    {
        public static bool TryLogRating(string token, int id, int rating)
        {
            using (var db = new NewsFeedAPIContext())
            {
                var currentUserRates = from userRate in db.UserRates
                                       where userRate.Token == token
                                       select userRate;
                var currentRatedNews = from userRate in currentUserRates
                                       where userRate.NewsInstanceID == id
                                       select userRate;
                if (currentRatedNews.Count() > 0)
                {
                    return false;
                }
                db.UserRates.Add(new UserRate { Token = token, NewsInstanceID = id, Rating = rating });
                db.SaveChanges();
                return true;
            }
        }

        public static NewsInstance RateNews(int id, int rating)
        {
            using (var db = new NewsFeedAPIContext())
            {
                NewsInstance newsInstance = db.NewsInstances.Find(id);
                if (newsInstance != null)
                {
                    newsInstance.RateSum += rating;
                    newsInstance.RateCount++;
                    db.Entry(newsInstance).State = EntityState.Modified;
                    db.SaveChanges();
                }
                return newsInstance;
            }
        }

        public static UserRate IsRateLogged(string token, int id)
        {
            using (var db = new NewsFeedAPIContext())
            {
                var currentUserRates = from userRate in db.UserRates
                                       where userRate.Token == token
                                       select userRate;
                var currentRatedNews = from userRate in currentUserRates
                                       where userRate.NewsInstanceID == id
                                       select userRate;
                return currentRatedNews.Count() > 0 ? currentRatedNews.First() : null;
            }
        }

        public static NewsInstance CancelRate(UserRate rate)
        {
            using (var db = new NewsFeedAPIContext())
            {
                db.UserRates.Remove(db.UserRates.Find(rate.ID));
                var currentNewsInstance = db.NewsInstances.Find(rate.NewsInstanceID);
                currentNewsInstance.RateCount--;
                currentNewsInstance.RateSum -= rate.Rating;
                db.Entry(currentNewsInstance).State = EntityState.Modified;
                db.SaveChanges();
                return currentNewsInstance;
            }
        }
    }
}