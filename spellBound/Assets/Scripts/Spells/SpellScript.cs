using UnityEngine;
public interface ISpellBehaviour
{
    void triggerSpell(GameObject owner);
}
public class SpellScript : MonoBehaviour, IHitbox
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

    //[HideInInspector]
    public GameObject owner;
    public float getDamage()
    {
        return damage;
    }

    public bool isOwner(string target)
    {
        return owner.CompareTag(target);
    }

    public void shoot()
    {
        if(GetComponent<Collider>() != null) GetComponent<Collider>().enabled = true;
        ISpellBehaviour[] spellBehaviours = GetComponents<ISpellBehaviour>();
        foreach(ISpellBehaviour i in spellBehaviours)
        {
            i.triggerSpell(owner);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<EntityHealthScript>() != null && ((other.gameObject.CompareTag("Enemy") && owner.CompareTag("Player")) || (other.gameObject.CompareTag("Player") && owner.CompareTag("Enemy"))))
        {
            if (impactEffect != null) GameObject.Instantiate(impactEffect, transform.position, transform.rotation);
            if (spellType == SpellType.Projectile) Destroy(gameObject);
        }
        else if (!owner.CompareTag("Player") && other.GetComponent<ShieldScript>() != null)
        {
            Destroy(gameObject);
        }
        else if (other.gameObject.CompareTag("Terrain")  || other.CompareTag("Interactable") && !other.isTrigger)
        {
            if (spellType == SpellType.Projectile) Destroy(gameObject);

        }
    }

}
