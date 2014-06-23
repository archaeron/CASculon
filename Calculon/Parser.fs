namespace Calculon

open FParsec
open FParsec.Primitives
open FParsec.CharParsers
open Calculon.Types

module Parser =
    let numberParser =
        pfloat |>> Number

    let listBetweenStrings sOpen sClose separator pElement f =
        between (pstring sOpen) (pstring sClose)
                (spaces >>. sepBy (pElement .>> spaces) (pstring separator >>. spaces) |>> f)

    let listParser =
        listBetweenStrings "[" "]" "," numberParser List

    let t = run listParser "[1, 2, 3]"

    let parse s =

        Assignment ("x", (Multiplication (Constant (Integer 5), Constant (Integer 4))))