using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using PenumbraMod.Content.Items.Placeable;

namespace PenumbraMod.Content.Items.Armors
{
	// The AutoloadEquip attribute automatically attaches an equip texture to this item.
	// Providing the EquipType.Body value here will result in TML expecting X_Arms.png, X_Body.png and X_FemaleBody.png sprite-sheet files to be placed next to the item's main texture.
	[AutoloadEquip(EquipType.Body)]
	public class RozeQuartzRobe : ModItem
	{
        public override void Load()
        {
            // The code below runs only if we're not loading on a server
            if (Main.netMode == NetmodeID.Server)
            {
                return;
            }

            // By passing this (the ModItem) into the item parameter we can reference it later in GetEquipSlot with just the item's name
            EquipLoader.AddEquipTexture(Mod, $"{Texture}_{EquipType.Legs}", EquipType.Legs, this);
        }
        public override void SetStaticDefaults() {
			base.SetStaticDefaults();
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}
        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return head.type == ItemID.MagicHat || head.type == ItemID.WizardHat;
        }
        public override void SetDefaults() {
			Item.width = 18; // Width of the item
			Item.height = 18; // Height of the item
			Item.value = Item.sellPrice(gold: 2); // How many coins the item is worth
			Item.rare = ItemRarityID.Blue; // The rarity of the item
			Item.defense = 2; // The amount of defense the item will give when equipped
		}

		public override void UpdateEquip(Player player) {
			player.manaCost -= 0.10f;
			player.statManaMax2 += 60;
		}
        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "8% increased magic critical strike chance";
            player.GetCritChance(DamageClass.Magic) += 0.08f;
        }
        public override void SetMatch(bool male, ref int equipSlot, ref bool robes)
        {
            // By changing the equipSlot to the leg equip texture slot, the leg texture will now be drawn on the player
            // We're changing the leg slot so we set this to true
            robes = true;
            // Here we can get the equip slot by name since we referenced the item when adding the texture
            // You can also cache the equip slot in a variable when you add it so this way you don't have to call GetEquipSlot
            equipSlot = EquipLoader.GetEquipSlot(Mod, Name, EquipType.Legs);
        }
        // Please see Content/ExampleRecipes.cs for a detailed explanation of recipe creation.
        public override void AddRecipes() 
		{
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.Robe);
			recipe.AddIngredient(ModContent.ItemType<RozeQuartz>(), 10);
            recipe.AddTile(TileID.Loom);
            recipe.Register();
        }
	}
}
