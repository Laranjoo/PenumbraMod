using PenumbraMod.Content.Items.Placeable;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PenumbraMod.Content.Items
{
    public class InfectedPickaxe : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 14;
            Item.DamageType = DamageClass.Melee;
            Item.width = 60;
            Item.height = 60;
            Item.useTime = 10;
            Item.useAnimation = 10;
            Item.useStyle = 1;
            Item.knockBack = 4;
            Item.value = 19000;
            Item.rare = 4;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.pick = 190;
            Item.tileBoost += 1;
            Item.crit = 7;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.NightmarePickaxe, 1);
            recipe.AddIngredient(ModContent.ItemType<InfectedBar>(), 15);
            recipe.AddIngredient(ItemID.ShadowScale, 10);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}