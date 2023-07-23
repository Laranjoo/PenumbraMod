using PenumbraMod.Content.Items.Consumables;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PenumbraMod.Content.Items
{
	public class RustyCopperSword : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Rusty Copper Sword"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
			/* Tooltip.SetDefault("[c/808080:Ancients said that this blade was made for the true adventurers...]" +
                "\n[c/696969:'You feel a good thing, it is you...']"); */
			
		}

		public override void SetDefaults()
		{
			Item.damage = 7;
			Item.DamageType = DamageClass.Melee;
			Item.width = 28;
			Item.height = 28;
			Item.useTime = 30;
			Item.useAnimation = 30;
			Item.useStyle = 1;
			Item.knockBack = 8;
			Item.value = 1000;
			Item.rare = 0;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
			Item.scale = 1.3f;
			
		}

		
	}
}