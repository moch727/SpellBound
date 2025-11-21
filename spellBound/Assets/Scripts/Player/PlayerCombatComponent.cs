using UnityEngine;

public class PlayerCombatComponent : MonoBehaviour
{
    private PlayerScript playerScript;
    private SpellManager spellManager;

    [SerializeField] private GameObject FireBall;

    private SpellScript currentSpell;
    private GameObject spellObject;

    private bool spellActive = false;

    private bool isAttacking = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerScript = GetComponent<PlayerScript>();
        spellManager = GetComponent<SpellManager>();
    }

    private void Update()
    {
        if (spellObject != null && !spellActive)
        {
            spellObject.GetComponent<SpellScript>().castSpell(transform.gameObject, 10);
            spellActive = true;
        }
    }
    public bool getSpellActive()
    {
        return spellActive;
    }
    public void setSpellActive(bool spellActive)
    {
        this.spellActive = spellActive;
    }
    public bool getIsAttacking()
    {
        return isAttacking;
    }
    public void setIsAttacking(bool isAttacking)
    {
        this.isAttacking = isAttacking;
    }
    public void createSpell(string spellName)
    {
        currentSpell = spellManager.generateSpell(spellName);
    }

    public SpellScript getCurrentSpell()
    {
        return currentSpell;
    }

    public void attack()
    {
        Transform pivot = GameObject.Find("MagicPivot").transform;
        spellObject = SpellScript.Instantiate(FireBall, pivot.transform.position, Quaternion.identity, pivot);


    }
}
