<span style="color:red">**This project is not yet ready for any type of public consumption**</span>

# Token Service #
This application will eventually be a single use token service.

### Endpoints ###
The Token Service implements a set of REST endpoints to create and validate tokens.

##### Endpoint Documentation provided with Swagger ####
Use the following endpoints to see API documentation.
* GET <this url>/swagger/
* GET <this url>/swagger/v1/swagger.json

##### Token Endpoints ###
This service implements the following endpoints. See the swagger documentation for request and response specifications
* POST <this url>/api/v1/Token/Generate
* POST <this url>/api/v1/Token/Validate

### Running this Application ###

This is a standard .Net Core ASP web application. As such it can be run from inside Visual Studio using the normal "default project" way.
The project also includes docker files that make it easy to run the project inside a docker container.

##### In IIS Express via VS2017 #####
The Token Service can be debugged while running inside IIS Express.
You can tell when you are running / debugging in IIS because the console
output appears inside the _Output_ pane inside Visual Studio.

1) Make sure _Token Service_ is the active project in the project drop down.
2) Select _IIS Express_ in the run dropdown that contains the green _play_ icon
3) Press the play icon, F5, Ctrl-F5 or use the _Debug_ menu in the menu bar
4) VS will switch to debug mode and run the application
   1) VS will launch a browser if the _Launch Browser_ select box is enabled in the Token Service project file.

##### As a _Project_ via VS2017 #####
The Token Service is marked as the _default project_.
This means you can run it via F5 or Ctrl-F5.
You can tell when you are running in this mode because the console output appears in a standalone window.
1) Make sure _Token Service_ is the active project in the toolbar dropdown.
2) Select _Token Service_ in the run drop down that contains the green _Play_ icon.
3) Press the play icon, F5, Ctrl-F5 or use the _Debug_ menu in the menu bar
4) VS will switch to debug mode and run the application
   1) VS will launch a browser if the _Launch Browser_ select box is enabled in the Token Service project file.

##### DotNet Core embedded web server via Command Line ####
1) Open a powershell or other command line prompt.  
    1) I typically use the _Git Shell_ installed with the GitHub application
2) CD into the TokenService project
3) type _dotnet run_ to run the project  
    1) Console output will provide the browser connection string to connect to the service  
    1) See the interesting links for more info on the dot net CLI


##### Docker via VS2017 ######
This requires that _Linux_ docker containers are enabled and configured.

See the _interesting links_ for more details on how to run this from inside VS2017
1) Highlight the _docker-compose_ node in the _Solution explorer_
2) Right mouse and click on _start new instance_
3) Visual Studio (VS) will attach to that process.  
    1) You can detach from the running docker by clicking on the red square in the debug bar
   The docker process will continue to run after disconnect

I was unable to get docker to appear in the debug drop down in the tool bar to run ith without _project right mouse_

You should be able to see and manipulate the image from the Docker command line
1) _docker ps_ to see the process and Container ID
2) _docker stop \<container id\>_ to remove the process

##### Docker via command line ######
\<to be documented\>

### Docker Configuration ###
1) You are running Windows 10+ or Windows Server with Docker enabled
2) Docker is configured for _Linux Containers_
3) Your user id is added to the docker-users group. Docker will provide instructions if you are not a member of this group.
4) Docker file sharing for deployment is enabled.  Visual Studio / Docker will provide instructions / make this change if it is not enabled.

### Interesting Links ###
I found these links useful while trying to understand how to use Docker with .Net Core 2.0 and with Visual Studio 2017

* Dot Net Core
  * [Dot Net CLI](https://docs.microsoft.com/en-us/dotnet/core/tools/?tabs=netcore2x)
  * [Dot Net Core linux tarballs](https://www.microsoft.com/net/download/linux)
  * [Dot Net Core binary officieal GitHub downloads](https://github.com/dotnet/core/blob/master/release-notes/download-archives/2.0.0-download.md)
* ASP.Net Core
  * [Docker image types documented at](https://www.hanselman.com/blog/NETAndDocker.aspx)
  * [Hosting the service in Docker](https://docs.microsoft.com/en-us/aspnet/core/publishing/docker)
  * [Visual Studio Tools for Docker](https://docs.microsoft.com/en-us/aspnet/core/publishing/visual-studio-tools-for-docker)
* ASP.Net Core Model Validation
  * [Model validation intro](https://docs.microsoft.com/en-us/aspnet/core/mvc/models/validation)
  * [Unit testing when using model validation](https://dotnetliberty.com/index.php/2016/01/04/how-to-unit-test-asp-net-5-mvc-6-modelstate/)
* JWT
  * [JWT validation and authorization in asp net core](https://blogs.msdn.microsoft.com/webdev/2017/04/06/jwt-validation-and-authorization-in-asp-net-core/)
* ASP .Net in Azure
  * [Deploy an ASP.NET container in a Docker Host](https://docs.microsoft.com/en-us/azure/vs-azure-tools-docker-hosting-web-apps-in-docker)
* ASP .Net in AWS
  * [Installing on Linux dotnet core Linux Prerequisites](https://docs.microsoft.com/en-us/dotnet/core/linux-prerequisites?tabs=netcore2x)
  * [Getting Started with .Net - Linux used to build install_dotnetcore](https://www.microsoft.com/net/learn/get-started/linuxubuntu)
  * [Creating and Deploying Elastic Beanstalk applications in DotNet using Vistual Studio](https://docs.aws.amazon.com/elasticbeanstalk/latest/dg/create_deploy_NET.html)
  * [AWS SDK for Dot Net](https://aws.amazon.com/sdk-for-net/)
  * [Using Visual Studio with AWS CodeStar](http://docs.aws.amazon.com/codestar/latest/userguide/setting-up-ide-vs.html)

-----------

This section generated by original AWS CodeStar template. 
* The default project _AspNetCoreWebService_ has been renamed to _TokenService_.
  * _scripts/start_service_ was modified to point at TokenService instead of _AspNetCoreWebService_
* This project has been updated to _DotNet Core 2.0_ 
  * The _AWS Console_ CodeDeploy pipeline section  _Environment: How to build_ also had to be updated to use core-2.0

The following documentation here has **not** been modified to reflect this.
 

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
