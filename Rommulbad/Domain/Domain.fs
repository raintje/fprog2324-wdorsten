namespace Rommulbad.Domain

open System
open Thoth.Json.Net

type Candidate =
    { Name: string
      DateOfBirth: DateTime
      GuardianId: string
      Diploma: string }

module Candidate =
    let encode: Encoder<Candidate> =
        fun candidate ->
            Encode.object
                [ "name", Encode.string candidate.Name
                  "date_of_birth", Encode.datetime candidate.DateOfBirth
                  "guardian_id", Encode.string candidate.GuardianId
                  "diploma", Encode.string candidate.Diploma ]

    let decode: Decoder<Candidate> =
        Decode.object (fun get ->
            { Name = get.Required.Field "name" Decode.string
              DateOfBirth = get.Required.Field "date_of_birth" Decode.datetime
              GuardianId = get.Required.Field "guardian_id" Decode.string
              Diploma = get.Required.Field "diploma" Decode.string })

type Session =
    { Deep: bool
      Date: DateTime
      Minutes: int }

module Session =
    let encode: Encoder<Session> =
        fun session ->
            Encode.object
                [ "deep", Encode.bool session.Deep
                  "date", Encode.datetime session.Date
                  "amount", Encode.int session.Minutes ]

    let decode: Decoder<Session> =
        Decode.object (fun get ->
            { Deep = get.Required.Field "deep" Decode.bool
              Date = get.Required.Field "date" Decode.datetime
              Minutes = get.Required.Field "amount" Decode.int })

type Guardian =
    { Id: string
      Name: string
      Candidates: List<Candidate> }

module Guardian =
    let encode: Encoder<Guardian> =
        fun guardian ->
            Encode.object
                [ "id", Encode.string guardian.Id
                  "name", Encode.string guardian.Name
                  "candidates", guardian.Candidates |> List.map Candidate.encode |> Encode.list ]

    let decode: Decoder<Guardian> =
        Decode.object (fun get ->
            { Id = get.Required.Field "id" Decode.string
              Name = get.Required.Field "name" Decode.string
              Candidates = get.Required.Field "candidates" (Decode.list Candidate.decode) })
