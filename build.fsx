// include Fake lib
#r @"packages\FAKE\tools\FakeLib.dll"
#r @"packages\FSharpLint.0.1.2\FSharpLint.Framework.dll"
#r @"packages\FSharpLint.0.1.2\FSharpLint.Application.dll"
open Fake
open System
open FSharpLint.Application


let solutionFile  = "Calculon"
let calculonProjectFile = "Calculon\Calculon.fsproj"
let testAssemblies = "Tests/bin/Release/*Tests*.dll"
let binDirs = ["Calculon/bin"; "Calculon.Repl/bin"; "Tests/bin"]
let fsharpCore = "packages\FSharpLint.0.1.2"

Target "RestorePackages" RestorePackages

Target "Clean" (fun _ ->
    CleanDirs binDirs
)

Target "Build" (fun _ -> 
    !! (solutionFile + ".sln") 
    |> MSBuildRelease "" "Rebuild" 
    |> ignore 
) 

Target "RunTests" (fun _ -> 
    !! testAssemblies  
    |> NUnit (fun p -> 
    { p with
        DisableShadowCopy = true 
        TimeOut = TimeSpan.FromMinutes 5. 
        OutputFile = "TestResults.xml"
        Framework = "v4.5"}) 
)

Target "Lint" (fun _ ->
    let printException (e:System.Exception) =
        System.Console.WriteLine("Exception Message:")
        System.Console.WriteLine(e.Message)
        System.Console.WriteLine("Exception Stack Trace:")
        System.Console.WriteLine(e.StackTrace)

    let failedToParseFileError (file:string) parseException =
        printfn "%A" file
        printException parseException

    let parserProgress = function
        | FSharpLint.Application.RunLint.Starting(file)
        | FSharpLint.Application.RunLint.ReachedEnd(file) -> ()
        | FSharpLint.Application.RunLint.Failed(file, parseException) ->
            failedToParseFileError file parseException

    let error = System.Action<ErrorHandling.Error>(fun error ->  
        let output = error.Info + System.Environment.NewLine + ErrorHandling.errorInfoLine error.Range error.Input 
        System.Console.WriteLine(output)) 


    let lintOptions: RunLint.ProjectParseInfo =
        {
            /// Function that when returns true cancels the parsing of the project, useful for cancellation tokens etc.
            FinishEarly = System.Func<_>(fun _ -> false)

            /// Absolute path to the .fsproj file.
            ProjectFile = calculonProjectFile

            /// Callback that's called at the start and end of parsing each file (or when a file fails to be parsed).
            Progress = System.Action<RunLint.ParserProgress>(parserProgress)

            /// Callback that's called when a lint error is detected.
            ErrorReceived = error

            /// Optionally force the lint to lookup FSharp.Core.dll from this directory.
            FSharpCoreDirectory = Some fsharpCore
        }
    RunLint.parseProject lintOptions |> printfn "%A"
)


// Default target
Target "Default" (fun _ ->
    trace "Hello World from FAKE"
)

"Clean" ==> "Build"
"Build" ==> "RunTests"

// start build
RunTargetOrDefault "Default"