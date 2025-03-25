namespace global

open FSharp.UMX
open System


module Guid =
    let inline gen () = Guid.NewGuid()
    let inline toStringN (x: Guid) = x.ToString "N"

type UserId = Guid<userId>
and [<Measure>] userId
module UserId =
    let gen () :UserId = System.Guid.NewGuid () |> UMX.tag
    let toString (value: UserId): string = Guid.toStringN %value

module EventCodec =
    /// For CosmosStore - we encode to JsonElement as that's what the store talks
    let genJsonElement<'t when 't :> TypeShape.UnionContract.IUnionContract> =
        FsCodec.SystemTextJson.CodecJsonElement.Create<'t>() |> FsCodec.SystemTextJson.Encoder.Uncompressed

    /// For stores other than CosmosStore, we encode to UTF-8 and have the store do the right thing
    let gen<'t when 't :> TypeShape.UnionContract.IUnionContract> =
        FsCodec.SystemTextJson.Codec.Create<'t>()