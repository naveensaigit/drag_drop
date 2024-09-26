using System.Collections;
using UnityEngine;
using TMPro;

// Class for the blinking text animation
public class BlinkEffect : MonoBehaviour
{
    private TextMeshProUGUI text;

    private void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        StartCoroutine("Blink");
    }

    IEnumerator Blink()
    {
        // Keep the blinking animation running forever
        while (true)
        {
            // Based on text's opacity (text.color.a)
            switch (text.color.a.ToString())
            {
                // If opacity == 0 (text not visible), set opacity = 1 (text visible) and wait for 0.5 seconds
                case "0":
                    text.color = new Color(text.color.r, text.color.g, text.color.b, 1);
                    yield return new WaitForSeconds(0.5f);
                    break;
                // If opacity == 1 (text visible), set opacity = 0 (text not visible) and wait for 0.5 seconds
                case "1":
                    text.color = new Color(text.color.r, text.color.g, text.color.b, 0);
                    yield return new WaitForSeconds(0.5f);
                    break;
            }
        }
    }
}