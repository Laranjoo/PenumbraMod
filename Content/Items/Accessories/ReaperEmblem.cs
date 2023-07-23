using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.GameContent.Creative;
using PenumbraMod.Content.Items.Placeable;
using PenumbraMod.Content.DamageClasses;

namespace PenumbraMod.Content.Items.Accessories
{
	// The AutoloadEquip attribute automatically attaches an equip texture to this item.
	// Providing the EquipType.Legs value here will result in TML expecting a X_Legs.png file to be placed next to the item's main texture.
	
	public class ReaperEmblem : ModItem
	{
		public override void SetStaticDefaults() {
			// Tooltip.SetDefault("15% increased reaper damage");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults() {
			Item.width = 28; // Width of the item
			Item.height = 28; // Height of the item
			Item.value = Item.sellPrice(silver: 85); // How many coins the item is worth
			Item.rare = ItemRarityID.LightRed; // The rarity of the item
			Item.accessory = true;
		}

		public override void UpdateEquip(Player player) {
			player.GetDamage<ReaperClass>() += 0.15f;
			
		}
	}
	
    public class AvengerRecipe : GlobalItem
    {
        public override bool AppliesToEntity(Item entity, bool lateInstantiation) => entity.type == ItemID.AvengerEmblem;
        public override void AddRecipes()
        {
			Recipe a = Recipe.Create(ItemID.AvengerEmblem);
			a.AddIngredient(ModContent.ItemType<ReaperEmblem>(), 1);
            a.AddIngredient(ItemID.SoulofMight, 5);
            a.AddIngredient(ItemID.SoulofSight, 5);
            a.AddIngredient(ItemID.SoulofFright, 5);
			a.AddTile(TileID.MythrilAnvil);
			a.Register();
        }
        
    }
}
