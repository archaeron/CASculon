namespace Calculon

open FParsec
open FParsec.Primitives
open FParsec.CharParsers
open Calculon.Types

module Parser =
    let numberParser : Parser<Const, Unit>=
        pfloat |>> Number


    let listBetweenSecondary pElement separator f = (spaces >>. sepBy (pElement .>> spaces) (pstring separator >>. spaces) |>> f)

    let matrixBetweenDelimiters sOpen sClose secondarySeparator separator  pElement f =
        between (pstring sOpen) (pstring sClose)
            (spaces >>. sepBy (listBetweenSecondary pElement secondarySeparator f .>> spaces) (pstring separator >>. spaces) |>> f)

    let matrixParser =
        matrixBetweenDelimiters "[" "]" "," ";" numberParser List

    let g = run matrixParser "[1,2,4;5,6,7]"
        
    let listBetweenStrings sOpen sClose separator pElement f =
        between (pstring sOpen) (pstring sClose)
                (spaces >>. sepBy (pElement .>> spaces) (pstring separator >>. spaces) |>> f)

    let listParser =
        listBetweenStrings "[" "]" "," numberParser List

    let t = run listParser "[1, 2, 3]"

    let parse s =
        Assignment ("x", (Multiplication (Constant (Number 5), Constant (Number 4))))