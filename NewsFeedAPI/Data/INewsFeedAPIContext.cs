using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsFeedAPI.Models
{
    public interface INewsFeedAPIContext : IDisposable
    {
        DbSet<NewsInstance> NewsInstances { get; set;  }
        DbSet<UserRate> UserRates { get; set; }
        int SaveChanges();
        void MarkAsModified(NewsInstance item);
        void MarkAsModified(UserRate item);
    }
}
