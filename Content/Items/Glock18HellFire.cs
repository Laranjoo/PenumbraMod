using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using PenumbraMod.Content;
using Terraria.Audio;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;

namespace PenumbraMod.Content.Items
{
	public class Glock18HellFire : ModItem
	{
		public override void SetStaticDefaults()
		{
            // DisplayName.SetDefault("Glock18 - HellFire"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
			/* Tooltip.SetDefault("'This looks fire...'"
				+ "\n'BANG BANG POW POW HAHAHAHA!'"); */
			
		}

		public override void SetDefaults()
		{
			Item.damage = 100;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 49;
			Item.height = 32;
			Item.useTime = 24;
			Item.useAnimation = 24;
			Item.useStyle = 5;
			Item.knockBack = 6;
			Item.value = 22000;
			Item.rare = ItemRarityID.Yellow;
            Item.UseSound = new SoundStyle("PenumbraMod/Assets/Sounds/Items/Glock17Sound")
            {
                Volume = 1.0f,
                PitchVariance = 0.3f,
                MaxInstances = 5,
            };
            Item.autoReuse = true;
			Item.shoot = ProjectileID.Bullet;
			Item.useAmmo = AmmoID.Bullet;
			Item.shootSpeed = 22f;
			Item.scale = 0.7f;
			Item.noMelee = true;
			Item.crit = 32;
		}
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-1f, 3f);
        }
        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            spriteBatch.Draw(TextureAssets.Item[Item.type].Value, Item.position - Main.screenPosition, null, lightColor, rotation, new Vector2(0, 0), scale * 0.7f, SpriteEffects.None, 0);
            return false;
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.HellstoneBar, 7);
            recipe.AddIngredient(ModContent.ItemType<MeltedEmber>(), 5);
            recipe.AddIngredient(ItemID.IllegalGunParts, 1);
            recipe.AddIngredient(ModContent.ItemType<Glock17>());
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }



    }
}