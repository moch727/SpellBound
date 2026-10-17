using UnityEngine;

public class LightDependentSpellScript : MonoBehaviour
{
    //Idea: Create this object that takes a spellObject and playerScript as input, placing all the spellObjects at the lights

    public SpellScript spell;
    public void CreateSpells(PlayerScript player, GameObject spellObject, GroundLight[] lights, float attackDamage)
    {
        spell = spellObject.GetComponent<SpellScript>();
        for (int i = 0; i < lights.Length; i++)
        {
            if (lights[i] != null)
            {
                SpellScript spell = GameObject.Instantiate(spellObject, lights[i].transform.position, lights[i].transform.rotation).GetComponent<SpellScript>(); //what if light is on wall?
                spell.owner = player.gameObject;
                spell.damage = attackDamage;

                for (int j = 0; j < spell.GetComponent<SpellScript>().otherProjectiles.Length; j++)
                {
                    spell.GetComponent<SpellScript>().otherProjectiles[j].GetComponent<SpellScript>().owner = gameObject;
                }

                lights[i].fadeSpeed = lights[i].GetComponent<Light>().intensity / 2f; //For removing light
                lights[i].GetComponent<SphereCollider>().radius = 0f;
            }
        }
    }

}
