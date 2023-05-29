using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundtrackGooFunk : SoundtrackManager
{
    public override void InitializeSoundtracks()
    {
        base.InitializeSoundtracks();
    }


    // rytming always with bombo 

    /// First sequence
    // play chords & drumkit

    /// Second sequence 
    // play bass & melodia

    /// Third sequence
    // play chords && melodia


    public override void RythmOn()
    {
        base.RythmOn();

        switch (currentIteration)
        {
            case 0:

                SelectInstrumentToBeat(3, 0.6f);
                SelectInstrumentToBeat(4, 0.6f);
                SelectInstrumentToBeat(5, 0.6f);

                break;
                
            case 1:
                
                SelectInstrumentToBeat(1, 0.6f);
                SelectInstrumentToBeat(6, 0.6f);

                break;

            case 2:

                SelectInstrumentToBeat(3, 0.6f);
                SelectInstrumentToBeat(4, 0.6f);
                SelectInstrumentToBeat(6, 0.6f);

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

                SelectInstrumentToStopBeating(3);
                SelectInstrumentToStopBeating(4);
                SelectInstrumentToStopBeating(5);

                break;

            case 1:

                SelectInstrumentToStopBeating(1);
                SelectInstrumentToStopBeating(6);

                break;

            case 2:

                SelectInstrumentToStopBeating(3);
                SelectInstrumentToStopBeating(4);
                SelectInstrumentToStopBeating(6);

                break;

            default:
                break;
        }
    }
    
    public override void FreedInitialize()
    {
        base.FreedInitialize();
    }

    public override void ConfigurateFinal()
    {
        AllBeating();
    }
}