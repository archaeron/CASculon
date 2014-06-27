// Learn more about F# at http://fsharp.net
// See the 'F# Tutorial' project for more help.
open FParsec
open FParsec.Primitives
open FParsec.CharParsers
open Calculon.Types
open Calculon.Repl



[<EntryPoint>]
let main argv = 
    printfn "%A" argv

    repl "" []
    0 // return an integer exit code

