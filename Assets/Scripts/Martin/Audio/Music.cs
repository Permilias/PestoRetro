using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Music
{
    public AudioClip clip;
    [Range(0, 1)]
    public float baseVolume;
}
