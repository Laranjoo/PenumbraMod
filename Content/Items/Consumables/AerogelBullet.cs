using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.GameContent.Creative;
using Terraria.Localization;
using Terraria.ModLoader;
using PenumbraMod.Content.Items.Placeable;

namespace PenumbraMod.Content.Items.Consumables
{
	public class AerogelBullet : ModItem
	{
		public override void SetStaticDefaults() {
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 99;
		}

		public override void SetDefaults() {
            Item.damage = 10; // The damage for projectiles isn't actually 12, it actually is the damage combined with the projectile and the item together.
            Item.DamageType = DamageClass.Ranged;
            Item.width = 8;
            Item.height = 8;
            Item.maxStack = 999;
            Item.consumable = true; // This marks the item as consumable, making it automatically be consumed when it's used as ammunition, or something else, if possible.
            Item.knockBack = 1.5f;
            Item.value = 10;
            Item.rare = ItemRarityID.Blue;
            Item.shoot = ModContent.ProjectileType<AerogelBulletProjectile>(); // The projectile that weapons fire when using this item as ammunition.
            Item.shootSpeed = 38f; // The speed of the projectile.
            Item.ammo = AmmoID.Bullet; // The ammo class this ammo belongs to.
        }


		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe(90);

			recipe.AddIngredient(ModContent.ItemType<AerogelBar>(), 3);
			recipe.AddIngredient(ItemID.MusketBall, 90);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}

	}
}