using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.Localization;
using Terraria;
using Terraria.ModLoader;
using ThoriumMod.Core.Handlers.WhisperingSetQuotesHandler;
using ThoriumMod.Items.BossForgottenOne;
using CalamityMod.Items.Weapons.DraedonsArsenal;
using CalamityMod.NPCs.DesertScourge;
using ThoriumMod.Utilities;

namespace InfernalEclipseAPI.Common.GlobalNPCs
{
    [ExtendsFromMod("ThoriumMod")]
    public class WhisperingSetCrossmodQuotes : GlobalNPC
    {
        public override bool IsLoadingEnabled(Mod mod)
        {
            return false;
        }

        private static int TypeRunning;

        private static Dictionary<int, WhisperingSetQuote> Quotes { get; set; }

        private static Dictionary<int, WhisperingSetQuote> FallbackQuotes { get; set; }

        private static LocalizedText GetQuoteText(string name)
        {
            string str = "SetBonusQuotes.";
            return ILocalizedModTypeExtensions.GetLocalization(ModContent.GetInstance<WhisperingHood>(), str + name, null);
        }

        private static LocalizedText GetFallbackQuoteText(string name)
        {
            string str = "SetBonusFallbackQuotes.";
            return ILocalizedModTypeExtensions.GetLocalization(ModContent.GetInstance<WhisperingHood>(), str + name, null);
        }

        private static void RegisterQuote(Func<NPC, bool> predicate, LocalizedText text, bool fallback = false)
        {
            WhisperingSetQuote whisperingSetQuote = new WhisperingSetQuote(predicate, text);
            if (fallback)
                FallbackQuotes[TypeRunning++] = whisperingSetQuote;
            else
                Quotes[TypeRunning++] = whisperingSetQuote;
        }

        public override void SetStaticDefaults()
        {
            Quotes = new Dictionary<int, WhisperingSetQuote>();
            RegisterQuote((npc =>
            {
                if (!npc.boss)
                    return false;
                return npc.type == ModContent.NPCType<DesertScourgeBody>() || npc.type == ModContent.NPCType<DesertScourgeHead>() || npc.type == ModContent.NPCType<DesertScourgeTail>();
            }), GetQuoteText("DesertScourage"));
            TypeRunning = 0;
            FallbackQuotes = new Dictionary<int, WhisperingSetQuote>();
            RegisterQuote(null, GetFallbackQuoteText("0"), true);
            RegisterQuote(null, GetFallbackQuoteText("1"), true);
            TypeRunning = 0;
        }

        public override void Unload()
        {
            Quotes = null;
            FallbackQuotes = null;
            TypeRunning = 0;
        }

        public override void OnKill(NPC npc)
        {
            for (int index = 0; index < npc.playerInteraction.Length; ++index)
            {
                if (npc.playerInteraction[index])
                {
                    Player player = Main.player[index];
                    if (player.active && !player.dead && player.GetThoriumPlayer().whisperingSet)
                    {
                        (int, bool)? nullable = new (int, bool)?();
                        foreach (KeyValuePair<int, WhisperingSetQuote> quote in Quotes)
                        {
                            if (quote.Value.IsMet(npc))
                            {
                                nullable = new (int, bool)?((quote.Key, false));
                                break;
                            }
                        }
                        if (nullable.HasValue)
                        {
                            player.GetThoriumPlayer().whisperingSet = false;
                            int type = nullable.Value.Item1;
                            bool fallback = nullable.Value.Item2;
                            if (TryGetQuote(nullable.Value.Item1, nullable.Value.Item2, out WhisperingSetQuote _))
                                WhisperingSetQuotesGlobalNPC.Create(player, type, fallback);
                        }
                    }
                }
            }
        }

        private static bool TryGetQuote(int type, bool fallback, out WhisperingSetQuote quote)
        {
            return !fallback ? Quotes.TryGetValue(type, out quote) : FallbackQuotes.TryGetValue(type, out quote);
        }
    }
}
