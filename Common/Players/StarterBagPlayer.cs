using PenumbraMod.Content.Items.Consumables;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PenumbraMod.Common.Players
{
	public class StarterBagPlayer : ModPlayer
	{
		
		public override IEnumerable<Item> AddStartingItems(bool mediumCoreDeath) {
			if (mediumCoreDeath) {
				return new[] {
					new Item(ModContent.ItemType<StarterBag>())
				};
			}

			return new[] {
				new Item(ModContent.ItemType<StarterBag>()),
				
			};
		}

	}
}
