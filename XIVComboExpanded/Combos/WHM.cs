using Dalamud.Game.ClientState.JobGauge.Types;

namespace XIVComboExpandedestPlugin.Combos
{
    internal static class WHM
    {
        public const byte ClassID = 6;
        public const byte JobID = 24;

        public const uint
            Cure = 120,
            Medica = 124,
            Cure2 = 135,
            AfflatusSolace = 16531,
            AfflatusRapture = 16534,
            AfflatusMisery = 16535,
            Stone1 = 119,
            Stone2 = 127,
            Stone3 = 3568,
            Stone4 = 7431,
            Glare1 = 16533,
            Glare3 = 25859,
            Aero1 = 121,
            Aero2 = 132,
            Dia = 16532;

        public static class Buffs
        {
            public const ushort Placeholder = 0;
        }

        public static class Debuffs
        {
            public const ushort
            Aero = 143,
            Aero2 = 144,
            Dia = 1871;
        }

        public static class Levels
        {
            public const byte
                Cure2 = 30,
                AfflatusSolace = 52,
                AfflatusRapture = 76,
                Stone = 1,
                Stone2 = 18,
                Stone3 = 54,
                Stone4 = 64,
                Glare = 72,
                Glare3 = 82,
                Aero1 = 4,
                Aero2 = 46,
                Dia = 72;
        }
    }

    internal class WhiteMageSolaceMiseryFeature : CustomCombo
    {
        protected override CustomComboPreset Preset => CustomComboPreset.WhiteMageSolaceMiseryFeature;

        protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
        {
            if (actionID == WHM.AfflatusSolace)
            {
                var gauge = GetJobGauge<WHMGauge>();

                if (gauge.BloodLily == 3)
                    return WHM.AfflatusMisery;
            }

            return actionID;
        }
    }

    internal class WhiteMageRaptureMiseryFeature : CustomCombo
    {
        protected override CustomComboPreset Preset => CustomComboPreset.WhiteMageRaptureMiseryFeature;

        protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
        {
            if (actionID == WHM.AfflatusRapture)
            {
                var gauge = GetJobGauge<WHMGauge>();

                if (gauge.BloodLily == 3)
                    return WHM.AfflatusMisery;
            }

            return actionID;
        }
    }

    internal class WhiteMageCureFeature : CustomCombo
    {
        protected override CustomComboPreset Preset => CustomComboPreset.WhiteMageCureFeature;

        protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
        {
            if (actionID == WHM.Cure2)
            {
                if (level < WHM.Levels.Cure2)
                    return WHM.Cure;
            }

            return actionID;
        }
    }

    internal class WhiteMageAfflatusFeature : CustomCombo
    {
        protected override CustomComboPreset Preset => CustomComboPreset.WhiteMageAfflatusFeature;

        protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
        {
            if (actionID == WHM.Cure2)
            {
                var gauge = GetJobGauge<WHMGauge>();
                if (IsEnabled(CustomComboPreset.WhiteMageSolaceMiseryFeature) && gauge.BloodLily == 3)
                    return WHM.AfflatusMisery;
                if (level >= WHM.Levels.AfflatusSolace && gauge.Lily > 0)
                    return WHM.AfflatusSolace;
            }

            if (actionID == WHM.Medica)
            {
                var gauge = GetJobGauge<WHMGauge>();
                if (IsEnabled(CustomComboPreset.WhiteMageRaptureMiseryFeature) && gauge.BloodLily == 3 && level >= WHM.Levels.AfflatusRapture)
                    return WHM.AfflatusMisery;
                if (level >= WHM.Levels.AfflatusRapture && gauge.Lily > 0)
                    return WHM.AfflatusRapture;
            }

            return actionID;
        }
    }

    internal class WHMDotMainComboFeature : CustomCombo
    {
        protected override CustomComboPreset Preset => CustomComboPreset.WHMDotMainComboFeature;

        protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
        {
            if (actionID == WHM.Glare3 || actionID == WHM.Glare1 || actionID == WHM.Stone1 || actionID == WHM.Stone2 || actionID == WHM.Stone3 || actionID == WHM.Stone4)
            {
                var inCombat = HasCondition(Dalamud.Game.ClientState.Conditions.ConditionFlag.InCombat);
                var diaDebuff = TargetFindOwnEffect(WHM.Debuffs.Dia);
                var aero1Debuff = TargetFindOwnEffect(WHM.Debuffs.Aero);
                var aero2Debuff = TargetFindOwnEffect(WHM.Debuffs.Aero2);
                var glare3 = GetCooldown(WHM.Glare3);

                if (IsEnabled(CustomComboPreset.WHMDotMainComboFeature) && level >= 4 && level <= 45)
                {
                    if ((TargetFindOwnEffect(WHM.Debuffs.Aero) is null && inCombat && level >= 4 && level <= 45) || (aero1Debuff.RemainingTime <= 3 && inCombat && level >= 4 && level <= 45))
                    {
                        return WHM.Aero1;
                    }
                }

                if (IsEnabled(CustomComboPreset.WHMDotMainComboFeature) && level >= 46 && level <= 71)
                {
                    if ((TargetFindOwnEffect(WHM.Debuffs.Aero2) is null && level >= 46 && level <= 71) || (aero2Debuff.RemainingTime <= 3 && inCombat && level >= 46 && level <= 71))
                    {
                        return WHM.Aero2;
                    }
                }

                if (IsEnabled(CustomComboPreset.WHMDotMainComboFeature) && level >= 72)
                {
                    if ((TargetFindOwnEffect(WHM.Debuffs.Dia) is null && inCombat && level >= 72) || (diaDebuff.RemainingTime <= 3 && inCombat && level >= 72))
                    {
                        return WHM.Dia;
                    }
                }
            }

            return actionID;
        }
    }
}
