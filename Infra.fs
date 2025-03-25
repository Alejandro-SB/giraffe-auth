namespace global

open FSharp.UMX
open System


module Guid =
    let inline gen () = Guid.NewGuid()
    let inline toStringN (x: Guid) = x.ToString "N"

type ClientId = Guid<clientId>
and [<Measure>] clientId
module ClientId =
    let gen () :ClientId = System.Guid.NewGuid () |> UMX.tag
    //let gen (): ClientId = Guid.gen |> UMX.tag
    let toString (value: ClientId): string = Guid.toStringN %value

module EventCodec =
    /// For CosmosStore - we encode to JsonElement as that's what the store talks
    let genJsonElement<'t when 't :> TypeShape.UnionContract.IUnionContract> =
        FsCodec.SystemTextJson.CodecJsonElement.Create<'t>() |> Encoder.Uncompressed

    /// For stores other than CosmosStore, we encode to UTF-8 and have the store do the right thing
    let gen<'t when 't :> TypeShape.UnionContract.IUnionContract> =
        FsCodec.NewtonsoftJson.Codec.Create<'t>()