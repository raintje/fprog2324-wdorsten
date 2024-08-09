namespace Rommulbad.Application

open Rommulbad.Domain

type ICandidate =
    abstract member GetCandidate: CandidateId -> Option<string * string * string>

module Candidate =
    let get (service: ICandidate) (CandidateId id) =
        service.GetCandidate(CandidateId id)
        |> Option.map (fun (n, gId, d) ->
            { Name = n
              GuardianId = gId
              Diploma = d })
