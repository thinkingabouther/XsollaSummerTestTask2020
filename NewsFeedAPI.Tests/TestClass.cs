using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Results;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NewsFeedAPI.Contollers;
using NewsFeedAPI.Models;
using NewsFeedAPI.Tests.TestNewsFeedAPIDB;
using NewsFeedAPI.ViewModel;

namespace NewsFeedAPI.Tests
{
    [TestClass]
    public class TestClass
    {
        NewsInstance GetNewsInstance()
        {
            return new NewsInstance { ID = 1, Category = "SomeCategory", Content = "Some content", Headline = "Some headline", RateSum = 10, RateCount = 3 };
        }

        [TestMethod]
        public void GetNewsInstance_ShouldReturnResult()
        {
            var context = new MockNewsFeedAPIContext();
            context.NewsInstances.Add(GetNewsInstance());
            var controller = new NewsInstancesController(context);
            var result = controller.GetNewsInstance(1) as OkNegotiatedContentResult<NewsInstanceViewModel>;
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Content.ID);
        }

        [TestMethod]
        public void GetNewsInstance_ShouldReturnNotfound()
        {
            var context = new MockNewsFeedAPIContext();
            context.NewsInstances.Add(GetNewsInstance());
            var controller = new NewsInstancesController(context);
            var result = controller.GetNewsInstance(2);
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public void GetNewsInstances_ShouldReturnNotfound()
        {
            var context = new MockNewsFeedAPIContext();
            var controller = new NewsInstancesController(context);
            var result = controller.GetNewsInstances();
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public void GetNewsInstances_ShouldReturnResult()
        {
            var context = new MockNewsFeedAPIContext();
            var controller = new NewsInstancesController(context);
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();
            context.NewsInstances.Add(GetNewsInstance());
            context.NewsInstances.Add(GetNewsInstance());
            ResponseMessageResult result = (ResponseMessageResult)controller.GetNewsInstances();
            Assert.IsNotNull(result);
            Assert.AreEqual(2, int.Parse(result.Response.Headers.GetValues("Count").FirstOrDefault()));
        }

        [TestMethod]
        public void PostNewsInstance_ShouldSuccess()
        {
            var context = new MockNewsFeedAPIContext();
            var controller = new NewsInstancesController(context);
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();
            var newInstance = GetNewsInstance();
            ResponseMessageResult result = (ResponseMessageResult)controller.PostNewsInstance(newInstance);
            Assert.IsNotNull(result);
            Assert.AreEqual($"NewsInstance/{ newInstance.ID}", result.Response.Headers.GetValues("Location").FirstOrDefault());
        }

        [TestMethod]
        public void PutNewsInstance_ShouldSuccess()
        {
            var context = new MockNewsFeedAPIContext();
            var controller = new NewsInstancesController(context);
            context.NewsInstances.Add(GetNewsInstance());
            var newInstance = GetNewsInstance();
            newInstance.RateCount = 100;
            var result = controller.PutNewsInstance(1, newInstance) as StatusCodeResult;
            Assert.IsNotNull(result);
            Assert.AreEqual(HttpStatusCode.NoContent, result.StatusCode);
        }

        [TestMethod]
        public void PutNewsInstance_ShouldFailDifferentID()
        {
            var context = new MockNewsFeedAPIContext();
            var controller = new NewsInstancesController(context);
            context.NewsInstances.Add(GetNewsInstance());
            var newInstance = GetNewsInstance();
            newInstance.RateCount = 100;
            var result = controller.PutNewsInstance(3, newInstance);
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(BadRequestErrorMessageResult));
        }

        [TestMethod]
        public void DeleteNewsInstance_ShouldReturnNotfound()
        {
            var context = new MockNewsFeedAPIContext();
            var controller = new NewsInstancesController(context);
            context.NewsInstances.Add(GetNewsInstance());
            var result = controller.DeleteNewsInstance(3);
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public void DeleteNewsInstance_ShouldSuccess()
        {
            var context = new MockNewsFeedAPIContext();
            var controller = new NewsInstancesController(context);
            context.NewsInstances.Add(GetNewsInstance());
            var result = controller.DeleteNewsInstance(1) as StatusCodeResult;
            Assert.IsNotNull(result);
            Assert.AreEqual(HttpStatusCode.NoContent, result.StatusCode);
            var result2 = controller.GetNewsInstance(1);
            Assert.IsNotNull(result2);
            Assert.IsInstanceOfType(result2, typeof(NotFoundResult));
        }

        [TestMethod]
        public void RateByID_ShouldFailNoToken()
        {
            var context = new MockNewsFeedAPIContext();
            var controller = new NewsInstancesController(context);
            context.NewsInstances.Add(GetNewsInstance());
            var controllerContext = new HttpControllerContext();
            var request = new HttpRequestMessage();
            //request.Headers.Add("Token", "123");
            controllerContext.Request = request;
            controller.ControllerContext = controllerContext;
            var result = controller.RateById(1, 1);
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(BadRequestErrorMessageResult));
            Assert.AreEqual("Token was not given. Issue a token via api/User/Token and attach it to as a header to your request", (result as BadRequestErrorMessageResult).Message);
        }

        [TestMethod]
        public void RateByID_ShouldFailUnregistredToken()
        {
            var context = new MockNewsFeedAPIContext();
            var controller = new NewsInstancesController(context);
            context.NewsInstances.Add(GetNewsInstance());
            var controllerContext = new HttpControllerContext();
            var request = new HttpRequestMessage();
            string token = "123dhjksad";
            request.Headers.Add("Token", token);
            controllerContext.Request = request;
            controller.ControllerContext = controllerContext;
            var result = controller.RateById(1, 1);
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(BadRequestErrorMessageResult));
            Assert.AreEqual($"Token {token} is not registred!", (result as BadRequestErrorMessageResult).Message);
        }

