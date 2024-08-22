namespace Rommulbad.Application

open Rommulbad.Domain
open Rommulbad.Domain.Validation
open Rommulbad.Domain.Validation.Candidate

type CandidateService =
    abstract member GetCandidate: CandidateName -> Option<CandidateName * System.DateTime * GuardianId * DiplomaKey>
    abstract member GetCandidates: unit -> seq<CandidateName * System.DateTime * GuardianId * DiplomaKey>
    abstract member SubmitCandidate: CandidateName * System.DateTime * GuardianId * string -> unit
    abstract member SetCandidateDiploma: CandidateName * DiplomaKey -> unit

module Candidate =
    let getOne (service: CandidateService) (CandidateName name) =
        service.GetCandidate(CandidateName name)
        |> Option.map (fun (n, dob, gId, d) ->
            { Name = string n
              DateOfBirth = dob
              GuardianId = string gId
              Diploma = string d })

    let getAll (service: CandidateService) =
        service.GetCandidates()
        |> Seq.map (fun (n, dob, gId, d) ->
            { Name = string n
              DateOfBirth = dob
              GuardianId = string gId
              Diploma = string d })

    let submit (service: CandidateService) (CandidateName name, dob, GuardianId gid, diploma) =
        if Candidate.isValidName name && Guardian.isValidGuardianId gid then
            service.SubmitCandidate(CandidateName name, dob, GuardianId gid, diploma)
            Ok $"{name} has been submitted"
        else
            Error $"Invalid candidate name: {name}"

    let setDiploma (candidateService: CandidateService) (CandidateName name, DiplomaKey diploma) =
        if Candidate.isValidDiploma diploma then
            candidateService.SetCandidateDiploma(CandidateName name, DiplomaKey diploma)
            Ok $"Diploma of participant {name} has been set to {diploma}"
        else
            Error $"Invalid diploma type: {diploma}"
