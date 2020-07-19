﻿using NewsFeedAPI.Data;
using NewsFeedAPI.Models;
using NewsFeedAPI.UserManager;
using NewsFeedAPI.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace NewsFeedAPI.Contollers
{
    public class UserRatingController : ApiController
    {
        private INewsFeedAPIContext db = new NewsFeedAPIContext();

        public UserRatingController() { }

        public UserRatingController(INewsFeedAPIContext context)
        {
            db = context;
        }

        /// <summary>
        /// Method to cancel the rate by id
        /// </summary>
        /// <returns>
        /// Returns bad request in case of token not given as header or if the instance was not rated by the user with given token
        /// Returns the modified entity in case the rate was cancelled successfully
        /// </returns>
        public IHttpActionResult DeleteUserRating(int newsInstanceId)
        {
            IEnumerable<string> values;
            if (!Request.Headers.TryGetValues("Token", out values))
            {
                return BadRequest("Token was not given. Use a token that was used to rate this instance");
            }
            string token = values.FirstOrDefault();
            var userRating = RatingManager.IsRateLogged(db, token, newsInstanceId);
            if (userRating == null)
            {
                return BadRequest("User with given token has not rated the news with given id");
            }
            return Ok((NewsInstanceViewModel)RatingManager.CancelRate(db, userRating));
        }

        /// <summary>
        /// Method to rate an instance by id
        /// </summary>
        /// <returns>
        /// Returns bad request in case of token not given as header/improper token or if user with the given token has already rated the instance
        /// Returns the modified entity in case the rate was successful
        /// </returns>
        public IHttpActionResult PostUserRating(int newsInstanceId, int rating)
        {
            IEnumerable<string> values;
            if (!Request.Headers.TryGetValues("Token", out values))
            {
                return BadRequest("Token was not given. Issue a token via api/User/Token and attach it to as a header to your request");
            }
            string token = values.FirstOrDefault();
            bool isNewsRated;
            try
            {
                isNewsRated = !RatingManager.TryLogRating(db, token, newsInstanceId, rating);
            }
            catch (UnregistredTokenException e)
            {
                return BadRequest(e.Message);
            }
            if (isNewsRated)
            {
                return BadRequest("User with this token has already rated that piece of news");
            }
            try
            {
                NewsInstance newsInstance = RatingManager.RateNews(db, newsInstanceId, rating);
                if (newsInstance == null)
                    return NotFound();
                return Ok((NewsInstanceViewModel)newsInstance);
            }
            catch (RatingOutOfBoundsException e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
