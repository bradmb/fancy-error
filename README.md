# FancyError
*ASP.NET MVC library that provides enhanced error pages for your application*

This project, which is still only a few days into development at the time of writing this, was designed to provide application users with a simple error page that provides basic information about the issue and links to status and support pages (if configured). It also has the ability to track exception types and, based on how many unique visitors have seen the error, display a potential outage message to let users know that something not normal might be going on.

----------

## How it works

The logic behind this is fairly simple, actually. When installed, this library will hook in to the application's error event handling code, adding itself as another consumer of that data. Upon throwing an error, FancyError takes that information, look for potential outage trends, and replaces the response back to the browser with its own embedded HTML page, replacing the standard yellow error screen.

## What it looks like

At this stage, the page design is still a work in progress. Currently it's a very basic red error page with not a whole lot of detail on it, but gives a general idea of what happened and if we're seeing a trend (outage) going on.

<img src="http://i.imgur.com/mDAQWtM.png" width="800" />

## Requirements

Currently the minimum requirement is an ASP.NET MVC application running .NET 4.5. It is possible this could work in non-MVC environments, but that has not yet been tested.

## How to install

The recommended way to install this is via a NuGet package, which will install the library and make the changes needed to your web.config file:

```PowerShell
PM> Install-Package FancyError -Pre
```

If you'd like to install this manually, grab the source code or a [copy of the compiled library from the releases page](https://github.com/bradmb/fancy-error/releases) and add it as a reference. Within your web.config, update it so FancyError is referenced within your *system.webServer* section:

```XML
  <system.webServer>
    <modules>
      <add name="FancyError" type="FancyError.HttpHandler, FancyError" />
    </modules>
  </system.webServer>
```

## How to setup

Out of the box this will run right away without any additional changes. However, it is suggested to setup the support and status page links for the page, otherwise they will not be displayed (because nobody likes a broken link). Here is a configuration example within an application's *Application_Start* section:

```C#
FancyError.FancyClient.Configure(new FancyError.Models.ClientConfiguration
{
    ApplicationName = "My Fancy App",
    SupportLink = "http://my.support.link",
    StatusLink = "http://my.status.link"
});
```

The configuration model also supports some additional parameters, including:

Parameter | Type | Description | Default
--- | --- | --- | ---
TrackUniqueVisitors | boolean | Ensures that the outage detection code does not count a user as more than one visit to the exception. If this is disabled, then it allows refreshing the page multiple times to trigger a potential outage | *true*
ErrorCountBeforeTrend | int | How many errors have to appears to visitors before it's considered a trend (and displaying the outage message) | *5*
ErrorCountTimeout | timespan | How long errors have to be quiet for before the trend tracking is reset and any potential outage message is removed | *1 minute*
ExternalTemplateLocation | string | If you want to provide your own error message page, provide the relative path (within your application's root) to that file here. This will override the entire internal page used | *empty*

## External Template Configuration

If you choose to use an external template, there are parameters you will likely want to setup in your template to ensure the page renders correctly:

Parameter | Description
--- | ---
{EXCEPTION_TYPE} | The exception's namespace and name
{EXCEPTION_MESSAGE} | The actual exception message
{STATUS_PAGE_TEXT} | When a potential outage is staged, displays text about how to visit the status page
{SUPPORT_PAGE_TEXT} | Displays text about how to visit the support page
{OUTAGE} | A CSS class name of "show-outage" or "hide-outage", depending on if a potential outage is staged
{APP_TITLE} | The application name parameter from the configuration