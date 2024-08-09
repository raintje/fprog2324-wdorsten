module Rommulbad.Domain.Session

open System
open Thoth.Json.Net

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
        
    let hasValidMinutes (minutes: int) = minutes >= 0 && minutes <= 30
