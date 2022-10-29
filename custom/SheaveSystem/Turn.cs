using System;
using System.Collections.Generic;
using System.Text;

using UnityEngine;
using PrecompiledExtensions;

namespace SheaveSystem
{
    class Turn : LiftTurnCablePathFetcher
    {

        private bool Initialized;
        private GenericTurn turn;

        private void Initialize()
        {
            Initialized = true;
            turn = LoadedData[0].GetComponent<GenericTurn>();
            if (turn == null)
                Console.WriteLine("Could not find generic turn on object");
        }

        public override void OnParameterUpdate(float angle1, float angle2, Transform prevTower, Transform nextTower, Transform currentTowerPos)
        {
            if (!Initialized) Initialize();
            turn.IncomingCablePoint = prevTower;
            turn.OutgoingCablePoint = nextTower;
            turn.Reset();
        }

        public override float GetLength(float angle1, float angle2)
        {
            if (!Initialized) Initialize();
            return turn.GetLength();
        }

        public override List<Transform> GetCablePath(float angle1, float angle2, Transform prevTower, Transform nextTower, Transform currentTowerPos, Transform relevantCablePoint, bool right)
        {
            if (!Initialized) Initialize();
            if (right)
            {
                return turn.CablePointsUp;
            } 
            else
            {
                return turn.CablePointsDown;
            }
        }
    }
}
