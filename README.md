# template-webapi

Overview
-------

This template will create a standard asp.net core web API project with a corresponding tests project.

* Autofac & Swashbuckle/swagger has been configured for your application project.

* Moc and xunit have been configured for your test project.

An example controller + tests & autofac module are present for reference


## Installing Template
-----------------
Before you create a project from a template, you must install the template on your local machine.

* `git clone projectRepoURL`
* `dotnet new --install projectRepoPath`

        Templates                               Short Name      Language      Tags
        ------------------------------------------------------------------------------------
        Console Application                     console         [C#], F#      Common/Console
        Class library                           classlib        [C#], F#      Common/Library
        ...
        ASP.NET core web api template v0.x      esw-webapi      [C#]          Web               <--- expected

        

## Create New Project
----------
Follow the below commands to create a new project from this templae. Use the [Structure](#Structure) section below for file system reference

* `dotnet new -l` (verify the project is installed, `esw-webapi` should be present)
* `dotnet new esw-webapi -n yourProjectName [-o alternateProjectFolder]`
    Content generation time: 2069.7161 ms
    The template "ASP.NET core web api template v0.x" created successfully.

### Verify
-----
Either

* Open project in VS, build
* Run unit / integration tests
* Run project, confirm swagger opens + API's are listed for project


OR
This will only work once you've built in VS, as VS needs to package restore

* `cd yourProjectName`
* `dotnet restore`
* `dotnet build`
* `dotnet test .\Tests\yourProjectName.tests\yourProjectName.tests.csproj`
* `dotnet run .\yourProjectName\yourProjectName.csproj`



## Structure

        MyProject
        |
        | -- MyProject.sln
        + -- MyProject
        |    |
        |    | -- MyProject.csproj
        |
        + -- Tests
             |
             + -- MyProject.tests
                  |
                  | -- MyProject.tests.csproj