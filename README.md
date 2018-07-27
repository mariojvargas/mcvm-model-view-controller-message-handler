# MVCM: Model View Controller + Service Mediator

This is an attempt at proposing the use of the Mediator Pattern as a good practice for decoupling MVC/API controllers from other domain logic in order to adhere to the [Single Reponsibility Principle](https://en.wikipedia.org/wiki/Single_responsibility_principle) and achieve greater [Separation of Concerns](https://en.wikipedia.org/wiki/Separation_of_concerns). The new acronym I am proposing is simply "MVCM". In full disclosure, it has nothing to do with my initials. :-)

I considered other terms, such as "Mediated MVC", "Messaging-Based Model View Controller (MMVC)", "Model View Controller + Message Handler", "Model View Controller + Mediator", etc., but the expanded term used in the title above makes the intention clearer--or so I think. I was almost settled for the term "Model View Controller + Message Handler", but after doing additional research, I was reminded that there are Message Handlers already in MVC, but they serve a different purpose. The reason for using the phrase *"Service Mediator"* over "Mediator" is that, at its root, the Mediator Design Pattern is a messaging mechanism that helps to decouple a component from direct communication with dependencies, such as services, domain services, repositories, DbContexts, etc. The word "Service" is a way to encompass all of these dependencies in a typical enterprise application. The "+" sign in the above title implies that it's an optional design *methodology* for simpler projects, but you can easily transition to it if the need arises. My recommendation is to apply this design methodology for both simple and large projects.

Using the Mediator Pattern has its drawbacks. One of them can be clearly seen in my implementation of the HTTP PATCH method in the [TodosController.cs](TodoMediatR.Demo.Api/Controllers/TodosController.cs) file. There is a lot going on there, but I think it's a shortcoming in the design of the JsonPatch API in .NET Core. In addition, [see this somewhat relevant discussion](https://softwareengineering.stackexchange.com/questions/352796/is-cqrs-mediatr-worth-it-when-developing-an-asp-net-application) about the worthiness of using the [CQRS pattern](https://martinfowler.com/bliki/CQRS.html) and MediatR. And now that I mention CQRS, the file and naming convention used in this project under the [Features](TodoMediatR.Demo.Api/Features) directory are inspired by the CQRS pattern.

## REST API Demo using MediatR

The sample project in this repository is a demonstration of using the MediatR Nuget package with the classic TODO App, which lets you easily integrate the Mediator Design Pattern in an MVC application.

Original `TodoController` code can be found at [https://github.com/aspnet/Docs/blob/master/aspnetcore/tutorials/first-web-api/samples/2.1/TodoApi/Controllers/TodoController.cs](https://github.com/aspnet/Docs/blob/master/aspnetcore/tutorials/first-web-api/samples/2.1/TodoApi/Controllers/TodoController.cs), which is part of a tutorial on ASP.NET Core 2.1 found at the following URL: [https://docs.microsoft.com/en-us/aspnet/core/tutorials/web-api-vsc?view=aspnetcore-2.1](https://docs.microsoft.com/en-us/aspnet/core/tutorials/web-api-vsc?view=aspnetcore-2.1).

## Technologies Being Showcased

* [ASP.NET Core 2.1](https://docs.microsoft.com/en-us/aspnet/core/index?view=aspnetcore-2.1)
* [Entity Framework Core 2.1](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore/2.1.0)
* Also making use of the [Microsoft.EntityFrameworkCore.InMemory](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.InMemory/2.1.0) Nuget package.
* [MediatR 5.0.1](https://www.nuget.org/packages/MediatR/5.0.1)
* [AutoMapper 7.0.1](https://www.nuget.org/packages/AutoMapper/7.0.1)

See [TodoApiMediaR.Demo.Api.csproj](TodoMediatR.Demo.Api/TodoApiMediatR.Demo.Api.csproj) for additional helper packages being used.

The [FluentValidation](https://www.nuget.org/packages/FluentValidation/) package is recommended for creating strongly-typed validation rules. This package plays well with the use of MediatR. Integration with `FluentValidation` is not included in this demo.

## Running This Demo

This demo's template was created using the .NET Core CLI's `dotnet new webapi` template. [Visual Studio Code](https://code.visualstudio.com/) was used as the preferred editor of choice.

You should be able to open, build and run this project using [Microsoft Visual Studio](https://visualstudio.microsoft.com/), but be aware that you will need .NET Core 2.1 installed, as indicated above. A solution file has been added to this project.

### From the .NET CLI

From the project's root, execute the following command:

```bash
$ dotnet run --project TodoMediatR.Demo.Api

...
Hosting environment: Production
Content root path: /Users/someone/repos/mcvm-model-view-controller-message-handler/TodoMediatR.Demo.Api
Now listening on: http://localhost:5000
Now listening on: https://localhost:5001
Application started. Press Ctrl+C to shut down.
```

The application is served under the following URLs:

* http://localhost:5000/api/todos/
* https://localhost:5001/api/todos/

## Sample Requests

### GET - List All To-dos

https://localhost:5001/api/todos/

#### GET Request Body

Empty request body

#### GET All To-dos Response

```json
[
    {
        "id": 1,
        "name": "Attend and listen to my presentation",
        "isComplete": true
    },
    {
        "id": 2,
        "name": "Study this code and play with it",
        "isComplete": false
    },
    {
        "id": 3,
        "name": "Learn more about ASP.NET Core",
        "isComplete": false
    },
    {
        "id": 4,
        "name": "Learn more about MediatR",
        "isComplete": false
    },
    {
        "id": 5,
        "name": "Implement MVCM in your projects",
        "isComplete": false
    },
]
```

### GET - Get To-do with ID 2

https://localhost:5001/api/todos/2

#### GET To-do Item Request Body

Empty request body

#### GET To-do Item Response

```json
{
    "name": "Study this code",
    "isComplete": false
}
```

### POST - Create a new To-do item

https://localhost:5001/api/todos/

#### POST Request Body

```json
{
    "name": "Tell everyone about MVCM"
}
```

#### POST Response (HTTP Status 201 - Created)

```json
{
    "id": 6,
    "name": "Tell everyone about MVCM",
    "isComplete": false
}
```

### PATCH - Partial update a To-do item

https://localhost:5001/api/todos/3

#### Sample PATCH Request

```json
[
    {
        "op": "replace",
        "path": "/isComplete",
        "value": true
    }
]
```

#### PATCH Response (HTTP Status 200 - OK)

```json
{
    "name": "Tell everyone about MVCM",
    "isComplete": true
}
```

### PUT - Update a To-do item (ID = 2)

https://localhost:5001/api/todos/2

#### Sample PUT Request Body

```json
{
    "name": "Study this code and play with it",
    "isComplete": false
}
```

#### PUT Response

```json
{
    "name": "Study this code and play with it",
    "isComplete": false
}
```

### DELETE - Delete To-do item with ID 1

https://localhost:5001/api/todos/1

#### Sample DELETE REquest Body

This requires no request body

#### DELETE Response

Returns HTTP 204 - No Content

## Slides

I presented the MVCM concept at [CONDG](http://condg.org/) on Juky 26, 2018 as a  lightning talk.

* [SlideShare Slides](https://www.slideshare.net/MarioVargas63/adding-another-m-to-mvc-mvcm-107535562)

## Additional Resources

Here are some resources for learning more about MediatR and how to apply it in MVC.

* [MediatR](https://github.com/jbogard/MediatR)
* For easily registering MediatR as a service in .NET Core: [MediatR.Extensions.Microsoft.DependencyInjection](https://www.nuget.org/packages/MediatR.Extensions.Microsoft.DependencyInjection/)
* [CQRS With MediatR and AutoMapper](https://lostechies.com/jimmybogard/2015/05/05/cqrs-with-mediatr-and-automapper/)
* (Microsoft) [Implementing the microservice application layer using the Web API](https://docs.microsoft.com/en-us/dotnet/standard/microservices-architecture/microservice-ddd-cqrs-patterns/microservice-application-layer-implementation-web-api)

### Dated but still useful resources

* [Put your controllers on a diet: GETs and queries](https://lostechies.com/jimmybogard/2013/10/29/put-your-controllers-on-a-diet-gets-and-queries/)
* [Put your controllers on a diet: POSTs and commands](https://lostechies.com/jimmybogard/2013/12/19/put-your-controllers-on-a-diet-posts-and-commands/)
* [Feature Folders in ASP.NET MVC](http://timgthomas.com/2013/10/feature-folders-in-asp-net-mvc/)

## Disclaimer

This project's goal is to demonstrate the feasibility of using the Mediator Pattern along with MVC. In no way is it trying to promote its project structure, naming conventions, as "best practices". However, the file structure used for listing out the MediatR queries and handlers is a suggested approach that one of my mentors introduced to me and that I have used in large scale applications.

## License

This code is licensed using the [MIT License](LICENSE).
