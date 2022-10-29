using System;
using System.Collections.Generic;
using System.Text;

namespace PrecompiledExtensions
{
    public class Utils
    {

        public static Enums.SeatOccupency[] ConvertToOccupency(int[] data)
        {
            return (Enums.SeatOccupency[])(object)data;
        }

    }
}
