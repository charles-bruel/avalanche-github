using UnityEngine;
using System;

namespace VarietyPack
{
	public class WheelGeneratorTower : MonoBehaviour, IWheelGenerator
	{ 
		public void UpdatePositions()
		{
			Vector3 lowerDelta = this.lowerTarget.position - this.basePos.position;
			Vector3 upperDelta = this.upperTarget.position - this.basePos.position;
			Vector2 flattenedLowerDelta = new Vector2(Mathf.Sqrt(lowerDelta.x * lowerDelta.x + lowerDelta.z * lowerDelta.z), lowerDelta.y);
			Vector2 flattenedUpperDelta = new Vector2(Mathf.Sqrt(upperDelta.x * upperDelta.x + upperDelta.z * upperDelta.z), upperDelta.y);
			if (Vector3.Dot(base.transform.forward, lowerDelta) < 0f)
			{
				flattenedLowerDelta.x *= -1f;
			}
			if (Vector3.Dot(base.transform.forward, upperDelta) < 0f)
			{
				flattenedUpperDelta.x *= -1f;
			}
			float lowerAngle = Mathf.Atan2(flattenedLowerDelta.y, flattenedLowerDelta.x) * 57.29578f;
			float upperAngle = Mathf.Atan2(flattenedUpperDelta.y, flattenedUpperDelta.x) * 57.29578f;
			while (lowerAngle < 90f)
			{
				lowerAngle += 360f;
			}
			while (upperAngle < 90f)
			{
				upperAngle += 360f;
			}
			float num3 = lowerAngle - 90f;
			float num4 = upperAngle + 90f;
			float num5 = num3 - num4;
			float num6 = Mathf.Abs(num5);
			this.Initialize();
			float num7 = this.lerp(num3, num4, ((float)this.lowerWheelCount + 1f) / (float)(this.totalWheelCount + 2));
			float num8 = (this.totalWheelLength + 2f * this.wheelSpacing) / (6.2831855f * (num5 / 360f));
			float num9 = Mathf.Sin(0.017453292f * num7) * num8;
			float num10 = Mathf.Cos(0.017453292f * num7) * num8;
			for (int i = 0; i < this.lowerWheelCount; i++)
			{
				int num11 = i + 1;
				float num12 = this.lerp(num3, num4, (float)num11 / (float)(this.totalWheelCount + 2));
				float y = Mathf.Sin(num12 * 0.017453292f) * num8 - num9;
				float z = Mathf.Cos(num12 * 0.017453292f) * num8 - num10;
				this.lowerWheels[i].transform.localPosition = new Vector3(0f, y, z);
				this.lowerWheels[i].transform.localRotation = Quaternion.Euler(-num12 + 90f, 0f, 90f);
				WheelScript temp = lowerWheels[i].GetComponent<WheelScript>();
				temp.above = (num3 - UnderCableOffsetValue >= num4);
				Vector3 temp2 = temp.transform.eulerAngles;
				temp2.x = 0;
				temp.catwalk.transform.eulerAngles = temp2;
			}
			for (int j = 0; j < this.upperWheelCount; j++)
			{
				int num13 = j + 1 + this.lowerWheelCount;
				float num14 = this.lerp(num3, num4, (float)num13 / (float)(this.totalWheelCount + 2));
				float y2 = Mathf.Sin(num14 * 0.017453292f) * num8 - num9;
				float z2 = Mathf.Cos(num14 * 0.017453292f) * num8 - num10;
				this.upperWheels[j].transform.localPosition = new Vector3(0f, y2, z2);
				this.upperWheels[j].transform.localRotation = Quaternion.Euler(-num14 + 90f, 0f, 90f);
				WheelScript temp = upperWheels[j].GetComponent<WheelScript>();
				temp.above = (num3 - UnderCableOffsetValue >= num4);
				Vector3 temp2 = temp.transform.eulerAngles;
				temp2.x = 0;
				temp.catwalk.transform.eulerAngles = temp2;
			}
		}

		public float lerp(float a, float b, float c)
		{
			return a * (1f - c) + b * c;
		}

