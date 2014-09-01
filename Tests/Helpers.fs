
module Tests.Helpers
open System
open Calculon.Types

let exprToChoice : Expr -> Choice<Expr, String> = Choice1Of2