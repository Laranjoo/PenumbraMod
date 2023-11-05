using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace PenumbraMod.Content.Items.Accessories
{
    [AutoloadEquip(EquipType.Shoes)]
    public class ExoSkeleton : ModItem
    {
        public override void SetStaticDefaults()
        {
            /* Tooltip.SetDefault("Both Frog, Ninja gears, and Mechanical glove effects" + 
				"\n12% increased melee damage" + 
				"\n6% increased movement speed" + 
				"\n8% increased damage." +
				"\nThe gear need 30 seconds to load a special mechanism" +
				"\nIt gives you holy protection for 10 seconds, granting you the ability to dodge the next attack" + 
				"\n''You feel like a real ninja!''"); */

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 30; // Width of the item
            Item.height = 28; // Height of the item
            Item.value = Item.sellPrice(gold: 22); // How many coins the item is worth
            Item.rare = ItemRarityID.Purple; // The rarity of the item
            Item.accessory = true;
        }
        int ti;
        public override void UpdateEquip(Player player)
        {
            ti++;
            player.moveSpeed += 0.06f;
            player.GetDamage(DamageClass.Melee) += 0.12f;
            player.GetAttackSpeed(DamageClass.Melee) += 0.12f;
            player.autoReuseGlove = true;
            player.meleeScaleGlove = true;
            player.jumpSpeedBoost += 2.4f;
            player.extraFall += 15;
            player.autoJump = true;
            player.accFlipper = true;
            player.spikedBoots = 2;
            player.blackBelt = true;
            player.dashType = 1;
            player.GetDamage(DamageClass.Generic) += 0.08f;
            if (ti == 1800)
            {
                player.AddBuff(BuffID.ShadowDodge, 600);
            }
            if (ti == 2400)
            {
                ti = 0;
            }
        }
        // Please see Content/ExampleRecipes.cs for a detailed explanation of recipe creation.
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.MasterNinjaGear, 1);
            recipe.AddIngredient(ItemID.FrogGear, 1);
            recipe.AddIngredient(ItemID.MechanicalGlove, 1);
            recipe.AddIngredient(ItemID.LunarBar, 8);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.Register();
        }
    }
}
