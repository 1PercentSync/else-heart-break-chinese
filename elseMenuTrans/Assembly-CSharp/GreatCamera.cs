using System;
using UnityEngine;

// Token: 0x02000062 RID: 98
public class GreatCamera : MonoBehaviour
{
	// Token: 0x06000330 RID: 816 RVA: 0x000185C8 File Offset: 0x000167C8
	private void Start()
	{
		global::UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
	}

	// Token: 0x06000331 RID: 817 RVA: 0x000185D8 File Offset: 0x000167D8
	private void Update()
	{
		if (Mathf.Approximately(Time.deltaTime, 0f))
		{
			return;
		}
		this.UpdateStates(Time.deltaTime);
	}

	// Token: 0x06000332 RID: 818 RVA: 0x00018608 File Offset: 0x00016808
	public void UpdateStates(float deltaTime)
	{
		this.easyAnimate.Update(deltaTime);
		this.UpdateShake();
		this.orbit.Update();
		NewCameraState newCameraState = NewCameraState.Lerp(this.orbit, this.fix, this.slider);
		base.transform.position = newCameraState.position + this._shake;
		base.transform.LookAt(newCameraState.lookTarget);
	}

	// Token: 0x06000333 RID: 819 RVA: 0x00018678 File Offset: 0x00016878
	public void Input_StartDrag()
	{
		this.orbit.BeginMove();
	}

	// Token: 0x06000334 RID: 820 RVA: 0x00018688 File Offset: 0x00016888
	public void Input_EndDrag()
	{
		this.orbit.EndMove();
	}

	// Token: 0x06000335 RID: 821 RVA: 0x00018698 File Offset: 0x00016898
	public void Input_Drag(float x, float y)
	{
		this.orbit.Move(x, y);
	}

	// Token: 0x06000336 RID: 822 RVA: 0x000186A8 File Offset: 0x000168A8
	public void Input_Zoom(float z)
	{
		this.orbit.Zoom(z);
	}

	// Token: 0x06000337 RID: 823 RVA: 0x000186B8 File Offset: 0x000168B8
	public void Input_ZoomDiscrete(int i)
	{
		this.orbit.ZoomDiscrete(i);
	}

	// Token: 0x06000338 RID: 824 RVA: 0x000186C8 File Offset: 0x000168C8
	public void Input_SetZoomDirectly(int pLevel)
	{
		this.orbit.zoomLevel = (float)pLevel;
	}

	// Token: 0x06000339 RID: 825 RVA: 0x000186D8 File Offset: 0x000168D8
	public void Input_SetTilt(float pTilt)
	{
		this.orbit.targetTilt = pTilt;
	}

	// Token: 0x0600033A RID: 826 RVA: 0x000186E8 File Offset: 0x000168E8
	public void Input_SetRotation(float pAngle)
	{
		this.orbit.targetAngle = pAngle;
	}

	// Token: 0x0600033B RID: 827 RVA: 0x000186F8 File Offset: 0x000168F8
	public void EnterFixedCamera(Transform pCameraPoint, Transform pTarget)
	{
		this.fix.position = pCameraPoint.position;
		this.fix.lookTarget = pTarget.position;
		this.AnimateSlider(this.slider, 1f);
	}

	// Token: 0x0600033C RID: 828 RVA: 0x00018738 File Offset: 0x00016938
	public void ExitFixedCamera()
	{
		this.AnimateSlider(this.slider, 0f);
	}

	// Token: 0x0600033D RID: 829 RVA: 0x0001874C File Offset: 0x0001694C
	public void Reset()
	{
		this.slider = 0f;
	}

	// Token: 0x0600033E RID: 830 RVA: 0x0001875C File Offset: 0x0001695C
	public void SetOrbitTarget(Vector3 pTargetPosition)
	{
		if (!NewCameraState.ValidVector3(pTargetPosition))
		{
			Debug.LogError("Invalid Vector3 in SetOrbitTarget!");
		}
		this.orbit.lookTarget = pTargetPosition;
	}

	// Token: 0x0600033F RID: 831 RVA: 0x00018790 File Offset: 0x00016990
	public void Shake(float pAmmount, float pOverTime, bool pFallof)
	{
		this._shakeFallof = pFallof;
		this._shakeAmmount = pAmmount;
		this._shakeTimerNow = (this._shakeTimerStart = Time.realtimeSinceStartup);
		this._shakeTimerGoal = this._shakeTimerNow + pOverTime;
	}

