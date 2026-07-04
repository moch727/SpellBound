using UnityEngine;

public class ItemScript : MonoBehaviour
{
    public GameObject item;

    public GameObject collect()
    {
        GetComponent<ParticleSystem>().Stop();
        Destroy(gameObject);
        return item;
    }
}
