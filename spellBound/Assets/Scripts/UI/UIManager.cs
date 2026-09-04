using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] PlayerScript player;

    [Header("FullScreen Effect")]
    [SerializeField] ScriptableRendererFeature effectRenderer;
    [SerializeField] float amplitude;
    [SerializeField] float speed;
    [SerializeField] float minIntensity;
    [SerializeField] Material darkenEffect;
    private bool flash;
    private float count;

    //For Fade in and out
    [HideInInspector] public bool fading;
    [SerializeField] float fadeSpeed;
    private float originalIntensity;
    private bool inComplete;

    //[SerializeField] PointBar lpBar;
    //[SerializeField] PointBar extraLPBar;
    [Header("Main UI")]
    [SerializeField] Slider healthBar;
    [SerializeField] Slider LPBar;
    [SerializeField] TextMeshProUGUI xpCOllected;

    [SerializeField] Slider[] rings;

    [SerializeField] Image spellIcon;

    [SerializeField] GameObject interactText;


    [SerializeField] Canvas furnaceScreen;

    [SerializeField] GameObject optionBox;
    [SerializeField] GameObject levelUpBox;

    [Header("Level UI")]
    [SerializeField] Button levelUP;
    [SerializeField] TextMeshProUGUI XPText;
    [SerializeField] TextMeshProUGUI recoverPercentText;
    [SerializeField] GameObject levelUpEffect;
    public int availablePoints;

    private Stack<GameObject> panelStack = new Stack<GameObject>();
    private void Awake()
    {
        interactText.SetActive(false);
        furnaceScreen.gameObject.SetActive(false);
        levelUpBox.SetActive(false);

        healthBar.GetComponent<RectTransform>().sizeDelta = new Vector2(10 * player.combat.maxHealth, healthBar.GetComponent<RectTransform>().sizeDelta.y);
        LPBar.GetComponent<RectTransform>().sizeDelta = new Vector2(15 * player.combat.maxLP, LPBar.GetComponent<RectTransform>().sizeDelta.y);
        //lpBar.setMaxValue(player.combat.maxLP);
        //extraLPBar.setMaxValue(player.combat.maxLP);
    }
    void LateUpdate()
    {
        //lpBar.setValue(player.combat.lp);
        //extraLPBar.setValue(player.combat.lp + player.combat.extraLP);
        UpdateStatBars();
        //UpdateFullScreenEffect();


        spellIcon.sprite = player.combat.getCurrentSpell().GetComponent<SpellScript>().spellIcon;

        if(player.interactable != null && player.currentAction != PlayerScript.Action.Interact)
        {
            interactText.SetActive(true);
        }
        else
        {
            interactText.SetActive(false);
        }

        if(levelUpBox.activeSelf) UpdateLevelScreen(); 
    }

    void UpdateFullScreenEffect()
    {
        if (fading) ManageFade();
        else ManageHurtEffect();

    }
    public void EnableFadeInOut() 
    {
        if (!fading)
        {
            originalIntensity = darkenEffect.GetFloat("_ScreenPower"); //save intensity to go back to after the fade is over
            fading = true;
        }
    }
    void ManageFade()
    {
        float value = 0;
        if (inComplete) value = originalIntensity; //If the fade in portion is complete, fade out to the original intensity
        darkenEffect.SetFloat("_ScreenPower", Mathf.MoveTowards(darkenEffect.GetFloat("_ScreenPower"), value, Time.deltaTime * fadeSpeed));

        if (Mathf.Abs(value - darkenEffect.GetFloat("_ScreenPower")) < 0.001f)
        {
            if (!inComplete)
            {
                fading = false;
                inComplete = true;
            }
            else
            {
                inComplete = false;
            }
        }
    }
    void ManageHurtEffect()
    {
        if ( player.combat.health / (float) player.combat.maxHealth <= 0.3f)
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
        else if (flash)
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
        else
        {
            darkenEffect.SetFloat("_ScreenPower", 1.5f);
        }
    }
    public void ShowFurnaceBox()
    {
        furnaceScreen.gameObject.SetActive(true);
        optionBox.SetActive(true);
        panelStack.Push(optionBox);

        //Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
    }
    public void ExitBox()
    {
        panelStack.Pop().SetActive(false);
        if(panelStack.Count <= 0)
        {
            furnaceScreen.gameObject.SetActive(false);
            player.animator.animateInteract("Resume");

            Time.timeScale = 1;
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            panelStack.Peek().SetActive(true);
        }
    }

    public void InitiateLevelScreen()
    {
        optionBox.SetActive(false);
        levelUpBox.SetActive(true);
        panelStack.Push(levelUpBox);

        //availablePoints = player.combat.XP;
    }
    void UpdateLevelScreen()
    {
        levelUP.interactable = (player.combat.XP >= player.combat.requiredXP);
        XPText.text = ((int)player.combat.XP).ToString() + "/" + ((int)player.combat.requiredXP).ToString();
        recoverPercentText.text = player.combat.level.ToString();

    }
    public void LevelUP()
    {
        player.combat.LevelUP();
        GameObject.Instantiate(levelUpEffect, player.transform.position, Quaternion.identity);
    }

    //public bool HasMaxPoints()
    //{
    //    return availablePoints == player.combat.XP;
    //}
    void UpdateStatBars()
    {
        healthBar.GetComponent<RectTransform>().sizeDelta = new Vector2(10 * player.combat.maxHealth, healthBar.GetComponent<RectTransform>().sizeDelta.y);
        LPBar.GetComponent<RectTransform>().sizeDelta = new Vector2(15 * player.combat.maxLP, LPBar.GetComponent<RectTransform>().sizeDelta.y);

        healthBar.value = Mathf.MoveTowards(healthBar.value, (float)player.combat.health / (float) player.combat.maxHealth, 2f * Time.deltaTime);
        LPBar.value = Mathf.MoveTowards(LPBar.value, (float)player.combat.LP / (float)player.combat.maxLP, 2f * Time.deltaTime);

        xpCOllected.text = ((int) player.combat.XP).ToString();

        for (int i = 0; i < rings.Length; i++)
        {
            if (player.combat.lights[i] == null) rings[i].value = 0;
            else
            {
                rings[i].value = player.combat.lights[i].fadePercent;
            }
        }
    }
}
