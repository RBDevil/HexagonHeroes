using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Text;

namespace HexagonHeroes.Client.Resources
{
    public static class Sounds
    {
        public static Dictionary<string, SoundEffectInstance> Container;

        public static void LoadSounds(ContentManager Content)
        {
            Container = new Dictionary<string, SoundEffectInstance>();

            Container.Add("buttonClick", Content.Load<SoundEffect>("buttonClick").CreateInstance());
        }
    }
}
