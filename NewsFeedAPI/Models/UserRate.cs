using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NewsFeedAPI.Models
{
    public class UserRate
    {
        [Key]
        public int ID { get; set; }
        public string Token { get; set; }
        public int NewsInstanceID { get; set; }
        [Range(-1, 5)]
        public int Rating { get; set; }
    }
}
