using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Material material;
    private float destroyCount;
    private bool onFire = false;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (onFire)
        {
            destroyCount += Time.deltaTime;
        }

        if(destroyCount > 2) //For burn effect
        {
            Destroy(gameObject);
        }
    }

    public void setOnFire()
    {
        gameObject.GetComponent<MeshRenderer>().material = material;
        destroyCount = 0;
        onFire = true;
    }
}
