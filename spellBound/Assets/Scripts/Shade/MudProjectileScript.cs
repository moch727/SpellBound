using UnityEngine;

public class MudProjectileScript : MonoBehaviour
{
    private SpellScript script;
    private Rigidbody rb;

    [SerializeField] ParticleSystem ballEffect;
    [SerializeField] ParticleSystem flashEffect;
    [SerializeField] ParticleSystem splashEffect;
    void Awake()
    {
        script = GetComponent<SpellScript>();
        rb = GetComponent<Rigidbody>();
    }
    public void startMotion()
    {
        GameObject target = script.owner.GetComponent<AIScript>().target;

        Vector3 targetPosition = new Vector3(target.transform.position.x, 0, target.transform.position.z);
        Vector3 currentPosition = new Vector3(transform.position.x, 0, transform.position.z);

        float travellTime = Mathf.Sqrt(2 * Mathf.Abs(target.transform .position.y - transform.position.y) / 9.8f); //kinematics formula
        float velocity = Vector3.Distance(targetPosition, currentPosition) / travellTime;

        rb.linearVelocity = script.owner.transform.forward * velocity;
        transform.parent = null;
        rb.useGravity = true;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Terrain") || collision.gameObject.CompareTag("Player"))
        {
            Destroy(ballEffect);
            flashEffect.gameObject.SetActive(true);
            GameObject.Instantiate(splashEffect, transform.position, Quaternion.identity);
            GetComponent<Collider>().enabled = false;
        }
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.gameObject.CompareTag("Terrain"))
    //    {
    //        Destroy(ballEffect);
    //        flashEffect.gameObject.SetActive(true);
    //        GameObject.Instantiate(splashEffect, transform.position, Quaternion.identity);
    //        GetComponent<Collider>().enabled = false;
    //    }
    //}
}
