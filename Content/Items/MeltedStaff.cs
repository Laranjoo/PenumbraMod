using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PenumbraMod.Content.NPCs.Bosses.Eyestorm;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace PenumbraMod.Content.Items
{
	public class MeltedStaff : ModItem
	{
		public override void SetStaticDefaults()
		{
            /* Tooltip.SetDefault("Fires a fireball" +
                "\nThe fireball explodes in 3 additional fragments"); */
            Item.staff[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.damage = 50;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 12 ;
            Item.width = 64;
            Item.height = 60;
            Item.useTime = 24;
            Item.useAnimation = 24;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.value = 123000;
            Item.rare = ItemRarityID.Orange;
            Item.UseSound = SoundID.Item20;
            Item.autoReuse = true;
            Item.noMelee = true;
            Item.shoot = ModContent.ProjectileType<MeltedFireball>();
            Item.shootSpeed = 14f;
        }
        float r;
        float a = 1f;
        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            Texture2D tex = ModContent.Request<Texture2D>("PenumbraMod/Content/Items/MeltedFireball").Value;
            Texture2D tex2 = ModContent.Request<Texture2D>("PenumbraMod/Content/Items/MeltedFireballef2").Value;
            if (Main.LocalPlayer.HeldItem.type == Item.type)
            {
                r += 0.1f;
                a -= 0.1f;
                spriteBatch.Draw(tex, position + new Vector2(12, -11), null, Color.White, r, tex.Size() / 2, scale * 1.07f, SpriteEffects.None, 0);
                spriteBatch.Draw(tex2, position + new Vector2(12, -11), null, Color.White * a, r, tex.Size() / 2, scale * 1.07f, SpriteEffects.None, 0);
            }
            else
            {
                a = 1f;
            }
           
            return true;
        }
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            Vector2 muzzleOffset = Vector2.Normalize(velocity) * 60f;

            if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
            {
                position += muzzleOffset;
            }
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<MeltedEmber>(), 15);
            recipe.AddIngredient(ItemID.Obsidian, 12);
            recipe.AddIngredient(ItemID.SoulofNight, 2);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}