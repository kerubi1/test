using Dalamud.Game.ClientState.JobGauge.Types;

namespace XIVComboExpandedestPlugin.Combos
{
    internal static class SGE
    {
        public const byte JobID = 40;

        public const uint
            Diagnosis = 24284,
            Holos = 24310,
            Ixochole = 24299,
            Taurochole = 24303,
            Druochole = 24296,
            Egeiro = 24287,
            Kardia = 24285,
            Soteria = 24294,
            Toxikon = 24304,
            Phlegma = 24289,
            Phlegmara = 24307,
            Phlegmaga = 24313,
            Dyskrasia = 24297,
            Dosis1 = 24283,
            Dosis2 = 24306,
            Dosis3 = 24312,
            Eukrasia = 24290,
            EukrasianDosis1 = 24293,
            EukrasianDosis2 = 24308,
            EukrasianDosis3 = 24314;

        public static class Buffs
        {
            public const ushort
                Kardia = 2604,
                Eukrasia = 2606;
        }

        public static class Debuffs
        {
            public const ushort
                EukrasianDosis1 = 2614,
                EukrasianDosis2 = 2615,
                EukrasianDosis3 = 2616;
        }

        public static class Levels
        {
            public const ushort
                Dosis = 1,
                Prognosis = 10,
                Soteria = 35,
                Druochole = 45,
                Kerachole = 50,
                Taurochole = 62,
                Ixochole = 52,
                Dosis2 = 72,
                Holos = 76,
                Rizomata = 74,
                Dosis3 = 82;
        }
    }

    internal class SageKardiaFeature : CustomCombo
    {
        protected override CustomComboPreset Preset => CustomComboPreset.SageKardiaFeature;

        protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
        {
            if (actionID == SGE.Soteria)
            {
                if (HasEffect(SGE.Buffs.Kardia) && IsActionOffCooldown(SGE.Soteria))
                    return SGE.Soteria;
                return SGE.Kardia;
            }

            return actionID;
        }
    }

    internal class SagePhlegmaToxicBalls : CustomCombo
    {
        protected override CustomComboPreset Preset => CustomComboPreset.SagePhlegmaToxicBalls;

        protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
        {
            var gauge = GetJobGauge<SGEGauge>();

            if (level >= SGE.Levels.Dosis3)
                if (GetCooldown(SGE.Phlegmaga).CooldownRemaining > 45 && gauge.Addersting > 0)
                    return OriginalHook(SGE.Toxikon);

            if (level >= SGE.Levels.Dosis2)
                if (GetCooldown(SGE.Phlegmara).CooldownRemaining > 45 && gauge.Addersting > 0)
                    return OriginalHook(SGE.Toxikon);

            if (GetCooldown(SGE.Phlegma).CooldownRemaining > 45 && gauge.Addersting > 0)
                return OriginalHook(SGE.Toxikon);

            return actionID;
        }
    }

    internal class SagePhlegmaBalls : CustomCombo
    {
        protected override CustomComboPreset Preset => CustomComboPreset.SagePhlegmaBalls;

        protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
        {
            if (level >= SGE.Levels.Dosis3)
                if (GetCooldown(SGE.Phlegmaga).CooldownRemaining > 45)
                    return OriginalHook(SGE.Dyskrasia);

            if (level >= SGE.Levels.Dosis2)
                if (GetCooldown(SGE.Phlegmara).CooldownRemaining > 45)
                    return OriginalHook(SGE.Dyskrasia);

            if (GetCooldown(SGE.Phlegma).CooldownRemaining > 45)
                return OriginalHook(SGE.Dyskrasia);

            return actionID;
        }
    }

    internal class SageTauroDruoFeature : CustomCombo
    {
        protected override CustomComboPreset Preset => CustomComboPreset.SageTauroDruoFeature;

        protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
        {
            if (!IsActionOffCooldown(SGE.Taurochole) || level < SGE.Levels.Taurochole)
                return SGE.Druochole;

            return actionID;
        }
    }

    internal class SGEDotMainComboFeature : CustomCombo
    {
        protected override CustomComboPreset Preset => CustomComboPreset.SGEDotMainComboFeature;

        protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
        {
            if (actionID == SGE.Dosis3 || actionID == SGE.Dosis2 || actionID == SGE.Dosis1)
            {
                var incombat = HasCondition(Dalamud.Game.ClientState.Conditions.ConditionFlag.InCombat);
                var edosis3debuff = TargetFindOwnEffect(SGE.Debuffs.EukrasianDosis3);
                var edosis2debuff = TargetFindOwnEffect(SGE.Debuffs.EukrasianDosis2);
                var edosis1debuff = TargetFindOwnEffect(SGE.Debuffs.EukrasianDosis1);
                var dosis3 = GetCooldown(SGE.Dosis3);

                if (IsEnabled(CustomComboPreset.SGEDotMainComboFeature) && level >= 82)
                {
                    if (HasEffect(SGE.Buffs.Eukrasia))
                        return SGE.EukrasianDosis3;
                    if ((!TargetHasEffect(SGE.Debuffs.EukrasianDosis3) && incombat && level >= 82) || (edosis3debuff.RemainingTime < 4 && incombat && level >= 82))
                        return SGE.Eukrasia;
                }

                if (IsEnabled(CustomComboPreset.SGEDotMainComboFeature) && level >= 72 && level <= 81)
                {
                    if (HasEffect(SGE.Buffs.Eukrasia))
                        return SGE.EukrasianDosis2;
                    if ((!TargetHasEffect(SGE.Debuffs.EukrasianDosis2) && incombat && level >= 72 && level <= 81) || (edosis2debuff.RemainingTime < 4 && incombat && level >= 72 && level <= 81))
                        return SGE.Eukrasia;
                }

                if (IsEnabled(CustomComboPreset.SGEDotMainComboFeature) && level >= 30 && level <= 71)
                {
                    if (HasEffect(SGE.Buffs.Eukrasia))
                        return SGE.EukrasianDosis1;
                    if ((!TargetHasEffect(SGE.Debuffs.EukrasianDosis1) && incombat && level >= 30 && level <= 71) || (edosis1debuff.RemainingTime < 4 && incombat && level >= 30 && level <= 71))
                        return SGE.Eukrasia;
                }
            }

            return actionID;
        }
    }
}
