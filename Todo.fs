module Numbers

open FsCodec.Core
open System
open FSharp.UMX

let [<Literal>] CategoryName = "Todo"
let private streamId = FsCodec.StreamId.gen UserId.toString

module Events =
    type Added =         { value: int }
    type Deleted =      { value: int }
    type Snapshotted =  { items: int[] }
    type Event =
        | Added         of Added
        | Deleted       of Deleted
        | Cleared
        | Snapshotted   of Snapshotted
        interface TypeShape.UnionContract.IUnionContract
    let codec = EventCodec.gen<Event>

module Fold =
    type State = { items: int list }
    let initial = { items = []}

    module Snapshot =

        let private generate state = Events.Snapshotted { items = Array.ofList state.items }
        let private isOrigin = function Events.Cleared | Events.Snapshotted _ -> true | _ -> false
        let config = isOrigin, generate

    let private evolve s = function
        | Events.Added item ->    { items = item.value :: s.items }
        | Events.Deleted          { value = id } -> { s with items = s.items  |> List.filter (fun x -> x <> id) }
        | Events.Cleared ->       { s with items = [] }
        | Events.Snapshotted      { items = items } -> { s with items = List.ofArray items }
    let fold = Array.fold evolve

module Decisions =

    let add value (state: Fold.State) = [|
        Events.Added { value = value } |]
    let delete id (state: Fold.State) = [|
        if state.items |> List.exists (fun x -> x = id) then
            Events.Deleted { value = id } |]
    let clear (state: Fold.State) = [|
        if state.items |> List.isEmpty |> not then
            Events.Cleared |]

type Service internal (resolve: UserId -> Equinox.Decider<Events.Event, Fold.State>) =

    member _.List(clientId): Async<int seq> =
        let decider = resolve clientId
        decider.Query(fun s -> s.items |> Seq.ofList)

    member _.TryGet(clientId, id) =
        let decider = resolve clientId
        decider.Query(fun s -> s.items |> List.tryFind (fun x -> x = id))

    member _.Create(clientId, template: int): Async<int> =
        let decider = resolve clientId
        decider.Transact(Decisions.add template, fun s -> s.items |> List.head)

    member _.Delete(clientId, id): Async<unit> =
        let decider = resolve clientId
        decider.Transact(Decisions.delete id)

    member _.Clear(clientId): Async<unit> =
        let decider = resolve clientId
        decider.Transact(Decisions.clear)

let create resolve = Service(streamId >> resolve)