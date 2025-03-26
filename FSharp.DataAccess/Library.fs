namespace FSharp.Identity.DataAccess

open System
open Dapper.FSharp
open System.Data
open Npgsql
open Dapper.FSharp.PostgreSQL
open Microsoft.AspNetCore.Identity

module DbAccess =
    Dapper.FSharp.PostgreSQL.OptionTypes.register()

    let [<Literal>] private connString = "postgres://postgres:postgres@localhost:5432/giraffe";
    let private Users = table'<IdentityUser> "Users"
    let private getConnection () = new NpgsqlConnection(connString) :> IDbConnection

    type UserContext =
        member this.createUser (user: IdentityUser)= 
            let conn = getConnection()

            task {
                insert {
                    into Users
                    value user
                    } |> conn.InsertAsync |> ignore
            }

        member this.findByName (name: string) =
            let conn = getConnection()

            task {
                select {
                    for u in Users do
                    where (u.NormalizedUserName = name)
                } |> conn.SelectAsync<IdentityUser>
            }
            
        
        
