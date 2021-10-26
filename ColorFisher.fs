module ColorFisher
    open System
    open System.Drawing

    let targetColor = 241uy, 203uy, 65uy
    let readBitmap = new Bitmap(1069, 932)
    let topLeft = new Point(1499, 316)
    let graphics = Graphics.FromImage(readBitmap)

    let compare = Color.compare targetColor

    let findFish (): bool =
        graphics.CopyFromScreen(topLeft, Point.Empty, readBitmap.Size)
        readBitmap.Save("screengrab.png")

        let compared = compare readBitmap Color.absDiffNorm
        let average = Color.findAverage compared
        let max = Color.findMax compared
        let ratio = max / average

        printfn "CF Ratio %f" ratio

        ratio > 2.5
