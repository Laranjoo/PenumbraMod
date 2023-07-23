using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace PenumbraMod.Content.Items
{
	public class BrokenWoodyThing : ModItem
	{
		public override void SetStaticDefaults() {
			// DisplayName.SetDefault("Broken Woody Thing");
			// Tooltip.SetDefault("This thing has an strange format...");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            
        }

		public override void SetDefaults()
		{
			Item.width = 36;
			Item.height = 36;
			Item.maxStack = 1;
			Item.value = 0;
			Item.rare = ItemRarityID.Gray;	
		}



	}
}