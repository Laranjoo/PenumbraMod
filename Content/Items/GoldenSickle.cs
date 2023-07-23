using Microsoft.Xna.Framework;
using PenumbraMod.Content.Buffs;
using PenumbraMod.Content.DamageClasses;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PenumbraMod.Content.Items
{
	public class GoldenSickle : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Golden Sickle"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
			/* Tooltip.SetDefault("This sickle can extract hay from the grass" +
                "\nIt has a small chance to inflict a buff that makes enemies drop more cash." +
                "\n[c/f9c720:Special ability:] When used, the scythe swing faster for 5 seconds, at cost of reaper energy will not increase."); */
			
		}

		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.Sickle);
			Item.damage = 13;
			Item.DamageType = ModContent.GetInstance<ReaperClass>();
			Item.value = 2340;
			Item.rare = ItemRarityID.Green;
			Item.autoReuse = true;
            Item.useTime = 20;
			Item.useAnimation = 20;


        }
        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if (Main.rand.NextBool(4))
            {

                Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, DustID.Enchanted_Gold);

            }
        }
		
        public override bool CanUseItem(Player player)
        {
			if (player.HasBuff(ModContent.BuffType<ReaperControl>()))
			{
				player.AddBuff(ModContent.BuffType<GoldSpeed>(), 360);

            }
			
			return true;
        }
       
        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
			if (Main.rand.NextBool(24))
			{
				player.AddBuff(BuffID.WeaponImbueGold, 300);
			}
           
        }
        public override void AddRecipes()
		{
			CreateRecipe()
			.AddIngredient(ItemID.GoldBar, 15)
            .AddIngredient(ItemID.Sickle, 1)
            .AddTile(TileID.Anvils)
			.Register();
		}
	}
}