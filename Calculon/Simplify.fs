module Calculon.Simplify

open Calculon.Types

let private constantAddition = 
    function 
    | Addition(Constant(Number a), Constant(Number b)) -> num (a + b)
    | Addition(Constant(BigInt a), Constant(BigInt b)) -> Constant(BigInt(a + b))
    | expr -> expr

let private constantMultiplication = 
    function 
    | Multiplication(Constant(Number a), Constant(Number b)) -> num (a * b)
    | expr -> expr

let private constantSubtraction = 
    function 
    | Subtraction(Constant(Number a), Constant(Number b)) -> num (a - b)
    | expr -> expr

let private constantExponentiation = 
    function 
    | Exponentiation(Constant(Number a), Constant(Number b)) -> 
        (a, b)
        |> System.Math.Pow
        |> num
    | expr -> expr

let rec simplify = 
    function 
    | Addition(a, b) -> Addition(simplify a, simplify b) |> constantAddition
    | Subtraction(a, b) -> Subtraction(simplify a, simplify b) |> constantSubtraction
    | Multiplication(a, b) -> Multiplication(simplify a, simplify b) |> constantMultiplication
    | Exponentiation(a, b) -> Exponentiation(simplify a, simplify b) |> constantExponentiation
    | expr -> expr