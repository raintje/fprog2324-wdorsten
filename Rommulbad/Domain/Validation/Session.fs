namespace Rommulbad.Domain.Validation

module Session =
    let hasValidMinutes (minutes: int) = minutes >= 0 && minutes <= 30
