AWS CodeStar Sample ASP.NET Core Web Service
==================================================

This sample code helps get you started with a simple ASP.NET Core web service
deployed by AWS CodeDeploy to an Amazon EC2 server.

What's Here
-----------

This sample includes:

* README.md - this file
* appspec.yml - this file is used by AWS CodeDeploy when deploying the web
  service to EC2
* buildspec.yml - this file is used by AWS CodeBuild to build the web
  service
* AspNetCoreWebService/ - this directory contains your ASP.NET Core service project files
* scripts/ - this directory contains scripts used by AWS CodeDeploy when
  installing and deploying your service on the Amazon EC2 instance


Getting Started
---------------

These directions assume you want to develop on your local computer, and not
from the Amazon EC2 instance itself. If you're on the Amazon EC2 instance, the
virtual environment is already set up for you, and you can start working on the
code.

To work on the sample code, you'll need to clone your project's repository to your
local computer. If you haven't, do that first. You can find instructions in the
AWS CodeStar user guide.

1. Install dotnet.  See https://www.microsoft.com/net/core

2. Build the service.

        $ cd AspNetCoreWebService
        $ dotnet restore
        $ dotnet build

3. Run Kestrel server.

        $ dotnet run

4. Open http://localhost:5000/ in a web browser to view your service.


What Do I Do Next?
------------------

Once you have a virtual environment running, you can start making changes to
the sample ASP.NET Core web service. We suggest making a small change to
/AspNetCoreWebservice/Controller/HelloController.cs first, so you can see how
changes pushed to your project's repository are automatically picked up by your
project pipeline and deployed to the Amazon EC2 instance. (You can watch the
pipeline progress on your project dashboard.) Once you've seen how that works,
start developing your own code, and have fun!

Learn more about AWS CodeStar by reading the user guide. Ask questions or make
suggestions on our forum.

User Guide: http://docs.aws.amazon.com/codestar/latest/userguide/welcome.html

Forum: https://forums.aws.amazon.com/forum.jspa?forumID=248
