using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace PenumbraMod.Content.Items.Consumables
{
	public class MysticalStrawberry : ModItem
	{
		public override void SetStaticDefaults() {
			// DisplayName.SetDefault("Mystical Strawberry");
			// Using references to language keys allow the tooltip to be easily translated
			// Listed below are some keys that you may find useful for making a food item
			// MinorStats, MediumStats, MajorStats, TipsyStats
			// These correspond to the WellFed, WellFed2, WellFed3, and Tipsy buffs respectively.
			// Make sure to match the tooltip with the buff you assign in SetDefaults
			// Tooltip.SetDefault("{$CommonItemTooltip.MediumStats}\n'An Mystic Strawberry that grown up in a Mountain...'");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 5;

			// This is to show the correct frame in the inventory
			// The MaxValue argument is for the animation speed, we want it to be stuck on frame 1
			// Setting it to max value will cause it to take 414 days to reach the next frame
			// No one is going to have game open that long so this is fine
			// The second argument is the number of frames, which is 3
			// The first frame is the inventory texture, the second frame is the holding texture,
			// and the third frame is the placed texture
			Main.RegisterItemAnimation(Type, new DrawAnimationVertical(int.MaxValue, 3));

			// This allows you to change the color of the crumbs that are created when you eat.
			// The numbers are RGB (Red, Green, and Blue) values which range from 0 to 255.
			// Most foods have 3 crumb colors, but you can use more or less if you desire.
			// Depending on if you are making solid or liquid food switch out FoodParticleColors
			// with DrinkParticleColors. The difference is that food particles fly outwards
			// whereas drink particles fall straight down and are slightly transparent
			ItemID.Sets.FoodParticleColors[Item.type] = new Color[3] {
				new Color(137, 32, 32),
				new Color(148, 28, 28),
				new Color(190, 20, 20)
			};

			ItemID.Sets.IsFood[Type] = true; 
		}

		public override void SetDefaults() {
			// This code matches the ApplePie code.

			// DefaultToFood sets all of the food related item defaults such as the buff type, buff duration, use sound, and animation time.
			Item.DefaultToFood(22, 22, BuffID.WellFed2, 36000); // 57600 is 16 minutes: 16 * 60 * 60
			
			Item.rare = ItemRarityID.Blue;
		}

		
		
	}
}