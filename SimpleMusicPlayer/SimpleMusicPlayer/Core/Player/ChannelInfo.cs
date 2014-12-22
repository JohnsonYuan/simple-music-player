using System;
using FMOD;
using SimpleMusicPlayer.Core.Interfaces;
using SimpleMusicPlayer.FMODStudio;

namespace SimpleMusicPlayer.Core.Player
{
    internal class ChannelInfo
    {
        public FMOD.Channel Channel { get; set; }
        public IMediaFile File { get; set; }

        public bool FadeVolume(float startVol, float endVol, float startPoint, float fadeLength, float currentTime)
        {
            if ((fadeLength > 0f) && (currentTime >= startPoint) && (currentTime <= startPoint + fadeLength))
            {
                var calcVolume = Math.Abs(((endVol - startVol) / fadeLength) * (currentTime - startPoint));
                if (startVol < endVol)
                {
                    this.Volume = calcVolume + startVol;
                }
                else
                {
                    this.Volume = startVol - calcVolume;
                }
                return true;
            }
            return false;
        }

        private float volume = -1f;

        public float Volume
        {
            get { return this.volume; }
            set
            {
                if (this.Channel == null || Equals(value, this.volume))
                {
                    return;
                }
                this.volume = value;

                this.Channel.setVolume(value).ERRCHECK();
            }
        }

        public void CleanUp()
        {
            if (this.Channel != null)
            {
                this.Channel.setVolume(0f).ERRCHECK(RESULT.ERR_INVALID_HANDLE);
                this.Channel.setCallback(null).ERRCHECK(RESULT.ERR_INVALID_HANDLE);
                this.Channel = null;
            }

            this.File.State = PlayerState.Stop;
            this.File = null;
        }
    }
}