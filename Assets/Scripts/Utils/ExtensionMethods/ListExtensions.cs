﻿using System.Collections.Generic;

namespace Utils
{
    public static class ListExtensions
    {
        private static System.Random _rng = new System.Random();

        // Fisher-Yates algorithm
        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = _rng.Next(n + 1);
                (list[k], list[n]) = (list[n], list[k]);
            }
        }
    }
}