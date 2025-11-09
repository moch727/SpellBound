using UnityEngine;

public class PlayerCombatComponent : MonoBehaviour
{
    private PlayerScript playerScript;
    private SpellManager spellManager;

    [SerializeField] private GameObject FireBall;

    private SpellScript currentSpell;
    private GameObject spellObject;

    private bool isAttacking = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerScript = GetComponent<PlayerScript>();
        spellManager = GetComponent<SpellManager>();
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
}
