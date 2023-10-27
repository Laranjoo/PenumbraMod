using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace PenumbraMod.Content.Items
{
	internal class BloodyHibiscus : ModItem
	{
		public override void SetStaticDefaults() {
			Item.ResearchUnlockCount = 1;
		}

		public override void SetDefaults() {
            Item.width = 36;
            Item.height = 36;
            Item.maxStack = 1;
            Item.useAnimation = 14;
            Item.useTime = 14;
            Item.value = ItemRarityID.Expert;
            Item.rare = ItemRarityID.LightRed;
            Item.consumable = true;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.UseSound = SoundID.Item20;
        }
        public override bool CanUseItem(Player player)
        {
            if (player.GetModPlayer<HibiscusPlayer>().hibiscusConsumed == true)
                return false;
            return true;
        }
        public override bool? UseItem(Player player) {
			player.GetModPlayer<HibiscusPlayer>().hibiscusConsumed = true;
            if (!Main.playerInventory)
            {
                player.controlInv = true;
                player.ToggleInv();
            }          
			return true;
		}
	}
    public class HibiscusPlayer : ModPlayer
    {
        public bool hibiscusConsumed;
   
        public override void SyncPlayer(int toWho, int fromWho, bool newPlayer)
        {
            ModPacket packet = Mod.GetPacket();
            packet.Write((byte)PenumbraMod.MessageType.Hibiscus);
            packet.Write((byte)Player.whoAmI);
            packet.Write(hibiscusConsumed);
            packet.Send(toWho, fromWho);
        }
        public void ReceivePlayerSync(BinaryReader reader)
        {
            hibiscusConsumed = reader.ReadBoolean();
        }

        public override void CopyClientState(ModPlayer targetCopy)
        {
            HibiscusPlayer clone = (HibiscusPlayer)targetCopy;
            clone.hibiscusConsumed = hibiscusConsumed;
        }

        public override void SendClientChanges(ModPlayer clientPlayer)
        {
            HibiscusPlayer clone = (HibiscusPlayer)clientPlayer;

            if (hibiscusConsumed != clone.hibiscusConsumed)
                SyncPlayer(toWho: -1, fromWho: Main.myPlayer, newPlayer: false);
        }

        // NOTE: The tag instance provided here is always empty by default.
        // Read https://github.com/tModLoader/tModLoader/wiki/Saving-and-loading-using-TagCompound to better understand Saving and Loading data.
        public override void SaveData(TagCompound tag)
        {
            tag["hibiscusConsumed"] = hibiscusConsumed;
        }

        public override void LoadData(TagCompound tag)
        {
            hibiscusConsumed = tag.GetBool("hibiscusConsumed");
        }
    }
}
