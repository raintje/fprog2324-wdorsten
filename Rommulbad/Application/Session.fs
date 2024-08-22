namespace Rommulbad.Application

open System

type SessionService =
    abstract member GetSessions: CandidateName -> seq<string * bool * DateTime * int>
    abstract member AddSession: CandidateName * bool * DateTime * int -> unit
    abstract member GetSessionsForDiploma: CandidateName * DiplomaKey -> seq<string * bool * DateTime * int>

module Session =
    let set (service: SessionService) (candidate, deep, date, minutes) =
        service.AddSession(candidate, deep, date, minutes)

    let get (service: SessionService) (CandidateName name) =
        service.GetSessions(CandidateName name) |> Seq.map id |> Seq.toList

    let getSessionsForDiploma (service: SessionService) (CandidateName name, DiplomaKey diploma) =
        service.GetSessionsForDiploma(CandidateName name, DiplomaKey diploma)
        |> Seq.map id
        |> Seq.toList
