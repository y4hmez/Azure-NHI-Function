//----------------------------------------------------------------------------------------
// This prelude allows scripts to be edited in Visual Studio or another F# editing environment 
#if !COMPILED
#I @"../../bin/Binaries/WebJobs.Script.Host"
#r "Microsoft.Azure.WebJobs.Host.dll"
#r "System.Web.Http"
#r "System.Net.Http"
#r "Newtonsoft.Json"
#r "System.Net.Http.Formatting"
#endif
//--------------------------------------------------
// This is the body of the function
 
 #load "..\shared\Nhi.fsx"

open System
open System.Linq
open System.Net
open System.Net.Http
open FSharp.Interop.Dynamic
open Newtonsoft.Json
open System.Threading.Tasks
open Microsoft.Azure.WebJobs.Host
open Microsoft.FSharp.Collections
open Nhi

let Run (req: HttpRequestMessage, log: TraceWriter) : Task<HttpResponseMessage> =
    async { 
            let q =
                req.GetQueryNameValuePairs()
                    |> Seq.tryFind (fun kv -> kv.Key = "nhi")
            match q with
            | Some kv ->
                                
                log.Info("NHI String = " + kv.Value)
                log.Info("Digit Sum = " + (GetDigitSum kv.Value).ToString())

                let isValid = IsValidNHI2 kv.Value
            
                return req.CreateResponse(HttpStatusCode.OK, "NHI: " + kv.Value + ": " + isValid.ToString())
            | None ->                
                try
                    return req.CreateResponse(HttpStatusCode.OK, "Did not provider NHI in query string")
                with e ->
                    return req.CreateErrorResponse(HttpStatusCode.BadRequest, e.ToString())

    } |> Async.StartAsTask