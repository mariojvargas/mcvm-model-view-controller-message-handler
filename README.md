# MVCM: Model View Controller + Message Handler

This is an attempt at proposing the use of the Mediator Pattern as a good practice for decoupling MVC/API controllers from other domain logic in order to adhere to the [Single Reponsibility Principle](https://en.wikipedia.org/wiki/Single_responsibility_principle) and achieve greater [Separation of Concerns](https://en.wikipedia.org/wiki/Separation_of_concerns). The new acronym I am proposing is simply "MVCM". As a disclaimer, it has nothing to do with my initials. :-)

I considered other terms, such as "Mediated MVC", "Messaging-Based Model View Controller (MMVC)", etc., but the expanded term used in the title above makes the intention clearer--or so I think. The reason for using the phrase "Message Handler" over "Mediator" is that, at its root, the Mediator Design Pattern is a messaging mechanism. The "+" sign in the above title implies that it's an optional design decision for simpler projects, but you can easily transition to it if the need arises. My recommendation is to use the pattern for both simple and large projects.

Using the Mediator Pattern has its drawbacks. One of them can be clearly seen in the implementation of the HTTP PATCH method in the [TodosController.cs](TodoMediatR.Demo.Api/Controllers/TodosController.cs) file. [See this somewhat relevant discussion](https://softwareengineering.stackexchange.com/questions/352796/is-cqrs-mediatr-worth-it-when-developing-an-asp-net-application) about the worthiness of using the CQRS pattern and MediatR.

## REST API Demo using MediatR

The sample project in this repository is a demonstration of using the MediatR Nuget package with the classic TODO App, which lets you easily implement the Mediator Design Pattern in MVC.

Original `TodoController` code can be found at [https://github.com/aspnet/Docs/blob/master/aspnetcore/tutorials/first-web-api/samples/2.1/TodoApi/Controllers/TodoController.cs](https://github.com/aspnet/Docs/blob/master/aspnetcore/tutorials/first-web-api/samples/2.1/TodoApi/Controllers/TodoController.cs), which is part of a tutorial on ASP.NET Core 2.1 found at the following URL: [https://docs.microsoft.com/en-us/aspnet/core/tutorials/web-api-vsc?view=aspnetcore-2.1](https://docs.microsoft.com/en-us/aspnet/core/tutorials/web-api-vsc?view=aspnetcore-2.1).

## Technologies Being Showcased

* [ASP.NET Core 2.1](https://docs.microsoft.com/en-us/aspnet/core/index?view=aspnetcore-2.1)
* [Entity Framework Core 2.1](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore/2.1.0)
* Also making use of the [Microsoft.EntityFrameworkCore.InMemory](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.InMemory/2.1.0) Nuget package.
* [MediatR 5.0.1](https://www.nuget.org/packages/MediatR/5.0.1)
* [AutoMapper 7.0.1](https://www.nuget.org/packages/AutoMapper/7.0.1)

See [TodoApiMediaR.Demo.Api.csproj](TodoMediatR.Demo.Api/TodoApiMediatR.Demo.Api.csproj) for additional helper packages being used.

The [FluentValidation](https://www.nuget.org/packages/FluentValidation/) package is recommended for creating strongly-typed validation rules. This package plays well with the use of MediatR. Integration with `FluentValidation` is not included in this demo.
