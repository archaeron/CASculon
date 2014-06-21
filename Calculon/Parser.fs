namespace Calculon

open FParsec
open FParsec.Primitives
open FParsec.CharParsers
open Calculon.Types

module Parser =
   
    let parse s =
        Multiplication (Constant (Integer 5), Constant (Integer 4))

        Assignment ("x", (Multiplication (Constant (Integer 5), Constant (Integer 4))))