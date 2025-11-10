using System;
using UnityEngine;

// Token: 0x0200002E RID: 46
public static class ArrayExtensions
{
	// Token: 0x060001FE RID: 510 RVA: 0x0000F520 File Offset: 0x0000D720
	public static T[] Mutate<T>(this T[] array, Func<T, T> f)
	{
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = f(array[i]);
		}
		return array;
	}

	// Token: 0x060001FF RID: 511 RVA: 0x0000F558 File Offset: 0x0000D758
	public static T[] Mutate<T>(this T[] array, Func<T> f)
	{
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = f();
		}
		return array;
	}

	// Token: 0x06000200 RID: 512 RVA: 0x0000F588 File Offset: 0x0000D788
	public static T[] SetAll<T>(this T[] array, T value)
	{
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = value;
		}
		return array;
	}

	// Token: 0x06000201 RID: 513 RVA: 0x0000F5B4 File Offset: 0x0000D7B4
	public static T[] SetWithIndex<T>(this T[] array, Func<int, T> f)
	{
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = f(i);
		}
		return array;
	}

	// Token: 0x06000202 RID: 514 RVA: 0x0000F5E4 File Offset: 0x0000D7E4
	public static T[] ForEachWithIndex<T>(this T[] array, Action<int, T> f)
	{
		for (int i = 0; i < array.Length; i++)
		{
			f(i, array[i]);
		}
		return array;
	}

	// Token: 0x06000203 RID: 515 RVA: 0x0000F614 File Offset: 0x0000D814
	public static T[] Map<T>(this T[] array, Func<T, T> f)
	{
		T[] array2 = new T[array.Length];
		for (int i = 0; i < array.Length; i++)
		{
			array2[i] = f(array[i]);
		}
		return array2;
	}

	// Token: 0x06000204 RID: 516 RVA: 0x0000F654 File Offset: 0x0000D854
	public static T RandNth<T>(this T[] array)
	{
		return array[global::UnityEngine.Random.Range(0, array.Length)];
	}
}
