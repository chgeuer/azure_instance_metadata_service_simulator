#r "nuget: Ply"

open System.IO
open System.Net.Http
open System.Net.Http.Json
open System.Web
open FSharp.Control.Tasks

type TokenResponse = { access_token: string }
//type ResourceGroupDefinition = { ManagedBy: string }

let fetch : (unit -> string) = (fun () ->
    task {
        let msiEndpoint = "http://169.254.169.254/metadata/identity/oauth2/token"
        let resource = "https://management.core.windows.net/"
        let apiVersion = "2017-09-01"
        let url = $"{msiEndpoint}?resource={HttpUtility.UrlEncode(resource)}&api-version={apiVersion}"

        use imdsClient = new HttpClient()
        imdsClient.DefaultRequestHeaders.Add("Metadata", "true")

        let! { access_token = access_token } = imdsClient.GetFromJsonAsync<TokenResponse>(url)
        return access_token
    }
    |> Async.AwaitTask
    |> Async.RunSynchronously)

printfn "%s" (fetch ())
