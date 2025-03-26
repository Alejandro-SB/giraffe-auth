module giraffe_auth.App

open System
open System.IO
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Cors.Infrastructure
open Microsoft.AspNetCore.Hosting
open Microsoft.Extensions.Hosting
open Microsoft.Extensions.Logging
open Microsoft.Extensions.DependencyInjection
open Giraffe
open Giraffe.OpenApi
open Giraffe.EndpointRouting
open Microsoft.AspNetCore.Identity
open FSharp.Identity.Extensions

// ---------------------------------
// Models
// ---------------------------------

type Message = { Text: string }

// ---------------------------------
// Views
// ---------------------------------

module Views =
    open Giraffe.ViewEngine

    let layout (content: XmlNode list) =
        html
            []
            [ head
                  []
                  [ title [] [ encodedText "giraffe_auth" ]
                    link [ _rel "stylesheet"; _type "text/css"; _href "/main.css" ] ]
              body [] content ]

    let partial () = h1 [] [ encodedText "giraffe_auth" ]

    let index (model: Message) =
        [ partial (); p [] [ encodedText model.Text ] ] |> layout

// ---------------------------------
// Web app
// ---------------------------------

let indexHandler (name: string) =
    let greetings = sprintf "Hello %s, from Giraffe!" name
    let model = { Text = greetings }
    let view = Views.index model
    htmlView view

let endpoints =
    [ GET [ route "/" (text "hello world") |> addOpenApiSimple ]
      subRoute "/numbers" [ POST [ route "/numbers" ([| 1; 2; 3 |] |> json) |> addOpenApiSimple ] ] ]

//let webApp =
//    choose
//        [ GET
//          >=> choose
//                  [ route "/" >=> indexHandler "world"
//                    routef "/hello/%s" indexHandler
//                    routef "/product/{%i}" (fun id -> [ 1; 2; 3 ] |> json)
//                    |> configureEndpoint _.WithName("GetProduct")
//                    |> addOpenApiSimple<int, int> ]
//          setStatusCode 404 >=> text "Not Found" ]

// ---------------------------------
// Error handler
// ---------------------------------

let errorHandler (ex: Exception) (logger: ILogger) =
    logger.LogError(ex, "An unhandled exception has occurred while executing the request.")
    clearResponse >=> setStatusCode 500 >=> text ex.Message

// ---------------------------------
// Config and Main
// ---------------------------------


let configureCors (builder: CorsPolicyBuilder) =
    builder
        .WithOrigins("http://localhost:5000", "https://localhost:5001")
        .AllowAnyMethod()
        .AllowAnyHeader()
    |> ignore

let configureApp (app: WebApplication) =
    let isDevelopment: bool = app.Environment.IsDevelopment()

    app.UseSwagger() |> ignore
    app.UseSwaggerUI() |> ignore

    (match isDevelopment with
     | true -> app.UseDeveloperExceptionPage()
     | false -> app.UseGiraffeErrorHandler(errorHandler).UseHttpsRedirection())
        .UseRouting()
        .UseEndpoints(fun e -> 
            e.MapGiraffeEndpoints(endpoints)
            e.MapGroup("/Account") 
                |> Microsoft.AspNetCore.Routing.IdentityApiEndpointRouteBuilderExtensions.MapIdentityApi<IdentityUser> 
                |> ignore
        )
        .UseCors(configureCors)
        .UseStaticFiles()
        
let configureServices (services: IServiceCollection) =
    services.AddAuthentication()
        .AddCookie() |> ignore
    services.AddAuthorization() |> ignore
    services.AddCors() |> ignore
    services.AddGiraffe() |> ignore
    services.AddEndpointsApiExplorer() |> ignore
    services.AddSwaggerGen() |> ignore
    services.AddTransient<TimeProvider>(fun _ -> TimeProvider.System) |> ignore
    services.AddIdentityApiEndpoints<IdentityUser>()|> ignore
    services.AddFSharpIdentity() |> ignore

    //services.AddIdentity<FSIdentityUser, FSIdentityRole>(fun options ->
    //    options.Password.RequireLowercase <- true
    //    options.Password.RequireUppercase <- true
    //    options.Password.RequireDigit <- true
    //    options.Lockout.MaxFailedAccessAttempts <- 5
    //    options.Lockout.DefaultLockoutTimeSpan <- TimeSpan.FromMinutes(15L)
    //    options.User.RequireUniqueEmail <- true
    //    // enable this if we use email verification
    //    // options.SignIn.RequireConfirmedEmail <- true;
    //    )
    //    |> ignore
//// tell asp.net identity to use the above store
//    .AddDefaultTokenProviders() // need for email verification token generation
//    |> ignore

let configureLogging (builder: ILoggingBuilder) =
    builder.AddConsole().AddDebug() |> ignore

[<EntryPoint>]
let main args =
    let contentRoot = Directory.GetCurrentDirectory()

    let builder = WebApplication.CreateBuilder(args)
    builder.Services |> configureServices
    builder.Logging |> configureLogging

    builder.Host.UseContentRoot(contentRoot) |> ignore

    let app = builder.Build()

    app |> configureApp |> ignore

    app.Run()
    0