	// Token: 0x1700004C RID: 76
	// (get) Token: 0x06000340 RID: 832 RVA: 0x000187D0 File Offset: 0x000169D0
	public bool isInFixedMode
	{
		get
		{
			return this.slider > 0.95f;
		}
	}

	// Token: 0x1700004D RID: 77
	// (get) Token: 0x06000341 RID: 833 RVA: 0x000187E0 File Offset: 0x000169E0
	public bool isShaking
	{
		get
		{
			return this._shakeTimerNow < this._shakeTimerGoal;
		}
	}

	// Token: 0x06000342 RID: 834 RVA: 0x000187F0 File Offset: 0x000169F0
	private void AnimateSlider(float pStart, float pGoal)
	{
		this.easyAnimate.Register(this, "ChangeState", new EasyAnimState<float>(this.changeStateTime, pStart, pGoal, new EasyAnimState<float>.InterpolationSampler(iTween.easeOutQuad), delegate(float p)
		{
			this.slider = p;
		}));
	}

	// Token: 0x06000343 RID: 835 RVA: 0x00018834 File Offset: 0x00016A34
	private void UpdateShake()
	{
		if (this._shakeTimerNow < this._shakeTimerGoal)
		{
			this._shakeTimerNow += Time.deltaTime;
			if (this._shakeTimerNow >= this._shakeTimerGoal)
			{
				this._shake = default(Vector3);
			}
			else
			{
				float num = 1f;
				if (this._shakeFallof)
				{
					num = 1f - (this._shakeTimerNow - this._shakeTimerStart) / (this._shakeTimerGoal - this._shakeTimerStart);
				}
				float num2 = num * this._shakeAmmount;
				this._shake = new Vector3(global::UnityEngine.Random.Range(-1f, 1f), global::UnityEngine.Random.Range(-1f, 1f), global::UnityEngine.Random.Range(-1f, 1f));
				this._shake.Normalize();
				this._shake *= num2;
			}
		}
	}

	// Token: 0x1700004E RID: 78
	// (get) Token: 0x06000344 RID: 836 RVA: 0x0001891C File Offset: 0x00016B1C
	public float zoom
	{
		get
		{
			return this.orbit.zoomLevel;
		}
	}

	// Token: 0x1700004F RID: 79
	// (get) Token: 0x06000345 RID: 837 RVA: 0x0001892C File Offset: 0x00016B2C
	public float tilt
	{
		get
		{
			return this.orbit.tilt;
		}
	}

	// Token: 0x06000346 RID: 838 RVA: 0x0001893C File Offset: 0x00016B3C
	public void SetAutoRotate(float pSpeed)
	{
		this.orbit.autoRotateSpeed = pSpeed;
	}

	// Token: 0x17000050 RID: 80
	// (get) Token: 0x06000347 RID: 839 RVA: 0x0001894C File Offset: 0x00016B4C
	public static float invertSignum
	{
		get
		{
			return (!GreatCamera.invertCamera) ? 1f : (-1f);
		}
	}

	// Token: 0x04000257 RID: 599
	private EasyAnimateTwo easyAnimate = new EasyAnimateTwo();

	// Token: 0x04000258 RID: 600
	public FixNewCameraState fix = new FixNewCameraState();

	// Token: 0x04000259 RID: 601
	public OrbitNewCameraState orbit = new OrbitNewCameraState();

	// Token: 0x0400025A RID: 602
	public float changeStateTime = 1f;

	// Token: 0x0400025B RID: 603
	private float _shakeTimerStart;

	// Token: 0x0400025C RID: 604
	private float _shakeTimerNow;

	// Token: 0x0400025D RID: 605
	private float _shakeTimerGoal;

	// Token: 0x0400025E RID: 606
	private float _shakeAmmount;

	// Token: 0x0400025F RID: 607
	private bool _shakeFallof;

	// Token: 0x04000260 RID: 608
	private Vector3 _shake = default(Vector3);

	// Token: 0x04000261 RID: 609
	[Range(0f, 1f)]
	public float slider;

	// Token: 0x04000262 RID: 610
	public static bool invertCamera;
}
