
namespace Calculon.Printer
open System
open Calculon.Types

let printConstant =
    function
    | Integer i -> sprintf "%i" i
    | Float f -> sprintf "%f" f
    | Complex (real, complex) -> "Hallo"
    | BigInt r -> "Hallo"
    | List l -> "Hallo"
    | Vector v -> "Hallo"
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
