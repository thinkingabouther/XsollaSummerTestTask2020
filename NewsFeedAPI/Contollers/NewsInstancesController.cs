using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.Results;
using System.Web.Script.Serialization;
using NewsFeedAPI.Data;
using NewsFeedAPI.Models;
using NewsFeedAPI.UserManager;
using NewsFeedAPI.ViewModel;

namespace NewsFeedAPI.Contollers
{
    public class NewsInstancesController : ApiController
    {
        private INewsFeedAPIContext db = new NewsFeedAPIContext();
        public NewsInstancesController()
        {
        }

        public NewsInstancesController(INewsFeedAPIContext context)
        {
            db = context;
        }

        /// <summary>
        /// Method to return array with all news instances from database.
        /// </summary>
        /// <returns>
        /// Returns JSON array with instances as a NewsInstanceViewModel 
        /// </returns>
        public IHttpActionResult GetNewsInstances()
        {
            if (db.NewsInstances.Count() < 1)
                return NotFound();
            var news = new List<NewsInstanceViewModel>();
            foreach (var newsInstance in db.NewsInstances)
            {
                news.Add((NewsInstanceViewModel)newsInstance);
            }
            var response = Request.CreateResponse(news);
            response.Headers.Add("Count", news.Count.ToString()) ;
            return base.ResponseMessage(response);
        }
        /// <summary>
        /// Method to return a particular news instance by id
        /// </summary>
        /// <returns>
        /// Returns an instance with given id as a NewsInstanceViewModel 
        /// </returns>
        [ResponseType(typeof(NewsInstance))]
        public IHttpActionResult GetNewsInstance(int id)
        {
            NewsInstance newsInstance = db.NewsInstances.Find(id);
            if (newsInstance == null)
                return NotFound();
            return Ok((NewsInstanceViewModel)newsInstance);
        }
        /// <summary>
        /// Method to add an instance to database
        /// </summary>
        /// <returns> 
        /// Returns BadRequest in case given instance is invalid
        /// Returns a locaton of the saved instance via header
        /// </returns>
        [ResponseType(typeof(NewsInstance))]
        public IHttpActionResult PostNewsInstance(NewsInstance newsInstance)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            db.NewsInstances.Add(newsInstance);
            db.SaveChanges();
            var response = Request.CreateResponse(HttpStatusCode.Created);
            response.Headers.Add("Location", $"NewsInstance/{newsInstance.ID}");
            return base.ResponseMessage(response);
        }
        /// <summary>
        /// Method to delete an instance from database by id
        /// </summary>
        /// <returns>
        /// Returns 404 code if the instance with given ID was not found
        /// </returns>
        [HttpDelete]
        public IHttpActionResult DeleteNewsInstance(int id)
        {
            NewsInstance newsInstance = db.NewsInstances.Find(id);
            if (newsInstance == null)
                return NotFound();
            db.NewsInstances.Remove(newsInstance);
            db.SaveChanges();
            return StatusCode(System.Net.HttpStatusCode.NoContent);
        }
        /// <summary>
        /// Method to modify an instance by id
        /// </summary>
        /// <returns>
        /// Returns bad request in case of given id is not equal to the one given in the model
        /// </returns>
        [ResponseType(typeof(void))]
        public IHttpActionResult PutNewsInstance(int id, NewsInstance newsInstance)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (id != newsInstance.ID)
                return BadRequest("ID of modified entity does not correspond with requested ID");
            db.MarkAsModified(newsInstance);
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest("Error saving entity to DB");
            }
            return StatusCode(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Method to get the news with rating greater or equal the one given as a parameter
        /// </summary>
        /// <returns>
        /// Returns JSON array with instances that fit the condition as a NewsInstanceViewModel 
        /// </returns>

        [HttpGet]
        public IHttpActionResult GetNewsInstances(double minRating)
        {
            var newsNoRatingExcluded = new List<NewsInstance>();
            foreach(var newsInstanceCandidate in db.NewsInstances)
            {
                if (newsInstanceCandidate.RateCount != 0) newsNoRatingExcluded.Add(newsInstanceCandidate);
            }
            var news = from newsInstance in newsNoRatingExcluded
                       where newsInstance.RateSum / (double)newsInstance.RateCount >= minRating
                       select newsInstance;
            if (news.Count() < 1)
                return NotFound();
            var newsViewModels = new List<NewsInstanceViewModel>();
            foreach (var newsInstance in news)
            {
                newsViewModels.Add((NewsInstanceViewModel)newsInstance);
            }
            var response = Request.CreateResponse(newsViewModels);
            response.Headers.Add("Count", news.Count().ToString());
            return base.ResponseMessage(response);

        }

        /// <summary>
        /// Method to get the news with given category
        /// </summary>
        /// <returns>
        /// Returns JSON array with instances that fit the condition as a NewsInstanceViewModel 
        /// </returns>

        [HttpGet]
        public IHttpActionResult GetNewsInstances(string category)
        {
            var news = from newsInstance in db.NewsInstances
                       where newsInstance.Category == category
                       select newsInstance;
            if (news.Count() < 1)
                return NotFound();
            var newsViewModels = new List<NewsInstanceViewModel>();
            foreach (var newsInstance in news)
            {
                newsViewModels.Add((NewsInstanceViewModel)newsInstance);
            }
            var response = Request.CreateResponse(newsViewModels);
            response.Headers.Add("Count", news.Count().ToString());
            return base.ResponseMessage(response);
        }

        [HttpGet]
        public IHttpActionResult GetNewsInstances(string category, double minRating)
        {
            var newsNoRatingExcluded = new List<NewsInstance>();
            foreach (var newsInstanceCandidate in db.NewsInstances)
            {
                if (newsInstanceCandidate.RateCount != 0) newsNoRatingExcluded.Add(newsInstanceCandidate);
            }
            var news = from newsInstance in newsNoRatingExcluded
                       where newsInstance.Category == category &&
                             newsInstance.RateSum / (double)newsInstance.RateCount >= minRating
                       select newsInstance;
            if (news.Count() < 1)
                return NotFound();
            var newsViewModels = new List<NewsInstanceViewModel>();
            foreach (var newsInstance in news)
            {
                newsViewModels.Add((NewsInstanceViewModel)newsInstance);
            }
            var response = Request.CreateResponse(newsViewModels);
            response.Headers.Add("Count", news.Count().ToString());
            return base.ResponseMessage(response);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                db.Dispose();
            base.Dispose(disposing);
        }
    }
}