// Learn more about F# at http://fsharp.net
// See the 'F# Tutorial' project for more help.
open Calculon.Repl.Core
open Nessos.UnionArgParser

type Format = 
    | Text
    | Latex

type CLIArguments = 
    | Format of string
    | Expression of string
    interface IArgParserTemplate with
        member s.Usage = 
            match s with
            | Format _ -> "What format should the repl return? (text/latex)"
            | Expression _ -> "Pass an expression that will be evaluated and directly returned"

let parseFormat = 
    function 
    | "latex" -> Latex
    | _ -> Text

let parseExpression = 
    function 
    | "" -> None
    | s -> Some s

let choosePrinter = 
    function 
    | Text -> Calculon.Printer.print
    | Latex -> Calculon.Printer.printLatex

[<EntryPoint>]
let main argv = 
    let settingsParser = UnionArgParser.Create<CLIArguments>()
    let results = settingsParser.Parse argv
    
    let printer = 
        results.GetResult(<@ Format @>, defaultValue = "text")
        |> parseFormat
        |> choosePrinter
    
    let expression = parseExpression <| results.GetResult(<@ Expression @>, defaultValue = "")
    do match expression with
       | None -> repl printer ([], 0)
       | Some expr -> print printer expr
    0 // return an integer exit code
