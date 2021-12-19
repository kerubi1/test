using Dalamud.Game.ClientState.Conditions;
using Dalamud.Game.ClientState.JobGauge.Types;

namespace XIVComboExpandedestPlugin.Combos
{
    internal static class RDM
    {
        public const byte JobID = 35;

        public const uint
            Verthunder = 7505,
            Veraero = 7507,
            Verthunder3 = 25855,
            Veraero3 = 25856,
            Veraero2 = 16525,
            Verthunder2 = 16524,
            Impact = 16526,
            Redoublement = 7516,
            EnchantedRedoublement = 7529,
            Zwerchhau = 7512,
            EnchantedZwerchhau = 7528,
            Moulinet = 7513,
            Riposte = 7504,
            EnchantedRiposte = 7527,
            Scatter = 7509,
            Verstone = 7511,
            Verfire = 7510,
            Jolt = 7503,
            Jolt2 = 7524,
            Verholy = 7526,
            Verflare = 7525,
            Scorch = 16530,
            Resolution = 25858;

        public static class Buffs
        {
            public const ushort
                Swiftcast = 167,
                VerfireReady = 1234,
                VerstoneReady = 1235,
                Acceleration = 1238,
                Dualcast = 1249,
                LostChainspell = 2560;
        }

        public static class Debuffs
        {
            public const ushort Placeholder = 0;
        }

        public static class Levels
        {
            public const byte
                Jolt = 2,
                Verthunder = 4,
                Verthunder2 = 18,
                Veraero2 = 22,
                Veraero = 10,
                Verraise = 64,
                Zwerchhau = 35,
                Redoublement = 50,
                Vercure = 54,
                Jolt2 = 62,
                Impact = 66,
                Verflare = 68,
                Verholy = 70,
                Scorch = 80,
                Resolution = 90;
        }
    }

    internal class RedMageAoECombo : CustomCombo
    {
        protected override CustomComboPreset Preset => CustomComboPreset.RedMageAoECombo;

        protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
        {
            if (actionID == RDM.Veraero2 || actionID == RDM.Verthunder2)
            {
                if (HasEffect(RDM.Buffs.Swiftcast) || HasEffect(RDM.Buffs.Dualcast) || HasEffect(RDM.Buffs.LostChainspell) || HasEffect(RDM.Buffs.Acceleration) || (OriginalHook(RDM.Impact) != RDM.Impact && OriginalHook(RDM.Impact) != RDM.Scatter))
                    return OriginalHook(RDM.Impact);

                return actionID;
            }

            return actionID;
        }
    }

    internal class RedMageMeleeCombo : CustomCombo
    {
        protected override CustomComboPreset Preset => CustomComboPreset.RedMageMeleeCombo;

        protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
        {
            if (actionID == RDM.Redoublement || actionID == RDM.Moulinet)
            {
                var gauge = GetJobGauge<RDMGauge>();

                if (IsEnabled(CustomComboPreset.RedMageMeleeComboPlus) && !IsEnabled(CustomComboPreset.RedMageMeleeComboPlusPlus))
                {
                    if (OriginalHook(RDM.Verthunder2) == RDM.Verflare)
                    {
                        if (IsEnabled(CustomComboPreset.RedMageMeleeComboPlusVerholy) && OriginalHook(RDM.Veraero2) == RDM.Verholy)
                            return RDM.Verholy;
                        return RDM.Verflare;
                    }
                }

                if (IsEnabled(CustomComboPreset.RedMageMeleeComboPlusPlus))
                {
                    if (OriginalHook(RDM.Verthunder2) == RDM.Verflare)
                    {
                        if (gauge.BlackMana >= gauge.WhiteMana && level >= RDM.Levels.Verholy)
                        {
                            if (HasEffect(RDM.Buffs.VerstoneReady) && !HasEffect(RDM.Buffs.VerfireReady) && (gauge.BlackMana - gauge.WhiteMana <= 9))
                                return RDM.Verflare;

                            return RDM.Verholy;
                        }
                        else if (level >= RDM.Levels.Verflare)
                        {
                            if (!HasEffect(RDM.Buffs.VerstoneReady) && HasEffect(RDM.Buffs.VerfireReady) && level >= RDM.Levels.Verholy && (gauge.WhiteMana - gauge.BlackMana <= 9))
                                return RDM.Verholy;

                            return RDM.Verflare;
                        }
                    }
                }

                if (actionID == RDM.Redoublement)
                {
                    if ((lastComboMove == RDM.Riposte || lastComboMove == RDM.EnchantedRiposte) && level >= RDM.Levels.Zwerchhau)
                        return OriginalHook(RDM.Zwerchhau);

                    if (lastComboMove == RDM.Zwerchhau && level >= RDM.Levels.Redoublement)
                        return OriginalHook(RDM.Redoublement);
                }

                if (IsEnabled(CustomComboPreset.RedMageMeleeComboPlus) || IsEnabled(CustomComboPreset.RedMageMeleeComboPlusPlus))
                {
                    if ((lastComboMove == RDM.Verflare || lastComboMove == RDM.Verholy) && level >= RDM.Levels.Scorch)
                        return RDM.Scorch;
                    if (level >= RDM.Levels.Resolution && lastComboMove == RDM.Scorch)
                        return RDM.Resolution;
                }

                if (actionID == RDM.Moulinet)
                    return OriginalHook(RDM.Moulinet);
                return OriginalHook(RDM.Riposte);
            }

            return actionID;
        }
    }

    internal class RedMageVerprocCombo : CustomCombo
    {
        protected override CustomComboPreset Preset => CustomComboPreset.RedMageVerprocCombo;

        protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
        {
            if (actionID == RDM.Verstone || actionID == RDM.Verfire)
            {
                if (level >= RDM.Levels.Resolution && lastComboMove == RDM.Scorch)
                    return RDM.Resolution;

                if (level >= RDM.Levels.Scorch && (lastComboMove == RDM.Verflare || lastComboMove == RDM.Verholy))
                    return RDM.Scorch;

                if (actionID == RDM.Verstone && OriginalHook(RDM.Verthunder2) == RDM.Verflare && level >= RDM.Levels.Verholy)
                    return RDM.Verholy;

                if (actionID == RDM.Verfire && OriginalHook(RDM.Verthunder2) == RDM.Verflare && level >= RDM.Levels.Verflare)
                    return RDM.Verflare;

                if (IsEnabled(CustomComboPreset.RedMageVerprocComboPlus))
                {
                    if (actionID == RDM.Verstone && (HasEffect(RDM.Buffs.Swiftcast) || HasEffect(RDM.Buffs.Dualcast) || HasEffect(RDM.Buffs.LostChainspell) || HasEffect(RDM.Buffs.Acceleration)) && level >= RDM.Levels.Veraero)
                        return OriginalHook(RDM.Veraero);

                    if (actionID == RDM.Verfire && (HasEffect(RDM.Buffs.Swiftcast) || HasEffect(RDM.Buffs.Dualcast) || HasEffect(RDM.Buffs.LostChainspell) || HasEffect(RDM.Buffs.Acceleration)) && level >= RDM.Levels.Verthunder)
                        return OriginalHook(RDM.Verthunder);
                }

                if (IsEnabled(CustomComboPreset.RedMageVerprocOpenerFeatureStone))
                {
                    if (actionID == RDM.Verstone && !HasEffect(RDM.Buffs.VerstoneReady) && !HasCondition(ConditionFlag.InCombat) && level >= RDM.Levels.Veraero)
                        return OriginalHook(RDM.Veraero);
                }

                if (IsEnabled(CustomComboPreset.RedMageVerprocOpenerFeatureFire))
                {
                    if (actionID == RDM.Verfire && !HasEffect(RDM.Buffs.VerfireReady) && !HasCondition(ConditionFlag.InCombat) && level >= RDM.Levels.Verthunder)
                        return OriginalHook(RDM.Verthunder);
                }

                if (actionID == RDM.Verstone && HasEffect(RDM.Buffs.VerstoneReady))
                    return RDM.Verstone;

                if (actionID == RDM.Verfire && HasEffect(RDM.Buffs.VerfireReady))
                    return RDM.Verfire;

                return OriginalHook(RDM.Jolt2);
            }

            return actionID;
        }

