using System;
using System.Collections.Generic;

// Token: 0x0200091C RID: 2332
public class SwapList<T>
{
	// Token: 0x06002CF4 RID: 11508 RVA: 0x0001F214 File Offset: 0x0001D414
	public SwapList(int initCapacity)
	{
		this.data = new List<T>(initCapacity);
		this.emptySlots = new Queue<int>();
	}

	// Token: 0x06002CF5 RID: 11509 RVA: 0x0001F233 File Offset: 0x0001D433
	public SwapList() : this(0)
	{
	}

	// Token: 0x06002CF6 RID: 11510 RVA: 0x0001F23C File Offset: 0x0001D43C
	public SwapList(IEnumerable<T> collection) : this(SwapList<T>.SizeHelper(collection))
	{
		this.AddRange(collection);
	}

	// Token: 0x06002CF7 RID: 11511 RVA: 0x000B0B68 File Offset: 0x000AED68
	private static int SizeHelper(IEnumerable<T> collection)
	{
		ICollection<T> collection2 = collection as ICollection<T>;
		if (collection2 == null)
		{
			return 0;
		}
		return collection2.Count;
	}

	// Token: 0x06002CF8 RID: 11512 RVA: 0x000B0B88 File Offset: 0x000AED88
	public void RemoveAt(int i)
	{
		if (i < 0 || i >= this.data.Count)
		{
			throw new IndexOutOfRangeException();
		}
		if (this.emptySlots.Contains(i))
		{
			return;
		}
		this.emptySlots.Enqueue(i);
		this.data[i] = default(T);
	}

	// Token: 0x17000745 RID: 1861
	// (get) Token: 0x06002CF9 RID: 11513 RVA: 0x0001F252 File Offset: 0x0001D452
	public int Length
	{
		get
		{
			return this.data.Count - this.emptySlots.Count;
		}
	}

	// Token: 0x17000746 RID: 1862
	// (get) Token: 0x06002CFA RID: 11514 RVA: 0x0001F26B File Offset: 0x0001D46B
	public int CapacityWithoutResizing
	{
		get
		{
			return this.data.Capacity;
		}
	}

	// Token: 0x17000747 RID: 1863
	public T this[int i]
	{
		get
		{
			return this.data[i];
		}
		set
		{
			this.data[i] = value;
		}
	}

	// Token: 0x17000748 RID: 1864
	// (get) Token: 0x06002CFD RID: 11517 RVA: 0x0001F295 File Offset: 0x0001D495
	public int[] EmptySlots
	{
		get
		{
			return this.emptySlots.ToArray();
		}
	}

	// Token: 0x17000749 RID: 1865
	// (get) Token: 0x06002CFE RID: 11518 RVA: 0x0001F2A2 File Offset: 0x0001D4A2
	public int Elements
	{
		get
		{
			return this.data.Count;
		}
	}

	// Token: 0x06002CFF RID: 11519 RVA: 0x000B0BE0 File Offset: 0x000AEDE0
	public List<int> AddRange(IEnumerable<T> collection)
	{
		ICollection<T> collection2 = collection as ICollection<T>;
		List<int> list;
		if (collection2 != null && this.Length + collection2.Count - this.CapacityWithoutResizing > 0)
		{
			this.data.Capacity = this.Length + collection2.Count;
			list = new List<int>(collection2.Count);
		}
		else
		{
			list = new List<int>();
		}
		foreach (T item in collection)
		{
			list.Add(this.Add(item));
		}
		return list;
	}

	// Token: 0x06002D00 RID: 11520 RVA: 0x000B0C7C File Offset: 0x000AEE7C
	public int Add(T item)
	{
		if (this.emptySlots.Count > 0)
		{
			int num = this.emptySlots.Dequeue();
			this.data[num] = item;
			return num;
		}
		this.data.Add(item);
		return this.data.Count - 1;
	}

	// Token: 0x04002142 RID: 8514
	private List<T> data;

	// Token: 0x04002143 RID: 8515
	private Queue<int> emptySlots;
}
