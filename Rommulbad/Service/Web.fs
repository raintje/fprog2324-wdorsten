module Rommulbad.Web

open Rommulbad.Domain
open Rommulbad.Application
open Rommulbad.Application.Encoder
open Giraffe
open Thoth.Json.Net
open Thoth.Json.Giraffe


let getCandidates: HttpHandler =
    fun next ctx ->
        task {
            let service =
                ctx.RequestServices.GetService(typeof<CandidateService>) :?> CandidateService

            let candidates = Candidate.getAll service

            return! ThothSerializer.RespondJsonSeq candidates Candidate.encode next ctx
        }

let getCandidate (name: string) : HttpHandler =
    let candidateName = CandidateName name

    fun next ctx ->
        task {
            let service =
                ctx.RequestServices.GetService(typeof<CandidateService>) :?> CandidateService

            let candidate = Candidate.getOne service candidateName

            match candidate with
            | Some candidate -> return! ThothSerializer.RespondJson candidate Candidate.encode next ctx
            | None -> return! RequestErrors.NOT_FOUND $"No candidate found with name {name}" next ctx
        }

let addSession (name: string) : HttpHandler =
    fun next ctx ->
        task {

            let! session = ThothSerializer.ReadBody ctx Session.decode

            match session with
            | Error e -> return! text $"Error while posting new session: {e}" next ctx
            | Ok { Deep = deep
                   Date = date
                   Minutes = minutes } ->
                let candidateService =
                    ctx.RequestServices.GetService(typeof<CandidateService>) :?> CandidateService

                let candidate = Candidate.getOne candidateService (CandidateName name)

                match candidate with
                | Some _ ->
                    let sessionService =
                        ctx.RequestServices.GetService(typeof<SessionService>) :?> SessionService

                    Session.set sessionService (CandidateName name, deep, date, minutes)
                    return! text "Session added" next ctx
                | None -> return! text $"No candidate with name {name} found." next ctx
        }

let getSessions (name: string) : HttpHandler =
    fun next ctx ->
        task {
            let sessionService =
                ctx.RequestServices.GetService(typeof<SessionService>) :?> SessionService

            let candidateService =
                ctx.RequestServices.GetService(typeof<CandidateService>) :?> CandidateService

            let candidate = Candidate.getOne candidateService (CandidateName name)

            match candidate with
            | None -> return! RequestErrors.NOT_FOUND $"No candidate found with name {name}" next ctx
            | Some _ ->
                let sessions =
                    sessionService.GetSessions(CandidateName name)
                    |> Seq.map (fun (n, d, dt, a) ->
                        { Candidate = string n
                          Deep = d
                          Date = dt
                          Minutes = a })

                return! ThothSerializer.RespondJsonSeq sessions Session.encode next ctx
        }

let getTotalMinutes (name: string) : HttpHandler =
    fun next ctx ->
        task {
            let sessionService =
                ctx.RequestServices.GetService(typeof<SessionService>) :?> SessionService

            let total =
                Session.get sessionService (CandidateName name)
                |> Seq.map (fun (_, _, _, a) -> a)
                |> Seq.sum

            return! ThothSerializer.RespondJson total Encode.int next ctx
        }

let setDiploma (name: string) : HttpHandler =
    fun next ctx ->
        task {
            let sessionService =
                ctx.RequestServices.GetService(typeof<SessionService>) :?> SessionService

            let! request = ThothSerializer.ReadBody ctx SetDiplomaRequest.decode

            match request with
            | Error e -> return! RequestErrors.BAD_REQUEST e next ctx
            | Ok { Diploma = diploma } ->

                let candidateService =
                    ctx.RequestServices.GetService(typeof<CandidateService>) :?> CandidateService

                let candidate = Candidate.getOne candidateService (CandidateName name)

                match candidate with
                | None -> return! RequestErrors.NOT_FOUND $"No candidate found with name {name}" next ctx
                | Some _ ->

                    let minutes =
                        Session.getSessionsForDiploma sessionService (CandidateName name, DiplomaKey diploma)
                        |> Seq.map (fun (_, _, _, m) -> m)
                        |> Seq.sum

                    let result =
                        Candidate.setDiploma candidateService (CandidateName name, DiplomaKey diploma, minutes)

                    match result with
                    | Ok message -> return! text message next ctx
                    | Error e -> return! RequestErrors.BAD_REQUEST e next ctx
        }

let addGuardian: HttpHandler =
    fun next ctx ->
        task {
            let! guardian = ThothSerializer.ReadBody ctx Guardian.decode

            match guardian with
            | Error e -> return! RequestErrors.BAD_REQUEST e next ctx
            | Ok { Id = id; Name = name } ->
                let guardianService =
                    ctx.RequestServices.GetService(typeof<GuardianService>) :?> GuardianService

                let msg = Guardian.register guardianService (GuardianId id, GuardianName name)

                return! text $"OK: {msg}" next ctx
        }

let addCandidate: HttpHandler =
    fun next ctx ->
        task {
            let! candidate = ThothSerializer.ReadBody ctx Candidate.decode

            let guardianService =
                ctx.RequestServices.GetService(typeof<GuardianService>) :?> GuardianService

            let candidateService =
                ctx.RequestServices.GetService(typeof<CandidateService>) :?> CandidateService

            match candidate with
            | Error e -> return! RequestErrors.BAD_REQUEST $"Error while posting new candidate: {e}" next ctx
            | Ok { Name = name
                   DateOfBirth = dob
                   GuardianId = gid
                   Diploma = diploma } ->

                let guardian = Guardian.getOne guardianService (GuardianId gid)

                match guardian with
                | None -> return! text $"Guardian with id {gid} not found." next ctx
                | Some _ ->
                    let candidate =
                        Candidate.submit candidateService (CandidateName name, dob, GuardianId gid, diploma)

                    match candidate with
                    | Ok msg -> return! text msg next ctx
                    | Error e -> return! RequestErrors.BAD_REQUEST e next ctx
        }


let routes: HttpHandler =
    choose
        [ GET >=> route "/candidate" >=> getCandidates
          GET >=> routef "/candidate/%s" getCandidate
          POST >=> route "/candidate" >=> addCandidate
          POST >=> routef "/candidate/%s/session" addSession
          GET >=> routef "/candidate/%s/session" getSessions
          GET >=> routef "/candidate/%s/session/total" getTotalMinutes
          PATCH >=> routef "/candidate/%s" setDiploma
          POST >=> route "/guardian" >=> addGuardian ]
