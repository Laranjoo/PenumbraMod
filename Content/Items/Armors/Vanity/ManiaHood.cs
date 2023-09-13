using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace PenumbraMod.Content.Items.Armors.Vanity
{
    // The AutoloadEquip attribute automatically attaches an equip texture to this item.
    // Providing the EquipType.Head value here will result in TML expecting a X_Head.png file to be placed next to the item's main texture.
    [AutoloadEquip(EquipType.Head)]
    public class ManiaHood : ModItem
    {
        public override void SetStaticDefaults()
        {
            ArmorIDs.Head.Sets.DrawHead[Item.headSlot] = false;
        }
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.rare = ItemRarityID.Cyan;
            Item.vanity = true;
        }
    }
}
