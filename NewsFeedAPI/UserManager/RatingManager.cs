using NewsFeedAPI.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NewsFeedAPI.Models;
using System.Data.Entity;
using System.Data;

namespace NewsFeedAPI.UserManager
{
    public class RatingManager
    {
        /// <summary>
        /// Method to log an information about a rate given by a user with given token
        /// </summary>
        /// <returns>
        /// Returns bool result showing whether a rate was logged. Returns false if user with given token already rated the instance with given id
        /// </returns>
        /// <exception cref="UnregistredTokenException">Thrown in case given token was not issued by the system </exception>
        /// <exception cref="RatingOutOfBoundsException">Thrown in case given rate does not fit the range attribute for the property </exception>
        public static bool TryLogRating(string token, int id, int rating)
        {
            using (var db = new NewsFeedAPIContext())
            { 
                if ((from userRate in db.UserRates where userRate.Token == token select userRate.Token).Count() < 1) 
                    throw new UnregistredTokenException($"Token {token} is not registred!");
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
                try
                {
                    db.SaveChanges();
                }
                catch (DBConcurrencyException)
                {
                    throw new RatingOutOfBoundsException("Rating has to fit in the bounds declared with the entity property");
                }
                return true;
            }
        }

        /// <summary>
        /// Method to add given rating to an instance with given id
        /// </summary>
        /// <returns>
        /// Returns an instance with modified rating
        /// </returns>
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

        /// <summary>
        /// Method to check whether the user with given token already rated the instance with given
        /// </summary>
        /// <returns>
        /// Returns UserRate instance if there is one. Returns null if the user hasn't rated the instance
        /// </returns>v
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

        /// <summary>
        /// Method to remove the rating that is given in UserRate instance
        /// </summary>
        /// <returns>
        /// Returns an instance with modified rating
        /// </returns>
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

    public class UnregistredTokenException : Exception
    {
        public UnregistredTokenException(string message) : base(message)
        {

        }
    }

    public class RatingOutOfBoundsException : Exception
    {
        public RatingOutOfBoundsException(string message) : base(message)
        {

        }
    }
}