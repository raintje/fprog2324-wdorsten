namespace Rommulbad.Application

type GuardianService =
    abstract member RegisterGuardian: GuardianId * GuardianName -> unit
    abstract member GetGuardian: GuardianId -> Option<string * string>
    abstract member GetAllGuardians: unit -> seq<string * string>

module Guardian =
    let register (service: GuardianService) (id, name) = service.RegisterGuardian(id, name)

    let getOne (service: GuardianService) id = service.GetGuardian(id)

    let getAll (service: GuardianService) = service.GetAllGuardians()
