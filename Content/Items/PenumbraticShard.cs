using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace PenumbraMod.Content.Items
{
	public class PenumbraticShard : ModItem
	{
		public override void SetStaticDefaults() {
			// DisplayName.SetDefault("Penumbratic Shard");
			/* Tooltip.SetDefault("The core of Penumbra" +
				"\nYour eyes brightens just to seeing it" +
				"\n''My precious...''"); */

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 25;
			Main.RegisterItemAnimation(Type, new DrawAnimationVertical(5, 5));
            ItemID.Sets.AnimatesAsSoul[Item.type] = true;
            
        }

		public override void SetDefaults()
		{
			Item.width = 28;
			Item.height = 28;
			Item.maxStack = 999;
			Item.value = 23250;
			Item.rare = ItemRarityID.Cyan;	
		}



	}
}