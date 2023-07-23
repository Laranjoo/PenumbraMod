using PenumbraMod.Content.Buffs;
using PenumbraMod.Content.DamageClasses;
using PenumbraMod.Content.Items.Consumables;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PenumbraMod.Content.Items
{
	public class WoodenScythe : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Wooden Scythe"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
			/* Tooltip.SetDefault("[c/808080:This scythe is the proof of your courage]" +
                "\n[c/808080:You feel strong holding this]" +
				"\n[c/9b611b:Special ability:] When used, the scythe grows and stuns enemies."); */

		}

		public override void SetDefaults()
		{
			Item.damage = 6;
			Item.DamageType = ModContent.GetInstance<ReaperClass>();
			Item.width = 42;
			Item.height = 36;
			Item.useTime = 30;
			Item.useAnimation = 30;
			Item.useStyle = 1;
			Item.knockBack = 2;
			Item.value = 1000;
			Item.rare = 0;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;

		}
		public override bool CanUseItem(Player player)
		{
			if (player.HasBuff(ModContent.BuffType<ReaperControl>()))
			{
				Item.scale = 1.7f;
			}
			else
			{
				Item.scale = 1f;
			}

			return true;
		}
		public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
		{
			if (player.HasBuff(ModContent.BuffType<ReaperControl>()))
			{
				target.AddBuff(ModContent.BuffType<StunnedNPC>(), 120);
			}



		}
		public override void AddRecipes()
		{
			CreateRecipe()
			.AddIngredient(ItemID.Wood, 15)
			.AddTile(TileID.WorkBenches)
			.Register();
		}
	}
}