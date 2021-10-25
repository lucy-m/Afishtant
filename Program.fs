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
        let res = ColorFisher.findFish()
        let hasFish = res > 0.98
        if hasFish
        then
            input.SendLeftClick()
        printfn "There is a fish - %f %b" res hasFish
    0
