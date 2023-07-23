using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Terraria.GameContent.ItemDropRules;

namespace PenumbraMod.Content.Items.Consumables
{
	
	public class StarterBag : ModItem
	{
		public override void SetStaticDefaults() {
			// DisplayName.SetDefault("Starter Bag");
			/* Tooltip.SetDefault("Your adventure begins..."
				+ "\n{$CommonItemTooltip.RightClickToOpen}"); */ // References a language key that says "Right Click To Open" in the language of the game

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 3;
		}

		public override void SetDefaults() {
			Item.maxStack = 1;
			Item.consumable = true;
			Item.width = 24;
			Item.height = 24;
			Item.rare = ItemRarityID.Orange;
			
		}

		public override bool CanRightClick() {
			return true;
		}

		public override void ModifyItemLoot(ItemLoot itemLoot) {
            itemLoot.Add(ItemDropRule.Common(ItemID.CopperHammer, 1));
            itemLoot.Add(ItemDropRule.Common(ItemID.Wood, 1, 50, 50));
            itemLoot.Add(ItemDropRule.Common(ItemID.StoneBlock, 1, 50, 50));
			itemLoot.Add(ItemDropRule.Common(ItemID.Gel, 1, 20, 20));
            itemLoot.Add(ItemDropRule.Common(ItemID.Torch, 1, 12, 12));
            itemLoot.Add(ItemDropRule.Common(ItemID.Chest, 2));
            itemLoot.Add(ItemDropRule.Common(ItemID.MiningPotion, 1, 3, 3));
            itemLoot.Add(ItemDropRule.Common(ItemID.ShinePotion, 1, 3, 3));
			itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<RedTape>(), 1));
            itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<EmptyBag>(), 1));
            itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<Marshmellow>(), 1, 5, 5));
            itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<RustyCopperSword>(), 1));
            itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<DiscordServer>(), 1));
        }

		
	}
    [AutoloadEquip(EquipType.Head)]
    public class EmptyStarterBag : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Empty starter bag");
            // Tooltip.SetDefault("Empty, but gorgeous!"); // References a language key that says "Right Click To Open" in the language of the game
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.maxStack = 1;
            Item.width = 24;
            Item.height = 24;
            Item.rare = ItemRarityID.White;
            Item.vanity = true;
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<EmptyBag>(), 1);
            recipe.AddIngredient(ModContent.ItemType<RedTape>(), 1);
            recipe.Register();
        }
    }
}
