using System;
using UnityEngine;

// Token: 0x02000012 RID: 18
public class EasyAnimState<T> : IEasyAnimState
{
	// Token: 0x06000055 RID: 85 RVA: 0x000043F0 File Offset: 0x000025F0
	public EasyAnimState(float pTime, T pStartValue, T pEndValue, EasyAnimState<T>.InterpolationSampler pSampler, EasyAnimState<T>.SetValue pSetterFunction, Function pCompleteCallback)
	{
		this.sampler = pSampler;
		this.setter = pSetterFunction;
		this.timer = 0f;
		this.maxTime = pTime;
		this.startValue = pStartValue;
		this.endValue = pEndValue;
		this.onComplete = pCompleteCallback;
	}

	// Token: 0x06000056 RID: 86 RVA: 0x0000443C File Offset: 0x0000263C
	public EasyAnimState(float pTime)
	{
		this.maxTime = pTime;
	}

	// Token: 0x06000057 RID: 87 RVA: 0x0000444C File Offset: 0x0000264C
	public EasyAnimState(float pTime, T pStartValue, T pEndValue, EasyAnimState<T>.InterpolationSampler pSampler, EasyAnimState<T>.SetValue pSetterFunction)
	{
		this.sampler = pSampler;
		this.setter = pSetterFunction;
		this.timer = 0f;
		this.maxTime = pTime;
		this.startValue = pStartValue;
		this.endValue = pEndValue;
	}

	// Token: 0x17000002 RID: 2
	// (get) Token: 0x06000058 RID: 88 RVA: 0x00004490 File Offset: 0x00002690
	// (set) Token: 0x06000059 RID: 89 RVA: 0x00004498 File Offset: 0x00002698
	public float timer { get; set; }

	// Token: 0x17000003 RID: 3
	// (get) Token: 0x0600005A RID: 90 RVA: 0x000044A4 File Offset: 0x000026A4
	// (set) Token: 0x0600005B RID: 91 RVA: 0x000044AC File Offset: 0x000026AC
	public float maxTime { get; set; }

	// Token: 0x17000004 RID: 4
	// (get) Token: 0x0600005C RID: 92 RVA: 0x000044B8 File Offset: 0x000026B8
	// (set) Token: 0x0600005D RID: 93 RVA: 0x000044C0 File Offset: 0x000026C0
	public T startValue { get; set; }

	// Token: 0x17000005 RID: 5
	// (get) Token: 0x0600005E RID: 94 RVA: 0x000044CC File Offset: 0x000026CC
	// (set) Token: 0x0600005F RID: 95 RVA: 0x000044D4 File Offset: 0x000026D4
	public T endValue { get; set; }

	// Token: 0x17000006 RID: 6
	// (get) Token: 0x06000060 RID: 96 RVA: 0x000044E0 File Offset: 0x000026E0
	// (set) Token: 0x06000061 RID: 97 RVA: 0x000044E8 File Offset: 0x000026E8
	public EasyAnimState<T>.InterpolationSampler sampler { get; set; }

	// Token: 0x17000007 RID: 7
	// (get) Token: 0x06000062 RID: 98 RVA: 0x000044F4 File Offset: 0x000026F4
	// (set) Token: 0x06000063 RID: 99 RVA: 0x000044FC File Offset: 0x000026FC
	public EasyAnimState<T>.SetValue setter { get; set; }

	// Token: 0x17000008 RID: 8
	// (get) Token: 0x06000064 RID: 100 RVA: 0x00004508 File Offset: 0x00002708
	// (set) Token: 0x06000065 RID: 101 RVA: 0x00004510 File Offset: 0x00002710
	public IEasyAnimState nextState { get; set; }

	// Token: 0x17000009 RID: 9
	// (get) Token: 0x06000066 RID: 102 RVA: 0x0000451C File Offset: 0x0000271C
	// (set) Token: 0x06000067 RID: 103 RVA: 0x00004524 File Offset: 0x00002724
	public Function onComplete { get; set; }

	// Token: 0x06000068 RID: 104 RVA: 0x00004530 File Offset: 0x00002730
	public EasyAnimState<T> Then(T pValue, float pAnimationTime)
	{
		this.nextState = new EasyAnimState<T>(pAnimationTime, this.endValue, pValue, this.sampler, this.setter);
		return (EasyAnimState<T>)this.nextState;
	}

	// Token: 0x06000069 RID: 105 RVA: 0x00004568 File Offset: 0x00002768
	public EasyAnimState<T> Then(EasyAnimState<T> easyAnimState)
	{
		this.nextState = easyAnimState;
		return (EasyAnimState<T>)this.nextState;
	}

	// Token: 0x0600006A RID: 106 RVA: 0x0000457C File Offset: 0x0000277C
	internal void Complete()
	{
		if (this.setter != null)
		{
			this.setter(this.endValue);
		}
		if (this.onComplete != null)
		{
			this.onComplete();
		}
	}

	// Token: 0x0600006B RID: 107 RVA: 0x000045BC File Offset: 0x000027BC
	public bool Interpolate(float pDeltaTime)
	{
		this.timer += pDeltaTime;
		if (this.timer >= this.maxTime)
		{
			this.Complete();
			return false;
		}
		if (this.setter != null)
		{
			float num = Mathf.Max(this.timer / this.maxTime, 0f);
			this.setter(this.sampler(this.startValue, this.endValue, num));
			return true;
		}
		return true;
	}

	// Token: 0x020000F6 RID: 246
	// (Invoke) Token: 0x06000737 RID: 1847
	public delegate void SetValue(T pValue);

	// Token: 0x020000F7 RID: 247
	// (Invoke) Token: 0x0600073B RID: 1851
	public delegate T InterpolationSampler(T pStart, T pEnd, float pValue);
}
