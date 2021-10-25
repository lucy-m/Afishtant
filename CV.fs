module CV

    open System.Drawing
    open System

    let readBitmap = new Bitmap(409, 467)
    let graphics = Graphics.FromImage(readBitmap)
    let topLeft = new Point(1792, 927)

    type GsImg = float[][]
    type CompareFn = float -> float -> float

    let absDiff (p1: float) (p2: float): float =
        Math.Abs(p1 - p2)

    let toGreyscale (img: Bitmap): GsImg =
        let xRange = [|0 .. img.Size.Width - 1|]
        let yRange = [|0 .. img.Size.Height - 1|]

        xRange
        |> Array.map (fun x ->
            yRange
            |> Array.map (fun y ->
                let p = img.GetPixel(x,y)
                float(p.R + p.G + p.B) / 3.0
            )
        )

    let computeDiff (img1: GsImg) (img2: GsImg) (compareFn: CompareFn): float =
        let xMax = Math.Min(Array.length img1, Array.length img2)
        let yMax = Math.Min(Array.head img1 |> Array.length, Array.head img2 |> Array.length)

        let diff =
            [|0 .. xMax - 1|]
            |> Array.collect (fun x ->
                [|0 .. yMax - 1|]
                |> Array.map (fun y ->
                    let p1 = img1.[x].[y]
                    let p2 = img2.[x].[y]

                    let diff = compareFn p1 p2 / 255.0

                    diff
                )
            )

        let avg = Array.average diff

        avg

    let templateMatch (img: GsImg) (template: GsImg) (compareFn: CompareFn): float[][] =
        let xMax = (Array.length img) - (Array.length template)
        let yMax = (Array.head img |> Array.length) - (Array.head template |> Array.length)

        printfn "xMax %i yMax %i" xMax yMax

        if xMax <= 0 || yMax <= 0
        then
            printfn("Template match is empty")
            Array.empty
        else
            let diffs =
                [|0 .. xMax - 1|]
                |> Array.map (fun x ->
                    [|0 .. yMax - 1|]
                    |> Array.map (fun y ->
                        let subImg =
                            Array.skip x img
                            |> Array.map (fun x' -> Array.skip y x')

                        let diff = computeDiff subImg template compareFn

                        diff
                    )
                )

            diffs

    let isMatch (matches: float[][]) (threshold: float): float =
        matches
        |> Array.fold (fun acc n ->
            let colMatches =
                n
                |> Array.fold (fun acc n ->
                    Math.Max(acc, n)
                ) 0.0

            Math.Max(acc, colMatches)
        ) 0.0

    let fishHook = Bitmap.FromFile("fishhook.png") :?> Bitmap |> toGreyscale

    let findFish (): float =
        graphics.CopyFromScreen(topLeft, Point.Empty, readBitmap.Size)
        readBitmap.Save("screengrab.png", Imaging.ImageFormat.Png)

        let readBitmapGs = readBitmap |> toGreyscale

        let tMatch = templateMatch readBitmapGs fishHook absDiff

        isMatch tMatch 0.9
