namespace Rommulbad.Domain.Validation.Candidate

open System.Text.RegularExpressions

module Candidate =
    let private hasOnlyLettersAndSpaces (name: string) = Regex.IsMatch(name, @"^[A-Za-z\s]+$")

    let private hasNoMoreThanTwoConsecutiveSpaces (name: string) = Regex.IsMatch(name, @"\s{2,}")

    let private hasCapitalizedFirstLetters (name: string) = Regex.IsMatch(name, @"[A-Z]")

    let isValidName (name: string) =
        hasOnlyLettersAndSpaces name
        && hasNoMoreThanTwoConsecutiveSpaces name
        && hasCapitalizedFirstLetters name

    let private isValidSessionHours (diploma: string, hours: int) =
        match diploma with
        | "A" -> hours >= 1 && hours <= 9
        | "B" -> hours >= 10 && hours <= 14
        | "C" -> hours >= 15
        | _ -> false

    let private isValidDiplomaKey (diploma: string) =
        diploma = "" || diploma = "A" || diploma = "B" || diploma = "C"

    let isValidDiploma (diploma: string) = isValidDiplomaKey diploma
