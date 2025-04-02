namespace FSharp.Identity.Stores

open System.Threading.Tasks
open Microsoft.AspNetCore.Identity
open Microsoft.Extensions.DependencyInjection
open FSharp.Identity.DataAccess


module FsharpIdentity =
    type UserStore() =
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
                ignore()

            member this.FindByIdAsync
                (userId: string, cancellationToken: System.Threading.CancellationToken)
                : System.Threading.Tasks.Task<IdentityUser> =
                raise (System.NotImplementedException())

            member this.FindByNameAsync
                (normalizedUserName: string, cancellationToken: System.Threading.CancellationToken)
                : System.Threading.Tasks.Task<IdentityUser> =
                task {
                    let! user = DbAccess.UserContext.findByName (normalizedUserName)
                    return Option.toObj (user)
                }

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
                Task.FromResult(user.UserName)

            member this.SetNormalizedUserNameAsync
                (user: IdentityUser, normalizedName: string, cancellationToken: System.Threading.CancellationToken)
                : System.Threading.Tasks.Task =
                user.NormalizedUserName <- normalizedName
                Task.CompletedTask

            member this.SetUserNameAsync
                (user: IdentityUser, userName: string, cancellationToken: System.Threading.CancellationToken)
                : System.Threading.Tasks.Task =
                user.UserName <- userName
                Task.CompletedTask

            member this.UpdateAsync
                (user: IdentityUser, cancellationToken: System.Threading.CancellationToken)
                : System.Threading.Tasks.Task<IdentityResult> =
                raise (System.NotImplementedException())

        interface IUserClaimStore<IdentityUser> with
            member this.AddClaimsAsync
                (
                    user: IdentityUser,
                    claims: System.Collections.Generic.IEnumerable<System.Security.Claims.Claim>,
                    cancellationToken: System.Threading.CancellationToken
                ) : System.Threading.Tasks.Task =
                raise (System.NotImplementedException())

            member this.GetClaimsAsync
                (user: IdentityUser, cancellationToken: System.Threading.CancellationToken)
                : System.Threading.Tasks.Task<System.Collections.Generic.IList<System.Security.Claims.Claim>> =
                raise (System.NotImplementedException())

            member this.GetUsersForClaimAsync
                (claim: System.Security.Claims.Claim, cancellationToken: System.Threading.CancellationToken)
                : System.Threading.Tasks.Task<System.Collections.Generic.IList<IdentityUser>> =
                raise (System.NotImplementedException())

            member this.RemoveClaimsAsync
                (
                    user: IdentityUser,
                    claims: System.Collections.Generic.IEnumerable<System.Security.Claims.Claim>,
                    cancellationToken: System.Threading.CancellationToken
                ) : System.Threading.Tasks.Task =
                raise (System.NotImplementedException())

            member this.ReplaceClaimAsync
                (
                    user: IdentityUser,
                    claim: System.Security.Claims.Claim,
                    newClaim: System.Security.Claims.Claim,
                    cancellationToken: System.Threading.CancellationToken
                ) : System.Threading.Tasks.Task =
                raise (System.NotImplementedException())

        interface IUserLoginStore<IdentityUser> with
            member this.AddLoginAsync
                (user: IdentityUser, login: UserLoginInfo, cancellationToken: System.Threading.CancellationToken)
                : System.Threading.Tasks.Task =
                raise (System.NotImplementedException())

            member this.FindByLoginAsync
                (loginProvider: string, providerKey: string, cancellationToken: System.Threading.CancellationToken)
                : System.Threading.Tasks.Task<IdentityUser> =
                raise (System.NotImplementedException())

            member this.GetLoginsAsync
                (user: IdentityUser, cancellationToken: System.Threading.CancellationToken)
                : System.Threading.Tasks.Task<System.Collections.Generic.IList<UserLoginInfo>> =
                raise (System.NotImplementedException())

            member this.RemoveLoginAsync
                (
                    user: IdentityUser,
                    loginProvider: string,
                    providerKey: string,
                    cancellationToken: System.Threading.CancellationToken
                ) : System.Threading.Tasks.Task =
                raise (System.NotImplementedException())

        interface IUserEmailStore<IdentityUser> with
            member this.FindByEmailAsync
                (normalizedEmail: string, cancellationToken: System.Threading.CancellationToken)
                : System.Threading.Tasks.Task<IdentityUser> =
                raise (System.NotImplementedException())

            member this.GetEmailAsync
                (user: IdentityUser, cancellationToken: System.Threading.CancellationToken)
                : System.Threading.Tasks.Task<string> =
                raise (System.NotImplementedException())

            member this.GetEmailConfirmedAsync
                (user: IdentityUser, cancellationToken: System.Threading.CancellationToken)
                : System.Threading.Tasks.Task<bool> =
                raise (System.NotImplementedException())

            member this.GetNormalizedEmailAsync
                (user: IdentityUser, cancellationToken: System.Threading.CancellationToken)
                : System.Threading.Tasks.Task<string> =
                raise (System.NotImplementedException())

            member this.SetEmailAsync
                (user: IdentityUser, email: string, cancellationToken: System.Threading.CancellationToken)
                : System.Threading.Tasks.Task =
                user.Email <- email
                Task.CompletedTask

            member this.SetEmailConfirmedAsync
                (user: IdentityUser, confirmed: bool, cancellationToken: System.Threading.CancellationToken)
                : System.Threading.Tasks.Task =
                raise (System.NotImplementedException())

            member this.SetNormalizedEmailAsync
                (user: IdentityUser, normalizedEmail: string, cancellationToken: System.Threading.CancellationToken)
                : System.Threading.Tasks.Task =
                raise (System.NotImplementedException())

        interface IUserPasswordStore<IdentityUser> with
            member this.GetPasswordHashAsync
                (user: IdentityUser, cancellationToken: System.Threading.CancellationToken)
                : Task<string> =
                raise (System.NotImplementedException())

            member this.HasPasswordAsync
                (user: IdentityUser, cancellationToken: System.Threading.CancellationToken)
                : Task<bool> =
                raise (System.NotImplementedException())

            member this.SetPasswordHashAsync
                (user: IdentityUser, passwordHash: string, cancellationToken: System.Threading.CancellationToken)
                : Task =
                user.PasswordHash <- passwordHash
                Task.CompletedTask


    type RoleStore() =
        interface IRoleStore<IdentityRole> with
            member this.CreateAsync
                (role: IdentityRole, cancellationToken: System.Threading.CancellationToken)
                : System.Threading.Tasks.Task<IdentityResult> =
                raise (System.NotImplementedException())

            member this.DeleteAsync
                (role: IdentityRole, cancellationToken: System.Threading.CancellationToken)
                : System.Threading.Tasks.Task<IdentityResult> =
                raise (System.NotImplementedException())

            member this.FindByIdAsync
                (roleId: string, cancellationToken: System.Threading.CancellationToken)
                : System.Threading.Tasks.Task<IdentityRole> =
                raise (System.NotImplementedException())

            member this.FindByNameAsync
                (normalizedRoleName: string, cancellationToken: System.Threading.CancellationToken)
                : System.Threading.Tasks.Task<IdentityRole> =
                raise (System.NotImplementedException())

            member this.GetNormalizedRoleNameAsync
                (role: IdentityRole, cancellationToken: System.Threading.CancellationToken)
                : System.Threading.Tasks.Task<string> =
                raise (System.NotImplementedException())

            member this.GetRoleIdAsync
                (role: IdentityRole, cancellationToken: System.Threading.CancellationToken)
                : System.Threading.Tasks.Task<string> =
                raise (System.NotImplementedException())

            member this.GetRoleNameAsync
                (role: IdentityRole, cancellationToken: System.Threading.CancellationToken)
                : System.Threading.Tasks.Task<string> =
                raise (System.NotImplementedException())

            member this.SetNormalizedRoleNameAsync
                (role: IdentityRole, normalizedName: string, cancellationToken: System.Threading.CancellationToken)
                : System.Threading.Tasks.Task =
                raise (System.NotImplementedException())

            member this.SetRoleNameAsync
                (role: IdentityRole, roleName: string, cancellationToken: System.Threading.CancellationToken)
                : System.Threading.Tasks.Task =
                raise (System.NotImplementedException())

            member this.UpdateAsync
                (role: IdentityRole, cancellationToken: System.Threading.CancellationToken)
                : System.Threading.Tasks.Task<IdentityResult> =
                raise (System.NotImplementedException())

            member this.Dispose() : unit =
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
