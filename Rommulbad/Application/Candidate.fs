namespace Rommulbad.Application

open Rommulbad.Domain

type ICandidate =
    abstract member GetCandidate: CandidateName -> Option<string * System.DateTime * string * string>
    abstract member GetAllCandidates: unit -> seq<string * System.DateTime * string * string>

module Candidate =
    let get (service: ICandidate) (CandidateName name) =
        service.GetCandidate(CandidateName name)
        |> Option.map (fun (n, dob, gId, d) ->
            { Name = n
              DateOfBirth = dob
              GuardianId = gId
              Diploma = d })

    let getAll (service: ICandidate) =
        service.GetAllCandidates()
        |> Seq.map (fun (n, dob, gId, d) ->
            { Name = n
              DateOfBirth = dob 
              GuardianId = gId
              Diploma = d })
