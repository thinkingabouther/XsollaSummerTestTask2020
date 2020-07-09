using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NewsFeedAPI.Models
{
    public class NewsInstance
    {
        [Key]
        public int ID { get; set; }

        public string Headline { get; set; }

        public string Content { get; set;}

        public int RateSum { get; set; }

        public int RateCount { get; set; }

        public string Category { get; set; }

    }
}