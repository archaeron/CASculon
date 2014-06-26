﻿module Calculon.Repl

open System
open Calculon.Parser
open FParsec
 
let print p str =
    match run p str with
    | Success(result, _, _)   -> printfn "Success: %A" result
    | Failure(errorMsg, _, _) -> printfn "Failure: %s" errorMsg

let rec repl s history =
    match s with
    | "quit" -> Console.WriteLine "byebye"
    | "history" -> printfn "%A" history
    | _ ->
        Console.Write ">> "
        let input = Console.ReadLine()
        print Parser.parse input
        repl input (input::history)