        internal class RedMageVeraeroVerThunderScorchFeature : CustomCombo
        {
            protected override CustomComboPreset Preset => CustomComboPreset.RedMageVeraeroVerThunderScorchFeature;

            protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
            {
                if (level >= RDM.Levels.Resolution && lastComboMove == RDM.Scorch)
                    return RDM.Resolution;

                if (level >= RDM.Levels.Scorch && (lastComboMove == RDM.Verflare || lastComboMove == RDM.Verholy))
                    return RDM.Scorch;

                return actionID;
            }
        }
    }

    internal class RedMageSmartcastAoECombo : CustomCombo
    {
        protected override CustomComboPreset Preset => CustomComboPreset.RedMageSmartcastAoEFeature;

        protected override uint[] ActionIDs { get; } = new[] { RDM.Veraero2, RDM.Verthunder2 };

        protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
        {
            if (actionID is RDM.Veraero2 or RDM.Verthunder2)
            {
                bool fastCasting = HasEffect(RDM.Buffs.Swiftcast) || HasEffect(RDM.Buffs.Dualcast) || HasEffect(RDM.Buffs.LostChainspell);

                if (
                    fastCasting
                    || HasEffect(RDM.Buffs.Acceleration)
                    || level < RDM.Levels.Verthunder2)
                    return OriginalHook(RDM.Impact);

                if (level < RDM.Levels.Veraero2)
                    return RDM.Verthunder2;

                RDMGauge gauge = GetJobGauge<RDMGauge>();

                if (gauge.BlackMana > gauge.WhiteMana)
                    return RDM.Veraero2;

                if (gauge.WhiteMana > gauge.BlackMana)
                    return RDM.Verthunder2;
            }

            return actionID;
        }
    }

    internal class RedmageSmartcastSingleCombo : CustomCombo
    {
        protected override CustomComboPreset Preset => CustomComboPreset.RedMageSmartcastSingleFeature;

        protected override uint[] ActionIDs { get; } = new[] { RDM.Veraero, RDM.Verthunder, RDM.Verstone, RDM.Verfire };

