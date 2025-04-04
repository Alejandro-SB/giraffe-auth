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
open FSharp.Identity.Stores.Extensions
open Equinox.MessageDb

let endpoints =
    [ GET [ route "/" (text "hello world") |> addOpenApiSimple ]
      subRoute "/numbers" [ POST [ route "/numbers" ([| 1; 2; 3 |] |> json) |> addOpenApiSimple ] ] ]

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
            |> ignore)
        .UseCors(configureCors)
        .UseStaticFiles()

let configureServices (services: IServiceCollection) =
    services.AddAuthentication().AddCookie() |> ignore
    services.AddAuthorization() |> ignore
    services.AddCors() |> ignore
    services.AddGiraffe() |> ignore
    services.AddEndpointsApiExplorer() |> ignore
    services.AddSwaggerGen() |> ignore
    services.AddTransient<TimeProvider>(fun _ -> TimeProvider.System) |> ignore
    services.AddIdentityApiEndpoints<IdentityUser>() |> ignore
    services.AddFSharpIdentity() |> ignore
    services.AddLogging() |> ignore

    let defaultCacheDuration = System.TimeSpan.FromMinutes (int64 20)
    let cacheStrategy cache = Equinox.CachingStrategy.SlidingWindow (cache, defaultCacheDuration)

    let create name codec initial fold accessStrategy (context, cache) =
        MessageDbCategory(context, name, codec, fold, initial, accessStrategy, cacheStrategy cache)

    let createUnoptimized name codec initial fold (context, cache) =
        let accessStrategy = AccessStrategy.Unoptimized
        create name codec initial fold accessStrategy (context, cache)

    let connection = Equinox.MessageDb.(FSharp.Identity.Stores.DbAccess.connString)

    ignore()

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
