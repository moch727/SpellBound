using UnityEngine;

public class SpellScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private string spellName;

    private float effectPercent;
    private float damage;

    private int[,] patternValues;

    public SpellLocomotionScript locomotion;

    void Start()
    {
        locomotion = GetComponent<SpellLocomotionScript>();
    }
    public void Instantiate(string spellName, float damage, float effectPercent, int[,] patternValues)
    {
        this.spellName = spellName;
        this.damage = damage;
        this.effectPercent = effectPercent;
        this.patternValues = patternValues;
    }

    public void castSpell(GameObject owner, float range)
    {
        locomotion.cast(owner, range);
    }
    public int[,] getPattern()
    {
        return patternValues;
    }
}
