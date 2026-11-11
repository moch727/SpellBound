using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using Unity.VisualScripting;

public class FurnaceScreenManager : MonoBehaviour
{
    private Canvas canvas;
    private PlayerCombatComponent playerCombat;
    [SerializeField] UIManager uIManager;

    [Header("Windows")]
    [SerializeField] GameObject optionBox;
    [SerializeField] GameObject levelUpBox;
    private Stack<GameObject> panelStack = new Stack<GameObject>();

    [Header("Level UI")]
    [SerializeField] Button levelUP;
    [SerializeField] TextMeshProUGUI XPText;
    [SerializeField] TextMeshProUGUI recoverPercentText;
    [SerializeField] GameObject levelUpEffect;

    void Awake()
    {
        canvas = GetComponent<Canvas>();
        canvas.enabled = false;
        levelUpBox.SetActive(false);
    }

    public void DisplayFurnacePanel(PlayerCombatComponent playerCombat)
    {
        this.playerCombat = playerCombat;
        canvas.enabled = true;
        optionBox.SetActive(true);
        panelStack.Push(optionBox);
    }

    public void InitiateLevelScreen()
    {
        optionBox.SetActive(false);
        levelUpBox.SetActive(true);
        panelStack.Push(levelUpBox);
        UpdateLevelScreen();
    }

    public void ExitBox()
    {
        panelStack.Pop().SetActive(false);
        if(panelStack.Count <= 0)
        {
            canvas.enabled = false;
            uIManager.ExitFurnaceBox();
        }
        else
        {
            panelStack.Peek().SetActive(true);
        }
    }

    void UpdateLevelScreen()
    {
        levelUP.interactable = playerCombat.XP >= playerCombat.requiredXP;
        XPText.text = ((int)playerCombat.XP).ToString() + "/" + ((int)playerCombat.requiredXP).ToString();
        recoverPercentText.text = playerCombat.level.ToString();
    }

    public void LevelUP()
    {
        playerCombat.LevelUP();
        UpdateLevelScreen();
        GameObject.Instantiate(levelUpEffect, playerCombat.transform.position, Quaternion.identity);
    }
}