		public WheelScript[] GetAllWheels()
		{
			this.Initialize();
			WheelScript[] array = new WheelScript[this.totalWheelCount];
			int i;
			for (i = 0; i < this.lowerWheelCount; i++)
			{
				array[i] = this.lowerWheels[i].GetComponent<WheelScript>();
			}
			for (int j = 0; j < this.upperWheelCount; j++)
			{
				array[i] = this.upperWheels[j].GetComponent<WheelScript>();
				i++;
			}
			return array;
		}

		public void Initialize()
		{
			this.totalWheelCount = this.lowerWheelCount + this.upperWheelCount;
			this.totalWheelLength = (float)this.totalWheelCount * this.wheelSpacing;
			if (this.HasInitialized && this.lowerWheelCount == this.lowerWheels.Length && this.upperWheelCount == this.upperWheels.Length)
			{
				return;
			}
			if (!this.HasInitialized)
			{
				this.lowerWheels = new GameObject[this.lowerWheelCount];
				this.upperWheels = new GameObject[this.upperWheelCount];
				for (int i = 0; i < this.lowerWheelCount; i++)
				{
					this.lowerWheels[i] = Instantiate<GameObject>(this.wheelPrefab, base.transform);
					this.lowerWheels[i].GetComponent<WheelScript>().catwalk = Instantiate<GameObject>(this.catwalkPrefab, this.lowerWheels[i].transform);
				}
				for (int j = 0; j < this.upperWheelCount; j++)
				{
					this.upperWheels[j] = Instantiate<GameObject>(this.wheelPrefab, base.transform);
					this.upperWheels[j].GetComponent<WheelScript>().catwalk = Instantiate<GameObject>(this.catwalkPrefab, this.upperWheels[j].transform);
				}
			}
			else
			{
				if (this.upperWheelCount > this.upperWheels.Length)
				{
					GameObject[] array = this.upperWheels;
					this.upperWheels = new GameObject[this.upperWheelCount];
					int k;
					for (k = 0; k < array.Length; k++)
					{
						this.upperWheels[k] = array[k];
					}
					while (k < this.upperWheelCount)
					{
						this.upperWheels[k] = Instantiate<GameObject>(this.wheelPrefab, base.transform);
						this.upperWheels[k].GetComponent<WheelScript>().catwalk = Instantiate<GameObject>(this.catwalkPrefab, this.upperWheels[k].transform);
						k++;
					}
				}
				else
				{
					GameObject[] array2 = this.upperWheels;
					this.upperWheels = new GameObject[this.upperWheelCount];
					int l;
					for (l = 0; l < this.upperWheelCount; l++)
					{
						this.upperWheels[l] = array2[l];
					}
					while (l < array2.Length)
					{
						DestroyImmediate(array2[l]);
						l++;
					}
				}
				if (this.lowerWheelCount > this.lowerWheels.Length)
				{
					GameObject[] array3 = this.lowerWheels;
					this.lowerWheels = new GameObject[this.lowerWheelCount];
					int m;
					for (m = 0; m < array3.Length; m++)
					{
						this.lowerWheels[m] = array3[m];
					}
					while (m < this.lowerWheelCount)
					{
						this.lowerWheels[m] = Instantiate<GameObject>(this.wheelPrefab, base.transform);
						this.lowerWheels[m].GetComponent<WheelScript>().catwalk = Instantiate<GameObject>(this.catwalkPrefab, this.lowerWheels[m].transform);
						m++;
					}
				}
				else
				{
					GameObject[] array4 = this.lowerWheels;
					this.lowerWheels = new GameObject[this.lowerWheelCount];
					int n;
					for (n = 0; n < this.lowerWheelCount; n++)
					{
						this.lowerWheels[n] = array4[n];
					}
					while (n < array4.Length)
					{
						DestroyImmediate(array4[n]);
						n++;
					}
				}
			}
			this.HasInitialized = true;
		}

		public Transform GetTransform()
        {
			return transform;
        }

		public Transform lowerTarget;

		public Transform upperTarget;

		public Transform basePos;

		public int lowerWheelCount;

		public int upperWheelCount;

		public GameObject wheelPrefab;

		public GameObject catwalkPrefab;

		public float wheelSpacing;

		public WheelMeshGenerator generator;

		public GameObject[] lowerWheels;

		public GameObject[] upperWheels;

		public int totalWheelCount;

		public float totalWheelLength;

		public bool HasInitialized;

		public float UnderCableOffsetValue;
	}

}
