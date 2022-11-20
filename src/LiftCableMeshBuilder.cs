using System;
using System.Collections.Generic;
using LBF.Unity;
using UnityEngine;

// Token: 0x0200066F RID: 1647
public partial class LiftCableMeshBuilder
{

	// Token: 0x06001EAD RID: 7853 RVA: 0x00015D3F File Offset: 0x00013F3F
	public void AddSection(Vector3 from, Vector3 to)
	{
		this.AddSectionReturn(from, to);
	}

	// Token: 0x06001EAF RID: 7855 RVA: 0x00015D4A File Offset: 0x00013F4A
	public void AddSectionBasic(Vector3 from, Vector3 to)
	{
		if (to == from)
		{
			return;
		}
		this.AddSectionBasicNoAdd(from, to);
		this.m_sections.Add(from);
	}

	// Token: 0x06001EB0 RID: 7856 RVA: 0x00075F80 File Offset: 0x00074180
	public Vector3[] AddSectionReturn(Vector3 from, Vector3 to)
	{
		if ((from - to).sqrMagnitude < 25f)
		{
			this.AddSectionBasic(from, to);
			return new Vector3[]
			{
				from,
				to
			};
		}
		int num = (int)Math.Max(4.0, Math.Floor((double)(from - to).ToHorizontal().magnitude / 4.0)) + 2;
		if (num > 20)
		{
			num = (int)Mathf.Pow((float)(num - 20), 0.8f) + 20;
		}
		Vector3[] array = new Vector3[num];
		array[0] = from;
		array[array.Length - 1] = to;
		from = array[0];
		to = array[array.Length - 1];
		double num2 = (double)(from.x - to.x);
		double num3 = (double)(from.z - to.z);
		double num4 = 0.0;
		double num5 = (double)from.y;
		double num6 = Math.Sqrt(num2 * num2 + num3 * num3);
		double num7 = (double)to.y;
		double num8 = (num4 + num6) / 2.0;
		double num9 = (num5 + num7) / 2.0;
		double num10 = num6 - num4;
		double num11 = num7 - num5;
		double num12 = Math.Sqrt(num10 * num10 + num11 * num11) * (1.0 + ((double)this.SagAmount - 1.0));
		if (num12 == 0.0)
		{
			array = new Vector3[]
			{
				array[0],
				array[1],
				array[array.Length - 2],
				array[array.Length - 1]
			};
		}
		else
		{
			double num13 = Math.Sqrt(num12 * num12 - num11 * num11) / num10;
			double num14 = (num13 < 3.0) ? Math.Sqrt(6.0 * (num13 - 1.0)) : (Math.Log(2.0 * num13) + Math.Log(Math.Log(2.0 * num13)));
			double num15 = num14;
			for (int i = 0; i < 5; i++)
			{
				double num16 = (Math.Sinh(num15) - num13 * num15) / (Math.Cosh(num15) - num13);
				num15 = num14;
			}
			double num17 = num15;
			double num18 = num10 / (2.0 * num17);
			double b = num8 - num18 * this.ATanh(num11 / num12);
			double c = num9 - num12 / (2.0 * Math.Tanh(num17));
			for (int j = 1; j < array.Length - 1; j++)
			{
				Vector3 vector = Vector3.Lerp(from, to, (float)j / ((float)array.Length - 1f));
				float magnitude = (vector - from).ToHorizontal().magnitude;
				vector.y = (float)this.Func((double)magnitude, num18, b, c);
				array[j] = vector;
			}
		}
		for (int k = 0; k < array.Length - 1; k++)
		{
			this.AddSectionBasic(array[k], array[k + 1]);
		}
		return array;
	}

	// Token: 0x06001EB1 RID: 7857 RVA: 0x000762AC File Offset: 0x000744AC
	private double ATanh(double value)
	{
		if (Math.Abs(value) > 1.0)
		{
			throw new ArgumentException("value");
		}
		return 0.5 * Math.Log((1.0 + value) / (1.0 - value));
	}

	// Token: 0x06001EB2 RID: 7858 RVA: 0x00015D6A File Offset: 0x00013F6A
	private double Func(double x, double a, double b, double c)
	{
		return a * Math.Cosh((x - b) / a) + c;
	}

	// Token: 0x06001EB4 RID: 7860 RVA: 0x000764D0 File Offset: 0x000746D0
	public void AddExtraCable(Vector3 offset)
	{
		List<Vector3> list = new List<Vector3>(this.m_sections.Count);
		for (int i = 0; i < this.m_sections.Count - 1; i++)
		{
			Vector3 vector5 = this.m_sections[i];
			Vector3 vector2 = this.m_sections[i + 1];
			Vector2 vector3 = (vector5 - vector2).ToHorizontal();
			float f = Mathf.Atan((vector5.y - vector2.y) / vector3.magnitude);
			vector3.Normalize();
			float y = Mathf.Cos(f) * offset.y;
			float num = Mathf.Sin(f) * offset.y;
			Vector3 a = new Vector3(num * vector3.x, y, num * vector3.y);
			Vector3 b = new Vector3(-vector3.y * offset.x, 0f, vector3.x * offset.x);
			Vector3 vector4 = a + b;
			if (i == 0)
			{
				list.Add(this.m_sections[i] + vector4);
			}
			else
			{
				list[i] = (list[i] + this.m_sections[i] + vector4) / 2f;
			}
			list.Add(this.m_sections[i + 1] + vector4);
		}
		for (int j = 0; j < list.Count - 1; j++)
		{
			this.AddSectionBasicNoAdd(list[j], list[j + 1]);
		}
	}

	// Token: 0x06001EB5 RID: 7861 RVA: 0x00076694 File Offset: 0x00074894
	public Vector3[] AddSectionBasicReturn(Vector3 from, Vector3 to)
	{
		Vector3[] array = new Vector3[(int)Mathf.Max(Mathf.Ceil((from - to).magnitude / 5f), 2f)];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = Vector3.Lerp(from, to, (float)i / (float)(array.Length - 1));
		}
		for (int j = 0; j < array.Length - 1; j++)
		{
			this.AddSectionBasic(array[j], array[j + 1]);
		}
		return array;
	}
}
