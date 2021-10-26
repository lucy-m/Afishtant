module Color

    open System
    open System.Drawing

    type Model = byte * byte * byte
    type Differ = Model -> Model -> float

    let absDiffNorm (c1: Model) (c2: Model): float =
        let r1, g1, b1 = c1
        let r2, g2, b2 = c2

        let diff b1 b2 = Math.Abs(float(b1)**2.0 - float(b2)**2.0)**0.5

        let total = (diff r1 r2) + (diff g1 g2) + (diff b1 b2)

        1.0 - total / 765.0

    let compare (targetColor: Model) (img: Bitmap) (diffFn: Differ): float seq seq =
        let xMax = img.Width
        let yMax = img.Height

        [0 .. 3 .. xMax-1]
        |> Seq.map (fun x ->
            [0 .. 3 .. yMax-1]
            |> Seq.map (fun y ->
                let px = img.GetPixel(x, y)
                let rgb = px.R, px.G, px.B

                diffFn rgb targetColor
            )
        )

    let findMax (src: float seq seq): float =
        src
        |> Seq.map (fun v ->
            v |> Seq.reduce (fun acc n -> Math.Max(acc, n))
        )
        |> Seq.reduce (fun acc n -> Math.Max(acc, n))

    let findAverage (src: float seq seq): float =
        src
        |> Seq.collect id
        |> Seq.average

    let findMatch (targetColor: Model) (img: Bitmap) (diffFn: Differ): float =
        let xMax = img.Width
        let yMax = img.Height

        [0 .. 3 .. xMax-1]
        |> Seq.map (fun x ->
            [0 .. 3 .. yMax-1]
            |> Seq.map (fun y ->
                let px = img.GetPixel(x, y)
                let rgb = px.R, px.G, px.B

                diffFn rgb targetColor
            )
            |> Seq.reduce (fun acc n -> Math.Max(acc, n))
        )
        |> Seq.reduce (fun acc n -> Math.Max(acc, n))

