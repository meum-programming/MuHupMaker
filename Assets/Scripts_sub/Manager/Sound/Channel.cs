using System;
using System.Collections.Generic;
using UnityEngine;

namespace HSMLibrary.Sound
{
    public class Channel
    {
        private int idx;

        private float volume = 1.0f;
        private float volumeMultiplier = 1.0f;

        internal GameObject channel;

        public Channel(int i)
        {
            idx = i;
            channel = new GameObject(String.Format("MusicChannel{0}", idx));
            channel.transform.parent = GameObject.Find("MusicManager").transform;
        }

        private AudioSource GetAudioSource()
        {
            return channel.GetComponent<AudioSource>();
        }

        public bool IsPlaying()
        {
            return GetAudioSource() != null;
        }

        public void AttachClip(string musicName)
        {
            AudioClip audioClip = Resources.Load(String.Format("Sound/BGM/{0}", musicName)) as AudioClip;
            if (audioClip != null)
            {
                AudioSource audioSrc = channel.AddComponent<AudioSource>();
                audioSrc.clip = audioClip;
            }
        }

        public void Play(string musicName, float delayTime = 0.0f)
        {
            AttachClip(musicName);

            AudioSource audioSrc = channel.GetComponent<AudioSource>();
            if (audioSrc != null)
            {
                audioSrc.ignoreListenerVolume = true;
                SetVolumeMultiplier(1.0f);
                audioSrc.loop = true;
                audioSrc.priority = 0;
                audioSrc.volume = volume * volumeMultiplier;
                audioSrc.PlayDelayed(delayTime);
            }
        }

        public void Stop()
        {
            UnityEngine.Object.Destroy(channel.GetComponent<AudioSource>());
        }

        public void SetVolumeMultiplier(float mul)
        {
            if (mul > 1.0f)
                mul = 1.0f;
            if (mul < 0.0f)
                mul = 0.0f;

            volumeMultiplier = Mathf.Log(mul + 1.0f) / Mathf.Log(2.0f);   // Log2(mul + 1)

            SetVolume(volume);
        }

        public void SetVolume(float vol)
        {
            volume = vol;

            AudioSource audioSrc = channel.GetComponent<AudioSource>();
            if (audioSrc != null)
            {
                audioSrc.volume = volume * volumeMultiplier;
            }
        }
    }

    public class ChannelManager
    {
        private List<Channel> channels = new List<Channel>();

        public ChannelManager()
        {
            for (int i = 0; i < 4; i++)
            {
                channels.Add(new Channel(i));
            }
        }

        private Channel GetStoppedChannel()
        {
            for (int i = 0; i < channels.Count; i++)
            {
                if (channels[i].channel.GetComponent<AudioSource>() == null)
                {
                    return channels[i];
                }
            }

            return null;
        }

        public Channel StealChannel()
        {
            Channel candidate = GetStoppedChannel();
            if (candidate == null)
            {
                candidate = channels[2];
            }

            return candidate;
        }
    }
}
