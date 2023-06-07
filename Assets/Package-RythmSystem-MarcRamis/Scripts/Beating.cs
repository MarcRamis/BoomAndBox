using UnityEngine;

[System.Serializable]
public class Beating
{
    //Song beats per minute
    //This is determined by the song you're trying to sync up to
    public float songBpm = 120;

    //The number of seconds for each song beat
    public float secPerBeat;

    //Current song position, in seconds
    public float songPosition;

    //Current song position, in beats
    public float songPositionInBeats;

    //How many seconds have passed since the song started
    public float dspSongTime;

    //The offset to the first beat of the song in seconds
    public float firstBeatOffset;
    
    //the number of beats in each loop
    public float beatsPerLoop = 2;

    //the total number of loops completed since the looping clip first started
    public int completedLoops = 0;

    //The current position of the song within the loop in beats.
    public float loopPositionInBeats;

    //The current relative position of the song within the loop measured between 0 and 1.
    public float loopPositionInAnalog;

    public void InitBeating()
    {
        //Calculate the number of seconds in each beat
        secPerBeat = 60f / songBpm;

        StartBeating();
    }
    
    public void UpdateBeating()
    {
        //determine how many seconds since the song started
        //songPosition = (float)(AudioSettings.dspTime - dspSongTime);
        songPosition = (float)(AudioSettings.dspTime - dspSongTime - firstBeatOffset);

        //determine how many beats since the song started
        songPositionInBeats = songPosition / secPerBeat;

        //calculate the ºp position
        if (songPositionInBeats >= (completedLoops + 1) * beatsPerLoop)
            completedLoops++;
        loopPositionInBeats = songPositionInBeats - completedLoops * beatsPerLoop;

        loopPositionInAnalog = loopPositionInBeats / beatsPerLoop;
    }

    public void StartBeating()
    {
        //Record the time when the music starts
        dspSongTime = (float)AudioSettings.dspTime;
    }
}