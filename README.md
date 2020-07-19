# NewsFeedAPI

## Overview

This is a simple news feed server API. It allows you to add and edit your news, export some from a server database and rate them. 

## What is done
- CRUD operations to work with news (could be seen [here](https://github.com/thinkingabouther/XsollaSummer2020BE/blob/master/NewsFeedAPI/Contollers/NewsInstancesController.cs))
- Methods to filter news (could be seen [here](https://github.com/thinkingabouther/XsollaSummer2020BE/blob/master/NewsFeedAPI/Contollers/NewsInstancesController.cs))
  - Getting top-rated news
  - Filtering news by category
- Methods to rate the news. User is allowed to rate each piece of news only once (based on tokens, could be seen [here](https://github.com/thinkingabouther/XsollaSummer2020BE/blob/master/NewsFeedAPI/Contollers/UserRatingController.cs) and [there](https://github.com/thinkingabouther/XsollaSummer2020BE/blob/master/NewsFeedAPI/Contollers/UserController.cs))
- Implementation covered with Unit Tests (could be seen [here](https://github.com/thinkingabouther/XsollaSummer2020BE/blob/master/NewsFeedAPI.Tests/TestClass.cs))
- [OpenAPI Spec](https://app.swaggerhub.com/apis-docs/thinkingabouther/NewsFeedAPI/0.2)

## Usage
API is hosted on the following URL: https://newsfeedapi20200717025549.azurewebsites.net

Refer to [OpenAPI specification](https://app.swaggerhub.com/apis-docs/thinkingabouther/NewsFeedAPI/0.2) to see all the available requests 
