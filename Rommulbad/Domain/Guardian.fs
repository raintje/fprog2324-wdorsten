module Rommulbad.Domain.Guardian

open System.Text.RegularExpressions
open Thoth.Json.Net
open Rommulbad.Domain.Candidate

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

    let private hasOnlyLettersAndSpaces (name: string) = Regex.IsMatch(name, @"^[A-Za-z\s]+$")

    let private hasNoMoreThanTwoConsecutiveSpaces (name: string) = Regex.IsMatch(name, @"\s{2,}")

    let private hasCapitalizedFirstLetters (name: string) = Regex.IsMatch(name, @"[A-Z]")

    let isValidGuardianName (name: string) =
        hasOnlyLettersAndSpaces name
        && hasNoMoreThanTwoConsecutiveSpaces name
        && hasCapitalizedFirstLetters name

    let isValidGuardianId (id: string) =
        Regex.IsMatch(id, @"^\d{3}-[A-Za-z]{4}$")
