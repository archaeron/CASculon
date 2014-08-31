/// here is how FSI does it:
// https://github.com/fsharp/fsharp/blob/master/src/fsharp/fsi/console.fs

module Calculon.Repl

open System
open Calculon.Parser
open Calculon.Printer
open FParsec
 
let print p str =
    match p str with
    | Choice1Of2 result   -> printfn "Success: %A" <| Printer.print result
    | Choice2Of2 errorMsg -> printfn "Failure: %s" errorMsg

let rec repl s history =
    match s with
    | "quit" -> Console.WriteLine "byebye"
    | "history" -> printfn "%A" history
    | _ ->
        Console.Write ">> "
        let input = Console.ReadLine()
        print Parser.parse input
        repl input (input::history)