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
    | Division of Expr * Expr
    | Addition of Expr * Expr
    | Subtraction of Expr * Expr
    | Identifier of Symbol
    | Assignment of Symbol * Expr
    | Call of Expr * Expr
    | Constant of Const
