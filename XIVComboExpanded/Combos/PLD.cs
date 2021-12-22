using Dalamud.Game.ClientState.Statuses;

namespace XIVComboExpandedestPlugin.Combos
{
    internal static class PLD
    {
        public const byte ClassID = 1;
        public const byte JobID = 19;

        public const uint
            FastBlade = 9,
            RiotBlade = 15,
            ShieldBash = 16,
            RageOfHalone = 21,
            CircleOfScorn = 23,
            SpiritsWithin = 29,
            GoringBlade = 3538,
            RoyalAuthority = 3539,
            LowBlow = 7540,
            TotalEclipse = 7381,
            Requiescat = 7383,
            HolySpirit = 7384,
            Prominence = 16457,
            HolyCircle = 16458,
            Confiteor = 16459,
            Expiacion = 25747,
            BladeOfFaith = 25748,
            BladeOfTruth = 25749,
            BladeOfValor = 25750,
            FightOrFlight = 20,
            Atonement = 16460;

        public static class Buffs
        {
            public const ushort
                Requiescat = 1368,
                SwordOath = 1902,
                FightOrFlight = 76;
        }

        public static class Debuffs
        {
            public const ushort
                BladeOfValor = 2721,
                GoringBlade = 725;
        }

        public static class Levels
        {
            public const byte
                RiotBlade = 4,
                RageOfHalone = 26,
                SpiritsWithin = 30,
                Prominence = 40,
                CircleOfScorn = 50,
                GoringBlade = 54,
                RoyalAuthority = 60,
                HolyCircle = 72,
                Atonement = 76,
                Confiteor = 80,
                Expiacion = 86,
                BladeOfFaith = 90,
                BladeOfTruth = 90,
                BladeOfValor = 90;
        }
    }

    internal class PaladinGoringBladeCombo : CustomCombo
    {
        protected override CustomComboPreset Preset => CustomComboPreset.PaladinGoringBladeCombo;

        protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
        {
            if (actionID == PLD.GoringBlade)
            {
                if (comboTime > 0)
                {
                    if (lastComboMove == PLD.FastBlade && level >= PLD.Levels.RiotBlade)
                        return PLD.RiotBlade;

                    if (lastComboMove == PLD.RiotBlade && level >= PLD.Levels.GoringBlade)
                        return PLD.GoringBlade;
                }

                return PLD.FastBlade;
            }

            return actionID;
        }
    }

    internal class PaladinRoyalAuthorityCombo : CustomCombo
    {
        protected override CustomComboPreset Preset => CustomComboPreset.PaladinRoyalAuthorityCombo;

        protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
        {
            if (actionID == PLD.RageOfHalone || actionID == PLD.RoyalAuthority)
            {
                var goringBlade = TargetFindOwnEffect(PLD.Debuffs.GoringBlade);
                var foF = HasEffect(PLD.Buffs.FightOrFlight);
                var valor = TargetFindOwnEffect(PLD.Debuffs.BladeOfValor);
                var requiescat = FindEffect(PLD.Buffs.Requiescat);

                if (IsEnabled(CustomComboPreset.PaladinRequiescatFeature))
                {
                    if (HasEffect(PLD.Buffs.Requiescat) && level >= 64 && !foF)
                    {
                        if ((IsEnabled(CustomComboPreset.PaladinConfiteorFeature) && requiescat.RemainingTime <= 3 && requiescat.RemainingTime > 0 && level >= 80) || requiescat.StackCount == 1 && level >= 80)
                            return PLD.Confiteor;
                        return PLD.HolySpirit;
                    }

                    if (lastComboMove == PLD.Confiteor && level >= 90)
                        return PLD.BladeOfFaith;
                    if (lastComboMove == PLD.BladeOfFaith && level >= 90)
                        return PLD.BladeOfTruth;
                    if (lastComboMove == PLD.BladeOfTruth && level >= 90)
                        return PLD.BladeOfValor;
                }

                if (IsEnabled(CustomComboPreset.PaladinRoyalGoringOption))
                {
                    if ((lastComboMove == PLD.RiotBlade && goringBlade is not null && goringBlade.RemainingTime > 10) || (lastComboMove == PLD.RiotBlade && valor is not null && valor.RemainingTime > 10))
                        return PLD.RoyalAuthority;
                    else
                    if ((lastComboMove == PLD.RiotBlade && goringBlade is null && level >= 54) || (lastComboMove == PLD.RiotBlade && valor is not null && valor.RemainingTime < 5 && level >= 54) || (lastComboMove == PLD.RiotBlade && goringBlade is not null && goringBlade.RemainingTime < 5 && level >= 54))
                        return PLD.GoringBlade;
                }

                if (IsEnabled(CustomComboPreset.PaladinAtonementFeature))
                {
                    if (lastComboMove == PLD.RiotBlade && level >= 60)
                        return PLD.RoyalAuthority;
                }

                if (comboTime > 0)
                {
                    if (lastComboMove == PLD.FastBlade)
                        return PLD.RiotBlade;
                }

                if (IsEnabled(CustomComboPreset.PaladinAtonementFeature))
                {
                    if (level >= PLD.Levels.Atonement && HasEffect(PLD.Buffs.SwordOath))
                        return PLD.Atonement;
                }

                return PLD.FastBlade;
            }

            return actionID;
        }
    }

