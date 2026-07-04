using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] PlayerScript player;

    //[SerializeField] PointBar lpBar;
    //[SerializeField] PointBar extraLPBar;
    [SerializeField] Slider healthBar;
    [SerializeField] Slider LPBar;

    [SerializeField] Slider[] rings;

    [SerializeField] Image spellIcon;

    [SerializeField] GameObject interactText;

    private void Awake()
    {
        interactText.SetActive(false);
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

        spellIcon.sprite = player.combat.getCurrentSpell().GetComponent<SpellScript>().spellIcon;

        if(player.interactable != null && player.currentAction != PlayerScript.Action.Interact)
        {
            interactText.SetActive(true);
        }
        else
        {
            interactText.SetActive(false);
        }
    }

    void UpdateStatBars()
    {
        healthBar.value = Mathf.MoveTowards(healthBar.value, (float)player.combat.health / (float) player.combat.maxHealth, 2f * Time.deltaTime);
        LPBar.value = Mathf.MoveTowards(LPBar.value, (float)player.combat.LP / (float)player.combat.maxLP, 2f * Time.deltaTime);

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
