using HexagonHeroes.Client.Resources;
using HexagonHeroes.Client.States.Game;
using HexagonHeroes.Client.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace HexagonHeroes.Client.States.Menu
{
    static class MenuState
    {
        static HeroTypes choosenHero;
        static UI_Updater UI;
        static bool active;
        public static void Update()
        {
            if (active)
            {
                UI.Update();
            }
        }
        public static void Draw(SpriteBatch sb, SpriteFont sf)
        {
            if (active)
            {
                UI.Draw(sb);
                string choosenHeroString;
                switch (choosenHero)
                {
                    case HeroTypes.Tank:
                        choosenHeroString = "Tank";
                        break;
                    case HeroTypes.Mage:
                        choosenHeroString = "Mage";
                        break;
                    case HeroTypes.Support:
                        choosenHeroString = "Support";
                        break;
                    case HeroTypes.Fighter:
                        choosenHeroString = "Fighter";
                        break;
                    default:
                        choosenHeroString = "Tank";
                        break;
                }

                sb.DrawString(sf, choosenHeroString, new Vector2(270, 200), Color.Black);
            }
        }
        public static void Exit()
        {
            active = false;
        }
        public static void Activate()
        {
            InitUI();
            choosenHero = HeroTypes.Tank;
            active = true;
        }
        static void InitUI()
        {
            UI = new UI_Updater();
            UI.AddButton(Textures.Container["button"], new Point(200, 50), PlayButtonAction);
            UI.AddButton(Textures.Container["button"], new Point(350, 200), NextHero);
            UI.AddButton(Textures.Container["button"], new Point(150, 200), PreviousHero);
        }
        static void PlayButtonAction()
        {
            Exit();
            GameState.Activate(choosenHero);
        }
        static void NextHero()
        {
            choosenHero += 1;
            if ((int)choosenHero > 4)
            {
                choosenHero = (HeroTypes)1;
            }     
        }
        static void PreviousHero()
        {
            choosenHero -= 1;
            if ((int)choosenHero < 1)
            {
                choosenHero = (HeroTypes)4;
            }
        }
    }
}
