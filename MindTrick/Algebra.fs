module Algebra

open System
open NBB.Core.Effects
open NBB.Core.Effects.FSharp

type MindTrick<'r, 'a> = Effect<'r*'a>

[<AutoOpen>]
module MindTrick = 
    let map (f:'a->'b) (mt:MindTrick<'r,'a>) = 
        effect {
            let! (r,a) = mt
            return (r,f a)
        }

    let apply (f: MindTrick<'r, 'a->'b>) (mt:MindTrick<'r,'a>) = 
        effect {
            let! (_,f) = f
            let! (r,a) = mt
            return (r, f a)
        }

    let bind (f: 'a->MindTrick<'r,'b>) (mt: MindTrick<'r,'a>) =
        effect {
            let! (r,a) = mt
            return! f a
        }

    let return' x = effect { return (x,x) }

    let initialValue (mt:MindTrick<'r,'a>) = 
        effect {
            let! (r,_) = mt
            return (r,r)
        }

    let liftEff (eff:Effect<'a>) = Effect.map (fun a -> (a,a)) eff
    let lift2 f = map f >> apply

    type MindTrickBuilder() =
        member _.Bind(eff, func) = bind func eff
        member _.Return(value) = return' value
        member _.ReturnFrom(value) = value
        member _.Zero() = return' ()
        member _.Delay(f: unit->MindTrick<'r,'a>) = f |> Effect.from |> Effect.flatten

    let mindTrick = MindTrickBuilder()

    let (<!>) = map
    let (>>=) eff func = bind func eff









