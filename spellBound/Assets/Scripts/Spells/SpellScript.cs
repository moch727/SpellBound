using UnityEngine;

public class SpellScript : MonoBehaviour
{
    public enum SpellType
    {
        Projectile, 
        Construct, //For spells that are created, then projectiles are fired from it
    }

    public GameObject owner;
    public GameObject otherProjectiles;

    private string spellName;
    public SpellType spellType;

    private float effectPercent;
    private float damage;
    public int lightcost;
    [SerializeField] GameObject lightPrefab;

    private int[,] patternValues;

    private SpellLocomotionScript locomotion;

    void Awake()
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

    public void shoot()
    {
        locomotion.startMotion(owner);
    }
    public int[,] getPattern()
    {
        return patternValues;
    }

    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.CompareTag("Destructable"))
        {
            collision.gameObject.GetComponent<InteractableObject>().setOnFire();
            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("HIT");
            Destroy(gameObject);
        }
        else
        {
            if (lightPrefab != null) GameObject.Instantiate(lightPrefab, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }

}
