module Rommulbad.Data.Adapter

open Microsoft.AspNetCore.Mvc.ViewFeatures
open Rommulbad.Application
open Rommulbad.Data

type SessionDAO(store: Store) =
    interface SessionService with
        member this.GetSessions(CandidateName name) =
            InMemoryDatabase.filter (fun (n, _, _, _) -> n = name) store.sessions

        member this.AddSession(CandidateName candidate, deep, date, minutes) =
            InMemoryDatabase.insert (candidate, date) (candidate, deep, date, minutes) store.sessions
            |> ignore

        // Diploma | Shallow pool | Threshold | Total minutes required
        // A       | allowed      | 1         | 120
        // B       | not allowed  | 10        | 150
        // C       | not allowed  | 15        | 180
        member this.GetSessionsForDiploma(CandidateName name, DiplomaKey diploma) =
            match diploma with
            | "A" -> InMemoryDatabase.filter (fun (c, _, _, m) -> c = name && m >= 1) store.sessions
            | "B" -> InMemoryDatabase.filter (fun (c, d, _, m) -> c = name && d = true && m >= 10) store.sessions
            | "C" -> InMemoryDatabase.filter (fun (c, d, _, m) -> c = name && d = true && m >= 15) store.sessions
            | _ -> failwith $"${diploma} given. Expected: A, B or C."

type GuardianDAO(store: Store) =
    interface GuardianService with
        member this.GetAllGuardians() = InMemoryDatabase.all store.guardians

        member this.GetGuardian(GuardianId gid) =
            InMemoryDatabase.lookup gid store.guardians

        member this.RegisterGuardian(GuardianId gid, GuardianName name) =
            InMemoryDatabase.insert (string gid) (string gid, name) store.guardians
            |> ignore

type CandidateDAO(store: Store) =
    interface CandidateService with
        member this.GetCandidate(CandidateName name) =
            InMemoryDatabase.lookup name store.candidates

        member this.GetCandidates() = InMemoryDatabase.all store.candidates

        member this.SetCandidateDiploma(CandidateName name, DiplomaKey diploma) =
            let candidate =
                match InMemoryDatabase.lookup name store.candidates with
                | Some(n, dob, gId, _) -> (n, dob, gId, diploma)
                | None -> failwith $"Candidate with name {name} not found."

            InMemoryDatabase.update name candidate store.candidates


        member this.SubmitCandidate(CandidateName name, dob: System.DateTime, GuardianId gid, diploma: string) =
            InMemoryDatabase.insert (string name) (string name, dob, string gid, diploma) store.candidates
            |> ignore
