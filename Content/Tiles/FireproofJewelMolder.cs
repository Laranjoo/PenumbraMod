using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace PenumbraMod.Content.Tiles
{
    public class FireproofJewelMolder : ModTile
    {
        // If you want to know more about tiles, please follow this link
        // https://github.com/tModLoader/tModLoader/wiki/Basic-Tile
        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileLavaDeath[Type] = true;
            AdjTiles = new int[] { TileID.Furnaces };
            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3);
            TileObjectData.addTile(Type);

            // AddMapEntry is for setting the color and optional text associated with the Tile when viewed on the map
            AddMapEntry(new Color(101, 90, 102), Language.GetText("Fireproof Jewel Molder"));

        }
        private static int FrameCounter;
        private static int Frame;
        bool dustac;
        // EXTREMELY LAZY WAY TO MAKE ANIMATION
        public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
        {
            if (Lighting.Mode == Terraria.Graphics.Light.LightMode.Color)
            {
                if (++FrameCounter > 12)
                {
                    Frame = (Frame + 1) % 15;
                    FrameCounter = 0;
                }
            }
            if (Lighting.Mode == Terraria.Graphics.Light.LightMode.White)
            {
                if (++FrameCounter > 14)
                {
                    Frame = (Frame + 1) % 15;
                    FrameCounter = 0;
                }
            }
            if (Lighting.Mode == Terraria.Graphics.Light.LightMode.Retro || Lighting.Mode == Terraria.Graphics.Light.LightMode.Trippy)
            {
                if (++FrameCounter > 40)
                {
                    Frame = (Frame + 1) % 15;
                    FrameCounter = 0;
                }
            }
            Texture2D texture = ModContent.Request<Texture2D>("PenumbraMod/Content/Tiles/FireproofJewelMolder").Value;
            Texture2D texture2 = ModContent.Request<Texture2D>("PenumbraMod/Content/Tiles/FireproofJewelMolder2").Value;
            Texture2D texture3 = ModContent.Request<Texture2D>("PenumbraMod/Content/Tiles/FireproofJewelMolder3").Value;
            Texture2D texture35 = ModContent.Request<Texture2D>("PenumbraMod/Content/Tiles/FireproofJewelMolder3,5").Value;
            Texture2D texture4 = ModContent.Request<Texture2D>("PenumbraMod/Content/Tiles/FireproofJewelMolder4").Value;
            Texture2D texture45 = ModContent.Request<Texture2D>("PenumbraMod/Content/Tiles/FireproofJewelMolder4,5").Value;
            Texture2D texture46 = ModContent.Request<Texture2D>("PenumbraMod/Content/Tiles/FireproofJewelMolder4,6").Value;
            Texture2D texture47 = ModContent.Request<Texture2D>("PenumbraMod/Content/Tiles/FireproofJewelMolder4,7").Value;
            Tile tile = Main.tile[i, j];         
            Vector2 zero = Main.drawToScreen ? Vector2.Zero : new Vector2(Main.offScreenRange);
            switch (Frame)
            {
                case 1:
                    Main.spriteBatch.Draw(
                texture,
                new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y) + zero,
                new Rectangle(tile.TileFrameX, tile.TileFrameY, 16, 16),
                Lighting.GetColor(i, j), 0f, default, 1f, SpriteEffects.None, 0f);
                    break;
                case 2:
                    Main.spriteBatch.Draw(
               texture,
               new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y) + zero,
               new Rectangle(tile.TileFrameX, tile.TileFrameY, 16, 16),
               Lighting.GetColor(i, j), 0f, default, 1f, SpriteEffects.None, 0f);
                    break;
                case 3:
                    Main.spriteBatch.Draw(
               texture2,
               new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y) + zero,
               new Rectangle(tile.TileFrameX, tile.TileFrameY, 16, 16),
               Lighting.GetColor(i, j), 0f, default, 1f, SpriteEffects.None, 0f);
                    break;
                case 4:
                    Main.spriteBatch.Draw(
                texture3,
                new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y) + zero,
                new Rectangle(tile.TileFrameX, tile.TileFrameY, 16, 16),
                Lighting.GetColor(i, j), 0f, default, 1f, SpriteEffects.None, 0f);
                    break;
                case 5:
                    Main.spriteBatch.Draw(
               texture4,
               new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y) + zero,
               new Rectangle(tile.TileFrameX, tile.TileFrameY, 16, 16),
               Lighting.GetColor(i, j), 0f, default, 1f, SpriteEffects.None, 0f);
                    break;
                case 6:
                    Main.spriteBatch.Draw(
                texture4,
                new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y) + zero,
                new Rectangle(tile.TileFrameX, tile.TileFrameY, 16, 16),
                Lighting.GetColor(i, j), 0f, default, 1f, SpriteEffects.None, 0f);
                    break;
                case 7:
                    Main.spriteBatch.Draw(
               texture4,
               new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y) + zero,
               new Rectangle(tile.TileFrameX, tile.TileFrameY, 16, 16),
               Lighting.GetColor(i, j), 0f, default, 1f, SpriteEffects.None, 0f);
                    break;
                case 8:
                    Main.spriteBatch.Draw(
               texture4,
               new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y) + zero,
               new Rectangle(tile.TileFrameX, tile.TileFrameY, 16, 16),
               Lighting.GetColor(i, j), 0f, default, 1f, SpriteEffects.None, 0f);
                    break;
                case 9:
                    Main.spriteBatch.Draw(
               texture4,
               new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y) + zero,
               new Rectangle(tile.TileFrameX, tile.TileFrameY, 16, 16),
               Lighting.GetColor(i, j), 0f, default, 1f, SpriteEffects.None, 0f);
                    break;
                case 10:
                    Main.spriteBatch.Draw(
               texture4,
               new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y) + zero,
               new Rectangle(tile.TileFrameX, tile.TileFrameY, 16, 16),
               Lighting.GetColor(i, j), 0f, default, 1f, SpriteEffects.None, 0f);
                    break;
                case 11:
                    Main.spriteBatch.Draw(
                texture35,
                new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y) + zero,
                new Rectangle(tile.TileFrameX, tile.TileFrameY, 16, 16),
                Lighting.GetColor(i, j), 0f, default, 1f, SpriteEffects.None, 0f);
                    break;
                case 12:
                    Main.spriteBatch.Draw(
                texture45,
                new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y) + zero,
                new Rectangle(tile.TileFrameX, tile.TileFrameY, 16, 16),
                Lighting.GetColor(i, j), 0f, default, 1f, SpriteEffects.None, 0f);
                    dustac = true;
                    break;
                case 13:
                    if (dustac)
                    {
                        for (int ie = 0; ie < 4; ie++)
                        {
                            Dust dust = Dust.NewDustPerfect(new Vector2(i * 16, j * 16), DustID.Smoke);
                            dust.noGravity = true;
                            dust.velocity.Y -= 3f;
                            dust.velocity.X -= 4f;
                            Dust dust2 = Dust.NewDustPerfect(new Vector2(i * 16, j * 16), DustID.Smoke);
                            dust2.noGravity = true;
                            dust2.velocity.Y -= 3f;
                            dust2.velocity.X += 4f;
                        }                   
                        dustac = false;
                    }                
                    Main.spriteBatch.Draw(
               texture46,
               new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y) + zero,
               new Rectangle(tile.TileFrameX, tile.TileFrameY, 16, 16),
               Lighting.GetColor(i, j), 0f, default, 1f, SpriteEffects.None, 0f);
                    break;
                case 14:
                    Main.spriteBatch.Draw(
               texture47,
               new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y) + zero,
               new Rectangle(tile.TileFrameX, tile.TileFrameY, 16, 16),
               Lighting.GetColor(i, j), 0f, default, 1f, SpriteEffects.None, 0f);
                    break;
                case 15:
                    Main.spriteBatch.Draw(
               texture,
               new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y) + zero,
               new Rectangle(tile.TileFrameX, tile.TileFrameY, 16, 16),
               Lighting.GetColor(i, j), 0f, default, 1f, SpriteEffects.None, 0f);
                    break;
                default:
                    Main.spriteBatch.Draw(
            texture,
            new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y) + zero,
            new Rectangle(tile.TileFrameX, tile.TileFrameY, 16, 16),
            Lighting.GetColor(i, j), 0f, default, 1f, SpriteEffects.None, 0f);
                    break;
            }    
            return false;
        }
    }
}
