﻿using System;
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
using NewsFeedAPI.ViewModel;

namespace NewsFeedAPI.Contollers
{
    public class NewsInstancesController : ApiController
    {
        private NewsFeedAPIContext db = new NewsFeedAPIContext();

        public IHttpActionResult GetNewsInstances()
        {
            if (db.NewsInstances.Count() < 1)
                return NotFound();
            return Ok(new { news = db.NewsInstances.Select(x => new NewsInstanceViewModel()
            {
                ID = x.ID,
                Headline = x.Headline,
                Content = x.Content,
                Rating = x.RateCount == 0? -1 : x.RateSum / (double)x.RateCount,
                Category = x.Category,
            })});
        }

        [ResponseType(typeof(NewsInstance))]
        public IHttpActionResult GetNewsInstance(int id)
        {
            NewsInstance newsInstance = db.NewsInstances.Find(id);
            if (newsInstance == null)
                return NotFound();
            return Ok(newsInstance);
        }

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

        public IHttpActionResult DeleteNewsInstance(int id)
        {
            NewsInstance newsInstance = db.NewsInstances.Find(id);
            if (newsInstance == null)
                return NotFound();
            db.NewsInstances.Remove(newsInstance);
            db.SaveChanges();
            return StatusCode(System.Net.HttpStatusCode.NoContent);
        }

        [ResponseType(typeof(void))]
        public IHttpActionResult PutNewsInstance(int id, NewsInstance newsInstance)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (id != newsInstance.ID)
                return BadRequest("ID of modified entity does not correspond with requested ID");
            db.Entry(newsInstance).State = EntityState.Modified;
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

        public IHttpActionResult RateById(int id, int rating)
        {
            NewsInstance newsInstance = db.NewsInstances.Find(id);
            newsInstance.RateSum += rating;
            newsInstance.RateCount++;
            db.Entry(newsInstance).State = EntityState.Modified;
            db.SaveChanges();
            return Ok(newsInstance);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                db.Dispose();
            base.Dispose(disposing);
        }
    }
}