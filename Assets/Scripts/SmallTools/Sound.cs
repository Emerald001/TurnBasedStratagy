using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound 
{
    public string name;
    public AudioClip clip;
    [HideInInspector] public AudioSource source;

    [Range(0, 1f)]
    public float volume;
    [Range(0.1f, 3f)]
    public float pitch;
    [Range(0, 1f)]
    public float spacialBlend;
    public bool loop;
}