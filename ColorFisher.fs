module ColorFisher
    open System
    open System.Drawing

    type Color = byte * byte * byte
    type ColorDiff = Color -> Color -> float

    let targetColor = 255uy, 241uy, 98uy
    let readBitmap = new Bitmap(1069, 932)
    let topLeft = new Point(1499, 316)
    let graphics = Graphics.FromImage(readBitmap)

    let absDiffNorm (c1: Color) (c2: Color): float =
        let r1, g1, b1 = c1
        let r2, g2, b2 = c2

        let diff b1 b2 = Math.Abs(float(b1) - float(b2))

        let total = (diff r1 r2) + (diff g1 g2) + (diff b1 b2)

        1.0 - total / 765.0

    let colorMatch (img: Bitmap) (diffFn: ColorDiff): float =
        let xMax = img.Width
        let yMax = img.Height

        [0 .. xMax-1]
        |> Seq.map (fun x ->
            [0 .. yMax-1]
            |> Seq.map (fun y ->
                let px = img.GetPixel(x, y)
                let rgb = px.R, px.G, px.B

                diffFn rgb targetColor
            )
            |> Seq.reduce (fun acc n -> Math.Max(acc, n))
        )
        |> Seq.reduce (fun acc n -> Math.Max(acc, n))

    let findFish (): float =
        graphics.CopyFromScreen(topLeft, Point.Empty, readBitmap.Size)
        readBitmap.Save("screengrab.png")
        colorMatch readBitmap absDiffNorm

