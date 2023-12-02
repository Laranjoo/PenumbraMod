using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace PenumbraMod.Content.Items.Consumables
{
	public class NeapolitanCake : ModItem
	{
		public override void SetStaticDefaults() {
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 5;
			Main.RegisterItemAnimation(Type, new DrawAnimationVertical(int.MaxValue, 3));
			ItemID.Sets.IsFood[Type] = true; 
		}
		public override void SetDefaults() {
			Item.DefaultToFood(22, 22, BuffID.WellFed3, 36000);			
			Item.rare = ItemRarityID.Blue;
		}
	}
}