module Users

open FsCodec.Core
open System
open FSharp.UMX
open Microsoft.AspNetCore.Identity
open System.Threading.Tasks

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

    let evolve state event =
        match event with
        | Events.Created item ->
            { userName = item.email
              email = item.email
              passwordHash = item.passwordHash }
        | Events.PasswordChanged p ->
            { state with
                passwordHash = p.newPasswordHash }

    let fold : State -> Events.Event seq -> State = Seq.fold evolve

module Decisions =
    let create (value: IdentityUser) (state: Fold.State) =
        [| Events.Created
               { email = value.Email
                 passwordHash = value.PasswordHash } |]

    let changePassword newPasswordHash (state: Fold.State) =
        [| Events.PasswordChanged { newPasswordHash = newPasswordHash } |]


type Service internal (resolve: UserId -> Equinox.Decider<Events.Event, Fold.State>) =

    member _.Create(clientId, payload: IdentityUser) : Task<unit> =
        let decider = resolve clientId
        decider.Transact(Decisions.create payload) |> Async.StartAsTask
    
    member _.ChangePassword(clientId, newPasswordHash: string) : Async<unit> =
        let decider = resolve clientId
        decider.Transact(Decisions.changePassword newPasswordHash)

let create resolve = Service(streamId >> resolve)
