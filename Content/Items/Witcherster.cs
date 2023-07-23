using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using PenumbraMod.Content;

namespace PenumbraMod.Content.Items
{
	public class Witcherster : ModItem
	{
		public override void SetStaticDefaults()
		{
            // DisplayName.SetDefault("Witcherster"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
            /* Tooltip.SetDefault("This big gun its terrifying!"
				+ "\nShoots an slow Greenfire Skull that speeds up in the time, inflicting poison on enemies."); */
			
		}

		public override void SetDefaults()
		{
			Item.damage = 40;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 128;
			Item.height = 42;
			Item.useTime = 36;
			Item.useAnimation = 36;
			Item.useStyle = 5;
			Item.knockBack = 6;
			Item.value = 2500;
			Item.rare = 3;
			Item.UseSound = SoundID.Item74;
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<GreenfireSkull>();
			Item.shootSpeed = 2f;
			Item.noMelee = true;
			Item.crit = 15;
		}

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-15f, 1f);
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            Vector2 muzzleOffset = Vector2.Normalize(velocity) * 127f;

            if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
            {
                position += muzzleOffset;
            }
        }

    }
}