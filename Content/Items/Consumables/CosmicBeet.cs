using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace PenumbraMod.Content.Items.Consumables
{
	// This file showcases how to create an item that increases the player's maximum health on use.
	// Within your ModPlayer, you need to save/load a count of usages. You also need to sync the data to other players.
	// The overlay used to display the custom life fruit can be found in Common/UI/ResourceDisplay/VanillaLifeOverlay.cs
	internal class CosmicBeet : ModItem
	{
		public static readonly int MaxBeets = 20;
		public static readonly int LifePerBeet = 5;
		public override void SetStaticDefaults() {
			Item.ResearchUnlockCount = 10;
		}

		public override void SetDefaults() {
			Item.CloneDefaults(ItemID.LifeFruit);
		}

		public override bool CanUseItem(Player player) {
			// This check prevents this item from being used before vanilla health upgrades are maxed out.
			return player.ConsumedLifeCrystals == Player.LifeCrystalMax && player.ConsumedLifeFruit == Player.LifeFruitMax;
		}

		public override bool? UseItem(Player player) {
			// Moving the exampleLifeFruits check from CanUseItem to here allows this example fruit to still "be used" like Life Fruit can be
			// when at the max allowed, but it will just play the animation and not affect the player's max life
			if (player.GetModPlayer<CosmicBeetPlayer>().cosmicbeet >= MaxBeets) {
				// Returning null will make the item not be consumed
				return null;
			}

			// This method handles permanently increasing the player's max health and displaying the green heal text
			player.UseHealthMaxIncreasingItem(LifePerBeet);

			// This field tracks how many of the example fruit have been consumed
			player.GetModPlayer<CosmicBeetPlayer>().cosmicbeet++;
			
			return true;
		}
	}
    public class CosmicBeetPlayer : ModPlayer
    {
        public int cosmicbeet;
        public override void ModifyMaxStats(out StatModifier health, out StatModifier mana)
        {
            health = StatModifier.Default;
            health.Base = cosmicbeet * CosmicBeet.LifePerBeet;
            // Alternatively:  health = StatModifier.Default with { Base = exampleLifeFruits * ExampleLifeFruit.LifePerFruit };
            mana = StatModifier.Default;
        }

        public override void SyncPlayer(int toWho, int fromWho, bool newPlayer)
        {
            ModPacket packet = Mod.GetPacket();
            packet.Write((byte)PenumbraMod.MessageType.CosmicBeetSync);
            packet.Write((byte)Player.whoAmI);
            packet.Write((byte)cosmicbeet);
            packet.Send(toWho, fromWho);
        }
        public void ReceivePlayerSync(BinaryReader reader)
        {
            cosmicbeet = reader.ReadByte();
        }

        public override void CopyClientState(ModPlayer targetCopy)
        {
            CosmicBeetPlayer clone = (CosmicBeetPlayer)targetCopy;
            clone.cosmicbeet = cosmicbeet;
        }

        public override void SendClientChanges(ModPlayer clientPlayer)
        {
            CosmicBeetPlayer clone = (CosmicBeetPlayer)clientPlayer;

            if (cosmicbeet != clone.cosmicbeet)
                SyncPlayer(toWho: -1, fromWho: Main.myPlayer, newPlayer: false);
        }

        // NOTE: The tag instance provided here is always empty by default.
        // Read https://github.com/tModLoader/tModLoader/wiki/Saving-and-loading-using-TagCompound to better understand Saving and Loading data.
        public override void SaveData(TagCompound tag)
        {
            tag["cosmicbeet"] = cosmicbeet;
        }

        public override void LoadData(TagCompound tag)
        {
            cosmicbeet = tag.GetInt("cosmicbeet");
        }
    }
}
