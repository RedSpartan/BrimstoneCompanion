﻿using RedSpartan.BrimstoneCompanion.Domain.Utilities;

namespace RedSpartan.BrimstoneCompanion.Domain
{
    public static class AttributeNames
    {
        public const string XP = "EXP";
        public const string GRIT = "GRT";
        public const string CORRUPTION = "CPT";
        public const string HEAVY = "HVY";
        public const string AGILITY = "AGI";
        public const string CUNNING = "CNG";
        public const string SPIRIT = "SPT";
        public const string STRENGTH = "STR";
        public const string LORE = "LOR";
        public const string LUCK = "LUK";
        public const string COMBAT = "CBT";
        public const string INITIATIVE = "INT";
        public const string MELEE = "MLE";
        public const string RANGE = "RNG";
        public const string WOUNDS = "WND";
        public const string HEALTH = "HLT";
        public const string HORROR = "HRR";
        public const string SANITY = "SAN";
        public const string DEFENCE = "DEF";
        public const string WILLPOWER = "WIL";
        public const string DOLLARS = "DLR";
        public const string DARKSTONE = "DKS";

        public static IList<string> Strings { get; } = new Lazy<IList<string>>(StringValues()).Value;

        private static IList<string> StringValues()
        {
            return typeof(AttributeNames).GetAllPublicConstantValues<string>();
        }

        /*return new List<string>
        {
            XP,
            GRIT,
            CORRUPTION,
            HEAVY,
            AGILITY,
            CUNNING,
            SPIRIT,
            STRENGTH,
            LORE,
            LUCK,
            COMBAT,
            INITIATIVE,
            MELEE,
            RANGE,
            WOUNDS,
            HEALTH,
            HORROR,
            SANITY,
            DEFENCE,
            WILLPOWER,
            DOLLARS,
            DARKSTONE
        };*/
    }
}