using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
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

    private float atkMultiplier = 1;


    public GroundLight[] lights = new GroundLight[3];
    public int lightNums;
    private float timeElapsed;

    //[HideInInspector]
    public int health;
    public int LP;

    [Header("Stats")]
    public float XP;
    public float requiredXP;

    public int level;
    public int Vitality = 1; //stat for health
    public int Clarity = 1;  //stat for lp
    public int Potency = 1; //stat for attack power
    public int Resistance = 1; //stat for defense

    public int Persistence = 1; //stat for light duration

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
    
    public void reduceHealth(int reduction)
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

            if (playerScript.environmentLight != null && projectile.CompareTag("Projectile"))  projectile = playerScript.environmentLight.replacedSpell;
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
                                spell.GetComponent<SpellScript>().damage *= atkMultiplier;
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

                    if(projectile.GetComponent<SpellLocomotionScript>() != null)
                    {
                        if (projectile.GetComponent<SpellLocomotionScript>().pivot.CompareTo("MagicPivotL") == 0) pivot = attachPointL;
                        else pivot = attachPointR;

                        spellObject = GameObject.Instantiate(projectile, pivot.transform.position, playerScript.cam.transform.rotation, pivot);
                    }
                    else spellObject = GameObject.Instantiate(projectile);

                    spellObject.GetComponent<SpellScript>().owner = gameObject;
                    spellObject.GetComponent<SpellScript>().damage *= atkMultiplier;

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
        if (spellObject != null)
        {
            spellObject.GetComponent<SpellScript>().shoot();
            if(spellObject.GetComponent<SphereCollider>() != null) spellObject.GetComponent<SphereCollider>().enabled = true;
        }
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

    public void LevelUP()
    {
        if (XP <= requiredXP)
        {
            level++;
            Vitality++;
            Potency++;
            Resistance++;

            XP -= requiredXP;

            float newMaxHealth = (float) maxHealth * (Mathf.Pow(1f + (float)(Vitality / 10f), 2f));
            maxHealth = (int) newMaxHealth;
            health = maxHealth;

            atkMultiplier = Mathf.Log(Potency) + 1f;

            requiredXP *= Mathf.Pow(1f + (float)(level/ 10f), 3f);
            //requiredXP *= (1 + (level / 10));
            //change required xp for next level
        }
    }
}
