using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace PenumbraMod.Content.Items
{
	public class DarkMatterItem : ModItem
	{
		public override void SetStaticDefaults() {
			// DisplayName.SetDefault("Dark Matter");
			/* Tooltip.SetDefault("The darkest thing in universe" +
				"\nYour feel powerful on looking at it" +
				"\n''Maybe if i...''"); */

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 30;
			Main.RegisterItemAnimation(Type, new DrawAnimationVertical(5, 13));
            ItemID.Sets.AnimatesAsSoul[Item.type] = true;
			ItemID.Sets.ItemNoGravity[Item.type] = true;
        }

		public override void SetDefaults()
		{
			Item.width = 24;
			Item.height = 52;
			Item.maxStack = 999;
			Item.value = 33250;
			Item.rare = ItemRarityID.Purple;	
		}



	}
}