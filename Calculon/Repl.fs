module Calculon.Repl

open System
open Calculon.Parser
open FParsec
 
let print p str =
    match run p str with
    | Success(result, _, _)   -> printfn "Success: %A" result
    | Failure(errorMsg, _, _) -> printfn "Failure: %s" errorMsg

let rec repl s =
    match s with
    | "quit" -> Console.WriteLine "byebye"
    | _ ->
        Console.Write ">> "
        let input = Console.ReadLine()
        print Parser.parse input
        repl input