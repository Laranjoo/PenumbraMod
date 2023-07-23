using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace PenumbraMod.Content.Items
{
	public class RedTape : ModItem
	{
		public override void SetStaticDefaults() {
			// DisplayName.SetDefault("Red Tape");
			// Tooltip.SetDefault("'I guess you won't need it anymore...'");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
			
          
            
        }

		public override void SetDefaults()
		{
			Item.width = 28;
			Item.height = 28;
			Item.maxStack = 1;
			Item.value = 0;
			Item.rare = ItemRarityID.Gray;	
		}



	}
}