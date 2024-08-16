namespace Rommulbad.Application

open System

type CandidateName = CandidateName of string
type GuardianId = GuardianId of string
type SessionTuple = SessionTuple of CandidateName * bool * DateTime * int
