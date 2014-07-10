namespace Calculon

open FParsec
open FParsec.Primitives
open FParsec.CharParsers
open Calculon.Types

module Parser =
    // parse whitespace
    let ws = spaces

    // parse a string with whitespace after it
    let str_ws str = pstring str .>> ws

    // parse a float number to an expression
    let numberParser =
        (pfloat .>> ws) |>> (Number >> Constant)

    // parse an identifier with the default options
    let identifierParser : Parser<Expr, Unit> =
        (identifier (IdentifierOptions()) .>> ws) |>> Identifier

    
    let opp = new OperatorPrecedenceParser<Expr,unit,unit>()
    let expr = opp.ExpressionParser

    // parse a list of pElement with a separator between the pElements
    let listParser separator pElement =
        ws >>. sepBy (pElement .>> ws) (str_ws separator)
    

    let matrixBetweenDelimiters sOpen sClose secondarySeparator separator  pElement f =
        between (pstring sOpen) (pstring sClose)
            (ws >>. sepBy (listParser secondarySeparator pElement .>> spaces) (str_ws separator) |>> f)

    let matrixParser : Parser<Expr, Unit> =
        matrixBetweenDelimiters "[" "]" "," ";" expr (Constant << Matrix)

    let constantParser =
        numberParser <|> identifierParser <|> matrixParser

    let term = (ws >>. constantParser) <|> between (str_ws "(") (str_ws ")") expr
    opp.TermParser <- term


    // operator precedence table
    // http://kevincantu.org/code/operators.html
    opp.AddOperator(InfixOperator("+", ws, 6, Associativity.Left, fun x y -> Addition (x, y)))
    opp.AddOperator(InfixOperator("-", ws, 7, Associativity.Left, fun x y -> Subtraction (x, y)))
    opp.AddOperator(InfixOperator("*", ws, 7, Associativity.Left, fun x y -> Multiplication (x, y)))
    opp.AddOperator(InfixOperator("/", ws, 7, Associativity.Left, fun x y -> Division (x, y)))
    opp.AddOperator(InfixOperator("^", ws, 8, Associativity.Right, fun x y -> Exponentiation (x, y)))




    //let assignmentParser =
    //    parse {
    //        let! identifier = identifierParser
    //        do! spaces
    //        do! str_ws "="
    //        let! expression = numberParser
    //        return Assignment (variable, expression)}



    let expressionParser =
        matrixParser <|> numberParser

    let parse =
        //expressionParser input
        expr