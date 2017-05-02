# template-webapi

Overview
-------

This template will create a standard asp.net core web API project with a corresponding tests project.

* Autofac & Swashbuckle/swagger has been configured for your application project.

* Moc and xunit have been configured for your test project.

An example controller + tests & autofac module are present for reference


Installing Template
-----------------
Before you create a project from a template, you must install the template on your local machine.

* `git clone projectRepoURL`
* `dotnet new --install projectRepoPath`

        Templates                          Short Name      Language      Tags
        -------------------------------------------------------------------------------
        Console Application                console         [C#], F#      Common/Console
        Class library                      classlib        [C#], F#      Common/Library
        ...
        ASP.NET core web api template      esw-webapi      [C#]          Web                <--- expected
        

New Project
----------
* `dotnet new -l` (verify the project is installed, `esw-webapi` should be present)
* `dotnet new esw-webapi -n yourProjectName [-o alternateProjectFolder]`