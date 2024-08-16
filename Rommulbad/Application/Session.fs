namespace Rommulbad.Application

open System

type ISession =
    abstract member GetSessions: CandidateName -> seq<string * bool * DateTime * int>
    abstract member AddSession: SessionTuple -> unit

module Session =
    let set (service: ISession) (SessionTuple(candidate, deep, date, minutes)) =
        service.AddSession(SessionTuple(candidate, deep, date, minutes))

    let get (service: ISession) (CandidateName name) =
        service.GetSessions(CandidateName name) |> Seq.map id |> Seq.toList
