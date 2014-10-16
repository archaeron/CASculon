// include Fake lib
#r @"packages\FAKE\tools\FakeLib.dll"
open Fake
open System


let solutionFile  = "Calculon"
let testAssemblies = "Tests/bin/Release/*Tests*.dll"
let binDirs = ["Calculon/bin"; "Calculon.Repl/bin"; "Tests/bin"]

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

// Default target
Target "Default" (fun _ ->
    trace "Hello World from FAKE"
)

"Clean" ==> "Build"
"Build" ==> "RunTests"

// start build
RunTargetOrDefault "Default"