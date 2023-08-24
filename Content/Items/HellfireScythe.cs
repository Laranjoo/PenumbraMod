using Microsoft.Xna.Framework;
using PenumbraMod.Content.Buffs;
using PenumbraMod.Content.DamageClasses;
using PenumbraMod.Content.Items.Consumables;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace PenumbraMod.Content.Items
{
	public class HellfireScythe : ModItem
	{
		public override void SetDefaults()
		{
			Item.damage = 35;
			Item.DamageType = ModContent.GetInstance<ReaperClass>();
			Item.width = 50;
			Item.height = 46;
			Item.useTime = 22;
			Item.useAnimation = 22;
			Item.useStyle = 1;
			Item.knockBack = 5;
			Item.value = 7870;
			Item.rare = ItemRarityID.Orange;
			Item.UseSound = SoundID.Item1;
			Item.shoot = ModContent.ProjectileType<EMPTY>();
			Item.shootSpeed = 8f;
			Item.autoReuse = true;
			
		}
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            return true;
        }
        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
			target.AddBuff(BuffID.OnFire, 60);
        }
        public override void AddRecipes()
		{
			CreateRecipe()
			.AddIngredient(ItemID.HellstoneBar, 16)
            .AddIngredient(ItemID.Obsidian, 6)
            .AddTile(TileID.Anvils)
			.Register();
		}
	}
}