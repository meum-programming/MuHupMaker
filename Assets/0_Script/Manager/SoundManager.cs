using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    //List<AudioSource> As = new List<AudioSource>();
    Dictionary<string, (AudioSource, AudioClip)> Clips = new Dictionary<string, (AudioSource, AudioClip)>();
    // Start is called before the first frame update
    public static SoundManager i;
    private void Awake()
    {
        i = this;
        DontDestroyOnLoad(this.gameObject);
    }
    public void Play(string key, bool loop = false) {
        (AudioSource, AudioClip) temp;
        if (!Clips.TryGetValue(key, out temp)) {
            temp.Item1 = gameObject.AddComponent<AudioSource>();
            temp.Item1.playOnAwake = false;
            temp.Item1.loop = loop;

            temp.Item2 = Resources.Load<AudioClip>(RPath.EffectSound+key);
            temp.Item1.clip = temp.Item2;
            Clips[key] = temp;
        }
        Clips[key].Item1.Play();
        
        
            
    }
}
