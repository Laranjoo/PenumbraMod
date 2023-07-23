using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace PenumbraMod.Content.Items
{
	public class MeltedEmber : ModItem
	{
		public override void SetStaticDefaults() {
			// DisplayName.SetDefault("Melted Bones");
			// Tooltip.SetDefault("This is hot...");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 25;
            
        }

		public override void SetDefaults()
		{
			Item.width = 28;
			Item.height = 28;
			Item.maxStack = 999;
			Item.value = 2400;
			Item.rare = ItemRarityID.LightRed;	
		}



	}
}