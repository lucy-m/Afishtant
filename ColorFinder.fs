module ColorFinder

    open System.Drawing

    let maxSearcher (readBitmap: Bitmap) (topLeft: Point) (targetColor: Color.Model): unit -> SearchResult.Model =
        let graphics = Graphics.FromImage(readBitmap)
        let compare = Color.compare targetColor

        let boundary: Boundary.Model =
            {
                x = topLeft.X
                y = topLeft.Y
                width = readBitmap.Width
                height = readBitmap.Height
            }

        fun () ->
            graphics.CopyFromScreen(topLeft, Point.Empty, readBitmap.Size)
            let compared = compare readBitmap Color.absDiffNorm
            let max = Color.findMax compared

            {
                boundary = boundary
                results = List.singleton max
            }

    let thresholdSearcher (readBitmap: Bitmap) (topLeft: Point) (targetColor: Color.Model): float -> SearchResult.Model =
        let graphics = Graphics.FromImage(readBitmap)
        let compare = Color.compare targetColor

        let boundary: Boundary.Model =
            {
                x = topLeft.X
                y = topLeft.Y
                width = readBitmap.Width
                height = readBitmap.Height
            }

        fun (threshold: float) ->
            graphics.CopyFromScreen(topLeft, Point.Empty, readBitmap.Size)
            let compared = compare readBitmap Color.absDiffNorm
            let results = compared |> Seq.filter (fun r -> r.value > threshold) |> Seq.toList

            {
                boundary = boundary
                results = results
            }

