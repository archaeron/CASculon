
module Calculon.Printer
open System
open Calculon.Types

let printConstant =
    function
    | Number i -> sprintf "%f" i
    | Complex (real, complex) -> "Hallo"
    | BigInt r -> "Hallo"
    | Matrix m -> "Hallo"


let rec print =
    function
    | Exponentiation (a, b) -> sprintf "%s^%s" (print a) (print b)
    | Multiplication (a, b) -> sprintf "%s * %s" (print a) (print b)
    | Addition (a, b) -> sprintf "%s + %s" (print a) (print b)
    | Variable s -> s
    | Assignment (s, expr) -> sprintf "%s = %s" s (print expr)
    | Call (a, b) -> sprintf "%s (%s)" (print a) (print b) 
    | Constant c -> printConstant c
