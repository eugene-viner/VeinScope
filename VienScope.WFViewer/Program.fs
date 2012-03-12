module Main
    open System
    open OpenTK
    open OpenTK.Graphics
    open OpenTK.Graphics.OpenGL
    open OpenTK.Audio
    open OpenTK.Audio.OpenAL
    open OpenTK.Input
    open System.Drawing

    type Game() =
        inherit GameWindow(800,600,GraphicsMode.Default)
        do 
            base.VSync <- VSyncMode.On
        let  mutable translationDelta = new Vector3() 
        let  mutable (rX:float) = 0.0
        member t.DrawShape1()=
            GL.Begin(BeginMode.Triangles)
            GL.Color3(Color.Yellow)
            GL.Vertex3(-1.0f, -1.0f, 4.0f)
            GL.Color3(Color.Green) 
            GL.Vertex3(1.0f, -1.0f, 4.0f)
            GL.Color3(Color.Red) 
            GL.Vertex3(0.0f, 1.0f, 4.0f)
            GL.End()

        override t.OnLoad e=
             GL.ClearColor Color.BlueViolet
             GL.Enable EnableCap.DepthTest

             base.Mouse.Move.Add(fun e -> rX <- (rX + float(1.0f)) (*translationDelta <- new Vector3(float32(e.XDelta),float32(e.YDelta),0.0f)*) )
            
        override t.OnResize e =
            base.OnResize e
            GL.Viewport(base.ClientRectangle.X, base.ClientRectangle.Y, base.ClientRectangle.Width, base.ClientRectangle.Height)
            let fovy  = Math.PI / float 4.0 |> float32
            let aspect : float32 = base.Width /  base.Height |> float32
            let projection = Matrix4.CreatePerspectiveFieldOfView(fovy,aspect, 1.0f, 64.0f)
            GL.MatrixMode(MatrixMode.Projection)
            GL.LoadMatrix(ref projection)
        override t.OnRenderFrame e = 
            base.OnRenderFrame(e)
            ClearBufferMask.ColorBufferBit ||| ClearBufferMask.DepthBufferBit  |> GL.Clear
            
            let modelview = Matrix4.LookAt(Vector3.Zero, Vector3.UnitZ, Vector3.UnitY)
            GL.MatrixMode MatrixMode.Modelview
            GL.LoadMatrix(ref modelview)
            //GL.Translate translationDelta
            GL.Rotate(rX,Vector3d.UnitY)
            t.DrawShape1()
           
            base.SwapBuffers()
        override t.OnUpdateFrame e =
            base.OnUpdateFrame(e)
            if base.Keyboard.[Key.Escape] then
                base.Exit()
            
        
                



 
    [<EntryPoint>]
    let main args =
        use g = new Game()
        g.Run() |> ignore
        // Return 0. This indicates success.
        0