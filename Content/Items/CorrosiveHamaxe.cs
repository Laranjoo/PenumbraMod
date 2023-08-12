using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PenumbraMod.Content.Items
{
    public class CorrosiveHamaxe : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 24;
            Item.DamageType = DamageClass.Melee;
            Item.width = 40;
            Item.height = 40;
            Item.useTime = 12;
            Item.useAnimation = 12;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 6;
            Item.value = 13000;
            Item.rare = ItemRarityID.Lime;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true; // Automatically re-swing/re-use this item after its swinging animation is over.

            Item.axe = 30; // How much axe power the weapon has, note that the axe power displayed in-game is this value multiplied by 5
            Item.hammer = 100; // How much hammer power the weapon has
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<CorrosiveShard>(), 18)
                 .AddIngredient(ModContent.ItemType<CorrodedPlating>(), 12)
               .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}
