/// here is how FSI does it:
// https://github.com/fsharp/fsharp/blob/master/src/fsharp/fsi/console.fs
//
module Calculon.Repl

open System
open Calculon.Parser
open Calculon.Printer
open FParsec
open System.Text
 
let print p input =
    match run p input with
    | Success(result, _, _)   -> printfn "Success: %A" <| Printer.print result
    | Failure(errorMsg, _, _) -> printfn "Failure: %s" errorMsg


let lineIndicator = ">> "
let offset = String.length lineIndicator
   
type internal Cursor =
    static member ResetTo(top,left) = 
        Console.CursorTop <- top
        Console.CursorLeft <- left
    static member Move(inset, delta) =
        let position = Console.CursorTop * (Console.BufferWidth - inset) + (Console.CursorLeft - inset) + delta
        let top  = position / (Console.BufferWidth - inset)
        let left = inset + position % (Console.BufferWidth - inset)
        Cursor.ResetTo(top,left)


let current = ref 0;
let rendered = ref 0;


type internal Anchor = 
    {top:int; left:int}
    static member Current(inset) = {top=Console.CursorTop;left= max inset Console.CursorLeft}

    member p.PlaceAt(inset, index) =
        //printf "p.top = %d, p.left = %d, inset = %d, index = %d, current = %d\n" p.top p.left inset index !current;
        let left = index + offset
        let top = p.top + ( (p.left - inset) + index) / (Console.BufferWidth - inset)
        Cursor.ResetTo(top,left)

            
        
let render (input:string) = 
    let anchor = ref (Anchor.Current(1));
    //printf "render\n";
    let curr = !current
    (!anchor).PlaceAt(1,0);
    let output = new StringBuilder()
    let mutable position = -1
    for i = 0 to input.Length - 1 do 
        if (i = curr) then
            position <- output.Length
        let c = input.Chars(i)
        output.Append(c) |> ignore;

    if (curr = input.Length) then
        position <- output.Length;

    // render the current text, computing a new value for "rendered"
    let old_rendered = !rendered 
    rendered := 0;
    for i = 0 to input.Length - 1 do 
       Console.Write(input.Chars(i));
       rendered := !rendered + 1;
    // blank out any dangling old text
    for i = !rendered to old_rendered - 1 do 
        Console.Write(' ');

    (!anchor).PlaceAt(1,position);



let rec inputLoop (input:string) (history, hIndex) = 
    let cki = Console.ReadKey true

    match cki.Key with
    | ConsoleKey.Enter ->
        input
    | ConsoleKey.UpArrow -> 
        if (hIndex > 0 && hIndex <= List.length history) then
            let i = List.nth (List.rev history) (hIndex - 1)
            render i
            inputLoop i (history, hIndex - 1)
        else
            inputLoop input (history, hIndex)
    | ConsoleKey.DownArrow -> 
        if (hIndex > 0 && hIndex <= List.length history) then
            let i = List.nth (List.rev history) (hIndex + 1)
            render i
            inputLoop i (history, hIndex + 1)
        else
            inputLoop input (history, hIndex)
    | ConsoleKey.Backspace ->
        let i =
            if (!current < 1 || (!current - 1) > String.length input) then
                input
            else
                current := !current - 1;
                input.Remove (!current, 1)
        render i
        inputLoop i (history, hIndex)
    | ConsoleKey.LeftArrow ->
        if (!current > 0 && (!current - 1 < String.length input)) then
            current := !current - 1;
            Cursor.Move(offset, - 1)
            //render input
        inputLoop input (history, hIndex)
    | ConsoleKey.RightArrow ->
        if (!current >= 0 && (!current < String.length input)) then
            current := !current + 1;
            Cursor.Move(offset, 1)
            //render input
        inputLoop input (history, hIndex)
    | _ ->
        let i =
            if !current > 0 && !current <= String.length input then
                (input.Insert (!current,cki.KeyChar.ToString()))
            else
                cki.KeyChar.ToString()  
        current := !current + 1;
        render i
        inputLoop i (history, hIndex)



let rec repl (history, hIndex) =
    Console.Write lineIndicator
    let input = inputLoop "" (history, hIndex)
    Console.Write('\n')
    match input with
    | "quit" -> Console.WriteLine "byebye"
    | "history" ->
        printfn "%A" history
        repl (input::history, hIndex + 1)
    | _ -> 
        print Parser.parse input
        current := 0
        repl (input::history, hIndex + 1)