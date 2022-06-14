using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class UnitAudioManager : MonoBehaviour
{
    public Sound[] sounds;

    public void Init() {
        foreach (Sound s in sounds) {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;

            s.source.spatialBlend = 1;
        }
    }

    public void PlayAudio(string name) {
        var audio = Array.Find(sounds, sound => sound.name == name);

        if (audio == null)
            return;

        audio.source.Play();
    }

    public void PlayLoopedAudio(string name, bool onOrOff) {
        var audio = Array.Find(sounds, sound => sound.name == name);

        if (audio == null)
            return;

        if (onOrOff)
            audio.source.Play();
        else
            audio.source.Stop();
    }
}