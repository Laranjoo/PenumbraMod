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
	public class Glock17 : ModItem
	{
		public override void SetStaticDefaults()
		{
            // DisplayName.SetDefault("Glock-17"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
			/* Tooltip.SetDefault("This pistol was used by bad guys!"
				+ "\n''Don't be a terrorist!''"); */
			
		}

		public override void SetDefaults()
		{
			Item.damage = 52;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 47;
			Item.height = 30;
			Item.useTime = 38;
			Item.useAnimation = 38;
			Item.useStyle = 5;
			Item.knockBack = 6;
			Item.value = 15000;
			Item.rare = ItemRarityID.LightRed;
            Item.UseSound = new SoundStyle("PenumbraMod/Assets/Sounds/Items/Glock18")
            {
                Volume = 1.2f,
                PitchVariance = 0.2f,
                MaxInstances = 5,
            };
            Item.autoReuse = false;
			Item.shoot = ProjectileID.Bullet;
			Item.useAmmo = AmmoID.Bullet;
			Item.shootSpeed = 22f;
            Item.scale = 0.7f;
            Item.noMelee = true;
			Item.crit = 24;
		}
        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            spriteBatch.Draw(TextureAssets.Item[Item.type].Value, Item.position - Main.screenPosition, null, lightColor, rotation, new Vector2(0, 0), scale * 0.7f, SpriteEffects.None, 0);
            return false;
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-1f, 3f);
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.HallowedBar, 12);
            recipe.AddIngredient(ItemID.IllegalGunParts, 1);
            recipe.AddIngredient(ItemID.Handgun, 1);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }



    }
}