using PenumbraMod.Content.Buffs;
using PenumbraMod.Content.DamageClasses;
using PenumbraMod.Content.Items.Consumables;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace PenumbraMod.Content.Items
{
	public class LeadScythe : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Lead Scythe"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
			/* Tooltip.SetDefault("Strong enough to cut your finger!" +
                "\n[c/3e5272:Special ability:] When used, you get 45% damage reduction on next monster attack."); */
			
		}

		public override void SetDefaults()
		{
			Item.damage = 17;
			Item.DamageType = ModContent.GetInstance<ReaperClass>();
			Item.width = 54;
			Item.height = 44;
			Item.useTime = 28;
			Item.useAnimation = 28;
			Item.useStyle = 1;
			Item.knockBack = 2;
			Item.value = 3870;
			Item.rare = 1;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
			
		}
        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (player.HasBuff(ModContent.BuffType<ReaperControl>()))
            {
                SoundEngine.PlaySound(SoundID.Item37, player.position);
                player.AddBuff(ModContent.BuffType<LeadForce>(), 899999);
			}
               


        }
        public override void AddRecipes()
		{
			CreateRecipe()
			.AddIngredient(ItemID.LeadBar, 16)
			.AddIngredient(ItemID.Wood, 10)
			.AddTile(TileID.WorkBenches)
			.Register();
		}
	}
}