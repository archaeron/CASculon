namespace Tests
open System
open NUnit.Framework
open Calculon.Types
open Calculon.Parser

[<TestFixture>]
type ParserTests() = 

    [<Test>]
    member x.SimpleNumber() =
        let expected: Choice<Expr, String> = 5.0 |> Number |> Constant |> Choice1Of2
        Assert.AreEqual(expected, parse "5")

    [<Test>]
    member x.Addition() =
        let number1 = 5.0 |> Number |> Constant
        let number2 = 9.0 |> Number |> Constant
        let expr: Choice<Expr, String> = Addition (number1, number2) |> Choice1Of2
        Assert.AreEqual(expr, parse "5 + 9")