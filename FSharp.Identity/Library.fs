namespace FSharp.Identity

open Microsoft.AspNetCore.Identity
open Microsoft.Extensions.DependencyInjection

module FsharpIdentity =
    type IdentityUser = { Id: string; Name: string }
    type IdentityRole = {Name : string}

    type UserStore () =
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
        interface IUserClaimStore<IdentityUser> with 
            member this.AddClaimsAsync (user: IdentityUser, claims: System.Collections.Generic.IEnumerable<System.Security.Claims.Claim>, cancellationToken: System.Threading.CancellationToken): System.Threading.Tasks.Task = 
                raise (System.NotImplementedException())
            member this.GetClaimsAsync (user: IdentityUser, cancellationToken: System.Threading.CancellationToken): System.Threading.Tasks.Task<System.Collections.Generic.IList<System.Security.Claims.Claim>> = 
                raise (System.NotImplementedException())
            member this.GetUsersForClaimAsync (claim: System.Security.Claims.Claim, cancellationToken: System.Threading.CancellationToken): System.Threading.Tasks.Task<System.Collections.Generic.IList<IdentityUser>> = 
                raise (System.NotImplementedException())
            member this.RemoveClaimsAsync (user: IdentityUser, claims: System.Collections.Generic.IEnumerable<System.Security.Claims.Claim>, cancellationToken: System.Threading.CancellationToken): System.Threading.Tasks.Task = 
                raise (System.NotImplementedException())
            member this.ReplaceClaimAsync (user: IdentityUser, claim: System.Security.Claims.Claim, newClaim: System.Security.Claims.Claim, cancellationToken: System.Threading.CancellationToken): System.Threading.Tasks.Task = 
                raise (System.NotImplementedException())
        interface IUserLoginStore<IdentityUser> with
            member this.AddLoginAsync (user: IdentityUser, login: UserLoginInfo, cancellationToken: System.Threading.CancellationToken): System.Threading.Tasks.Task = 
                raise (System.NotImplementedException())
            member this.FindByLoginAsync (loginProvider: string, providerKey: string, cancellationToken: System.Threading.CancellationToken): System.Threading.Tasks.Task<IdentityUser> = 
                raise (System.NotImplementedException())
            member this.GetLoginsAsync (user: IdentityUser, cancellationToken: System.Threading.CancellationToken): System.Threading.Tasks.Task<System.Collections.Generic.IList<UserLoginInfo>> = 
                raise (System.NotImplementedException())
            member this.RemoveLoginAsync (user: IdentityUser, loginProvider: string, providerKey: string, cancellationToken: System.Threading.CancellationToken): System.Threading.Tasks.Task = 
                raise (System.NotImplementedException())


    type RoleStore () =
        interface IRoleStore<IdentityRole> with
            member this.CreateAsync (role: IdentityRole, cancellationToken: System.Threading.CancellationToken): System.Threading.Tasks.Task<IdentityResult> = 
                raise (System.NotImplementedException())
            member this.DeleteAsync (role: IdentityRole, cancellationToken: System.Threading.CancellationToken): System.Threading.Tasks.Task<IdentityResult> = 
                raise (System.NotImplementedException())
            member this.FindByIdAsync (roleId: string, cancellationToken: System.Threading.CancellationToken): System.Threading.Tasks.Task<IdentityRole> = 
                raise (System.NotImplementedException())
            member this.FindByNameAsync (normalizedRoleName: string, cancellationToken: System.Threading.CancellationToken): System.Threading.Tasks.Task<IdentityRole> = 
                raise (System.NotImplementedException())
            member this.GetNormalizedRoleNameAsync (role: IdentityRole, cancellationToken: System.Threading.CancellationToken): System.Threading.Tasks.Task<string> = 
                raise (System.NotImplementedException())
            member this.GetRoleIdAsync (role: IdentityRole, cancellationToken: System.Threading.CancellationToken): System.Threading.Tasks.Task<string> = 
                raise (System.NotImplementedException())
            member this.GetRoleNameAsync (role: IdentityRole, cancellationToken: System.Threading.CancellationToken): System.Threading.Tasks.Task<string> = 
                raise (System.NotImplementedException())
            member this.SetNormalizedRoleNameAsync (role: IdentityRole, normalizedName: string, cancellationToken: System.Threading.CancellationToken): System.Threading.Tasks.Task = 
                raise (System.NotImplementedException())
            member this.SetRoleNameAsync (role: IdentityRole, roleName: string, cancellationToken: System.Threading.CancellationToken): System.Threading.Tasks.Task = 
                raise (System.NotImplementedException())
            member this.UpdateAsync (role: IdentityRole, cancellationToken: System.Threading.CancellationToken): System.Threading.Tasks.Task<IdentityResult> = 
                raise (System.NotImplementedException())
            member this.Dispose (): unit = 
                raise (System.NotImplementedException())
open FsharpIdentity

module Extensions = 
    type IServiceCollection with
        member inline services.AddFSharpIdentity() = 
            services.AddScoped<IUserStore<IdentityUser>, UserStore>() |> ignore
            services.AddScoped<IUserClaimStore<IdentityUser>, UserStore>() |> ignore
            services.AddScoped<IUserLoginStore<IdentityUser>, UserStore>() |> ignore
            services.AddScoped<IRoleStore<IdentityRole>, RoleStore>() |> ignore

            services