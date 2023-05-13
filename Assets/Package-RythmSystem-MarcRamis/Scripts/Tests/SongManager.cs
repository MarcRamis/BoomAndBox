using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SongManager : MonoBehaviour
{
    //the current position of the song (in seconds)
    float songPosition;

    //the current position of the song (in beats)
    float songPosInBeats;

    //the duration of a beat
    float secPerBeat;

    //how much time (in seconds) has passed since the song started
    float dsptimesong;

    //beats per minute of a song
    float bpm;

    //keep all the position-in-beats of notes in the song
    float[] notes;

    //the index of the next note to be spawned
    //int nextIndex = 0;

    void Start()
    {
        //calculate how many seconds is one beat
        //we will see the declaration of bpm later
        secPerBeat = 60f / secPerBeat;

        //record the time when the song starts
        dsptimesong = (float)AudioSettings.dspTime;

        //start the song
        GetComponent<AudioSource>().Play();
    }
    
    void Update()
    {
        //calculate the position in seconds
        songPosition = (float)(AudioSettings.dspTime - dsptimesong);

        //calculate the position in beats
        songPosInBeats = songPosition / secPerBeat;
    }
}
