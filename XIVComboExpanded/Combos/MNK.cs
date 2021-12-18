using System.Linq;

using Dalamud.Game.ClientState.JobGauge.Enums;
using Dalamud.Game.ClientState.JobGauge.Types;
using Dalamud.Game.ClientState.Statuses;

namespace XIVComboExpandedestPlugin.Combos
{
    internal static class MNK
    {
        public const byte ClassID = 2;
        public const byte JobID = 20;

        public const uint
            Bootshine = 53,
            DragonKick = 74,
            TrueStrike = 54,
            SnapPunch = 56,
            TwinSnakes = 61,
            Demolish = 66,
            ArmOfTheDestroyer = 62,
            PerfectBalance = 69,
            Rockbreaker = 70,
            Meditation = 3546,
            FormShift = 4262,
            FourPointFury = 16473,
            HowlingFist = 25763,
            Enlightenment = 16474,
            MasterfulBlitz = 25764,
            ShadowOfTheDestroyer = 25767;

        public static class Buffs
        {
            public const ushort
                TwinSnakes = 101,
                OpoOpoForm = 107,
                RaptorForm = 108,
                CoerlForm = 109,
                PerfectBalance = 110,
                LeadenFist = 1861,
                FormlessFist = 2513;
        }

        public static class Debuffs
        {
            public const ushort
                Demolish = 246;
        }

        public static class Levels
        {
            public const byte
                TrueStrike = 4,
                SnapPunch = 6,
                Meditation = 15,
                Rockbreaker = 30,
                Demolish = 30,
                FourPointFury = 45,
                HowlingFist = 40,
                DragonKick = 50,
                FormShift = 52,
                MasterfulBlitz = 60,
                Enlightenment = 70,
                ShadowOfTheDestroyer = 82;
        }
    }

    internal class MonkSTCombo : CustomCombo
    {
        protected override CustomComboPreset Preset => CustomComboPreset.MonkSTCombo;

        protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
        {
            if (OriginalHook(MNK.MasterfulBlitz) != MNK.MasterfulBlitz && level >= MNK.Levels.MasterfulBlitz)
                return OriginalHook(MNK.MasterfulBlitz);

            if (!HasEffect(MNK.Buffs.PerfectBalance) && !HasEffect(MNK.Buffs.FormlessFist) && (actionID == MNK.TrueStrike || actionID == MNK.TwinSnakes))
            {
                if (HasEffect(MNK.Buffs.OpoOpoForm))
                {
                    if (IsEnabled(CustomComboPreset.MnkBootshineFeature))
                    {
                        return HasEffect(MNK.Buffs.LeadenFist) || level < MNK.Levels.DragonKick ? MNK.Bootshine : MNK.DragonKick;
                    }

                    return actionID == MNK.TrueStrike ? MNK.Bootshine : MNK.DragonKick;
                }

                if (HasEffect(MNK.Buffs.RaptorForm))
                {
                    if (level < MNK.Levels.TrueStrike)
                        return MNK.Bootshine;
                    return actionID == MNK.TrueStrike ? MNK.TrueStrike : MNK.TwinSnakes;
                }

                if (HasEffect(MNK.Buffs.CoerlForm))
                {
                    if (level < MNK.Levels.SnapPunch)
                        return MNK.Bootshine;
                    return actionID == MNK.TrueStrike ? MNK.SnapPunch : MNK.Demolish;
                }

                if (IsEnabled(CustomComboPreset.MnkBootshineFeature))
                {
                    return level >= MNK.Levels.DragonKick ? MNK.DragonKick : MNK.Bootshine;
                }

                return actionID == MNK.TrueStrike ? MNK.Bootshine : MNK.DragonKick;
            }

            if (HasEffect(MNK.Buffs.PerfectBalance) && actionID != MNK.FormShift)
            {
                Status? pb = FindEffect(MNK.Buffs.PerfectBalance);

                if (pb == null) return actionID;

                if (actionID == MNK.PerfectBalance)
                {
                    return HasEffect(MNK.Buffs.LeadenFist) ? MNK.Bootshine : MNK.DragonKick;
                }

                switch (pb.StackCount)
                {
                    case 3:
                        if (IsEnabled(CustomComboPreset.MnkBootshineFeature))
                        {
                            return HasEffect(MNK.Buffs.LeadenFist) ? MNK.Bootshine : MNK.DragonKick;
                        }

                        return actionID == MNK.TrueStrike ? MNK.Bootshine : MNK.DragonKick;
                    case 2:
                        return actionID == MNK.TrueStrike ? MNK.TrueStrike : MNK.TwinSnakes;
                    case 1:
                        return actionID == MNK.TrueStrike ? MNK.SnapPunch : MNK.Demolish;
                }
            }

            if (HasEffect(MNK.Buffs.FormlessFist))
            {
                switch (actionID)
                {
                    case MNK.TrueStrike:
                        return HasEffect(MNK.Buffs.LeadenFist) && IsEnabled(CustomComboPreset.MnkBootshineFeature) ? MNK.Bootshine : MNK.DragonKick;
                    case MNK.TwinSnakes:
                        return MNK.TwinSnakes;
                    case MNK.PerfectBalance:
                        return MNK.Demolish;
                    case MNK.FormShift:
                        if (!IsEnabled(CustomComboPreset.MonkAoEComboFormOption))
                        {
                            if (!IsEnabled(CustomComboPreset.MnkBootshineFeature))
                                return MNK.Bootshine;
                            if (!IsEnabled(CustomComboPreset.MonkSTComboFormOption))
                                return MNK.SnapPunch;
                        }

                        break;
                }
            }

            return actionID;
        }
    }

