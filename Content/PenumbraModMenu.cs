using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.ModLoader;

namespace PenumbraMod.Content
{
    public class PenumbraModMenu : ModMenu
    {
        public override Asset<Texture2D> Logo => base.Logo;
        public override void Update(bool isOnTitleScreen)
        {
            Main.dayTime = false;
            Main.time = 40001;
        }
        // Inspired from stars above
        float MousePositionFloatX;
        float MousePositionFloatY;
        float rotation;
        float vel;
        public override bool PreDrawLogo(SpriteBatch spriteBatch, ref Vector2 logoDrawCenter, ref float logoRotation, ref float logoScale, ref Color drawColor)
        {
            Texture2D MenuBG = ModContent.Request<Texture2D>("PenumbraMod/Content/PenumbraBG").Value;
            Texture2D MenuBGMoon = ModContent.Request<Texture2D>("PenumbraMod/Content/PenumbraBGMoon").Value;
            Vector2 zero = Vector2.Zero;
            Vector2 logoDrawPos = new Vector2(Main.screenWidth / 2, Main.screenHeight / 2);
            rotation += 0.002f;
            vel -= 0.25f;
            float width = (float)Main.screenWidth / (float)MenuBG.Width;
            float height = (float)Main.screenHeight / (float)MenuBG.Height;
            MousePositionFloatX = ((Math.Min(Main.screenWidth, Main.MouseScreen.X) - 0) * 100) / (Main.screenWidth - 0) / 100;
            MousePositionFloatY = ((Math.Min(Main.screenHeight, Main.MouseScreen.Y) - 0) * 100) / (Main.screenHeight - 0) / 100;

            if (MousePositionFloatX < 0)
            {
                MousePositionFloatX = 0;
            }
            if (MousePositionFloatY < 0)
            {
                MousePositionFloatY = 0;
            }
            if (width != height)
            {
                if (height > width)
                {
                    width = height;
                    zero.X -= ((float)MenuBG.Width * width - (float)Main.screenWidth) * 0.5f;
                }
                else
                {
                    zero.Y -= ((float)MenuBG.Height * width - (float)Main.screenHeight) * 0.5f;
                }
            }
            spriteBatch.Draw(
                MenuBG, // texture
                new Vector2(zero.X + MathHelper.Lerp(-98, -82, MousePositionFloatX), zero.Y + MathHelper.Lerp(-50, -47, MousePositionFloatY)), // position
                null, // rectangle
                Color.White, // color
                0f, // rotation
               new Vector2(vel % 1024, 0), // origin
                width * 1.1f, // scale
                0, // spriteeffects
                0f); // layerdepth
            spriteBatch.Draw(
                MenuBG, // texture
               new Vector2(zero.X + MathHelper.Lerp(-98, -82, MousePositionFloatX), zero.Y + MathHelper.Lerp(-50, -47, MousePositionFloatY)), // position
               null, // rectangle
               Color.White, // color
               0f, // rotation
              new Vector2(1024 + vel % 1024, 0), // origin
               width * 1.1f, // scale
               0, // spriteeffects
               0f); // layerdepth
            spriteBatch.Draw(
                 MenuBGMoon, //The texture being drawn.
                 logoDrawPos + new Vector2(zero.X + 95 + MathHelper.Lerp(-98, -82, MousePositionFloatX), zero.Y + 55 +  MathHelper.Lerp(-50, -47, MousePositionFloatY)), //The position of the texture.
                 new Rectangle(0, 0, MenuBGMoon.Width, MenuBGMoon.Height),
                 Color.White, //The color of the texture.
                 rotation, // The rotation of the texture.
                 MenuBGMoon.Size() * 0.5f, //The centerpoint of the texture.
                 1.1f, //The scale of the texture.
                 SpriteEffects.None,
                 0f);
            return true;
        }
        public override int Music => MusicLoader.GetMusicSlot(Mod, "Assets/Music/TitleScreen");

        public override string DisplayName => "Penumbra Mod";


    }
}
