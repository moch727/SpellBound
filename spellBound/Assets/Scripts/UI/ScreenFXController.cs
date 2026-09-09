using UnityEngine;
using UnityEngine.Rendering.Universal;
using static UnityEngine.Rendering.DebugUI;

public class ScreenFXController : MonoBehaviour
{
    [SerializeField] ScriptableRendererFeature effectRenderer;
    [SerializeField] PlayerScript player; //For tracking hp

    [Header("Hurt oscillation")]
    [SerializeField] float amplitude;
    [SerializeField] float speed;
    [SerializeField] float minIntensity;
    [SerializeField] Material darkenEffect;
    public bool flash;
    private float count;
    public bool enableHurtEffect;


    //For Fade in and out
    [SerializeField] float fadeSpeed;
    private float originalIntensity;
    private float fadeTargetValue;

    public enum ScreenState
    {
        None, FadingIn, FadingOut, FadeInOut, Black
    }

    public ScreenState state;
    private void Awake()
    {
        darkenEffect.SetFloat("_ScreenPower", 1.5f);
    }

    void LateUpdate()
    {
        if (player != null && state == ScreenState.None && player.currentAction != PlayerScript.Action.CutScene) ManageHurtEffect();
        if (state == ScreenState.FadeInOut) ManageFadeInOut();
        if (state == ScreenState.FadingIn || state == ScreenState.FadingOut)
        {
            ManageFade(fadeTargetValue);
        }
    }

    public void FadeIn()
    {
        if (state != ScreenState.FadingIn)
        {
            originalIntensity = darkenEffect.GetFloat("_ScreenPower"); //save the original intensity to go back to
            fadeTargetValue = 0f;
            state = ScreenState.FadingIn;
        }

    }
    public void Fadeout()
    {
        if (state != ScreenState.FadingOut)
        {
            fadeTargetValue = originalIntensity;
            state = ScreenState.FadingOut;
        }
    }
    void ManageFade(float value)
    {
        darkenEffect.SetFloat("_ScreenPower", Mathf.MoveTowards(darkenEffect.GetFloat("_ScreenPower"), value, Time.deltaTime * fadeSpeed));

        if (Mathf.Abs(value - darkenEffect.GetFloat("_ScreenPower")) < 0.001f)
        {
            if (value <= 0f)
            {
                state = ScreenState.Black;
            }
            else
            {
                state = ScreenState.None;
            }
        }
    }

    public void EnableFadeInOut()
    {
        if (state != ScreenState.FadeInOut)
        {
            originalIntensity = darkenEffect.GetFloat("_ScreenPower"); //save intensity to go back to after the fade is over
            //enableHurtEffect = false;
            state = ScreenState.FadeInOut;
        }
    }
    void ManageFadeInOut()
    {
        float value = 0;
        darkenEffect.SetFloat("_ScreenPower", Mathf.MoveTowards(darkenEffect.GetFloat("_ScreenPower"), value, Time.deltaTime * fadeSpeed));

        if (Mathf.Abs(value - darkenEffect.GetFloat("_ScreenPower")) < 0.001f)
        {
            Fadeout();
        }
    }

    void ManageHurtEffect()
    {
        if (player.inLight)
        {
            darkenEffect.SetFloat("_ScreenPower", Mathf.MoveTowards(darkenEffect.GetFloat("_ScreenPower"), 5f, Time.deltaTime * fadeSpeed));
        }
        else
        {
            darkenEffect.SetFloat("_ScreenPower", Mathf.MoveTowards(darkenEffect.GetFloat("_ScreenPower"), 1.5f, Time.deltaTime * fadeSpeed));
        }

        if (player.combat.health / (float)player.combat.maxHealth <= 0.3f)
        {
            if (!flash)
            {
                count = 0;
                flash = true;
            }
            float value = amplitude * Mathf.Sin((count * speed / Mathf.PI) - Mathf.PI / 2) + (amplitude + minIntensity);
            darkenEffect.SetFloat("_ScreenPower", value);

            count += Time.deltaTime;
            //flash
        }
        else if (flash) //smooth transition to reset
        {
            float value = amplitude * Mathf.Sin(count * speed / Mathf.PI) + (amplitude + minIntensity);

            darkenEffect.SetFloat("_ScreenPower", value);
            count += Time.deltaTime;

            if (Mathf.Abs(value - minIntensity) < 0.001f)
            {
                count = 0;
                flash = false;
            }

        }
    }
}
