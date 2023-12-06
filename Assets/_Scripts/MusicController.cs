using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    public static MusicController instance;
    void Start() => instance = this;

    public float transitionSeconds = 4;
    public List<AudioSource> SongSources = new List<AudioSource>();
    public float[] TrackVol;
    public float[] DesiredVol;

    void Awake()
    {
        for (int i = 0; i < SongSources.Count; i++)
        {
            SongSources[i].volume = TrackVol[i];
            SongSources[i].Play(); // all music should NOT be Play On Awake bc there are synching issues with that
        }
    }

    void Update()
    {
        for (int i = 0; i < SongSources.Count; i++)
        {
            if (DesiredVol[i] != TrackVol[i])
            {
                TrackVol[i] = Mathf.MoveTowards(TrackVol[i], DesiredVol[i], Time.deltaTime / transitionSeconds);
                SongSources[i].volume = TrackVol[i];
            }
        }
    }

    public void ChangeVolume(int track, float volume)
    {
        DesiredVol[track] = volume;
    }

    public void ChangeVolume(int track, float volume, float time)
    {
        transitionSeconds = time;
        DesiredVol[track] = volume;
    }
}
