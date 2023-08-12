using Microsoft.Xna.Framework;
using PenumbraMod.Content.Items.Placeable;
using PenumbraMod.Content.Rarities;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static PenumbraMod.Content.Items.MeltedScythe;

namespace PenumbraMod.Content.Items
{
    public class InfectionClaws : ModItem
    {

        public override void SetDefaults()
        {
            Item.damage = 85;
            Item.DamageType = DamageClass.Melee;
            Item.width = 36;
            Item.height = 36;
            Item.useTime = 14;
            Item.useAnimation = 14;
            Item.useStyle = 1;
            Item.knockBack = 8;
            Item.value = 12400;
            Item.rare = ModContent.RarityType<InfectionClawsRarity>();
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.crit = 24;
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<InfectedBar>(), 15);
            recipe.AddIngredient(ItemID.LightsBane, 1);
            recipe.AddIngredient(ItemID.RottenChunk, 17);
            recipe.AddIngredient(ItemID.WormTooth, 8);
            recipe.AddIngredient(ItemID.SoulofNight, 6);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}