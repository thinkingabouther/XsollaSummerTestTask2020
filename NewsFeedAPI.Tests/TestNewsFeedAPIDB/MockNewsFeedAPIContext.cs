using NewsFeedAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsFeedAPI.Tests.TestNewsFeedAPIDB
{
    public class MockNewsFeedAPIContext : INewsFeedAPIContext
    {
        public MockNewsFeedAPIContext()
        {
            NewsInstances = new NewsMockDBSet();
            UserRates = new UserRatesMockDBSet();
        }

        public DbSet<NewsInstance> NewsInstances { get; set; }

        public DbSet<UserRate> UserRates { get; set; }

        public void Dispose()
        { }

        public void MarkAsModified(NewsInstance item)
        { }

        public void MarkAsModified(UserRate item)
        { }

        public int SaveChanges()
        {
            return 1;
        }
    }
    
}
