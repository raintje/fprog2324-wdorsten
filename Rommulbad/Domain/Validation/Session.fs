namespace Rommulbad.Domain.Validation

module Session =
    let private hasNoNegativeNumbers (minutes: int) = minutes >= 0
    let private hasNoMinutesAboveThirty (minutes: int) = minutes <= 30

    let hasValidMinutes (minutes: int) =
        hasNoNegativeNumbers minutes && hasNoMinutesAboveThirty minutes
