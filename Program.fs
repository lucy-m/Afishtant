// Learn more about F# at http://docs.microsoft.com/dotnet/fsharp

open System
open System.Threading
open System.Diagnostics

[<EntryPoint>]
let main argv =

    while true do
        let res = ColorFisher.findFish()
        let hasFish = res > 0.9
        printfn "There is a fish - %f %b" res hasFish
        Thread.Sleep(50)
    0 // return an integer exit code
