namespace Rommulbad.Domain.Validation

open System.Text.RegularExpressions

module Guardian =
    let private hasOnlyLettersAndSpaces (name: string) = Regex.IsMatch(name, @"^[A-Za-z\s]+$")

    let private hasNoMoreThanTwoConsecutiveSpaces (name: string) = Regex.IsMatch(name, @"\s{2,}")

    let private hasCapitalizedFirstLetters (name: string) = Regex.IsMatch(name, @"[A-Z]")

    let isValidGuardianName (name: string) =
        hasOnlyLettersAndSpaces name
        && hasNoMoreThanTwoConsecutiveSpaces name
        && hasCapitalizedFirstLetters name

    let isValidGuardianId (id: string) =
        Regex.IsMatch(id, @"^\d{3}-[A-Za-z]{4}$")
