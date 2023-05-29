public class SoundtrackDaniSong : SoundtrackManager
{
    public override void InitializeSoundtracks()
    {
        base.InitializeSoundtracks();
    }

    // rytming always with clap

    /// First sequence
    // play hithat and bajo 

    /// Second sequence 
    // play solo kick and piano

    /// Third Sequence
    // play hithat and kick

    public override void RythmOn()
    {
        base.RythmOn();

        switch (currentIteration)
        {
            case 0:

                SelectInstrumentToBeat(2, 0.6f);
                SelectInstrumentToBeat(3, 0.6f);

                break;

            case 1:

                SelectInstrumentToBeat(4, 0.6f);
                SelectInstrumentToBeat(5, 0.6f);

                break;

            case 2:

                SelectInstrumentToBeat(3, 0.6f);
                SelectInstrumentToBeat(4, 0.6f);

                break;

            default:
                break;
        }
    }

    public override void RythmOff()
    {
        base.RythmOff();
        switch (currentIteration)
        {
            case 0:

                SelectInstrumentToStopBeating(2);
                SelectInstrumentToStopBeating(3);

                break;

            case 1:

                SelectInstrumentToStopBeating(4);
                SelectInstrumentToStopBeating(5);

                break;

            case 2:
                
                SelectInstrumentToStopBeating(3);
                SelectInstrumentToStopBeating(4);

                break;

            default:
                break;
        }
    }

    public override void ConfigurateFinal()
    {
        MaxVolume();
    }
}
