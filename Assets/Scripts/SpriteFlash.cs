using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteFlash : MonoBehaviour {

    [SerializeField] Color flashColor;
	[SerializeField] float flashDuration;

	private Material mat;
    private IEnumerator flashCoroutine;

	private void Awake()
    {
		mat = GetComponent<SpriteRenderer>().material;
        flashCoroutine = null;
    }

    private void Start()
    {
        mat.SetColor("_FlashColor", flashColor);
    }

#if false
    private void Update() {
		if(Input.GetKeyDown(KeyCode.Space))
			Flash();
	}
#endif

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
    private void SetFlashAmount(float flashAmount)
    {
        mat.SetFloat("_FlashAmount", flashAmount);
    }

}