    internal class PaladinProminenceCombo : CustomCombo
    {
        protected override CustomComboPreset Preset => CustomComboPreset.PaladinProminenceCombo;

        protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
        {
            if (actionID == PLD.Prominence)
            {
                if (IsEnabled(CustomComboPreset.PaladinRequiescatFeature))
                {
                    if (HasEffect(PLD.Buffs.Requiescat) && level >= PLD.Levels.HolyCircle)
                    {
                        var requiescat = FindEffect(PLD.Buffs.Requiescat);

                        if (IsEnabled(CustomComboPreset.PaladinConfiteorFeature) && requiescat.StackCount == 1)
                            return PLD.Confiteor;
                        return PLD.HolyCircle;
                    }
                }

                if (comboTime > 0)
                {
                    if (lastComboMove == PLD.TotalEclipse && level >= PLD.Levels.Prominence)
                        return PLD.Prominence;
                }

                if (IsEnabled(CustomComboPreset.PaladinConfiteorFeature))
                {
                    if (lastComboMove == PLD.Confiteor && level >= 90)
                    {
                        return PLD.BladeOfFaith;
                    }

                    if (lastComboMove == PLD.BladeOfFaith && level >= 90)
                    {
                        return PLD.BladeOfTruth;
                    }

                    if (lastComboMove == PLD.BladeOfTruth && level >= 90)
                    {
                        return PLD.BladeOfValor;
                    }

                    if (level >= PLD.Levels.Confiteor)
                    {
                        var requiescat = FindEffect(PLD.Buffs.Requiescat);
                        if (requiescat != null)
                            return PLD.Confiteor;
                    }
                }

                return PLD.TotalEclipse;
            }

            return actionID;
        }
    }

    internal class PaladinConfiteorFeature : CustomCombo
    {
        protected override CustomComboPreset Preset => CustomComboPreset.PaladinConfiteorFeature;

        protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
        {
            if (actionID == PLD.HolySpirit || actionID == PLD.HolyCircle)
            {
                if (OriginalHook(PLD.Confiteor) != PLD.Confiteor)
                    return OriginalHook(PLD.Confiteor);

                Status? requiescat = FindEffect(PLD.Buffs.Requiescat);

                if (requiescat != null)
                {
                    if (requiescat.StackCount <= 1 && level >= PLD.Levels.Confiteor)
                    {
                        return OriginalHook(PLD.Confiteor);
                    }
                }

                return actionID;
            }

            return actionID;
        }
    }

    internal class PaladinRequiescatCombo : CustomCombo
    {
        protected override CustomComboPreset Preset => CustomComboPreset.PaladinRequiescatCombo;

        protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
        {
            if (actionID == PLD.Requiescat)
            {
                if ((HasEffect(PLD.Buffs.Requiescat) && level >= PLD.Levels.Confiteor) || OriginalHook(PLD.Confiteor) != PLD.Confiteor)
                    return OriginalHook(PLD.Confiteor);

                return PLD.Requiescat;
            }

            return actionID;
        }
    }

    internal class PaladinScornfulSpiritsFeature : CustomCombo
    {
        protected override CustomComboPreset Preset => CustomComboPreset.PaladinScornfulSpiritsFeature;

        protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
        {
            if (actionID == PLD.SpiritsWithin || actionID == PLD.CircleOfScorn)
            {
                if (level >= PLD.Levels.SpiritsWithin && level <= PLD.Levels.Expiacion)
                    return CalcBestAction(actionID, PLD.SpiritsWithin, PLD.CircleOfScorn);

                if (level >= PLD.Levels.Expiacion)
                    return CalcBestAction(actionID, PLD.Expiacion, PLD.CircleOfScorn);

                if (level >= PLD.Levels.CircleOfScorn)
                    return CalcBestAction(actionID, PLD.SpiritsWithin, PLD.CircleOfScorn);

                return PLD.SpiritsWithin;
            }

            return actionID;
        }
    }
}
