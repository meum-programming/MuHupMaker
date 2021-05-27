using System.Collections;
using UnityEngine;

namespace HSMLibrary.Sound
{
    public enum FadeStatus
    {
        NONE,
        FADING_OUT,
        FADING_IN,
        BGM_OFF
    }

    public class MusicManager : MonoBehaviour
    {
        private const float kFadeTime = 0.2f;

        private ChannelManager channelManager;

        private Channel primaryChannel;
        
        private string primaryMusicName = null;
        
        private string holdedNextMusicName = null;
        private int holdedCount = 0;

        private new bool enabled = true;

        private void Start()
        {
            channelManager = new ChannelManager();
        }

        public void ToggleMusic(bool enabled)
        {
            if (this.enabled != enabled)
            {
                this.enabled = enabled;

                if (enabled)
                {
                    Hold(false);
                }
                else
                {
                    holdedNextMusicName = primaryMusicName;
                    Stop();
                }
            }
        }

        public void Hold(bool hold)
        {
            if (hold)
            {
                holdedCount++;
            }
            else
            {
                if (holdedCount > 0)
                {
                    holdedCount--;
                }
            }

            if (holdedCount == 0 && holdedNextMusicName != null)
            {
                Play(holdedNextMusicName);
                holdedNextMusicName = null;
            }
        }

        public void Play(string musicName)
        {
            if (musicName.Equals(primaryMusicName))
            {
                return;
            }

            if (holdedCount > 0 || !enabled)
            {
                holdedNextMusicName = musicName;
                return;
            }

            bool delayNextMusic = false;
            if (primaryChannel != null && primaryChannel.IsPlaying())
            {
                StartCoroutine(CoFadeOutChannel(primaryChannel, kFadeTime));
                delayNextMusic = true;
            }

            primaryMusicName = musicName;
            primaryChannel = channelManager.StealChannel();
            primaryChannel.Play(musicName, delayNextMusic ? kFadeTime : 0);
        }

        public void Stop()
        {
            if (primaryChannel != null && primaryChannel.IsPlaying())
            {
                primaryMusicName = null;
                StartCoroutine(CoFadeOutChannel(primaryChannel, kFadeTime));
            }
        }

        private IEnumerator CoFadeOutChannel(Channel channel, float fadeTime)
        {
            float fadeVolume = 1.0f;
            float fadeStartTime = Time.time;
            while (true)
            {
                float delta = (Time.time - fadeStartTime);
                //fadeVolume -= Time.deltaTime / fadeTime;
                fadeVolume = 1.0f - (delta / fadeTime);

                if (fadeVolume <= 0.0f)
                {
                    fadeVolume = 0.0f;
                }

                //Debug.LogWarning("Mul: " + fadeVolume.ToString());

                channel.SetVolumeMultiplier(fadeVolume);

                if (fadeVolume == 0.0f)
                {
                    yield return new WaitForEndOfFrame();

                    channel.Stop();

                    break;
                }

                yield return new WaitForEndOfFrame();
            }
        }
    }
}
