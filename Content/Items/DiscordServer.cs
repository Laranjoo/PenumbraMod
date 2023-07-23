using Microsoft.Xna.Framework;
using PenumbraMod.Content.DamageClasses;
using PenumbraMod.Content.Items.Placeable;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Social;

namespace PenumbraMod.Content.Items
{
	public class DiscordServer : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Discord Server"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
			// Tooltip.SetDefault("Use this to join the official Penumbra Mod discord server!");
			
		}

		public override void SetDefaults()
		{
			Item.width = 58;
			Item.height = 46;
			Item.rare = ItemRarityID.Green;
			Item.shoot = ModContent.ProjectileType<EMPTY>();
			Item.useStyle = ItemUseStyleID.HoldUp;
		}
       
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            string url = "https://discord.gg/YgMntSXv8F";
            var psi = new System.Diagnostics.ProcessStartInfo();
            psi.UseShellExecute = true;
            psi.FileName = url;
            System.Diagnostics.Process.Start(psi);
            return true;

        }
	}
}