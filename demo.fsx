#r "nuget: Ply"
#r "nuget: Newtonsoft.Json"

open System.IO
open System.Net.Http
open System.Web
open Newtonsoft.Json
open FSharp.Control.Tasks

type TokenDefinition = { access_token: string }
//type ResourceGroupDefinition = { ManagedBy: string }

// https://dev.to/tunaxor/making-http-requests-in-f-1n0b
task {
    let msiEndpoint = "http://169.254.169.254/metadata/identity/oauth2/token"
    let resource = "https://management.core.windows.net/"
    let apiVersion = "2017-09-01"
    let url = $"{msiEndpoint}?resource={HttpUtility.UrlEncode(resource)}&api-version={apiVersion}"

    use client = new HttpClient()
    use request = new HttpRequestMessage(HttpMethod.Get, url)
    request.Headers.Add("Metadata", "true");
    let! response = client.SendAsync(request)

    let! responseBody = response.Content.ReadAsStringAsync()
    let accessToken = JsonConvert.DeserializeObject<TokenDefinition>(responseBody).access_token

    do! File.WriteAllTextAsync("./access_token.txt", accessToken)
}
|> Async.AwaitTask
|> Async.RunSynchronously
