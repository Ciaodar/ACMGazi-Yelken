using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RadialFade : MonoBehaviour
{
    public Image radialImage;
    public float fadeSpeed = 2f;
    private bool fadingOut = false;
    private bool fadingIn = false;
    private Vector3 targetPosition;

    private void Update()
    {
        if (fadingOut)
        {
            radialImage.transform.localScale += Vector3.one * (fadeSpeed * Time.deltaTime);
        }
        else if (fadingIn)
        {
            radialImage.transform.localScale -= Vector3.one * (fadeSpeed * Time.deltaTime);
            if (radialImage.transform.localScale.x <= 0.01f)
            {
                fadingIn = false;
                radialImage.gameObject.SetActive(false);
            }
        }
    }

    public void StartFadeOut(Vector3 worldPos)
    {
        radialImage.gameObject.SetActive(true);
        Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);
        radialImage.transform.position = screenPos;
        radialImage.transform.localScale = Vector3.one * 0.01f;
        fadingOut = true;
        fadingIn = false;
    }

    public void StartFadeIn(Vector3 worldPos)
    {
        radialImage.gameObject.SetActive(true);
        Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);
        radialImage.transform.position = screenPos;
        radialImage.transform.localScale = Vector3.one * 3f; // büyük başla
        fadingIn = true;
        fadingOut = false;
    }
}
