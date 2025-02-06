using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace MonoBird {
    public class Sounds {

        public static float volume = 0.1f;

        private static Dictionary<string, SoundEffect> SoundEffects;
        public static Dictionary<string, SoundEffectInstance> SoundEffectInstances;

        public static void Load() {

            Sounds.SoundEffects = new Dictionary<string, SoundEffect> {

                {"die", Main.contentManager.Load<SoundEffect>("Assets\\sfx_die")},
                {"hit", Main.contentManager.Load<SoundEffect>("Assets\\sfx_hit")},
                {"point", Main.contentManager.Load<SoundEffect>("Assets\\sfx_point")},
                {"swoosh", Main.contentManager.Load<SoundEffect>("Assets\\sfx_swooshing")},
                {"wing", Main.contentManager.Load<SoundEffect>("Assets\\sfx_wing")},
            };

            Sounds.SoundEffectInstances = new Dictionary<string, SoundEffectInstance>();

            foreach (var s in Sounds.SoundEffects) {

                Sounds.SoundEffectInstances.Add(s.Key, s.Value.CreateInstance());
            }
        }

        public static void Update() {

            Sounds.volume = MathHelper.Clamp(Sounds.volume, 0f, 1f);
        }
    }
}