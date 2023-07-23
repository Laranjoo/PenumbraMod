using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using PenumbraMod.Content.Items.Placeable;
using PenumbraMod.Content.Buffs;
using Microsoft.Xna.Framework;

namespace PenumbraMod.Content.NPCs
{
	public class vhcmknife : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Strange knife"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
			/* Tooltip.SetDefault("This knife has a strange green toxic liquid on inside..." +
				"\n''Why'd you got this?''"); */
			
		}

		public override void SetDefaults()
		{
			Item.damage = 20;
			Item.DamageType = DamageClass.Melee;
			Item.width = 40;
			Item.height = 48;
			Item.useTime = 26;
			Item.useAnimation = 26;
			Item.useStyle = 1;
			Item.knockBack = 8;
			Item.value = 12000;
			Item.rare = 3;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<knifeproj>();
			Item.shootSpeed = 8f;
			Item.noUseGraphic = true;
			Item.noMelee = true;
		}
       
	}
}