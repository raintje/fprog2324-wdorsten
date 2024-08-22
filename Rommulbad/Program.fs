open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Hosting
open Microsoft.Extensions.Hosting
open Microsoft.Extensions.DependencyInjection
open Giraffe
open Rommulbad.Application
open Thoth.Json.Giraffe
open Thoth.Json.Net
open Rommulbad
open Rommulbad.Data
open Rommulbad.Data.Adapter

let configureApp (app: IApplicationBuilder) =
    // Add Giraffe to the ASP.NET Core pipeline
    app.UseGiraffe Web.routes

let configureServices (services: IServiceCollection) =
    // Add Giraffe dependencies
    services
        .AddGiraffe()
        .AddSingleton<Store>(Store())
        .AddSingleton<Json.ISerializer>(ThothSerializer(skipNullField = false, caseStrategy = CaseStrategy.CamelCase))
        .AddScoped<CandidateService>(fun serviceProvider ->
            let store = serviceProvider.GetService(typeof<Store>) :?> Store
            CandidateDAO(store) :> CandidateService)
        .AddScoped<SessionService>(fun serviceProvider ->
            let store = serviceProvider.GetService(typeof<Store>) :?> Store
            SessionDAO(store) :> SessionService)
        .AddScoped<GuardianService>(fun serviceProvider ->
            let store = serviceProvider.GetService(typeof<Store>) :?> Store
            GuardianDAO(store) :> GuardianService)
    |> ignore

[<EntryPoint>]
let main _ =
    Host
        .CreateDefaultBuilder()
        .ConfigureWebHostDefaults(fun webHostBuilder ->
            webHostBuilder.Configure(configureApp).ConfigureServices(configureServices)
            |> ignore)
        .Build()
        .Run()

    0
