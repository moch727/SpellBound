using UnityEngine;

public class PlayerCombatComponent : MonoBehaviour
{
    private PlayerScript playerScript;
    private SpellManager spellManager;

    [SerializeField] GameObject[] level1Projectiles;
    [SerializeField] GameObject[] level2Projectiles;
    private int index = 0;

    private GameObject spellObject;
    private bool spellActive = false;

    private bool isAttacking = false;

    public int maxLP = 10;
    public int lp = 10;
    void Awake()
    {
        playerScript = GetComponent<PlayerScript>();
        //spellManager = GetComponent<SpellManager>();
    }

    public bool getSpellActive()
    {
        spellActive = (spellObject != null);
        return spellActive;
    }
    public void setIsAttacking(bool isAttacking)
    {
        this.isAttacking = isAttacking;
    }
    public void attack()
    {
        if(lp >= level1Projectiles[index].GetComponent<SpellScript>().lightcost)
        {
            Transform pivot = GameObject.Find("MagicPivot").transform;

            GameObject projectile;
            if (playerScript.getLightlvl() == 2) projectile = level2Projectiles[index];
            else projectile = level1Projectiles[index];


            spellObject = GameObject.Instantiate(projectile, pivot.transform.position, playerScript.cam.transform.rotation, pivot);
            spellObject.transform.position += playerScript.cam.transform.forward * 1;
            spellObject.GetComponent<SpellScript>().owner = playerScript.cam;


            spellObject.GetComponent<SpellScript>().shoot();

            lp -= spellObject.GetComponent <SpellScript>().lightcost;
            if (lp < 0) lp = 0;
        }
    }
}
