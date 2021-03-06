﻿using System;

namespace PKHeX.Core
{
    /// <summary>
    /// Finds the index of the most recent save block for <see cref="SAV4"/> blocks.
    /// </summary>
    public static class SAV4BlockDetection
    {
        private const int First = 0;
        private const int Second = 1;
        private const int Same = 2;

        /// <summary>
        /// Compares the footers of the two blocks to determine which is newest.
        /// </summary>
        /// <returns>0=Primary, 1=Secondary.</returns>
        public static int CompareFooters(byte[] data, int offset1, int offset2)
        {
            // Major Counters
            var major1 = BitConverter.ToUInt32(data, offset1);
            var major2 = BitConverter.ToUInt32(data, offset2);
            var result1 = CompareCounters(major1, major2);
            if (result1 == First)
                return First;
            if (result1 == Second)
                return Second;

            // Minor Counters
            var minor1 = BitConverter.ToUInt32(data, offset1 + 4);
            var minor2 = BitConverter.ToUInt32(data, offset2 + 4);
            var result2 = CompareCounters(minor1, minor2);
            return result2 == Second ? Second : First; // Same -> First, shouldn't happen for valid saves.
        }

        private static int CompareCounters(uint counter1, uint counter2)
        {
            // Uninitialized
            if (counter1 == uint.MaxValue && counter2 == 0)
                return Second;
            if (counter1 == 0 && counter2 == uint.MaxValue)
                return First;

            // Different
            if (counter1 > counter2)
                return First;
            if (counter1 < counter2)
                return Second;

            return Same;
        }
    }
}
