using UnityEngine;

public interface IHitbox
{
    // bool collided();
    float getDamage();
    bool isOwner(string target);
}

public class MeleeHitbox : MonoBehaviour, IHitbox
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
        return owner.getDamage();
    }

    public bool isOwner(string target)
    {
        return owner.CompareTag(target);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player")) hasCollided = true;
    }
}
