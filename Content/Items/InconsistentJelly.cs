using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace PenumbraMod.Content.Items
{
	public class InconsistentJelly : ModItem
	{
		public override void SetStaticDefaults() {
			// DisplayName.SetDefault("Inconsistent Jelly");
			// Tooltip.SetDefault("'This jelly looks strange...'");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 25;
			Main.RegisterItemAnimation(Type, new DrawAnimationVertical(5, 7));
            ItemID.Sets.ItemNoGravity[Item.type] = true;
            ItemID.Sets.AnimatesAsSoul[Item.type] = true;
            
        }

		public override void SetDefaults()
		{
			Item.width = 28;
			Item.height = 28;
			Item.maxStack = 999;
			Item.value = 1250;
			Item.rare = ItemRarityID.Orange;	
		}

	}
}