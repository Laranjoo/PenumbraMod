using PenumbraMod.Content.Buffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PenumbraMod.Content.Items
{
    public class TestSword : ModItem
    {
        public override string Texture => "Terraria/Images/Item_" + ItemID.PlatinumBroadsword;
        public override void SetDefaults()
        {
            Item.damage = 5;
            Item.DamageType = DamageClass.Melee;
            Item.width = 54;
            Item.height = 54;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = 1;
            Item.knockBack = 8;
            Item.value = 0;
            Item.rare = 2;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;

        }
        //  public override void MeleeEffects(Player player, Rectangle hitbox)
        //  {
        //   if (Main.rand.NextBool(4))
        //    {

        //        Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, DustID.BlueTorch);

        //     }
        //    }
        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<StunnedNPC>(), 300);
        }
    }
}