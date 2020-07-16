using NewsFeedAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsFeedAPI.Tests.TestNewsFeedAPIDB
{
    class UserRatesMockDBSet : MockDBSet<UserRate>
    {
        public override UserRate Find(params object[] keyValues)
        {
            try
            {
                return this.SingleOrDefault(rates => rates.ID == (int)keyValues.Single());
            }
            catch (InvalidOperationException)
            {
                return (from rate in this 
                        where rate.Rating != -1 
                        select rate).SingleOrDefault();
            }
        }
    }
}
