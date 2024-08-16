module Rommulbad.Data.Adapter

open Rommulbad.Application
open Rommulbad.Data

type CandidateDAO(store: Store) =
    interface ICandidate with
        member this.GetCandidate(CandidateName name) =
            InMemoryDatabase.lookup name store.candidates

        member this.GetAllCandidates() = InMemoryDatabase.all store.candidates

type SessionDAO(store: Store) =
    interface ISession with
        member this.GetSessions(CandidateName name) =
            InMemoryDatabase.filter (fun (n, _, _, _) -> n = name) store.sessions

        member this.AddSession(SessionTuple(CandidateName candidate, deep, date, minutes)) =
            InMemoryDatabase.insert (candidate, date) (candidate, deep, date, minutes) store.sessions
            |> ignore
