namespace Rommulbad.Domain

open System

/// - name (consists of words seperated by spaces)
/// - date of birth
/// - guardian id (see guardian id)
/// - highest swimming diploma (A, B, or C, with C being the highest)
type Candidate =
    { Name: string
      DateOfBirth: DateTime
      GuardianId: string
      Diploma: string }

/// - candidate name (foreign key to employees)
/// - shallow pool or not
/// - date
/// - minutes(int)
type Session =
    { Candidate: string
      Deep: bool
      Date: DateTime
      Minutes: int }

/// - id (3 digits followed by dash and 4 letters, e.g. 133-LEET)
/// - name (consists of words separated by spaces)
type Guardian = { Id: string; Name: string }
