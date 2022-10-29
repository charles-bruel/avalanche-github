using System.Collections.Generic;
using UnityEngine;
using System;

namespace VarietyPack
{
	public class WheelMeshGenerator : MonoBehaviour
	{
		public void GenerateMesh()
		{
			this.Initialize();
			if (wheelGenerator == null)
			{
				Console.WriteLine("No wheel generator");
				return;
			}
			base.transform.rotation = Quaternion.identity;
			MeshFilter component = base.GetComponent<MeshFilter>();
			if (component == null)
			{
				Console.WriteLine("No mesh (filter)");
				return;
			}
			Mesh mesh = component.mesh;
			if (mesh == null)
			{
				Console.WriteLine("No mesh");
				return;
			}
			mesh.Clear();
			List<Vector3> vertices = new List<Vector3>();
			List<int> indices = new List<int>();
			WheelScript[] allWheels = wheelGenerator.GetAllWheels();
			if (allWheels == null && allWheels.Length == 0)
			{
				Console.WriteLine("No wheels");
				return;
			}
			foreach (WheelScript wheelScript in allWheels)
            {
				if (wheelScript == null) {
					Console.WriteLine("Been passed a null wheel");
					return;
				}
			}
			Vector3[,] meshPoints = new Vector3[allWheels.Length + 1, 2];
			for (int i = 1; i < allWheels.Length; i++)
			{
				Vector3 vector = this.Midpoint(allWheels[i - 1].meshPoints[2].position, allWheels[i].meshPoints[0].position) - base.transform.position;
				Vector3 vector2 = this.Midpoint(allWheels[i - 1].meshPoints[3].position, allWheels[i].meshPoints[1].position) - base.transform.position;
				meshPoints[i, 0] = vector;
				meshPoints[i, 1] = vector2;
			}
			meshPoints[0, 0] = allWheels[0].meshPoints[0].position - base.transform.position;
			meshPoints[0, 1] = allWheels[0].meshPoints[1].position - base.transform.position;
			meshPoints[meshPoints.GetLength(0) - 1, 0] = allWheels[allWheels.Length - 1].meshPoints[2].position - base.transform.position;
			meshPoints[meshPoints.GetLength(0) - 1, 1] = allWheels[allWheels.Length - 1].meshPoints[3].position - base.transform.position;
			float num = -thickness * wheelGenerator.GetTransform().localScale.x;
			float num2 = -thickness * wheelGenerator.GetTransform().localScale.x;
			if (num > 0f)
			{
				num = 0f;
			}
			if (num2 < 0f)
			{
				num2 = 0f;
			}
			Vector3 b = wheelGenerator.GetTransform().right * num;
			Vector3 b2 = wheelGenerator.GetTransform().right * num2;
			for (int j = 0; j < meshPoints.GetLength(0) - 1; j++)
			{
				this.AddQuad(vertices, indices, meshPoints[j, 0] + b, meshPoints[j + 1, 0] + b, meshPoints[j, 1] + b, meshPoints[j + 1, 1] + b);
				this.AddQuad(vertices, indices, meshPoints[j, 0] + b2, meshPoints[j, 1] + b2, meshPoints[j + 1, 0] + b2, meshPoints[j + 1, 1] + b2);
				this.AddQuad(vertices, indices, meshPoints[j, 1] + b2, meshPoints[j, 1] + b, meshPoints[j + 1, 1] + b2, meshPoints[j + 1, 1] + b);
				this.AddQuad(vertices, indices, meshPoints[j, 0] + b2, meshPoints[j + 1, 0] + b2, meshPoints[j, 0] + b, meshPoints[j + 1, 0] + b);
			}
			this.AddQuad(vertices, indices, meshPoints[0, 0] + b2, meshPoints[0, 0] + b, meshPoints[0, 1] + b2, meshPoints[0, 1] + b);
			this.AddQuad(vertices, indices, meshPoints[meshPoints.GetLength(0) - 1, 0] + b2, meshPoints[meshPoints.GetLength(0) - 1, 1] + b2, meshPoints[meshPoints.GetLength(0) - 1, 0] + b, meshPoints[meshPoints.GetLength(0) - 1, 1] + b);
			mesh.vertices = vertices.ToArray();
			mesh.triangles = indices.ToArray();
			mesh.RecalculateBounds();
			mesh.RecalculateNormals();
			component.mesh = mesh;
		}

		private void AddQuad(List<Vector3> vertices, List<int> tris, Vector3 pos1, Vector3 pos2, Vector3 pos3, Vector3 pos4)
		{
			vertices.Add(pos1);
			vertices.Add(pos3);
			vertices.Add(pos2);
			vertices.Add(pos4);
			int num = -1;
			foreach (int num2 in tris)
			{
				if (num2 > num)
				{
					num = num2;
				}
			}
			num++;
			tris.Add(num);
			tris.Add(num + 2);
			tris.Add(num + 1);
			tris.Add(num + 2);
			tris.Add(num + 3);
			tris.Add(num + 1);
		}

		public Vector3 Midpoint(Vector3 a, Vector3 b)
		{
			return new Vector3((a.x + b.x) / 2f, (a.y + b.y) / 2f, (a.z + b.z) / 2f);
		}

		public void Initialize()
		{
			if (this.HasInitialized)
			{
				return;
			}

			MeshFilter component = GetComponent<MeshFilter>();
			Mesh mesh = component.mesh;
			if (mesh == null)
			{
				mesh = new Mesh();
			}

			mesh.MarkDynamic();

			component.mesh = mesh;

			this.HasInitialized = true;
		}

		public IWheelGenerator wheelGenerator;

		public bool HasInitialized;

		public float thickness;
	}
}