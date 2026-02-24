# Project
This is a simple setup of a Blazor Web App application with C# and .NET 9.

This application is built from the ground up with the `web` template. 

# Blazor Web App
In this little practice project the following basic concepts are showcased:
- Creating and using razor `components`.
- Passing parameters (`props`).
- Basic routing with `@page`.
- Rendering with razor syntax (`@foreach`).
- LifeCycle methods (`OnInitialized`).

# App.razor
This is the `.razor` file that lives in `Components` and serves as our main entry point for our components. In `Program.cs` we can see that this is the file we are refering to when we use `MapRazorComponents<blazorapp.Components.App>()`.

This file is very slim, it s just HTML, and imports a single `JavaScript` file:

```html
<script src="_framework/blazor.web.js"></script>
```

This little file bundles the `Blazor` runtime required for `interactivity`, WebSocket for server mode etc...

As we can see its imported from the `_framework`, which means its part of the `.NET` framework.

## Components
Components are reusable `.razor` files that consist of HTML and Razor syntax.

### Using a component
We can simply use a component by tagging it <`MyCompoment`>

### Passing parameters (props)
To pass parameters to a component we have to first make sure:
- The component can take parameters, this is enabled from the compoment with the `[Parameter]` attribute.
- When using the component, we pass it attributes using properties and razor syntax for the variables to pass.
    ```c#
    <MyComponent Name="@MyName"> // Assuming we have a variable called MyName in this context
    ```

### Code blocks
We can create code blocks for a component using the `razor` syntax.

Lets assume this code block belongs to `MyComponent`, it has a single property called `Name`, which we are setting in the example above. It has the `[Parameter]` attribute assigned.

```c#
@code {
    [Parameter]
    public string Name {get; set;} = string.Empty;
}
```

## Pages
A page is just another `.razor` file, however, in order to use it we need to add two directives at the top of the file: `@page` and `@rendermode`.
- `@page` - The endpoint from where we can access this page, `"/Home"`
    - This `@page` is then later compiled into a class with a `RouteAttribute` than will be picked up a `Router` once it scans the assembly for classes with `RouteAttribute`.
    - This is in essence how routing works in a nutshell in a blazor app.
- `@rendermode` - We got a few modes to choose from:
    - `null (static SSR) or ommitted @rendermode` - Renders the HTML once, for any updates, we need to reload the page.
    - `InteractiveServer` - Real time dynamic interactivity on the client-side **(Using a SignalR WebSocket connection)**. Without this, this page/component is just static (rendered once on the server and sent to the browser). This is what allows UI updates to happen without reloading the page. 
        - All C# event handling, state changes etc happen on the server.
        - The browser just pings of small events to the server, server sends back DOM updates.
        - **The good** - The .NET runtime is on the server and not downloaded on the client, which means fast startup.
        - **The bad** - Relies heavinly on an ongoing connection, because its using WebSocket connection.
    - `InteractiveWebAssembly (client.-side)` - This one downloads the .NET runtime + the compiled app to the browser. Everything runs locally, similar to what React/Vue does. This provides offline access since we no longer rely on an ongoing connection. However the initial load is big, because we have to download everything.
    - `InteractiveAuto (hybrid)` - Starts with the server (fast because of no download), then switches to `WebAssembly` in the background.
    
## Routing
Unlike traditional server-side frameworks ([MVC](https://github.com/yosang/csharp-mvc) or [Razor Pages](https://github.com/yosang/csharp-razor-pages)), Blazor routing is mostly client-side (handled inside the browser by the Blazor runtime), even though in Interactive Server mode the actual rendering can involve the server via SignalR.

- `Router` - A built in Blazor component that lives in `Microsoft.AspNetCore.Components.Routing`.
    - It basically just intercepts all navigation in the app through (`NavigationManager.NavigateTo()`)
    - It scans the entire project .NET assembly for `[RouteAttribute]`, automacially identifying any route defined with the `@pages ""` directive.
        - This happens when we pass the prop to the component `AppAssembly="@typeof(Program).Assembly"`.
            - AppAssembly is the property.
            - We get the type of our project, which is `Program.cs` and retrieve the `.Assembly` property.

- `Found` - Child component of the `Router` family that defines what happens when a route is found.
    - The `Found` component has a `Context` property that takes a name for the content to display, this can be named anything but it must be a string.
        - Usually its just named `routeData`.
    - `RouteView` - Child component of `Found` that renders the content of the current page component.
        - It provides a `RouteData` which is required, and optionally a `DefaultLayout` property.
        - We can link the previously defined `routeData` variable in the `RouteData` property of `RouteView` with `razor` syntax `@routeData`.
- `NotFound` - Child component that defines what happens if we dont match a route.

Example:
```c#
<Router AppAssembly="@typeof(Program).Assembly">
    <Found Context="routeData">
        <RouteView RouteData="@routeData" />
    </Found>
    <NotFound>
        <p>Not found :/</p>
    </NotFound>
</Router>
```

## Imports
In order to be able to use the `Router` and `InteractiveServer` statics, we must import the namespaces, we can either use them with their fully qualified name `@using Microsoft.AspNetCore.Components.Web.RenderMode` and `@using Microsoft.AspNetCore.Components.Routing` but to save some time since we might create more components we are just using an `_Imports.razor` file which brings them into the assembly for us.

This file is placed in `Components`, but it can also be placed in root, the imports will apply to all `.razor` files below it.

```c#
@using static Microsoft.AspNetCore.Components.Web.RenderMode

@using Microsoft.AspNetCore.Components.Routing
```
# Usage
1. Clone the repository with `git clone https://github.com/yosang/csharp-blazor`
1. Build the application with `dotnet build`
2. Run it with `dotnet run` / `dotnet watch` (for hot reload).

# Endpoints
- `"/"` - Shows a list of products
- `"/details/:id"` - Shows a single product

# Author
[Yosmel Chiang](https://github.com/yosang)