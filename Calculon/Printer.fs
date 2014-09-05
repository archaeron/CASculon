module Calculon.Printer
open System
open Calculon.Types

let intersperse = String.concat


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
        let listOfStrings = m |> List.map (intersperse ", " << List.map print)
        let out = listOfStrings |> intersperse "; "
        "[" + out + "]"

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


let rec printLatexConstant =
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
        let listOfStrings = m |> List.map (intersperse "& " << List.map printLatex)
        let out = listOfStrings |> intersperse "\\\\"
        "\left( \\begin{array}{ccc}" + out + "\end{array} \\right)"

and printLatex =
    function
    | Exponentiation (a, b) -> sprintf "(%s ^ %s)" (printLatex a) (printLatex b)
    | Multiplication (a, b) -> sprintf "(%s \cdot %s)" (printLatex a) (printLatex b)
    | Division (a, b) -> sprintf "\dfrac{%s}{%s}" (printLatex a) (printLatex b)
    | Addition (a, b) -> sprintf "(%s + %s)" (printLatex a) (printLatex b)
    | Subtraction (a, b) -> sprintf "(%s - %s)" (printLatex a) (printLatex b)
    | Identifier i -> i
    | Assignment (s, expr) -> sprintf "%s = %s" s (printLatex expr)
    | Call (a, b) -> sprintf "%s (%s)" (printLatex a) (printLatex b) 
    | Constant c -> printLatexConstant c


   