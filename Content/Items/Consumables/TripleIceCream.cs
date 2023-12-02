using Terraria;
using Terraria.ID;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;
using PenumbraMod.Content.Buffs;

namespace PenumbraMod.Content.Items.Consumables
{
	public class TripleIceCream : ModItem
	{
		public override void SetStaticDefaults() {
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 5;
		}

        public override void SetDefaults()
        {
            Item.DefaultToFood(22, 22, BuffID.WellFed2, 36000, true);
            Item.rare = ItemRarityID.Blue;
        }
    }
}
