using UnityEngine;

namespace VarietyPack
{
    public interface IWheelGenerator
    {
        void UpdatePositions();
        WheelScript[] GetAllWheels();
        Transform GetTransform();
    }
}
