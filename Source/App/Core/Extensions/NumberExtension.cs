using System;

namespace Project.Core.Extensions
{
    public static class NumberExtension
    {
        public static double ToRound2Decimal(this double value)
        {
            return Math.Round(value, 2);
        }

        public static float ToRound2Decimal(this float value)
        {
            return (float) Math.Round(value, 2);
        }


        public static decimal ToRound2Decimal(this decimal value)
        {
            return Math.Round(value, 2);
        }

    }
}