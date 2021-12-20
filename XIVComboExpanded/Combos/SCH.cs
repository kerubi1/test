using Dalamud.Game.ClientState.JobGauge.Types;

namespace XIVComboExpandedestPlugin.Combos
{
    internal static class SCH
    {
        public const byte ClassID = 15;
        public const byte JobID = 28;

        public const uint
            FeyBless = 16543,
            Consolation = 16546,
            EnergyDrain = 167,
            Aetherflow = 166,
<<<<<<< HEAD
            Ruin1 = 163,
            Broil1 = 3584,
            Broil2 = 7435,
            Broil3 = 16541,
            Broil4 = 25865,
            Bio1 = 17864,
            Bio2 = 17865,
            Biolysis = 16540;
=======
            Indomitability = 3583,
            Lustrate = 189,
            Excogitation = 7434,
            SacredSoil = 188,
            SummonEos = 17215,
            SummonSelene = 17216,
            WhisperingDawn = 16537,
            FeyIllumination = 16538,
            Dissipation = 3587,
            Aetherpact = 7437,
            SummonSeraph = 16545;
>>>>>>> c00e78a36ffad122d1a0d4aebc18167cf87e99c2

        public static class Buffs
        {
            public const ushort
                Recitation = 1896;
        }

        public static class Debuffs
        {
            public const ushort
                Bio1 = 179,
                Bio2 = 189,
                Biolysis = 1895;
        }

        public static class Levels
        {
            public const byte Placeholder = 0;
        }
    }

    internal class ScholarFairyFeature : CustomCombo
    {
        protected override CustomComboPreset Preset => CustomComboPreset.ScholarFairyFeature;

        protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
        {
            var gauge = GetJobGauge<SCHGauge>();
            if (!Service.BuddyList.PetBuddyPresent && gauge.SeraphTimer == 0)
                return IsEnabled(CustomComboPreset.ScholarSeleneOption) ? SCH.SummonSelene : SCH.SummonEos;

            return actionID;
        }
    }

    internal class ScholarSeraphConsolationFeature : CustomCombo
    {
        protected override CustomComboPreset Preset => CustomComboPreset.ScholarSeraphConsolationFeature;

        protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
        {
            if (actionID == SCH.FeyBless)
            {
                var gauge = GetJobGauge<SCHGauge>();
                if (gauge.SeraphTimer > 0)
                    return SCH.Consolation;
            }

            return actionID;
        }
    }

    internal class SCHDotMainComboFeature : CustomCombo
    {
        protected override CustomComboPreset Preset => CustomComboPreset.SCHDotMainComboFeature;

        protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
        {
            if (actionID == SCH.Broil4 || actionID == SCH.Broil3 || actionID == SCH.Broil2 || actionID == SCH.Broil1 || actionID == SCH.Ruin1)
            {
                var incombat = HasCondition(Dalamud.Game.ClientState.Conditions.ConditionFlag.InCombat);
                var biolysisDebuff = TargetFindOwnEffect(SCH.Debuffs.Biolysis);
                var bio2Debuff = TargetFindOwnEffect(SCH.Debuffs.Bio2);
                var bio1Debuff = TargetFindOwnEffect(SCH.Debuffs.Bio1);
                var broil4 = GetCooldown(SCH.Broil4);

                if (IsEnabled(CustomComboPreset.SCHDotMainComboFeature) && level >= 72)
                {
                    if ((!TargetHasEffect(SCH.Debuffs.Biolysis) && incombat && level >= 72) || (biolysisDebuff.RemainingTime < 3 && incombat && level >= 72))
                        return SCH.Biolysis;
                }

                if (IsEnabled(CustomComboPreset.SCHDotMainComboFeature) && level >= 26 && level <= 71)
                {
                    if ((!TargetHasEffect(SCH.Debuffs.Bio2) && incombat && level >= 26 && level <= 71) || (bio2Debuff.RemainingTime < 3 && incombat && level >= 26 && level <= 71))
                        return SCH.Bio2;
                }

                if (IsEnabled(CustomComboPreset.SCHDotMainComboFeature) && level >= 2 && level <= 25)
                {
                    if ((!TargetHasEffect(SCH.Debuffs.Bio1) && incombat && level >= 2 && level <= 25) || (bio1Debuff.RemainingTime < 3 && incombat && level >= 2 && level <= 25))
                        return SCH.Bio1;
                }
            }

            if (IsEnabled(CustomComboPreset.ScholarEverythingFeature))
            {
                if (HasEffect(SCH.Buffs.Recitation) && (actionID == SCH.Indomitability || actionID == SCH.Excogitation))
                    return actionID;
                var gauge = GetJobGauge<SCHGauge>();
                if (gauge.Aetherflow == 0)
                    return SCH.Aetherflow;
            }

            return actionID;
        }
    }
}
