// Learn more about F# at http://fsharp.org

open System
open NBB.Core.Effects.FSharp
open Algebra


let thinkOfANumber = mindTrick {
    printfn "Think of a number"
    return Console.ReadLine() |> int 
}

let doubleThatNumber mt = (*)2 <!> mt
let addEight mt = (+)8 <!> mt
let minusHalfThatNumber mt = (fun x -> x/2) <!> mt
let minusTheNumberYouStartedWith mt = effect {
    let! (initial, current) = mt
    return (current, current-initial)
}

let program = thinkOfANumber |> doubleThatNumber |> addEight |> minusHalfThatNumber |> minusTheNumberYouStartedWith

let p x = (x*2 + 8) /2 - x

[<EntryPoint>]
let main argv =
    
    let interpreter = Interpreter.createInterpreter()
    let (_,x) = 
        program
        |> Effect.interpret interpreter
        |> Async.RunSynchronously
    printfn "The answer is %i" x
    0 // return an integer exit code
