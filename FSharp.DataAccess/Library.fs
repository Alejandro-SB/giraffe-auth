namespace FSharp.DataAccess

open System
open Dapper.FSharp
open System.Data
open Npgsql
open Dapper.FSharp.PostgreSQL

module DbAccess =
    Dapper.FSharp.PostgreSQL.OptionTypes.register()

    type DbUser = {Id: string; Name:string}

    let [<Literal>] connString = "postgres://postgres:postgres@localhost:5432/giraffe";
    let userTable = table'<DbUser> "Users"
    let getConnection () = new NpgsqlConnection(connString) :> IDbConnection

    let createUser (n: int)= 
        let user = {Id = n.ToString(); Name = "User " + n.ToString()}
        let conn = getConnection()

        task {
            insert {
                into userTable
                value user
                } |> conn.InsertOutputAsync |> ignore
        } 
        
        
