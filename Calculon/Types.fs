module Calculon.Types

type Symbol = string

type Const =
    | Number of float
    | Complex of float * float
    | BigInt of bigint
    | Matrix of Expr list list
and Expr =
    | Exponentiation of Expr * Expr
    | Multiplication of Expr * Expr
    | Addition of Expr * Expr
    | Variable of Symbol
    | Assignment of Symbol * Expr
    | Call of Expr * Expr
    | Constant of Const
