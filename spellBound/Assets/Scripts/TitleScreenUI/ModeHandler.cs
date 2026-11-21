using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ModeHandler : MonoBehaviour
{
    //TODO: Users will be able to press buttons as they are fading when interactable, prevent that
    [SerializeField] Button[] buttons;
    [SerializeField] float hideFade;
    [SerializeField] float fadeDelay;
    private float originalFadeDuration;

    [Header("FullScreen Fade")]
    [SerializeField] BlackFadeScript fadeScript;
    void Awake()
    {
        ColorBlock cb = buttons[0].colors;
        originalFadeDuration = cb.fadeDuration;
        foreach (Button b in buttons)
        {
            b.interactable = false;
        }
    }
    void Start()
    {
        fadeScript.SetVisibility(true, 1);
        StartCoroutine(LoadTitle());
    }
    IEnumerator LoadTitle()
    {
        yield return new WaitForSeconds(fadeScript.StartFade(0));
        StartCoroutine(DisplayButtons());
    }
    public void OnPlay(Button button)
    {
        StartCoroutine(StartGame(Array.IndexOf(buttons, button)));
    }

    public void OnSettings(Button button)
    {
        // StartCoroutine(FadeButtons(Array.IndexOf(buttons, button)));
    }

    public void OnExit(Button button)
    {
        StartCoroutine(ExitGame(Array.IndexOf(buttons, button)));
    }

    public void ResetButtonStates()
    {
        StartCoroutine(DisplayButtons());
    }

    IEnumerator DisplayButtons()
    {
        yield return StartCoroutine(FadeButtons(0, true));
        //Reset button fade duration
        foreach (Button b in buttons)
        {
            Debug.Log("Reset!");
            ColorBlock buttonColorBlock = b.colors;
            buttonColorBlock.fadeDuration = originalFadeDuration;
            b.colors = buttonColorBlock;
        }
    }

    //Command design pattern to be able to revert fading?
    IEnumerator FadeButtons(int startIndex, bool enable)
    {
        int i = startIndex;
        do
        {
            Button b = buttons[i];
            if (!b.Equals(gameObject))
            {
                ColorBlock buttonColorBlock = b.colors;
                buttonColorBlock.fadeDuration = hideFade;
                b.interactable = enable;
                b.colors = buttonColorBlock;
                yield return new WaitForSeconds(fadeDelay);
            }
            i = (i + 1) % buttons.Length;
        }
        while(i != startIndex);
    }

    IEnumerator StartGame(int index)
    {
        yield return StartCoroutine(FadeButtons(index, false));
        yield return new WaitForSeconds(fadeScript.StartFade(1));
        Debug.Log("Start game!");
        SceneManager.LoadScene("Level");
    }

    IEnumerator ExitGame(int index)
    {
        yield return StartCoroutine(FadeButtons(index, false));
        yield return new WaitForSeconds(fadeScript.StartFade(1));

        Debug.Log("Exit!");
        Application.Quit();
    }
}
