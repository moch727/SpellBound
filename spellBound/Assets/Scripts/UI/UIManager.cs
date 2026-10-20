using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] PlayerScript player;

    [Header("Main UI")]
    public Canvas gameplayUI;
    [SerializeField] Slider healthBar;
    [SerializeField] Slider LPBar;
    [SerializeField] TextMeshProUGUI xpCOllected;

    [SerializeField] Slider[] rings;

    [Header("MagicSlots")]
    [SerializeField] MagicSlotUIManager magicSlotUIManager;
    [SerializeField] Image spellIcon;

    [SerializeField] GameObject interactText;


    [Header("FurnaceScreen")]
    [SerializeField] FurnaceScreenManager furnaceScreenManager;

    [Header("Death Screen")]
    [SerializeField] Canvas deathUI;
    [SerializeField] Button continueButton;

    [SerializeField] ScreenFXController screenFXController;
    private Stack<GameObject> panelStack = new Stack<GameObject>();
    private void Awake()
    {
        interactText.SetActive(false);
        deathUI.gameObject.SetActive(false);

        healthBar.GetComponent<RectTransform>().sizeDelta = new Vector2(10 * player.combat.maxHealth, healthBar.GetComponent<RectTransform>().sizeDelta.y);
        LPBar.GetComponent<RectTransform>().sizeDelta = new Vector2(15 * player.combat.maxLP, LPBar.GetComponent<RectTransform>().sizeDelta.y);


        //lpBar.setMaxValue(player.combat.maxLP);
        //extraLPBar.setMaxValue(player.combat.maxLP);
    }
    void LateUpdate()
    {
        UpdateStatBars();
        magicSlotUIManager.UpdateSpellSlots(player.combat.currentIndex(), player.combat.spells);

        if(player.interactable != null && player.currentAction != PlayerScript.Action.Interact)
        {
            interactText.SetActive(true);
        }
        else
        {
            interactText.SetActive(false);
        }

        if(player.currentAction == PlayerScript.Action.Dead && screenFXController.state == ScreenFXController.ScreenState.Black)
        {
            deathUI.gameObject.SetActive(true);
            UpdateDeathScreen();
        }
    }

    void UpdateStatBars()
    {
        healthBar.GetComponent<RectTransform>().sizeDelta = new Vector2(10 * player.combat.maxHealth, healthBar.GetComponent<RectTransform>().sizeDelta.y);
        LPBar.GetComponent<RectTransform>().sizeDelta = new Vector2(15 * player.combat.maxLP, LPBar.GetComponent<RectTransform>().sizeDelta.y);

        healthBar.value = Mathf.MoveTowards(healthBar.value, (float)player.combat.health / (float)player.combat.maxHealth, 2f * Time.deltaTime);
        LPBar.value = Mathf.MoveTowards(LPBar.value, (float)player.combat.LP / (float)player.combat.maxLP, 2f * Time.deltaTime);

        xpCOllected.text = ((int)player.combat.XP).ToString();

        for (int i = 0; i < rings.Length; i++)
        {
            if (player.combat.lights[i] == null) rings[i].value = 0;
            else
            {
                rings[i].value = player.combat.lights[i].fadePercent;
            }
        }
    }

    void UpdateSpellSlots()
    {
        spellIcon.sprite = player.combat.currentSpell().GetComponent<SpellScript>().spellIcon;
    }

    public void ShowFurnaceBox()
    {
        furnaceScreenManager.DisplayFurnacePanel(player.combat);
        // Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
    }

    public void ExitFurnaceBox()
    {
        player.animator.animateTrigger("Resume");

        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
    }
    void UpdateDeathScreen()
    {
        Cursor.lockState = CursorLockMode.None;
    }

    public void Continue()
    {
        Cursor.lockState = CursorLockMode.Locked;
        player.ResetCharacter();
        screenFXController.Fadeout();
        deathUI.gameObject.SetActive(false);
        gameplayUI.gameObject.SetActive(true);
    }
}
