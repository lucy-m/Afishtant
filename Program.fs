// Learn more about F# at http://docs.microsoft.com/dotnet/fsharp

open System
open System.Threading
open System.Diagnostics
open Interceptor

[<EntryPoint>]
let main argv =

    let input =
        new Input(
            MouseFilterMode = MouseFilterMode.All
        )
    input.Load() |> ignore

    let fishHookSearcher =
        let targetColor = 234uy, 175uy, 46uy
        let readBitmap = new System.Drawing.Bitmap(563, 462)
        let topLeft = new System.Drawing.Point(1720, 440)
        ColorFinder.thresholdSearcher readBitmap topLeft targetColor

    let fishReelSearcher =
        let targetColor = 4uy, 252uy, 180uy
        let readBitmap = new System.Drawing.Bitmap(563, 924)
        let topLeft = new System.Drawing.Point(1720, 440)
        ColorFinder.thresholdSearcher readBitmap topLeft targetColor

    let mutable isReeling = false

    let onMousePress (e: MousePressedEventArgs) =
        if (e.State = MouseState.LeftDown || e.State = MouseState.RightDown) && isReeling
        then
            isReeling <- false
            printfn "Reel cancelled %s" (e.State.ToString())
    GameOverlay.run()

    let stopwatch = new Stopwatch()

    input.OnMousePressed.Add(onMousePress)

    while true do
        stopwatch.Start()
        let result =
            if isReeling
            then fishReelSearcher 0.89
            else fishHookSearcher 0.89

        GameOverlay.drawResult <- Option.Some result

        if isReeling
        then
            if List.length result.results >= 3
            then
                input.SendMouseEvent(MouseState.LeftDown)
                Thread.Sleep(400)
            else
                input.SendMouseEvent(MouseState.LeftUp)
                Thread.Sleep(400)
        else
            if List.length result.results >= 7
            then
                printfn "Fish found! Reeling"
                input.SendLeftClick()
                isReeling <- true

        stopwatch.Reset()
    0
