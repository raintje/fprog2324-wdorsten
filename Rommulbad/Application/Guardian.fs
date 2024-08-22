namespace Rommulbad.Application

type GuardianService =
    abstract member RegisterGuardian: GuardianId * GuardianName -> unit
    abstract member GetGuardian: GuardianId -> Option<GuardianId * GuardianName>
    abstract member GetAllGuardians: unit -> seq<GuardianId * GuardianName>

module Guardian =
    let set (service: GuardianService) (id, name) = service.RegisterGuardian(id, name)

    let getOne (service: GuardianService) id = service.GetGuardian(id)

    let getAll (service: GuardianService) = service.GetAllGuardians()
