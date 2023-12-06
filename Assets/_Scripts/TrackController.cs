using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackController : MonoBehaviour
{
    public bool DoCustomLoop;
    public float LoopSpot;
    public float LoopDuration;

    AudioSource aud;
    void Start() => aud = GetComponent<AudioSource>();

    void Update()
    {
        if (DoCustomLoop)
            if (aud.time >= LoopSpot)
                aud.time -= LoopDuration;
    }
}
