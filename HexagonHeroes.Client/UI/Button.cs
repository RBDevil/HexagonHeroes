using HexagonHeroes.Client.Resources;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace HexagonHeroes.Client.UI
{
    public class Button
    {
        public delegate void ClickAction();
        ClickAction clickAction;
        Texture2D texture;
        Point position;
        Rectangle rectangle;
        public Button(Texture2D texture, Point position, ClickAction clickAction)
        {
            this.position = position;
            this.clickAction = clickAction;
            this.texture = texture;
            rectangle = new Rectangle(position.X, position.Y, texture.Width, texture.Height);
        }
        public void Update(Point mousePosition, bool click)
        {
            if (click && rectangle.Contains(mousePosition))
            {
                Sounds.Container["buttonClick"].Play();
                clickAction?.Invoke();
            }
        }
        public void Draw(SpriteBatch sb)
        {
            sb.Draw(texture, position.ToVector2(), Color.White);
        }
    }
}