        protected override uint Invoke(uint actionID, uint lastComboActionId, float comboTime, byte level)
        {
            const int
                LONG_DELTA = 6,
                PROC_DELTA = 5,
                FINISHER_DELTA = 11,
                IMBALANCE_DIFF_MAX = 30;

            if (actionID is RDM.Veraero or RDM.Verthunder or RDM.Verstone or RDM.Verfire)
            {
                bool verfireUp = HasEffect(RDM.Buffs.VerfireReady);
                bool verstoneUp = HasEffect(RDM.Buffs.VerstoneReady);
                RDMGauge gauge = GetJobGauge<RDMGauge>();
                int black = gauge.BlackMana;
                int white = gauge.WhiteMana;

                if (actionID is RDM.Veraero or RDM.Verthunder)
                {
                    if (level < RDM.Levels.Verthunder)
                        return RDM.Jolt;

                    if (level is < RDM.Levels.Veraero and >= RDM.Levels.Verthunder)
                        return OriginalHook(RDM.Verthunder);

                    // This is for the long opener only, so we're not bothered about fast casting or finishers or anything like that
                    if (black < white)
                        return OriginalHook(RDM.Verthunder);

                    if (white < black)
                        return OriginalHook(RDM.Veraero);

                    return actionID;
                }

                if (actionID is RDM.Verstone or RDM.Verfire)
                {
                    bool fastCasting = HasEffect(RDM.Buffs.Swiftcast) || HasEffect(RDM.Buffs.Dualcast) || HasEffect(RDM.Buffs.LostChainspell);
                    bool accelerated = HasEffect(RDM.Buffs.Acceleration);
                    bool isFinishing1 = gauge.ManaStacks == 3;
                    bool isFinishing2 = comboTime > 0 && lastComboActionId is RDM.Verholy or RDM.Verflare;
                    bool isFinishing3 = comboTime > 0 && lastComboActionId is RDM.Scorch;
                    bool canFinishWhite = level >= RDM.Levels.Verholy;
                    bool canFinishBlack = level >= RDM.Levels.Verflare;
                    int blackThreshold = white + IMBALANCE_DIFF_MAX;
                    int whiteThreshold = black + IMBALANCE_DIFF_MAX;

                    // If we're ready to Scorch or Resolution, just do that. Nice and simple. Sadly, that's where the simple ends.
                    if (isFinishing3 && level >= RDM.Levels.Resolution)
                        return RDM.Resolution;
                    if (isFinishing2 && level >= RDM.Levels.Scorch)
                        return RDM.Scorch;

                    if (isFinishing1 && canFinishBlack)
                    {
                        if (black >= white && canFinishWhite)
                        {
                            // If we can already Verstone, but we can't Verfire, and Verflare WON'T imbalance us, use Verflare
                            if (verstoneUp && !verfireUp && (black + FINISHER_DELTA <= blackThreshold))
                                return RDM.Verflare;

                            return RDM.Verholy;
                        }

                        // If we can already Verfire, but we can't Verstone, and we can use Verholy, and it WON'T imbalance us, use Verholy
                        if (verfireUp && !verstoneUp && canFinishWhite && (white + FINISHER_DELTA <= whiteThreshold))
                            return RDM.Verholy;

                        return RDM.Verflare;
                    }

                    if (fastCasting || accelerated)
                    {
                        if (level is < RDM.Levels.Veraero and >= RDM.Levels.Verthunder)
                            return RDM.Verthunder;

                        if (verfireUp == verstoneUp)
                        {
                            // Either both procs are already up or neither is - use whatever gives us the mana we need
                            if (black < white)
                                return OriginalHook(RDM.Verthunder);

                            if (white < black)
                                return OriginalHook(RDM.Veraero);

                            // If mana levels are equal, prioritise the colour that the original button was
                            return actionID is RDM.Verstone
                                ? OriginalHook(RDM.Veraero)
                                : OriginalHook(RDM.Verthunder);
                        }

                        if (verfireUp)
                        {
                            // If Veraero is feasible, use it
                            if (white + LONG_DELTA <= whiteThreshold)
                                return OriginalHook(RDM.Veraero);

                            return OriginalHook(RDM.Verthunder);
                        }

                        if (verstoneUp)
                        {
                            // If Verthunder is feasible, use it
                            if (black + LONG_DELTA <= blackThreshold)
                                return OriginalHook(RDM.Verthunder);

                            return OriginalHook(RDM.Veraero);
                        }
                    }

                    if (verfireUp && verstoneUp)
                    {
                        // Decide by mana levels
                        if (black < white)
                            return RDM.Verfire;

                        if (white < black)
                            return RDM.Verstone;

                        // If mana levels are equal, prioritise the original button
                        return actionID;
                    }

                    // Only use Verfire if it won't imbalance us
                    if (verfireUp && black + PROC_DELTA <= blackThreshold)
                        return RDM.Verfire;

                    // Only use Verstone if it won't imbalance us
                    if (verstoneUp && white + PROC_DELTA <= whiteThreshold)
                        return RDM.Verstone;

                    // If neither's up or the one that is would imbalance us, just use Jolt
                    return OriginalHook(RDM.Jolt2);
                }
            }

            return actionID;
        }
    }
}
