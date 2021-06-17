using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

namespace MG.Shared.Global
{
    public static class Sound
    {
        private static SoundEffect ThrustSound;                                 // Wave file
        private static SoundEffectInstance ThrustSoundInstance;                 // Sound effect instance that can still be controled after playback begins
        private static bool thrust;

        private static SoundEffect dripSound;                                   // Wave file
        private static SoundEffect fuzzSound;                                   // Wave file
        private static SoundEffect impulse44;                                   // Wave file

        private static bool mute;
        private static float volume;
        private static float SoundLevel;

        public static void LoadContent(ContentManager content)
        {
            SoundLevel = 0.02f;
            dripSound = content.Load<SoundEffect>("Audio/Drip");
            fuzzSound = content.Load<SoundEffect>("Audio/Fuzz");
            ThrustSound = content.Load<SoundEffect>("Audio/Thrust");
            impulse44 = content.Load<SoundEffect>("Audio/Impulse44");
            ThrustSoundInstance = ThrustSound.CreateInstance();
            ThrustSoundInstance.Volume = SoundLevel;
            ThrustSoundInstance.IsLooped = true;
            Thrust = Vector2.Zero;
        }
        public static bool Mute { get => mute; set => mute = value; }
        public static Vector2 Thrust
        {
            set
            {
                if ((value == Vector2.Zero) | mute)
                {
                    if (thrust)
                    {
                        ThrustSoundInstance.Stop();
                        thrust = false;
                    }
                }
                else
                {
                    if (!thrust)
                    {
                        ThrustSoundInstance.Play();
                        thrust = true;
                    }
                    ThrustSoundInstance.Pan = -(Vector2.Normalize(value)).X * 0.5f;
                }
            }
        }
        public static float Volume { get => volume; set => volume = value; }
        public static void Drip()
        {
            if (!mute) dripSound.Play(SoundLevel, 0, 0);
        }
        public static void Fuzz(float pitch = 1.0f)
        {
            if (!mute) fuzzSound.Play(SoundLevel * 0.5f, pitch, 0);
        }
        public static void Impulse44(float pitch = 1.0f)
        {
            if (!mute) impulse44.Play(SoundLevel * 10f, pitch, 0);
        }
    }
}
