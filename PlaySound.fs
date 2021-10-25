module PlaySound

    open System.Media

    let bellPath = @"D:\My Files\Dev\Sounds\small-bell.wav"

    let soundPlayer = new SoundPlayer()

    let playBell () =
        soundPlayer.SoundLocation <- bellPath
        soundPlayer.Play()
