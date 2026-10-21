using System.Linq;
using UnityEngine;

public class LightDependentSpellScript : MonoBehaviour
{
    //Idea: Create this object that takes a spellObject and playerScript as input, placing all the spellObjects at the lights

    public SpellScript spell;
    public void CreateSpells(GameObject player, GameObject spellObject, Transform[] locations, float attackDamage)
    {
        spell = spellObject.GetComponent<SpellScript>();
        foreach(Transform t in locations.Where(t => t != null))
        {
            SpellScript spell = GameObject.Instantiate(spellObject, t.position, t.rotation).GetComponent<SpellScript>(); //what if light is on wall?
            spell.owner = player;
            spell.damage = attackDamage;

            for (int j = 0; j < spell.GetComponent<SpellScript>().otherProjectiles.Length; j++)
            {
                spell.GetComponent<SpellScript>().otherProjectiles[j].GetComponent<SpellScript>().owner = gameObject;
            }
        }
    }

}
