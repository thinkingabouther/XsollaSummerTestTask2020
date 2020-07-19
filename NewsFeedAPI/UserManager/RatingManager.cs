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
    public enum LoggingError
    {
        NoError,
        NoNewsRated,
        UnregistredToken,
        RatingOutOfBounds,
        SavingError
    }

    public class RatingManager
    {
        private static int minRating = 1;
        private static int maxRating = 5;

        /// <summary>
        /// Method to log an information about a rate given by a user with given token
        /// </summary>
        /// <returns>
        /// Returns bool result showing whether a rate was logged. Returns false if user with given token already rated the instance with given id
        /// </returns>
        /// <exception cref="UnregistredTokenException">Thrown in case given token was not issued by the system </exception>
        /// <exception cref="RatingOutOfBoundsException">Thrown in case given rate does not fit the range attribute for the property </exception>
        public static void TryLogRating(INewsFeedAPIContext db, string token, int id, int rating, out LoggingError loggingError)
        {
            var ratesWithGivenToken = from userRate in db.UserRates 
                                      where userRate.Token == token 
                                      select userRate;
            if (rating < minRating || rating > maxRating)
            {
                loggingError = LoggingError.RatingOutOfBounds;
                return;
            }
            if (ratesWithGivenToken.Count() < 1)
            {
                loggingError = LoggingError.UnregistredToken;
                return;
            }
            var newsRatedWithGivenToken = from userRate in ratesWithGivenToken
                                          where userRate.NewsInstanceID == id
                                          select userRate;
            if (newsRatedWithGivenToken.Count() > 0)
            {
                loggingError = LoggingError.NoNewsRated;
                return;
            }
            db.UserRates.Add(new UserRate { Token = token, NewsInstanceID = id, Rating = rating });
            try
            {
                db.SaveChanges();
            }
            catch (DBConcurrencyException)
            {
                loggingError = LoggingError.SavingError;
                return;
            }
            loggingError = LoggingError.NoError;
        }

        /// <summary>
        /// Method to add given rating to an instance with given id
        /// </summary>
        /// <returns>
        /// Returns an instance with modified rating
        /// </returns>
        public static NewsInstance RateNews(INewsFeedAPIContext db, int id, int rating)
        {
            NewsInstance newsInstance = db.NewsInstances.Find(id);
            if (newsInstance != null)
            {
                newsInstance.RateSum += rating;
                newsInstance.RateCount++;
                db.MarkAsModified(newsInstance);
                db.SaveChanges();
            }
            return newsInstance;
        }

        /// <summary>
        /// Method to check whether the user with given token already rated the instance with given
        /// </summary>
        /// <returns>
        /// Returns UserRate instance if there is one. Returns null if the user hasn't rated the instance
        /// </returns>v
        public static UserRate CheckRateLogged(INewsFeedAPIContext db, string token, int id)
        {
            var currentUserRates = from userRate in db.UserRates
                                    where userRate.Token == token
                                    select userRate;
            var currentRatedNews = from userRate in currentUserRates
                                    where userRate.NewsInstanceID == id
                                    select userRate;
            return currentRatedNews.Count() > 0 ? currentRatedNews.First() : null;
        }

        /// <summary>
        /// Method to remove the rating that is given in UserRate instance
        /// </summary>
        /// <returns>
        /// Returns an instance with modified rating
        /// </returns>
        public static NewsInstance CancelRate(INewsFeedAPIContext db, UserRate rate)
        {
            db.UserRates.Remove(db.UserRates.Find(rate.ID));
            var currentNewsInstance = db.NewsInstances.Find(rate.NewsInstanceID);
            currentNewsInstance.RateCount--;
            currentNewsInstance.RateSum -= rate.Rating;
            db.MarkAsModified(currentNewsInstance);
            db.SaveChanges();
            return currentNewsInstance;
        }   
    }
}