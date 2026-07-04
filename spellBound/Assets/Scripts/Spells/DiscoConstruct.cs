using UnityEngine;

public class DiscoConstruct : MonoBehaviour
{
    private Rigidbody rb;
    [SerializeField] GameObject projectile;

    public float FIREINTERVAL;
    public float DURATION;

    private float totalTime;
    private float timeElapsed;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        this.enabled = false;
    }
    void Update()
    {
        transform.rotation = Random.rotation;
        timeElapsed += Time.deltaTime;
        totalTime += Time.deltaTime;
        if (totalTime > DURATION)
        {
            Destroy(gameObject);
        }
        else if (timeElapsed > FIREINTERVAL)
        {

            GameObject spellObject;
            spellObject = GameObject.Instantiate(projectile, transform.position, transform.rotation);
            spellObject.transform.position += transform.forward * 1f;
            spellObject.GetComponent<SpellScript>().owner = gameObject;

            spellObject.GetComponent<SpellScript>().shoot();
            timeElapsed = 0;
        }
    }
}
