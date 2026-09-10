using UnityEngine;
using UnityEngine.UI;

public class SpellScript : MonoBehaviour
{
    public enum SpellType
    {
        Projectile, 
        Construct, //For spells that are created, then projectiles are fired from it
        LightDependant
    }

    public GameObject[] otherProjectiles;
    public GameObject impactEffect;

    public string spellName;
    public SpellType spellType;
    public Sprite spellIcon;
    public int animID;

    public float damage;
    public int lightcost;
    [SerializeField] GameObject lightPrefab;
    private bool lightPlaced;

    private SpellLocomotionScript locomotion;

    //[HideInInspector]
    public GameObject owner;

    void Awake()
    {
        locomotion = GetComponent<SpellLocomotionScript>();
    }

    public void shoot()
    {
        if(GetComponent<Collider>() != null) GetComponent<Collider>().enabled = true;
        if (locomotion != null) locomotion.startMotion(owner);
        else if (GetComponent<MudProjectileScript>() != null) GetComponent<MudProjectileScript>().startMotion();
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((!other.isTrigger && (other.gameObject.CompareTag("Enemy") && owner.CompareTag("Player")) || (other.gameObject.CompareTag("Player") && owner.CompareTag("Enemy"))) || other.CompareTag("Interactable"))
        {
            if (impactEffect != null) GameObject.Instantiate(impactEffect, transform.position, transform.rotation);
            if (spellType == SpellType.Projectile) Destroy(gameObject);
        }
        else if (!owner.CompareTag("Player") && other.GetComponent<ShieldScript>() != null)
        {
            Destroy(gameObject);
        }
        else if (other.gameObject.CompareTag("Terrain"))
        {
            if (lightPrefab != null && !lightPlaced)
            {
                GameObject lightObject = GameObject.Instantiate(lightPrefab, transform.position, Quaternion.identity);
                lightObject.GetComponent<GroundLight>().attached = other.gameObject;
                owner.GetComponent<PlayerCombatComponent>().addLight(lightObject);
                lightPlaced = true;
            }
            if (spellType == SpellType.Projectile) Destroy(gameObject);

        }
    }

}
