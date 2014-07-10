
module Calculon.Printer
open System
open Calculon.Types

let rec printConstant =
    function
    | Number i ->
        if Math.Floor i = i
        then
            sprintf "%i" <| int i
        else
            sprintf "%f" i
    | Complex (real, complex) -> "Complex"
    | BigInt r -> "BigInt"
    | Matrix m ->
        let listOfStrings = m |> List.map (List.map print)
        sprintf "%A" listOfStrings

and print =
    function
    | Exponentiation (a, b) -> sprintf "(%s ^ %s)" (print a) (print b)
    | Multiplication (a, b) -> sprintf "(%s * %s)" (print a) (print b)
    | Division (a, b) -> sprintf "(%s / %s)" (print a) (print b)
    | Addition (a, b) -> sprintf "(%s + %s)" (print a) (print b)
    | Subtraction (a, b) -> sprintf "(%s - %s)" (print a) (print b)
    | Identifier i -> i
    | Assignment (s, expr) -> sprintf "%s = %s" s (print expr)
    | Call (a, b) -> sprintf "%s (%s)" (print a) (print b) 
    | Constant c -> printConstant c
