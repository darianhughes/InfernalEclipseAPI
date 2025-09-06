using System;
using System.Collections.Generic;
using CalamityMod;
using CalamityMod.Items;
using CalamityMod.Sounds;
using InfernalEclipseAPI.Core.DamageClasses.MythicClass;
using InfernumMode.Content.Rarities.InfernumRarities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace InfernalEclipseAPI.Content.Items.Weapons.Legendary.CelestialIllumination
{
    public class CelestialIlluminationPlayer : ModPlayer
    {
        public int CelestialStarCharge;
        public override void ResetEffects()
        {
            if (Player.HeldItem.type != ModContent.ItemType<CelestialIllumination>())
            {
                CelestialStarCharge = 0;
            }
        }
    }
}