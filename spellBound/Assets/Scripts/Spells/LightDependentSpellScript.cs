using UnityEngine;

public class LightDependentSpellScript : MonoBehaviour
{
    //Idea: Create this object that takes a spellObject and playerScript as input, placing all the spellObjects at the lights

    public SpellScript spell;

    private void Start()
    {
        //script = GetComponent<SpellScript>();
        //if(script.owner != null) playerCombatComponent = script.owner.GetComponent<PlayerCombatComponent>();

        //GroundLight[] lights = playerCombatComponent.lights;
        //for (int i = 0; i < lights.Length; i++)
        //{
        //    if (lights[i] != null && Vector3.Distance(transform.position, lights[i].transform.position) > 0.0001f) //is not the same one this thing is currently on
        //    {
        //        SpellScript spell = GameObject.Instantiate(gameObject, lights[i].transform.position, lights[i].transform.rotation).GetComponent<SpellScript>(); //what if light is on wall?
        //        spell.owner = script.owner;
        //        spell.damage = script.damage;
        //        //for (int j = 0; j < spell.GetComponent<SpellScript>().otherProjectiles.Length; j++)
        //        //{
        //        //    spell.GetComponent<SpellScript>().otherProjectiles[j].GetComponent<SpellScript>().owner = gameObject;
        //        //}

        //        //lights[i].fadeSpeed = lights[i].GetComponent<Light>().intensity / 2f; //For removing light after using it
        //        //lights[i].GetComponent<SphereCollider>().radius = 0f;
        //        //incrementLights();
        //    }

        //}
    }

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
                //incrementLights();
            }

        }
    }

    }
