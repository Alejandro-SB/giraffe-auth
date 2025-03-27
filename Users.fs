module Numbers

open FsCodec.Core
open System
open FSharp.UMX

[<Literal>]
let CategoryName = "Users"

let private streamId = FsCodec.StreamId.gen UserId.toString

module Events =
    type Created = { email: string; passwordHash: string }
    type PasswordChanged = { newPasswordHash: string }

    type Event =
        | Created of Created
        | PasswordChanged of PasswordChanged

        interface TypeShape.UnionContract.IUnionContract

    let codec = EventCodec.gen<Event>

module Fold =
    type State =
        { userName: string
          email: string
          passwordHash: string }

    let initial: State option = None

    let private evolve (s: State) =
        function
        | Events.Created item ->
            { userName = item.email
              email = item.email
              passwordHash = item.passwordHash }
        | Events.PasswordChanged p ->
            { s with
                passwordHash = p.newPasswordHash }

    let fold = Array.fold evolve

type CreateUserPayload = { email: string; passwordHash: string }

module Decisions =
    let create (value: CreateUserPayload) (state: Fold.State) =
        [| Events.Created
               { email = value.email
                 passwordHash = value.passwordHash } |]

    let changePassword newPasswordHash (state: Fold.State) =
        [| Events.PasswordChanged { newPasswordHash = newPasswordHash } |]


type Service internal (resolve: UserId -> Equinox.Decider<Events.Event, Fold.State>) =
    
    member _.Create(clientId, payload: CreateUserPayload) : Async<unit> =
        let decider = resolve clientId
        decider.Transact(Decisions.create payload)
    
    member _.ChangePassword(clientId, newPasswordHash: string) : Async<unit> =
        let decider = resolve clientId
        decider.Transact(Decisions.changePassword newPasswordHash)

let create resolve = Service(streamId >> resolve)
