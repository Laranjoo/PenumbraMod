using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PenumbraMod.Content.Items
{
	public class TerroristsKnife : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Terrorist's Knife"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
			// Tooltip.SetDefault("This small knife was used by terrorists...");

		}

		public override void SetDefaults()
		{
			Item.damage = 119;
			Item.DamageType = DamageClass.Melee;
			Item.width = 30;
			Item.height = 30;
			Item.useTime = 8;
			Item.useAnimation = 8;
			Item.useStyle = 1;
			Item.knockBack = 4;
			Item.value = 4200;
			Item.rare = ItemRarityID.Yellow;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
			Item.crit = 42;
		}


	}
}