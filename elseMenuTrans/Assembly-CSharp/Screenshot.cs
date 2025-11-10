using System;
using System.IO;
using UnityEngine;

// Token: 0x02000086 RID: 134
public class Screenshot : MonoBehaviour
{
	// Token: 0x06000400 RID: 1024 RVA: 0x0001CDDC File Offset: 0x0001AFDC
	public static string ScreenShotName(int width, int height)
	{
		return string.Format("{0}/../../Goodies/screenshots/screen_{1}x{2}_{3}.png", new object[]
		{
			Application.dataPath,
			width,
			height,
			DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss")
		});
	}

	// Token: 0x06000401 RID: 1025 RVA: 0x0001CE28 File Offset: 0x0001B028
	public void TakeHiResShot()
	{
		this.takeHiResShot = true;
	}

	// Token: 0x06000402 RID: 1026 RVA: 0x0001CE34 File Offset: 0x0001B034
	private void LateUpdate()
	{
		this.takeHiResShot |= Input.GetKeyDown(KeyCode.F12);
		if (this.takeHiResShot)
		{
			RenderTexture renderTexture = new RenderTexture(3840, 2880, 24);
			base.camera.targetTexture = renderTexture;
			Texture2D texture2D = new Texture2D(3840, 2880, TextureFormat.RGB24, false);
			base.camera.Render();
			RenderTexture.active = renderTexture;
			texture2D.ReadPixels(new Rect(0f, 0f, 3840f, 2880f), 0, 0);
			base.camera.targetTexture = null;
			RenderTexture.active = null;
			global::UnityEngine.Object.DestroyImmediate(renderTexture);
			byte[] array = texture2D.EncodeToPNG();
			string text = Screenshot.ScreenShotName(3840, 2880);
			File.WriteAllBytes(text, array);
			Debug.Log(string.Format("Took screenshot to: {0}", text));
			this.takeHiResShot = false;
		}
	}

	// Token: 0x04000317 RID: 791
	public const int RES_WIDTH = 3840;

	// Token: 0x04000318 RID: 792
	public const int RES_HEIGHT = 2880;

	// Token: 0x04000319 RID: 793
	private bool takeHiResShot;
}
