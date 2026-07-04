using UnityEngine;

public class MudProjectileScript : MonoBehaviour
{
    private SpellScript script;
    private Rigidbody rb;

    [SerializeField] ParticleSystem ballEffect;
    [SerializeField] ParticleSystem flashEffect;
    [SerializeField] ParticleSystem splashEffect;

    private bool collided = false;
    private float time = 3f;
    void Awake()
    {
        script = GetComponent<SpellScript>();
        rb = GetComponent<Rigidbody>();
    }
    public void startMotion()
    {
        float velocity = 5f;
        //float height = transform.position.y - script.owner.GetComponent<AIScript>().target.transform.position.y;
        rb.linearVelocity = script.owner.transform.forward * velocity;
        //rb.linearVelocity = (new Vector3(script.owner.GetComponent<AIScript>().target.transform.position.x - transform.position.x, 0, script.owner.GetComponent<AIScript>().target.transform.position.z - transform.position.z)).normalized * velocity;
        //rb.AddForce(script.owner.transform.forward * velocity, ForceMode.Impulse);
        transform.parent = null;
        rb.useGravity = true;
    }
    void Update()
    {
        //if(projectileEffect.time >= 0.7f && !collided) projectileEffect.Pause(true);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Terrain"))
        {
            Destroy(ballEffect);
            flashEffect.gameObject.SetActive(true);
            GameObject.Instantiate(splashEffect, transform.position, Quaternion.identity);
            Debug.Log("aaaaaa");
            //collided = true;
            //projectileEffect.Play();
        }
        else if (collision.gameObject.CompareTag("Terrain"))
        {
            //Destroy(ballEffect);
            //flashEffect.gameObject.SetActive(true);
            //GameObject.Instantiate(splashEffect, transform.position, Quaternion.identity);
            //Debug.Log("aaaaaa");
        }
    }
}
