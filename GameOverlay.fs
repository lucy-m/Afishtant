module GameOverlay
    open GameOverlay.Drawing
    open GameOverlay.Windows
    open System.Collections.Generic

    let scaling = 1.5f
    let brushes = new Dictionary<string, SolidBrush>()

    let mutable drawResult: SearchResult.Model Option = Option.None

    let scaleInt (v: int): float32 = float32(v) / scaling

    let window_setupGraphics (e: SetupGraphicsEventArgs) =
        let graphics = e.Graphics
        brushes.Add("white", graphics.CreateSolidBrush(255, 255, 255))
        brushes.Add("paleblue", graphics.CreateSolidBrush(92, 175, 219, 180))
        brushes.Add("mediumblue", graphics.CreateSolidBrush(36, 70, 181))
        brushes.Add("violet", graphics.CreateSolidBrush(86, 4, 138))
        brushes.Add("verypink", graphics.CreateSolidBrush(217, 26, 175))

    let window_drawGraphics (e: DrawGraphicsEventArgs) =
        let graphics = e.Graphics

        match drawResult with
        | Option.Some result ->
            let boundaryRectangle =
                new Rectangle(
                    scaleInt result.boundary.x,
                    scaleInt result.boundary.y,
                    scaleInt (result.boundary.x + result.boundary.width),
                    scaleInt (result.boundary.y + result.boundary.height)
                )

            let points =
                result.results
                |> Seq.map (fun r ->
                    new Circle(
                        scaleInt r.x + boundaryRectangle.Left,
                        scaleInt r.y + boundaryRectangle.Top,
                        14.0f
                    ),
                    match r.value with
                    | x when x > 0.99 -> "verypink"
                    | x when x > 0.97 -> "violet"
                    | x when x > 0.93 -> "mediumblue"
                    | _ -> "paleblue"
                )

            graphics.ClearScene()
            graphics.DrawRectangle(brushes.["white"], boundaryRectangle, 1.0f)

            for (circle, color) in points do
                graphics.DrawCircle(brushes.[color], circle, 1.5f)

        | Option.None -> printfn "No result to draw"

    let graphics = new Graphics(MeasureFPS = true)
    let window = new GraphicsWindow(0, 0, 3840, 2160, graphics, FPS=30, IsTopmost=true, IsVisible=true)

    window.SetupGraphics.Add(window_setupGraphics)
    window.DrawGraphics.Add(window_drawGraphics)

    let run () =
        window.Create()
