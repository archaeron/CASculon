﻿module Calculon.Types

type Const =
    | Number of float
    | Complex of float * float
    | BigInt of bigint
    | Matrix of Const list list

type Symbol = string

type Expr =
    | Exponentiation of Expr * Expr
    | Multiplication of Expr * Expr
    | Addition of Expr * Expr
    | Variable of Symbol
    | Assignment of Symbol * Expr
    | Call of Expr * Expr
    | Constant of Const
