using NewsFeedAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsFeedAPI.Tests.TestNewsFeedAPIDB
{
    class NewsMockDBSet : MockDBSet<NewsInstance>
    {
        public override NewsInstance Find(params object[] keyValues)
        {
            return this.SingleOrDefault(news => news.ID == (int)keyValues.Single());
        }
    }
}
