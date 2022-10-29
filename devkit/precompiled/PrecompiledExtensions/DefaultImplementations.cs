using UnityEngine;
using System;

namespace PrecompiledExtensions
{
    //This class takes a transform which should be a parent to an equal (or greater) number of gameobjects to the number of seats.
    //For each seat that isn't empty, the gameobject is activated, and deactived for empty seats. This behavior can also be inverted.
    //EXAMPLE: Visually putting skis on the outside of gondolas.
    public class LiftVehicleSeatManagerPerSkier : LiftVehicleSeatManager
    {
        //PROVIDED DATA LAYOUT
        //Data is provided in 3 arrays, one for GameObjects, one for ints and one for floats.
        //These are INDICES into the array for various parameters
        //GAME OBJECT PARAM LAYOUT
        //Elements Parents (this is the parent of each activated element)
        private const int ELEMENTS_PARENT = 0;
        //INT PARAM LAYOUT
        //Is inverted (0 for false - skier present activates element, anything else for true)
        private const int IS_INVERTED = 0;

        public override void ManageSeats(Enums.SeatOccupency[] SeatStatus)
        {
            Initialize();

            if (SeatsParent == null)
            {
                Console.WriteLine("NO PARENT PROVIDED");
                return;
            }
            if (SeatsParent.childCount < SeatStatus.Length)
            {
                Console.WriteLine("INSUFFICIENT CHILDREN");
                return;
            }

            for (int i = 0;i < SeatStatus.Length;i++)
            {
                SeatsParent.GetChild(i).gameObject.SetActive((SeatStatus[i] != Enums.SeatOccupency.EMPTY) ^ IsInverted);//yay boolean magic
            }
        }

        public void Initialize()
        {
            if (HasInitialized)
            {
                return;
            }
            HasInitialized = true;

            SeatsParent = LoadedData[ELEMENTS_PARENT].transform;

            for (int i = 0;i < SeatsParent.childCount;i++)
            {
                SeatsParent.GetChild(i).gameObject.SetActive(false);
            }

            if (IntParameters.Length != 0) 
            {
                IsInverted = IntParameters[IS_INVERTED] != 0;
            }

            if (LoadedData.Length == 0)
            {
                Console.WriteLine("INSUFFICIENT GAMEOBJECT PARAMETERS");
            }
        }

        private bool IsInverted = false;

        private bool HasInitialized = false;

        private Transform SeatsParent;

    }

    //This class takes two transforms.
    //If the required number of seats are filled (defaults to 1), it deactivates the "empty" gameobject and activates the "full" gameobject, and vice versa.
    //If null values are passed, they will be ignored, allowing you to only specify a "empty" or a "full" gameobject, however it still relies on having 2 gameobject parameters.
    //EXAMPLE: Chair bar raising/lowering
    public class LiftVehicleSeatManagerSingle : LiftVehicleSeatManager
    {
        //PROVIDED DATA LAYOUT
        //Data is provided in 3 arrays, one for GameObjects, one for ints and one for floats.
        //These are INDICES into the array for various parameters
        //GAME OBJECT PARAM LAYOUT
        //"Empty" Gameobject
        private const int EMPTY_OBJECT = 0;
        //"Full" Gameobject
        private const int FULL_OBJECT = 1;
        //INT PARAM LAYOUT
        //The required number of seats to be filled for the change to activate. Defaults to 1.
        private const int REQUIRED_NUMBER = 0;

        public override void ManageSeats(Enums.SeatOccupency[] SeatStatus)
        {
            Initialize();

            if (Invalid)
            {
                return;
            }

            int nonEmptySeats = 0;
            for(int i = 0;i < SeatStatus.Length;i++)
            {
                if(SeatStatus[i] != Enums.SeatOccupency.EMPTY)
                {
                    nonEmptySeats++;
                }
            }

            bool isActive = nonEmptySeats >= RequiredSeats;

            if(Inactive != null)
            {
                Inactive.SetActive(!isActive);
            }
            
            if(Active != null)
            {
                Active.SetActive(isActive);
            }
        }

        public void Initialize()
        {
            if (HasInitialized)
            {
                return;
            }
            HasInitialized = true;

            if(LoadedData.Length < 2)
            {
                Invalid = true;
                Console.WriteLine("INSUFFICIENT GAMEOBJECT PARAMETERS");
                return;
            }

            Inactive = LoadedData[EMPTY_OBJECT];
            Active = LoadedData[FULL_OBJECT];

            if (IntParameters.Length > 0)
            {
                RequiredSeats = IntParameters[REQUIRED_NUMBER];
            }
        }

        private bool HasInitialized = false;

        private bool Invalid = false;

        private int RequiredSeats = 1;

        private GameObject Inactive;

        private GameObject Active;
    }

    //This class rotates an element to be roughly inline with the previous and subsequent towers.
    public class LiftTowerCablePathFetcherSimple : LiftTowerCablePathFetcher
    {
        //PROVIDED DATA LAYOUT
        //Data is provided in 3 arrays, one for GameObjects, one for ints and one for floats.
        //These are INDICES into the array for various parameters
        //GAME OBJECT PARAM LAYOUT
        //Gameobject to rotate
        private const int EMPTY_OBJECT = 0;
        //FLOAT PARAM LAYOUT
        //A multiplier
        private const int MUL = 0;
        //An offset
        private const int ADDX = 1;
        private const int ADDY = 2;
        private const int ADDZ = 3;
        //INT PARAM LAYOUT
        //Id of euler angle to change (0 for x, 1 for y, 2 for z)
        private const int CHANGE_ID = 0;

        public override void OnParameterUpdate(Transform prevTower, Transform nextTower, Transform currentTowerPos)
        {
            Vector3 temp = nextTower.position - prevTower.position;
            float dy = temp.y;
            temp.y = 0;
            float dx = temp.magnitude;

            float theta = Mathf.Atan(dy/dx) * Mathf.Rad2Deg;

            Quaternion temp2 = LoadedData[EMPTY_OBJECT].transform.localRotation;
            Vector3 temp3 = temp2.eulerAngles;//Awful and ugly but it works
            temp3.x = FloatParameters[ADDX];
            temp3.y = FloatParameters[ADDY];
            temp3.z = FloatParameters[ADDZ];
            switch (IntParameters[CHANGE_ID])
            {
                case 0:
                    temp3.x = theta * FloatParameters[MUL] + FloatParameters[ADDX];
                    break;
                case 1:
                    temp3.y = theta * FloatParameters[MUL] + FloatParameters[ADDY];
                    break;
                case 2:
                    temp3.z = theta * FloatParameters[MUL] + FloatParameters[ADDZ];
                    break;
            }
            temp2.eulerAngles = temp3;
            LoadedData[0].transform.localRotation = temp2;
        }
    }

}