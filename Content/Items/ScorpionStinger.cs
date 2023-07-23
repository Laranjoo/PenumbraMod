using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace PenumbraMod.Content.Items
{
	public class ScorpionStinger : ModItem
	{
		public override void SetDefaults() {
			// This method quickly sets the whip's properties.
			// Mouse over to see its parameters.
			Item.DefaultToWhip(ModContent.ProjectileType<ScorpionStingerProj>(), 45, 4, 4); 
			Item.rare = ItemRarityID.LightRed;
			Item.channel = true;
		}

		// Please see Content/ExampleRecipes.cs for a detailed explanation of recipe creation.
		public override void AddRecipes() {
			CreateRecipe()
				.AddIngredient<ScorpionShell>(12)
				.AddTile(TileID.Anvils)
				.Register();
		}
		
		// Makes the whip receive melee prefixes
		public override bool MeleePrefix() {
			return true;
		}
	}
}
