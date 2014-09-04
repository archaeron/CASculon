namespace Tests
open System
open NUnit.Framework
open Calculon.Types
open Calculon.Parser
open Tests.Helpers

[<TestFixture>]
type ParserTests() = 

    [<Test>]
    member x.SimpleNumber() =
        let expr = num 5.0
        Assert.AreEqual(exprToChoice expr, parse "5")

    [<Test>]
    member x.Addition() =
        let number1 = num 5.0
        let number2 = num 9.0
        let expr = add number1 number2
        Assert.AreEqual(exprToChoice expr, parse "5 + 9")

    [<Test>]
    member x.Division() =
        let number1 = num 5.0
        let number2 = num 9.0
        let expr = div number1 number2
        Assert.AreEqual(exprToChoice expr, parse "5 / 9")

    [<Test>]
    member x.Precedence() =
        let number1 = num 4.0
        let number2 = num 2.0
        let number3 = num 3.0
        let mult = mult number2 number3
        let expr = add number1 mult
        Assert.AreEqual(exprToChoice expr, parse "4 + 2 * 3")