// Learn more about F# at http://docs.microsoft.com/dotnet/fsharp

open System
open System.Threading
open System.Diagnostics
open Interceptor

[<EntryPoint>]
let main argv =

    let input = new Input()
    input.KeyboardFilterMode <- KeyboardFilterMode.All
    input.Load() |> ignore

    while true do
        let hasFish = ColorFisher.findFish()

        if hasFish
        then input.SendLeftClick()

        let autoReelerVal = AutoReeler.tryReel()

        match autoReelerVal with
        | Option.Some AutoReeler.MouseUp -> input.SendMouseEvent(MouseState.LeftUp)
        | Option.Some AutoReeler.MouseDown -> input.SendMouseEvent(MouseState.LeftDown)
        | _ -> ()

        //let autoReelerStr =
        //    match autoReelerVal with
        //    | Option.Some AutoReeler.MouseUp -> "Mouse up"
        //    | Option.Some AutoReeler.MouseDown -> "Mouse down"
        //    | _ -> "No action"

        //printfn "Fish detector %f %b\t auto reeler val %s" findFishVal hasFish autoReelerStr
    0
