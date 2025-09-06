using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using CalamityMod;
using CalamityMod.Buffs.StatBuffs;
using CalamityMod.Items;
using CalamityMod.Items.Materials;
using InfernalEclipseAPI.Content.Items.Weapons.BossRush.Swordofthe14thGlitch;
using InfernumMode.Content.Rarities.InfernumRarities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace InfernalEclipseAPI.Content.Items.Weapons.Melee.SwordoftheCorrupted
{
    public class Swordofthe13thGlitch : ModItem
    {
        public static int UseTime => _13thGltichCombo.MaxUpdates * (int)Math.Round(0.65f * 60f);
        public static int BaseDamage => 175;
        public static float PlayerDashSpeed => 75f;
        public static float PlayerPostHitSpeed => 30f;

        public override void SetStaticDefaults()
        {
            ItemID.Sets.ItemsThatAllowRepeatedRightClick[Type] = true;
        }

        public override void SetDefaults()
        {
            Item.width = 109;
            Item.height = 115;
            Item.damage = BaseDamage;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 11;
            Item.useAnimation = 11;
            Item.useTurn = true;
            Item.DamageType = DamageClass.MeleeNoSpeed;
            Item.UseSound = null;
            Item.knockBack = 8f;
            Item.autoReuse = true;
            Item.noUseGraphic = true;
            Item.channel = true;
            Item.shoot = ModContent.ProjectileType<_13thGltichCombo>();
            Item.shootSpeed = 9f;
            Item.rare = ModContent.RarityType<InfernumRedSparkRarity>();
            if (ModLoader.TryGetMod("NoxusBoss", out Mod noxus) && noxus.TryFind("NamelessDeityRarity", out ModRarity r))
                Item.rare = r.Type;
            Item.value = CalamityGlobalItem.RarityBlueBuyPrice;
        }
        public override bool AltFunctionUse(Player player) => !player.HasBuff<RageMode>();

        public override bool CanUseItem(Player player)
        {
            return player.ownedProjectileCounts[Item.shoot] <= 0;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo src, Vector2 pos, Vector2 vel, int type, int dmg, float kb)
        {
            if (player.ownedProjectileCounts[type] > 0) return false;

            Vector2 v = (Main.MouseWorld - player.Center).SafeNormalize(Vector2.UnitX);
            int p = Projectile.NewProjectile(src, player.Center, v, type, dmg, kb, player.whoAmI);
            if ((uint)p < Main.maxProjectiles)
            {
                var proj = Main.projectile[p];
                if (proj.ModProjectile is _13thGltichCombo h)
                {
                    h.Mode = (player.altFunctionUse == 2) ? _13thGltichCombo.AttackMode.Slashes   // right click
                                                              : _13thGltichCombo.AttackMode.Dash;     // left click
                }

                proj.ai[1] = Math.Sign(v.X == 0f ? player.direction : v.X); // face intended dir
                proj.netUpdate = true;
            }
            return false;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.LunarBar, ModLoader.TryGetMod("ThoriumMod", out Mod thorium) ? 9 : 12);
            if (thorium != null) recipe.AddIngredient(thorium.Find<ModItem>("TerrariumCore"), 3);
            if (ModLoader.TryGetMod("SOTS", out Mod sots)) recipe.AddIngredient(sots.Find<ModItem>("RubyKeystone").Type);
            else recipe.AddIngredient(ItemID.Ruby, 5);
            recipe.AddIngredient<DarkPlasma>(3);
            recipe.AddIngredient<TwistingNether>(3);
            recipe.AddIngredient<RuinousSoul>(3);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.Register();
        }
    }

    public class CorruptedPlayer : ModPlayer
    {
        private const string CalamityModName = "CalamityMod";
        private const string CalamityPlayerFullNameA = "CalamityMod.CalPlayer.CalamityPlayer";
        private const string CalamityPlayerFullNameB = "CalamityMod.CalamityPlayer"; // fallback if namespace changes
        private const string RageField = "rage";
        private const string RageMaxField = "rageMax";
        private const string RageActiveField = "rageModeActive";
        private const string RageBuffNameA = "RageMode";
        private static readonly SoundStyle RageAutoSfx = new("InfernalEclipseAPI/Assets/Sounds/NamelessDeityRageFail");

        // reflection cache
        private static bool _triedInit;
        private static bool _ready;
        private static Mod _calamity;
        private static Type _calPlayerType;
        private static MethodInfo _miGetModPlayerGeneric;
        private static FieldInfo _fiRage, _fiRageMax, _fiRageActive;
        private static int _rageBuffType = -1;

        public override void PostUpdate()
        {
            // Only when holding/using the spin variant
            bool holdingSpinVariant = Player.HeldItem?.type == ModContent.ItemType<Swordofthe13thGlitch>();
            bool usingHoldout = Player.ownedProjectileCounts[ModContent.ProjectileType<_13thGltichCombo>()] > 0 || Player.channel;

            if (!holdingSpinVariant)
                return;

            if (!EnsureCalamityReflection())
                return;

            TryActivateRage(Player);
        }

        private static bool EnsureCalamityReflection()
        {
            if (_ready) return true;
            if (_triedInit) return false;
            _triedInit = true;

            if (!ModLoader.TryGetMod(CalamityModName, out _calamity))
                return false;

            // type
            _calPlayerType =
                _calamity.Code?.GetType(CalamityPlayerFullNameA, false) ??
                _calamity.Code?.GetType(CalamityPlayerFullNameB, false);
            if (_calPlayerType is null)
                return false;

            // Player.GetModPlayer<T>()
            _miGetModPlayerGeneric = typeof(Player)
                .GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .FirstOrDefault(m => m.Name == "GetModPlayer" && m.IsGenericMethodDefinition && m.GetParameters().Length == 0);
            if (_miGetModPlayerGeneric is null)
                return false;

            // fields
            var flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
            _fiRage = _calPlayerType.GetField(RageField, flags);
            _fiRageMax = _calPlayerType.GetField(RageMaxField, flags);
            _fiRageActive = _calPlayerType.GetField(RageActiveField, flags);
            if (_fiRage is null || _fiRageMax is null || _fiRageActive is null)
                return false;

            // buff type
            if (!_calamity.TryFind(RageBuffNameA, out ModBuff rageBuff))
                return false;
            _rageBuffType = rageBuff.Type;

            _ready = true;
            return true;
        }

        private static object GetCalamityPlayer(Player p)
        {
            var mi = _miGetModPlayerGeneric.MakeGenericMethod(_calPlayerType);
            return mi.Invoke(p, null);
        }

        private static void TryActivateRage(Player p)
        {
            var cp = GetCalamityPlayer(p);
            if (cp is null) return;

            float rage = Convert.ToSingle(_fiRage.GetValue(cp));
            float rageMax = Convert.ToSingle(_fiRageMax.GetValue(cp));
            bool isActive = Convert.ToBoolean(_fiRageActive.GetValue(cp));

            // full bar and not already active
            if (rage >= rageMax && !isActive)
            {
                // flip the calamity internal active flag
                _fiRageActive.SetValue(cp, true);

                // add the buff (Calamity drains/handles duration internally)
                // original decompile used 2 ticks; keep the same
                p.AddBuff(_rageBuffType, 2, quiet: true);

                // local SFX only
                if (Main.myPlayer == p.whoAmI)
                    SoundEngine.PlaySound(RageAutoSfx, p.Center);
            }
        }
    }
}
