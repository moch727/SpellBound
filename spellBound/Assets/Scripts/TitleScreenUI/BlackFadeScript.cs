using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BlackFadeScript : MonoBehaviour
{
    [SerializeField] float fadeDuration;
    private float fadeSpeed;
    private Image screen;
    private Color currentColor = Color.black;
    void Awake()
    {
        screen = GetComponent<Image>();
        currentColor.a = 0;
        fadeSpeed = 1f/ fadeDuration;
        screen.color = currentColor;
        screen.enabled = false;
    }

    public void SetVisibility(bool pEnabled, float opacity)
    {
        screen.enabled = pEnabled;
        currentColor.a = opacity;
        screen.color = currentColor;
    }

    public float StartFade(float targetAlpha)
    {
        screen.enabled = true;
        StartCoroutine(Fade(targetAlpha));
        return fadeDuration;
    }

    IEnumerator Fade(float targetAlpha)
    {
        while (currentColor.a != targetAlpha)
        {
            currentColor.a = Mathf.MoveTowards(currentColor.a, targetAlpha, fadeSpeed * Time.deltaTime);
            screen.color = currentColor;

            yield return null;
        }
        screen.enabled = !(targetAlpha == 0);
    }
}
