using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.DataStructures;
using Terraria;
using Terraria.ModLoader;
using ThoriumMod.Empowerments;
using ThoriumMod;

namespace InfernalEclipseAPI.Content.RogueThrower.StealthFixes
{
    [ExtendsFromMod("ThoriumMod")]
    public class StealthConsumptionFixes : GlobalProjectile
    {
        public override bool InstancePerEntity => true;

        private const int DamageLocalAIIndex = 1;
        private bool stealthConsumed = false;

        private bool hasHit = false;

        public override void OnSpawn(Projectile projectile, IEntitySource source)
        {
            Mod thorium = ModLoader.GetMod("ThoriumMod");
            Mod calamity = ModLoader.GetMod("CalamityMod");

            if (thorium == null || calamity == null)
                return;

            int gelGloveProID = thorium.Find<ModProjectile>("GelGlovePro")?.Type ?? -1;
            int cosmicDaggerID = thorium.Find<ModProjectile>("YakaArrowPro")?.Type ?? -1;

            if (projectile.type == gelGloveProID || projectile.type == cosmicDaggerID)
            {
                Player owner = Main.player[projectile.owner];

                // Consume stealth immediately on spawn (after storing damage)
                calamity.Call("ConsumeStealth", owner, 0.2f);
                stealthConsumed = true;
            }
        }

        public override void ModifyHitNPC(Projectile projectile, NPC target, ref NPC.HitModifiers modifiers)
        {
            Mod thorium = ModLoader.GetMod("ThoriumMod");
            if (thorium == null)
                return;

            int gelGloveProID = thorium.Find<ModProjectile>("GelGlovePro")?.Type ?? -1;
            if (projectile.type == gelGloveProID)
            {
                ref StatModifier local = ref modifiers.SourceDamage;
                local += 0.004166667f * projectile.localAI[0];
            }
        }

        //EXHAUSTION REMOVAL
        //GEL GLOVE: SETS EXHAUSTION TO 0 AFTER USING WEAPON... an exploitable band-aid fix but it wont matter in the future.
        public override void AI(Projectile projectile)
        {
            base.AI(projectile);

            Mod thorium = ModLoader.GetMod("ThoriumMod");
            if (projectile.type == thorium.Find<ModProjectile>("GelGlovePro")?.Type)
            {
                Player player = Main.player[projectile.owner];
                var thoriumPlayer = player.GetModPlayer<ThoriumPlayer>();
                thoriumPlayer.throwerExhaustion = 0f;
            }
        }

        //COSMIC DAGGER EXHAUSTION ACCESSORY FIX
        public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
        {
            Player player = Main.player[projectile.owner];
            Mod thorium = ModLoader.GetMod("ThoriumMod");

            if (thorium == null)
                return;

            int yakaArrowID = thorium.Find<ModProjectile>("YakaArrowPro")?.Type ?? -1;

            if (projectile.type == yakaArrowID)
            {
                // Reset projectile damage on first hit
                if (!hasHit)
                {
                    hasHit = true;

                    Item heldItem = player.HeldItem;

                    // Get damage modifier for the item's damage type (melee, ranged, rogue, etc.)
                    StatModifier damageMod = player.GetTotalDamage(heldItem.DamageType);

                    float finalDamage = damageMod.ApplyTo(heldItem.damage);
                    projectile.damage = (int)Math.Round(finalDamage);
                }

                // Clear exhaustion on accessory condition
                var modPlayer = player.GetModPlayer<RogueThrowerPlayer>();
                if (modPlayer.HasExhaustionClearingAccessoryEquipped())
                {
                    var thoriumPlayer = player.GetModPlayer<ThoriumPlayer>();
                    thoriumPlayer.throwerExhaustion = 0f;
                }
            }
        }
    }
}
