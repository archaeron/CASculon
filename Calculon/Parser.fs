namespace Calculon

open FParsec
open FParsec.Primitives
open FParsec.CharParsers
open Calculon.Types

module Parser =
    let str_ws str = skipString str >>. spaces

    let numberParser : Parser<Const, Unit> =
        pfloat |>> Number

    let variableParser : Parser<Symbol, Unit> =
        identifier (IdentifierOptions())

    let assignmentParser =
        parse {
            let! variable = variableParser
            do! spaces
            do! str_ws "="
            let! expression = numberParser
            return Assignment (variable, Constant expression)}
        

    let listBetweenStrings sOpen sClose separator pElement f =
        between (pstring sOpen) (pstring sClose)
                (spaces >>. sepBy (pElement .>> spaces) (pstring separator >>. spaces) |>> f)

    let listParser =
        listBetweenStrings "[" "]" "," numberParser List

    let t = run listParser "[1, 2, 3]"

    let parse s =

        Assignment ("x", (Multiplication (Constant (Number 5), Constant (Number 4))))