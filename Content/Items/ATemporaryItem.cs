using Microsoft.Xna.Framework;
using PenumbraMod.Content;
using PenumbraMod.Content.DamageClasses;
using PenumbraMod.Content.Rarities;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace PenumbraMod.Content.Items
{
    public class ATemporaryItem : ModItem
    {
        public override string Texture => "PenumbraMod/EMPTY";
        public override void SetDefaults()
        {
            Item.useTime = 10;
            Item.useAnimation = 10;
            Item.useStyle = 1;
            Item.knockBack = 6;
            Item.value = 1000;
            Item.UseSound = SoundID.Item1;
            Item.consumable = true;
            Item.maxStack = 999;
            Item.shoot = ModContent.ProjectileType<EMPTY>();
            Item.shootSpeed = 1f;
            Item.DamageType = ModContent.GetInstance<ReaperClass>();
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            player.GetModPlayer<ReaperClassDPlayer>().ReaperEnergy = 10000;
            player.AddBuff(ModContent.BuffType<TemporaryBuff>(), 10);
            return true;
        }
        public override bool ConsumeItem(Player player)
        {
            player.GetModPlayer<ReaperClassDPlayer>().ReaperEnergy = 10000;
            player.AddBuff(ModContent.BuffType<TemporaryBuff>(), 10);
            return true;
        }
    }
    public class TemporaryBuff : ModBuff
    {
        public override string Texture => "PenumbraMod/EMPTY";
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Gold Speed"); // Buff display name
            /* Description.SetDefault("You feel like the scythe is light"
                + "\nScythe speed increased by 70%"); */ // Buff description
            Main.buffNoTimeDisplay[Type] = true;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<ReaperClassDPlayer>().ReaperEnergy = 10000;
        }
    }
}