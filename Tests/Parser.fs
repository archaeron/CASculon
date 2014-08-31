namespace Tests
open System
open NUnit.Framework
open Calculon.Types
open Calculon.Parser

[<TestFixture>]
type ParserTests() = 

    [<Test>]
    member x.TestCase() =
        let expected: Choice<Expr, String> = 5.0 |> Number |> Constant |> Choice1Of2
        Assert.AreEqual(expected, parse "5")

