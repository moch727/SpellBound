using UnityEngine;

public class MeleeHitbox : MonoBehaviour
{
    private Rigidbody rb;
    public bool hasCollided = false;
    public AIScript owner;
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        GetComponent<Collider>().enabled = false;
    }

    public void setDamageBox(bool enable)
    {
        GetComponent<Collider>().enabled = enable;
        if (!GetComponent<Collider>().enabled) hasCollided = false;
    }

    public float getDamage()
    {
        //return owner.currentAttack.attackDamage;
        return owner.getDamage();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player")) hasCollided = true;
    }
}
