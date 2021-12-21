using Dalamud.Game.ClientState.JobGauge.Enums;
using Dalamud.Game.ClientState.JobGauge.Types;

namespace XIVComboExpandedestPlugin.Combos
{
    internal static class AST
    {
        public const byte JobID = 33;

        public const uint
            Benefic = 3594,
            Benefic2 = 3610,
            Draw = 3590,
            Balance = 4401,
            Bole = 4404,
            Arrow = 4402,
            Spear = 4403,
            Ewer = 4405,
            Spire = 4406,
            MinorArcana = 7443,
            SleeveDraw = 7448,
            Malefic4 = 16555,
            LucidDreaming = 7562,
            Ascend = 3603,
            Swiftcast = 7561,
            CrownPlay = 25869,
            Astrodyne = 25870,
            FallMalefic = 25871,
            Malefic1 = 3596,
            Malefic2 = 3598,
            Malefic3 = 7442,
            Combust = 3599,
            Play = 17055,
            LordOfCrowns = 7444,
            LadyOfCrown = 7445,
            Combust3 = 16554,
            Combust2 = 3608,
            Combust1 = 3599,
            Helios = 3600,
            AspectedHelios = 3601;

        public static class Buffs
        {
            public const ushort
                LordOfCrownsDrawn = 2054,
                LadyOfCrownsDrawn = 2055,
                Balance = 913,
                Bole = 914,
                Arrow = 915,
                Spear = 916,
                Ewer = 917,
                Spire = 918;
        }

        public static class Debuffs
        {
            public const ushort
                Combust1 = 838,
                Combust2 = 843,
                Combust3 = 1881;
        }

        public static class Levels
        {
            public const byte
                Benefic2 = 26,
                MinorArcana = 50,
                Draw = 30,
                CrownPlay = 70;
        }
    }

    internal class AstrologianCardsOnDrawFeature : CustomCombo
    {
        protected override CustomComboPreset Preset => CustomComboPreset.AstrologianCardsOnDrawFeature;

        protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
        {
            if (actionID == AST.Play)
            {
                var gauge = GetJobGauge<ASTGauge>();
                if (!gauge.ContainsSeal(SealType.NONE) && IsEnabled(CustomComboPreset.AstrologianAstrodynePlayFeature) && (gauge.DrawnCard != CardType.NONE || GetCooldown(AST.Draw).CooldownRemaining > 30))
                    return AST.Astrodyne;

                if (HasEffect(AST.Buffs.Balance) || HasEffect(AST.Buffs.Bole) || HasEffect(AST.Buffs.Arrow) || HasEffect(AST.Buffs.Spear) || HasEffect(AST.Buffs.Ewer) || HasEffect(AST.Buffs.Spire))
                    return OriginalHook(AST.Play);

                return AST.Draw;
            }

            return actionID;
        }
    }

    internal class AstrologianAstrodynePlayFeature : CustomCombo
    {
        protected override CustomComboPreset Preset => CustomComboPreset.AstrologianAstrodynePlayFeature;

        protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
        {
            if (actionID == AST.Play && !IsEnabled(CustomComboPreset.AstrologianCardsOnDrawFeature))
            {
                var gauge = GetJobGauge<ASTGauge>();
                if (!gauge.ContainsSeal(SealType.NONE))
                    return AST.Astrodyne;
            }

            return actionID;
        }
    }

    internal class AstrologianMinorArcanaPlayFeature : CustomCombo
    {
        protected override CustomComboPreset Preset => CustomComboPreset.AstrologianMinorArcanaPlayFeature;

        protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
        {
            if (actionID == AST.MinorArcana)
            {
                if (HasEffect(AST.Buffs.LordOfCrownsDrawn) || HasEffect(AST.Buffs.LadyOfCrownsDrawn))
                    return OriginalHook(AST.CrownPlay);
            }

            return actionID;
        }
    }

    internal class AstrologianBeneficFeature : CustomCombo
    {
        protected override CustomComboPreset Preset => CustomComboPreset.AstrologianBeneficFeature;

        protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
        {
            if (actionID == AST.Benefic2)
            {
                if (level < AST.Levels.Benefic2)
                    return AST.Benefic;
            }

            return actionID;
        }
    }

    internal class ASTDotMainComboFeature : CustomCombo
    {
        protected override CustomComboPreset Preset => CustomComboPreset.ASTDotMainComboFeature;

        protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
        {
            if (actionID == AST.FallMalefic || actionID == AST.Malefic4 || actionID == AST.Malefic3 || actionID == AST.Malefic2 || actionID == AST.Malefic1)
            {
                var incombat = HasCondition(Dalamud.Game.ClientState.Conditions.ConditionFlag.InCombat);
                var combust3Debuff = TargetFindOwnEffect(AST.Debuffs.Combust3);
                var combust2Debuff = TargetFindOwnEffect(AST.Debuffs.Combust2);
                var combust1Debuff = TargetFindOwnEffect(AST.Debuffs.Combust1);
                var fallmalefic = GetCooldown(AST.FallMalefic);

                if (IsEnabled(CustomComboPreset.ASTDotMainComboFeature) && level >= 72)
                {
                    if ((TargetFindOwnEffect(AST.Debuffs.Combust3) is null && incombat && level >= 72) || (combust3Debuff.RemainingTime < 3 && incombat && level >= 72))
                        return AST.Combust3;
                }

                if (IsEnabled(CustomComboPreset.ASTDotMainComboFeature) && level >= 46 && level <= 71)
                {
                    if ((TargetFindOwnEffect(AST.Debuffs.Combust2) is null && level >= 46 && level <= 71) || (combust2Debuff.RemainingTime < 3 && incombat && level >= 46 && level <= 71))
                        return AST.Combust2;
                }

                if (IsEnabled(CustomComboPreset.ASTDotMainComboFeature) && level >= 4 && level <= 45)
                {
                    if ((TargetFindOwnEffect(AST.Debuffs.Combust1) is null && level <= 45) || (combust1Debuff.RemainingTime < 3 && incombat && level >= 4 && level <= 45))
                        return AST.Combust1;
                }
            }

            return actionID;
        }
    }
}
