using System;
using System.Collections.Generic;
using System.Text;

namespace PrecompiledExtensions
{
    public class LiftVehicleSeatManager : ExtensionItem
    {
        public void ManageSeatsInt(int[] SeatStatus)
        {
            ManageSeats(Utils.ConvertToOccupency(SeatStatus));
        }

        public virtual void ManageSeats(Enums.SeatOccupency[] SeatStatus)
        {

        }
    }
}
