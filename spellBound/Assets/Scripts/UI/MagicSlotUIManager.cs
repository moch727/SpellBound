using UnityEngine;
using UnityEngine.UI;

public class MagicSlotUIManager : MonoBehaviour
{
    [SerializeField] Image currentSpell;

    [SerializeField] Image[] spellList;
    [SerializeField] Image[] slotList;

    [SerializeField] Sprite defaultSlot;
    [SerializeField] Sprite selectedSlot;
    public void UpdateSpellSlots(int current, GameObject[] spells)
    {
        currentSpell.sprite = spells[current].GetComponent<SpellScript>().spellIcon;
        for (int i = 0; i < spellList.Length; i++)
        {
            spellList[i].sprite = spells[i].GetComponent<SpellScript>().spellIcon;
            Color transparency = Color.white;
            if (i == current)
            {
                slotList[i].sprite = selectedSlot;
                transparency.a = 1f;
            }
            else
            {
                slotList[i].sprite = defaultSlot;
                transparency.a = 0.2f;
            }

            spellList[i].color = transparency;
        }

    }
}
