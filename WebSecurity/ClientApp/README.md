# Hydra

[![Twitter Follow](https://img.shields.io/twitter/follow/cloudideaas?label=Followers&style=social&url=https://twitter.com/cloudideaas)](https://twitter.com/cloudideaas)
[![Gitter](https://badges.gitter.im/Join%20Chat.svg)](https://gitter.im/cloudideaas-hydra)
![npm](https://img.shields.io/npm/dt/@cloudideaas/hydra)
![GitHub commit activity](https://img.shields.io/github/commit-activity/w/cloudideaas/hydra)

Hydra is a app generation tool with resulting source code. Generated front-end support for Ionic/Angular. Restful service layer support for .NET Core. Back-end support for SQL Server, Postgres, and CosmosDb. Other supported technologies coming soon as Hydra is highly extensible. Also includes support for mobile capability, Docker, Helm, Azure Devops, and Kubernetes.

![Hydra Splash](https://www.cloudideaas.com/images/HydraSplashNarrow.png)

## Current Features

- Generates a Visual Studio boiler plate solution:
 ```
hydra generate workspace
 ```
- Generates a Business Model and Entity Domain Model, respectively:
 ```
hydra generate businessmodel
hydra generate entities
 ```
- Generates front-end framework (i.e. Ionic) and resulting application:
 ```
hydra generate start
hydra generate app
 ```

## How it Works

Most everything in life is naturally hierarchical, your family, your work organization, data relationships, 
user interfaces, security models, etc. Hydra starts with what is called a "business model".  Business models are broken 
into the following levels: stakeholders, organizational units, roles (people and systems), responsibilities (or features - for systems),
tasks, and data items.  Once you get to the data item level, then you can define the detail that results in properties of entity models, 
columns in database tables, and elements on a screen.

Most rapid application development generators are designed for end users that have no concept of development best practices.  Hydra is 
different in that it is designed with the developer in mind.  It results in source code utilizing preferred coding styles, readability, 
design patterns, and code quality.  It doesn't end there.  Hydra is highly extensible, allowing for generator participation, interface driven
extensions, and even full control UI, business logic, and servicing.  It aims to remove the 80% of application development that is repetitive,
dull, and drone.  It does this while at the same time, allowing for creativity and full autonomy.

## Help

[//]: # (BEGIN HELP)

- [Hydra App Generator](http://www.cloudideaas.com/hydra/index.htm)
  - [Installation](http://www.cloudideaas.com/hydra/_5v00t4esm.htm)
  - [Usage](http://www.cloudideaas.com/hydra/_5v80sh92z.htm)
    - [Hydra Command Line Interface](http://www.cloudideaas.com/hydra/_5vf1505tf.htm)
    - [Generating a Workspace](http://www.cloudideaas.com/hydra/_5vg0mzny1.htm)
    - [Generating a Business Model](http://www.cloudideaas.com/hydra/_5vg17mo2j.htm)
    - [Business Model Input Files](http://www.cloudideaas.com/hydra/_5vp12c15a.htm)
    - [Generating an Entity Domain Model](http://www.cloudideaas.com/hydra/_5vk0nighe.htm)
    - [Entity Domain Model Input Files](http://www.cloudideaas.com/hydra/_5vp129wm3.htm)
    - [Entity Domain Model Attributes and Properties](http://www.cloudideaas.com/hydra/_5vp12dzw5.htm)
    - [Generating an App](http://www.cloudideaas.com/hydra/_5vp12f7wt.htm)
    - [Look at the generated Entities Project](http://www.cloudideaas.com/hydra/_5vp12i5on.htm)
    - [Writing custom UI's with Razor syntax](http://www.cloudideaas.com/hydra/_5vp12ja23.htm)
  - [Extensibility](http://www.cloudideaas.com/hydra/_5vp12lcrr.htm)
    - [Extension model](http://www.cloudideaas.com/hydra/_5vp12lyja.htm)
    - [ApplicationGenerator.Interfaces](http://www.cloudideaas.com/hydra/_5vp12m97q.htm)
    - [Generator Engine](http://www.cloudideaas.com/hydra/_5vp12p1d7.htm)
    - [ApplicationFolderHierarchy](http://www.cloudideaas.com/hydra/_5vp12pqzx.htm)
    - [Generator Configuration](http://www.cloudideaas.com/hydra/_5vp12q8tf.htm)
    - [AbstraX Model](http://www.cloudideaas.com/hydra/_5vp12qvvq.htm)
    - [AbstraX Providers](http://www.cloudideaas.com/hydra/_5vp12sw1i.htm)
      - [Facets](http://www.cloudideaas.com/hydra/_5vp12tbhd.htm)
      - [Facet Handlers](http://www.cloudideaas.com/hydra/_5vp12tjjq.htm)
  - [Visual Studio Integration](http://www.cloudideaas.com/hydra/_5vp12xxqv.htm)
  - [Contribution](http://www.cloudideaas.com/hydra/_5vp12ykw2.htm)


[//]: # (END HELP)

## Requirements

- Windows 10 or higher
- NodeJs (for npm)
- Windows Build Tools (dotnet, MSBuild, etc) for development
- Hydra App Generator Windows Cli (downloadable through the npm Hydra installation package)
- Preferred though not required: Visual Studio or VS Code

### Razor Syntax

![Razor Syntax](https://www.cloudideaas.com/images/RazorSyntax.png)

```cs
@using Ripley.Entities
@using AbstraX

@model EntityModel<Post>
@{
    Layout = "_Repeater.cshtml";

    ViewBag.VirtualScroll = true;
    ViewData["Title"] = "Posts";

    var media = Model.CreateScriptObject<PostMedia>();
    var user = Model.GetLoggedInUser<User>();
}

@section ItemTemplate
{
    <div>
        @* relies on the facet handler on the entity or entity property *@
        <@Model.Entity.Predicate />

        @* creates a binding to an element in the view to code *@
        <ion-input formControlName="@Model.Entity.TimeStamp" type="text"></ion-input>

        @* create an element with script *@
        <input type="file" id="files" name="files[]" multiple onchange="handleFileSelect($event)" />

        @Html.Partial("PostFeedback", Model.Entity.PostFeedbacks, new ViewDataDictionary(this.ViewData) { { "counter", 1 } });
    </div>
}

@section CustomScripts
{
    <script>

        handleFileSelect(evt : Event) {

            let files = evt.target.files; // FileList object

            // files is a FileList of File objects. List some properties.

            let output = [];
            let @media;
            let @user;

            for (let i = 0, f; f = files[i]; i++) {

                @media.FileContents = f.readAsBinaryString();
                @media.FileName = f.name;
                @media.User = user;

                @Model.Create(media);
                                
                output.push('<li><strong>', escape(f.name), '</strong> (', f.type || 'n/a', ') - ',
                          f.size, ' bytes, last modified: ',
                          f.lastModifiedDate ? f.lastModifiedDate.toLocaleDateString() : 'n/a',
                          '</li>');
            }
          }

    </script>
}    
```

## Known Issues

Hydra is a "work in progress".  Please be patient as stability evolves.

### For more information

Visit: http://www.cloudideaas.com/hydra

**Enjoy!**
