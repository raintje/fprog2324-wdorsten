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

    let private validateDiploma (diploma: string) =
        diploma = "" || diploma = "A" || diploma = "B" || diploma = "C"
