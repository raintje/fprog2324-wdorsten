module Rommulbad.Domain.Candidate

open System.Text.RegularExpressions
open Thoth.Json.Net

type Candidate =
    { Name: string
      GuardianId: string
      Diploma: string }

module Candidate =
    let encode: Encoder<Candidate> =
        fun candidate ->
            Encode.object
                [ "name", Encode.string candidate.Name
                  "guardian_id", Encode.string candidate.GuardianId
                  "diploma", Encode.string candidate.Diploma ]

    let decode: Decoder<Candidate> =
        Decode.object (fun get ->
            { Name = get.Required.Field "name" Decode.string
              GuardianId = get.Required.Field "guardian_id" Decode.string
              Diploma = get.Required.Field "diploma" Decode.string })

    let private hasOnlyLettersAndSpaces (name: string) = Regex.IsMatch(name, @"^[A-Za-z\s]+$")

    let private hasNoMoreThanTwoConsecutiveSpaces (name: string) = Regex.IsMatch(name, @"\s{2,}")

    let private hasCapitalizedFirstLetters (name: string) = Regex.IsMatch(name, @"[A-Z]")

    let isValidName (name: string) =
        hasOnlyLettersAndSpaces name
        && hasNoMoreThanTwoConsecutiveSpaces name
        && hasCapitalizedFirstLetters name

    let private validateDiploma (diploma: string) =
        diploma = "" || diploma = "A" || diploma = "B" || diploma = "C"
