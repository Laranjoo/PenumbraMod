using Microsoft.Xna.Framework;
using PenumbraMod.Content;
using PenumbraMod.Content.Rarities;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace PenumbraMod.Content.Items
{
	public class NatureGrass : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Nature Grass"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
			/* Tooltip.SetDefault("From the pure nature..." +
				"\nFires a Nature Beam that pushes enemies far"); */

		}

		public override void SetDefaults()
		{
			Item.damage = 98;
			Item.DamageType = DamageClass.Melee;
			Item.width = 60;
			Item.height = 60;
			Item.useTime = 32;
			Item.useAnimation = 32;
			Item.useStyle = 1;
			Item.knockBack = 16f;
			Item.value = 1000;
			Item.rare = ModContent.RarityType<NatureGrassRarity>();
			Item.UseSound = SoundID.Item78;
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<NatureBeam>();
			Item.shootSpeed = 12f;

		}
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{

			const int NumProjectiles = 3;

			for (int i = 0; i < NumProjectiles; i++)
			{

				Vector2 newVelocity = velocity.RotatedByRandom(MathHelper.ToRadians(0));

				// Decrease velocity randomly for nicer visuals.
				newVelocity *= 1f - Main.rand.NextFloat(0.4f);

				// Create a projectile.
				Projectile.NewProjectileDirect(source, position, newVelocity, ModContent.ProjectileType<NatureBeam2>(), 0, 12, player.whoAmI);

			}

			return true;
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.ChlorophyteBar, 22);
			recipe.AddIngredient(ItemID.BladeofGrass, 1);
			recipe.AddIngredient(ItemID.Vine, 5);
			recipe.AddIngredient(ItemID.SoulofSight, 7);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}
}