    internal class MonkAoECombo : CustomCombo
    {
        protected override CustomComboPreset Preset => CustomComboPreset.MonkAoECombo;

        protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
        {
            if (actionID == (IsEnabled(CustomComboPreset.MonkAoEComboFormOption) ? MNK.FormShift : MNK.FourPointFury))
            {
                Status? pb = FindEffect(MNK.Buffs.PerfectBalance);

                if (pb != null && HasEffect(MNK.Buffs.PerfectBalance))
                {
                    if (pb.StackCount == 3)
                        return OriginalHook(MNK.ArmOfTheDestroyer);
                    if (pb.StackCount == 2)
                        return MNK.FourPointFury;
                    if (pb.StackCount == 1)
                        return MNK.Rockbreaker;
                }

                if (HasEffect(MNK.Buffs.FormlessFist))
                    return MNK.FourPointFury;
            }

            if (actionID == MNK.MasterfulBlitz)
            {
                if (OriginalHook(MNK.MasterfulBlitz) != MNK.MasterfulBlitz && level >= MNK.Levels.MasterfulBlitz)
                    return OriginalHook(MNK.MasterfulBlitz);

                if (HasEffect(MNK.Buffs.PerfectBalance) || HasEffect(MNK.Buffs.FormlessFist))
                {
                    if (level >= MNK.Levels.ShadowOfTheDestroyer)
                    {
                        return OriginalHook(MNK.ArmOfTheDestroyer);
                    }

                    return MNK.Rockbreaker;
                }

                if (HasEffect(MNK.Buffs.OpoOpoForm))
                    return OriginalHook(MNK.ArmOfTheDestroyer);

                if (HasEffect(MNK.Buffs.RaptorForm) && level >= MNK.Levels.FourPointFury)
                    return MNK.FourPointFury;

                if (HasEffect(MNK.Buffs.CoerlForm) && level >= MNK.Levels.Rockbreaker)
                    return MNK.Rockbreaker;

                return OriginalHook(MNK.ArmOfTheDestroyer);
            }

            return actionID;
        }
    }

    internal class MonkDragonKickBalanceFeature : CustomCombo
    {
        protected override CustomComboPreset Preset => CustomComboPreset.MnkDragonKickBalanceFeature;

        protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
        {
            if (actionID == MNK.DragonKick && !IsEnabled(CustomComboPreset.MonkSTCombo))
            {
                if (OriginalHook(MNK.MasterfulBlitz) != MNK.MasterfulBlitz && level >= MNK.Levels.MasterfulBlitz)
                    return OriginalHook(MNK.MasterfulBlitz);
            }

            return actionID;
        }
    }

    internal class MnkBootshineFeature : CustomCombo
    {
        protected override CustomComboPreset Preset => CustomComboPreset.MnkBootshineFeature;

        protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
        {
            if (actionID == MNK.DragonKick)
            {
                if (HasEffect(MNK.Buffs.LeadenFist) && (
                    HasEffect(MNK.Buffs.FormlessFist) || HasEffect(MNK.Buffs.PerfectBalance) ||
                    HasEffect(MNK.Buffs.OpoOpoForm) || HasEffect(MNK.Buffs.RaptorForm) || HasEffect(MNK.Buffs.CoerlForm)))
                    return MNK.Bootshine;

                if (level < MNK.Levels.DragonKick)
                    return MNK.Bootshine;
            }

            return actionID;
        }
    }

    internal class MonkHowlingFistMeditationFeature : CustomCombo
    {
        protected override CustomComboPreset Preset => CustomComboPreset.MonkHowlingFistMeditationFeature;

        protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
        {
            if (actionID == MNK.HowlingFist || actionID == MNK.Enlightenment)
            {
                if (OriginalHook(MNK.Meditation) == MNK.Meditation)
                    return MNK.Meditation;

                // Enlightenment
                return OriginalHook(MNK.HowlingFist);
            }

            return actionID;
        }
    }

    internal class MonkPerfectBalanceFeature : CustomCombo
    {
        protected override CustomComboPreset Preset => CustomComboPreset.MonkPerfectBalanceFeature;

        protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
        {
            if (actionID == MNK.PerfectBalance)
            {
                if (OriginalHook(MNK.MasterfulBlitz) != MNK.MasterfulBlitz && level >= MNK.Levels.MasterfulBlitz)
                    return OriginalHook(MNK.MasterfulBlitz);
            }

            return actionID;
        }
    }
}