        [TestMethod]
        public void RateByID_ShouldFailRepeatedRate()
        {
            var context = new MockNewsFeedAPIContext();
            var newsController = new NewsInstancesController(context);
            var userContoller = new UserController(context);
            context.NewsInstances.Add(GetNewsInstance());
            var controllerContext = new HttpControllerContext();
            var request = new HttpRequestMessage();
            string token = (userContoller.GetToken() as OkNegotiatedContentResult<string>).Content; 
            request.Headers.Add("Token", token);
            controllerContext.Request = request;
            newsController.ControllerContext = controllerContext;
            newsController.RateById(1, 1);
            var result = newsController.RateById(1, 1);
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(BadRequestErrorMessageResult));
            Assert.AreEqual($"User with this token has already rated that piece of news", (result as BadRequestErrorMessageResult).Message);
        }

        [TestMethod]
        public void RateByID_ShouldSuccess()
        {
            var context = new MockNewsFeedAPIContext();
            var newsController = new NewsInstancesController(context);
            var userContoller = new UserController(context);
            context.NewsInstances.Add(GetNewsInstance());
            var controllerContext = new HttpControllerContext();
            var request = new HttpRequestMessage();
            string token = (userContoller.GetToken() as OkNegotiatedContentResult<string>).Content;
            request.Headers.Add("Token", token);
            controllerContext.Request = request;
            newsController.ControllerContext = controllerContext;
            var result = newsController.RateById(1, 1) as OkNegotiatedContentResult<NewsInstanceViewModel>;
            Assert.IsNotNull(result);
            Assert.AreEqual(2.75, result.Content.Rating);
        }

        [TestMethod]
        public void CancelRateByID_ShouldFailNoToken()
        {
            var context = new MockNewsFeedAPIContext();
            var controller = new NewsInstancesController(context);
            context.NewsInstances.Add(GetNewsInstance());
            var controllerContext = new HttpControllerContext();
            var request = new HttpRequestMessage();
            //request.Headers.Add("Token", "123");
            controllerContext.Request = request;
            controller.ControllerContext = controllerContext;
            var result = controller.CancelRateById(1);
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(BadRequestErrorMessageResult));
            Assert.AreEqual("Token was not given. Use a token that was used to rate this instance", (result as BadRequestErrorMessageResult).Message);
        }

        [TestMethod]
        public void CancelRateByID_ShouldSuccess()
        {
            var context = new MockNewsFeedAPIContext();
            var newsController = new NewsInstancesController(context);
            var userContoller = new UserController(context);
            context.NewsInstances.Add(GetNewsInstance());
            var controllerContext = new HttpControllerContext();
            var request = new HttpRequestMessage();
            string token = (userContoller.GetToken() as OkNegotiatedContentResult<string>).Content;
            request.Headers.Add("Token", token);
            controllerContext.Request = request;
            newsController.ControllerContext = controllerContext;
            newsController.RateById(1, 1);
            var result = newsController.CancelRateById(1) as OkNegotiatedContentResult<NewsInstanceViewModel>;
            Assert.IsNotNull(result);
            Assert.AreEqual(10/3d, result.Content.Rating);
        }

        [TestMethod]
        public void CancelRateByID_ShouldFailNotRatedNews()
        {
            var context = new MockNewsFeedAPIContext();
            var newsController = new NewsInstancesController(context);
            var userContoller = new UserController(context);
            context.NewsInstances.Add(GetNewsInstance());
            var controllerContext = new HttpControllerContext();
            var request = new HttpRequestMessage();
            string token = (userContoller.GetToken() as OkNegotiatedContentResult<string>).Content;
            request.Headers.Add("Token", token);
            controllerContext.Request = request;
            newsController.ControllerContext = controllerContext;
            var result = newsController.CancelRateById(1);
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(BadRequestErrorMessageResult));
            Assert.AreEqual("User with given token has not rated the news with given id", (result as BadRequestErrorMessageResult).Message);
        }

        [TestMethod]
        public void GetTopNewsInstances_ShouldFailNotFound()
        {
            var context = new MockNewsFeedAPIContext();
            var controller = new NewsInstancesController(context);
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();
            context.NewsInstances.Add(GetNewsInstance());
            context.NewsInstances.Add(GetNewsInstance());
            var result = controller.GetTopNewsInstances(6);
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public void GetTopNewsInstances_ShouldSuccess()
        {
            var context = new MockNewsFeedAPIContext();
            var controller = new NewsInstancesController(context);
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();
            context.NewsInstances.Add(GetNewsInstance());
            context.NewsInstances.Add(GetNewsInstance());
            ResponseMessageResult result = (ResponseMessageResult)controller.GetTopNewsInstances(3);
            Assert.IsNotNull(result);
            Assert.AreEqual(2, int.Parse(result.Response.Headers.GetValues("Count").FirstOrDefault()));
        }

        [TestMethod]
        public void GetNewsInstancesCategory_ShouldFailNotFound()
        {
            var context = new MockNewsFeedAPIContext();
            var controller = new NewsInstancesController(context);
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();
            context.NewsInstances.Add(GetNewsInstance());
            context.NewsInstances.Add(GetNewsInstance());
            var result = controller.GetNewsInstancesCategory("12");
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public void GetNewsInstancesCategory_ShouldSuccess()
        {
            var context = new MockNewsFeedAPIContext();
            var controller = new NewsInstancesController(context);
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();
            context.NewsInstances.Add(GetNewsInstance());
            context.NewsInstances.Add(GetNewsInstance());
            ResponseMessageResult result = (ResponseMessageResult)controller.GetNewsInstancesCategory("SomeCategory");
            Assert.IsNotNull(result);
            Assert.AreEqual(2, int.Parse(result.Response.Headers.GetValues("Count").FirstOrDefault()));
        }
    }
}
