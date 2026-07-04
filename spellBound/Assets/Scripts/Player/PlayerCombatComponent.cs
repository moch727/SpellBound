using Unity.VisualScripting;
using UnityEngine;

public class PlayerCombatComponent : MonoBehaviour
{
    private PlayerScript playerScript;
    private SpellManager spellManager;

    public GameObject[] spells;
    [SerializeField] int numOfSpells = 1;
    public int index = 0;


    [SerializeField] Transform attachPointL;
    public Transform attachPointR;

    private GameObject spellObject;
    private bool spellActive = false;

    public int maxHealth;
    public int maxLP;


    public GroundLight[] lights = new GroundLight[3];
    public int lightNums;
    private float timeElapsed;

    //[HideInInspector]
    public int health;
    public int LP;

    void Awake()
    {
        playerScript = GetComponent<PlayerScript>();
        health = maxHealth;
        LP = maxLP;
    }
    private void Update()
    {
        if (playerScript.inLight)
        {
            timeElapsed += Time.deltaTime;
            if (timeElapsed >= 1) //1 is lp recovery interval
            {
                if (health < maxHealth) health += 1;
                else if (LP < maxLP) LP += 1; //combat.maxExtraLP
                timeElapsed = 0f;
            }
        }
    }
    public GameObject currentSpell() 
    {
        return spells[index];
    }
    public void spellChange(int index)
    {
        if(index < numOfSpells) this.index = index;
    }

    public void addSpell(GameObject spell)
    {
        spells[index+1] = spell;
        numOfSpells++;
    }
    public bool getSpellActive()
    {
        spellActive = (spellObject != null);
        return spellActive;
    }
    public GameObject getCurrentSpell()
    {
        return spells[index];
    }
    
    public void reduceLP(int reduction)
    {
        health -= reduction;
        if(health < 0) health = 0;
    }
    public void cast()
    {
        if (health >= spells[index].GetComponent<SpellScript>().lightcost)
        {
            GameObject projectile;
            projectile = spells[index];
            Transform pivot;

            switch (projectile.GetComponent<SpellScript>().spellType)
            {
                case (SpellScript.SpellType.LightDependant):
                    if(lightNums > 0 && (spellObject == null || (spellObject != null && projectile.GetComponent<SpellScript>().spellName != spellObject.GetComponent<SpellScript>().spellName))) //To prevent spamming of spikes while one is active
                    { //Only tr
                        //(spellObject == null || (spellObject != null && spellObject.GetComponent<SpellScript>().spellType != SpellScript.SpellType.LightDependant))
                        GameObject spell = null;
                        for (int i = 0; i < lights.Length; i++)
                        {
                            if (lights[i] != null)
                            {
                                spell = GameObject.Instantiate(projectile, lights[i].transform.position, lights[i].transform.rotation); //what if light is on wall?
                                spell.GetComponent<SpellScript>().owner = gameObject;
                                for (int j = 0; j < spell.GetComponent<SpellScript>().otherProjectiles.Length; j++)
                                {
                                    spell.GetComponent<SpellScript>().otherProjectiles[j].GetComponent<SpellScript>().owner = gameObject;
                                }

                                lights[i].fadeSpeed = lights[i].GetComponent<Light>().intensity / 2f;
                                lights[i].GetComponent<SphereCollider>().radius = 0f;
                                incrementLights();
                            }

                        }
                        spellObject = spell;
                    }
                    break;

                default:
                    if (projectile.GetComponent<SpellLocomotionScript>().pivot.CompareTo("MagicPivotL") == 0) pivot = attachPointL;
                    else pivot = attachPointR;

                    spellObject = GameObject.Instantiate(projectile, pivot.transform.position, playerScript.cam.transform.rotation, pivot);
                    spellObject.GetComponent<SpellScript>().owner = gameObject;
                    break;
            }

            if(spellObject != null)
            {
                LP -= projectile.GetComponent<SpellScript>().lightcost;
                if (LP < 0)
                {
                    health += LP;
                    LP = 0;
                }

                if (health < 0) health = 0;
            }

        }
    }

    public void attack()
    {
        if (spellObject != null) spellObject.GetComponent<SpellScript>().shoot();
    }

    public void addLight(GameObject o)
    {
        int count = 0;
        for (int i = 0; i < lights.Length; i++)
        {
            if (lights[i] != null) count++;
        }
        if(count != lightNums) lightNums = count;

        if(lightNums >= 3)
        {
            //incrementLights();
            lights[0].fadeSpeed = 50;
            lights[0] = null;
            incrementLights();
            lightNums--;
        }
        lights[lightNums] = o.GetComponent<GroundLight>();
        lightNums++;
    }

    private void incrementLights()
    {
        //for(int i = 0; i < lightNums; i++)
        //{
        //    if (lights[i] == null) lightNums--;
        //}
        for (int i = 0; i < lights.Length - 1; i++)
        {
            if (lights[i] == null)
            {
                lights[i] = lights[i + 1];
                lights[i + 1] = null;
            }
        }
    }
}
