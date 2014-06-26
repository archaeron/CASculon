﻿namespace Calculon

open FParsec
open FParsec.Primitives
open FParsec.CharParsers
open Calculon.Types

module Parser =
    let str_ws str = skipString str >>. spaces

    let numberParser =
        pfloat |>> (Constant << Number)
    
    let arithmeticParser sign f = 
        tuple2 numberParser (spaces .>> str_ws sign >>. spaces >>. numberParser) |>> f

    let product =
        arithmeticParser "*" Multiplication

    let addition =
        arithmeticParser "+" Addition

    let variableParser : Parser<Symbol, Unit> =
        identifier (IdentifierOptions())

    let assignmentParser =
        parse {
            let! variable = variableParser
            do! spaces
            do! str_ws "="
            let! expression = numberParser
            return Assignment (variable, expression)}


    let listParser pElement separator =
        spaces >>. sepBy (pElement .>> spaces) (pstring separator >>. spaces)
    

    let matrixBetweenDelimiters sOpen sClose secondarySeparator separator  pElement f =
        between (pstring sOpen) (pstring sClose)
            (spaces >>. sepEndBy (listParser pElement secondarySeparator .>> spaces) (pstring separator >>. spaces) |>> f)

    let matrixParser : Parser<Expr,Unit> =
        matrixBetweenDelimiters "[" "]" "," ";" numberParser (Constant << Matrix)

    let expressionParser =
         attempt product <|> attempt addition <|> attempt matrixParser <|> numberParser

    let parse input =
        expressionParser input