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
            Stone = 119,
            Stone2 = 127,
            Stone3 = 3568,
            Stone4 = 7431,
            Glare = 16533,
            Glare3 = 25859,
            Aero = 121,
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
            Dia = 2035;
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
                Aero = 4,
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

    internal class WhiteMageDotFeature : CustomCombo
    {
        protected override CustomComboPreset Preset => CustomComboPreset.WhiteMageDotFeature;
        protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
        {
            if (actionID == WAR.StormsPath)
            {
                var heavyswingCD = GetCooldown(WAR.HeavySwing);
                var upheavalCD = GetCooldown(WAR.Upheaval);
                var innerreleaseCD = GetCooldown(WAR.InnerRelease);
                var beserkCD = GetCooldown(WAR.Berserk);
                var stormseyeBuff = FindEffectAny(WAR.Buffs.SurgingTempest);
                var innerReleaseBuff = HasEffect(WAR.Buffs.InnerRelease);
                if (comboTime > 0)
                {
                    var gauge = GetJobGauge<WARGauge>().BeastGauge;
                    if (lastComboMove == WAR.Maim && level >= 50 && !HasEffectAny(WAR.Buffs.SurgingTempest))
                        return WAR.StormsEye;
                    if (lastComboMove == WAR.HeavySwing && level >= WAR.Levels.Maim)
                        return WAR.Maim;
                    if (lastComboMove == WAR.Maim && level >= WAR.Levels.StormsPath)
                    {
                        if (stormseyeBuff.RemainingTime < 10 && IsEnabled(CustomComboPreset.WarriorStormsEyeCombo) && level >= 50)
                            return WAR.StormsEye;
                        return WAR.StormsPath;
                    }
                }

                return WAR.HeavySwing;
            }

            return actionID;
        }
    }

}
