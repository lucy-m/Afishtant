module AutoReeler
    open System
    open System.Drawing

    let targetColor = 0uy, 249uy, 175uy
    let readBitmap = new Bitmap(1069, 1332)
    let topLeft = new Point(1499, 316)
    let graphics = Graphics.FromImage(readBitmap)

    type Result = MouseUp | MouseDown

    let mutable lastResult = MouseUp

    let compare = Color.compare targetColor

    let tryReel (): Result Option =
        graphics.CopyFromScreen(topLeft, Point.Empty, readBitmap.Size)
        readBitmap.Save("screengrabar.png")

        let compared = compare readBitmap Color.absDiffNorm
        let average = Color.findAverage compared
        let max = Color.findMax compared
        let ratio = max / average

        printfn "\tAR Ratio %f" ratio

        let result =
            if ratio > 2.3
            then MouseDown
            else MouseUp

        let ret =
            match result, lastResult with
            | MouseUp, MouseDown -> MouseUp |> Option.Some
            | MouseDown, MouseUp -> MouseDown |> Option.Some
            | _ -> Option.None

        if ret |> Option.isSome
        then lastResult <- result

        ret
