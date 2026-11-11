using Unity.VisualScripting;
using UnityEngine;

public class EntityHealthScript : MonoBehaviour
{
    [SerializeField] float MAXHEALTH;
    [SerializeField] float INITIALDEFENSE;
    public float health;
    private float defense;

    void Awake()
    {
        initialize();
        defense = INITIALDEFENSE;
    }

    public void initialize()
    {
        health = MAXHEALTH;
    }
    public void setMaxHealth(float newAmount)
    {
        MAXHEALTH = newAmount;
    }
    
    public void setDefense(float newAmount)
    {
        defense = newAmount;
    }
    public void increaseHealth(int amount)
    {
        if(health + amount > MAXHEALTH) health = MAXHEALTH;
        else health += amount;
    }

    public void reduceHealth(int damage)
    {
        health -=  damage * (1-defense);
        if (health <= 0)
        {
            health = 0;
        }
    }

    public float getHealth()
    {
        return health;
    }

    public float getMAXHealth()
    {
        return MAXHEALTH;
    }

    public float getHealthPercent()
    {
        return health/MAXHEALTH;
    }

    public float getDefense()
    {
        return defense;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<IHitbox>(out IHitbox hitbox) && !hitbox.isOwner(gameObject.tag))
        {
            reduceHealth((int)hitbox.getDamage());
        }
    }
}
