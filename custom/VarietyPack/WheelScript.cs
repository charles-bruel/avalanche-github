using UnityEngine;

namespace VarietyPack
{
	public class WheelScript : MonoBehaviour
	{
		public bool above
		{
			set
			{
				Vector3 localPosition = this.cableAttachPoint.localPosition;
				if (value)
				{
					localPosition.x = ((localPosition.x > 0f) ? localPosition.x : (-localPosition.x));
				}
				else
				{
					localPosition.x = ((localPosition.x < 0f) ? localPosition.x : (-localPosition.x));
				}
				this.cableAttachPoint.localPosition = localPosition;
			}
		}

		public Transform cableAttachPoint;

		public Transform[] meshPoints;

		public GameObject catwalk;
	}
}
