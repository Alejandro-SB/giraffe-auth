namespace FSharp.Identity

open Microsoft.AspNetCore.Identity

module FsharpIdentity =
    type IdentityUser = { Id: string; Name: string }

    type UserManager =
        interface IUserStore<IdentityUser> with
            member this.CreateAsync
                (user: IdentityUser, cancellationToken: System.Threading.CancellationToken)
                : System.Threading.Tasks.Task<IdentityResult> =
                raise (System.NotImplementedException())

            member this.DeleteAsync
                (user: IdentityUser, cancellationToken: System.Threading.CancellationToken)
                : System.Threading.Tasks.Task<IdentityResult> =
                raise (System.NotImplementedException())

            member this.Dispose() : unit =
                raise (System.NotImplementedException())

            member this.FindByIdAsync
                (userId: string, cancellationToken: System.Threading.CancellationToken)
                : System.Threading.Tasks.Task<IdentityUser> =
                raise (System.NotImplementedException())

            member this.FindByNameAsync
                (normalizedUserName: string, cancellationToken: System.Threading.CancellationToken)
                : System.Threading.Tasks.Task<IdentityUser> =
                raise (System.NotImplementedException())

            member this.GetNormalizedUserNameAsync
                (user: IdentityUser, cancellationToken: System.Threading.CancellationToken)
                : System.Threading.Tasks.Task<string> =
                raise (System.NotImplementedException())

            member this.GetUserIdAsync
                (user: IdentityUser, cancellationToken: System.Threading.CancellationToken)
                : System.Threading.Tasks.Task<string> =
                raise (System.NotImplementedException())

            member this.GetUserNameAsync
                (user: IdentityUser, cancellationToken: System.Threading.CancellationToken)
                : System.Threading.Tasks.Task<string> =
                raise (System.NotImplementedException())

            member this.SetNormalizedUserNameAsync
                (user: IdentityUser, normalizedName: string, cancellationToken: System.Threading.CancellationToken)
                : System.Threading.Tasks.Task =
                raise (System.NotImplementedException())

            member this.SetUserNameAsync
                (user: IdentityUser, userName: string, cancellationToken: System.Threading.CancellationToken)
                : System.Threading.Tasks.Task =
                raise (System.NotImplementedException())

            member this.UpdateAsync
                (user: IdentityUser, cancellationToken: System.Threading.CancellationToken)
                : System.Threading.Tasks.Task<IdentityResult> =
                raise (System.NotImplementedException())
