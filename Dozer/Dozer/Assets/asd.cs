using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
 
public class asd : MonoBehaviour
{
    public int avgFrameRate;
    public TextMeshProUGUI display_Text;
    public int avgFrameRate2;
    public TextMeshProUGUI display_Text2;

    private void Start()
    {
        StartCoroutine(CheckFPS());
        StartCoroutine(CheckFPSFaster());
    }

    IEnumerator CheckFPS()
    {
        float current = 0;
        current = (int)(1f / Time.unscaledDeltaTime);
        avgFrameRate = (int)current;
        display_Text.text = avgFrameRate.ToString() + " FPS";
        yield return new WaitForSeconds(1f);
        StartCoroutine(CheckFPS());
    }
    IEnumerator CheckFPSFaster()
    {
        float current = 0;
        current = (int)(1f / Time.unscaledDeltaTime);
        avgFrameRate2 = (int)current;
        display_Text2.text = avgFrameRate2.ToString() + " FPS";
        yield return new WaitForSeconds(0.1f);
        StartCoroutine(CheckFPSFaster());
    }
}