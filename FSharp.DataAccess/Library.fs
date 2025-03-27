namespace FSharp.Identity.DataAccess

open System
open Dapper.FSharp
open System.Data
open Npgsql
open Dapper.FSharp.PostgreSQL
open Microsoft.AspNetCore.Identity
open System.Threading.Tasks

module DbAccess =
    Dapper.FSharp.PostgreSQL.OptionTypes.register ()

    [<Literal>]
    let private connString = "User ID=postgres;Password=postgres;Host=localhost;Port=5432;Database=giraffe;"

    let private Users = table'<IdentityUser> "Users" |> inSchema "public"

    let private getConnection () =
        new NpgsqlConnection(connString) :> IDbConnection

    type UserContext =
        //member this.createUser (user: IdentityUser)=
        //    let conn = getConnection()

        //    task {
        //        insert {
        //            into Users
        //            value user
        //            } |> conn.InsertAsync |> ignore
        //    }

        static member findByName(name: string) : Task<IdentityUser option> =
            let conn = getConnection ()
                
            task {
                let! result = 
                    select {
                        for u in Users do
                            where (u.NormalizedUserName = name)
                    }
                    |> conn.SelectAsync<IdentityUser>

                return result |> Seq.tryHead
            }

