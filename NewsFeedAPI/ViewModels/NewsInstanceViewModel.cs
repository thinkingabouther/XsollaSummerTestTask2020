using NewsFeedAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NewsFeedAPI.ViewModel
{
    public class NewsInstanceViewModel
    {
        public int ID { get; set; }
        public string Headline { get; set; }

        public string Content { get; set; }

        public double Rating { get; set; }
        public string Category { get; set; }

        public static explicit operator NewsInstanceViewModel(NewsInstance newsInstance)
        {
            return new NewsInstanceViewModel
            {
                ID = newsInstance.ID,
                Headline = newsInstance.Headline,
                Content = newsInstance.Content,
                Rating = newsInstance.RateCount == 0 ? -1 : newsInstance.RateSum / (double)newsInstance.RateCount,
                Category = newsInstance.Category,
            };
        }
    }
}