using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] PlayerScript player;

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
        UpdateStatBars();


        spellIcon.sprite = player.combat.currentSpell().GetComponent<SpellScript>().spellIcon;

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
