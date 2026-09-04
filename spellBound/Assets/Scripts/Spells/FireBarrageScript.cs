using UnityEngine;

public class FireBarrageScript : MonoBehaviour
{
    [SerializeField] GameObject projectile;
    [SerializeField] ParticleSystem effect;

    [SerializeField] Transform minTransform;
    [SerializeField] Transform maxTransform;

    private SpellScript spellScript;

    private Vector3 targetLocation;

    public float FIREINTERVAL;
    public float DURATION;

    private float totalTime;
    private float timeElapsed;

    private void Start()
    {
        spellScript = GetComponent<SpellScript>();

        ParticleSystem.MainModule main = effect.main;
        effect.Stop();
        main.duration = DURATION;
        main.startLifetime = DURATION;
        effect.Play();

        //transform.rotation = spellScript.owner.GetComponent<PlayerScript>().cam.transform.rotation;
        targetLocation = spellScript.owner.GetComponent<PlayerScript>().cam.transform.position + spellScript.owner.GetComponent<PlayerScript>().cam.transform.forward * 3f;
        transform.position = targetLocation;
        transform.parent = null;

        Vector3 lookPosition = spellScript.owner.transform.position;
        lookPosition.y = targetLocation.y;
        transform.LookAt(lookPosition, Vector3.up);
        transform.Rotate(Vector3.up * 180f);
    }
    void Update()
    {
        //if(transform.parent == null) transform.position = Vector3.MoveTowards(transform.position, targetLocation, Time.deltaTime * 3f);

        if(Vector3.Distance(transform.position, targetLocation) < 0.1f)
        {
            timeElapsed += Time.deltaTime;
            totalTime += Time.deltaTime;
            if (totalTime > DURATION)
            {
                Destroy(gameObject);
            }
            else if (timeElapsed > FIREINTERVAL)
            {

                GameObject spellObject;
                float x = Random.Range(minTransform.position.x, maxTransform.position.x);
                float y = Random.Range(minTransform.position.y, maxTransform.position.y);
                Vector3 randomPosition = new Vector3(x, y, transform.position.z);

                spellObject = GameObject.Instantiate(projectile, randomPosition, transform.rotation);
                spellObject.transform.position += transform.forward * 1f;
                spellObject.GetComponent<SpellScript>().owner = gameObject;

                spellObject.GetComponent<SpellScript>().shoot();
                timeElapsed = 0;
            }
        }

    }
}
