using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpriteFlash : MonoBehaviour {

    [SerializeField] Color flashColor;
	[SerializeField] float flashDuration;

	private Material mat;
    private IEnumerator flashCoroutine;

	private void Awake()
    {
        if (TryGetComponent<SpriteRenderer>(out SpriteRenderer renderer))
        {
            mat = renderer.material;
        }
        else if (TryGetComponent<Image>(out Image image))
        {
            mat = image.material;
        }
        flashCoroutine = null;
    }

    private void Start()
    {
        mat.SetColor("_FlashColor", flashColor);
    }
    
    private void OnDestroy()
    {
        SetFlashAmount(0);
    }

#if false
    private void Update() {
		if(Input.GetKeyDown(KeyCode.Space))
			Flash();
	}
#endif

    /// <summary>
    /// １回点滅
    /// </summary>
	public void Flash(){
        if (flashCoroutine != null)
        {
            //StopCoroutine(flashCoroutine);
            return;
        }

        flashCoroutine = DoFlash();
        StartCoroutine(flashCoroutine);
    }


    private IEnumerator DoFlash()
    {
        float lerpTime = 0;

        while (lerpTime < flashDuration)
        {
            lerpTime += Time.deltaTime;
            float perc = lerpTime / flashDuration;

            SetFlashAmount(1f - perc);
            yield return null;
        }

        SetFlashAmount(0);

        flashCoroutine = null;
    }

    /// <summary>
    /// ループ点滅
    /// </summary>
    public void FlashLoop()
    {
        flashCoroutine = DoFlashLoop();
        StartCoroutine(flashCoroutine);
    }

    private IEnumerator DoFlashLoop()
    {
        float lerpTime = 0;
        float lerpValue = -1;

        while (true)
        {
            lerpTime += (lerpValue * Time.deltaTime);
            float perc = lerpTime / flashDuration;

            SetFlashAmount(perc);
            yield return null;

            if (lerpTime >= flashDuration)
            {
                lerpValue = -1;
            }
            if (lerpTime <= 0)
            {
                lerpValue = 1;
            }
        }
    }

    /// <summary>
    /// シェーダーに値を渡す
    /// </summary>
    /// <param name="flashAmount">割合</param>
    private void SetFlashAmount(float flashAmount)
    {
        mat.SetFloat("_FlashAmount", flashAmount);
    }

}
