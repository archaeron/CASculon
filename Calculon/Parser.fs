﻿namespace Calculon

open FParsec
open FParsec.Primitives
open FParsec.CharParsers
open Calculon.Types

module Parser =
    let ws = spaces
    let str_ws str = pstring str >>. ws

    let numberParser =
        (pfloat .>> ws) |>> (Constant << Number)
    

    let opp = new OperatorPrecedenceParser<Expr,unit,unit>()
    let expr = opp.ExpressionParser
    let term = numberParser <|> between (str_ws "(") (str_ws ")") expr
    opp.TermParser <- term

    type Assoc = Associativity

    opp.AddOperator(InfixOperator("+", ws, 1, Assoc.Left, fun x y -> Addition (x, y)))
    opp.AddOperator(InfixOperator("*", ws, 2, Assoc.Left, fun x y -> Multiplication (x, y)))
 


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

    let parse =
        //expressionParser input
        expr