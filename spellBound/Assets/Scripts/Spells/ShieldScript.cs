using UnityEngine;

public class ShieldScript : MonoBehaviour
{
    [SerializeField] GameObject spawnEffect;
    [SerializeField] float dissolveTargetValue;
    private Material material;
    private ParticleSystem effect;

    private Vector3 endPosition;
    private bool spawnComplete;
    private bool startDissolve;
    void Start()
    {
        effect = GameObject.Instantiate(spawnEffect, transform.position + transform.up * 0.25f, transform.rotation).GetComponent<ParticleSystem>();

        if (effect.transform.rotation.eulerAngles.x >= 90 || effect.transform.rotation.eulerAngles.z >= 90)
        {
            transform.Rotate(transform.forward * 90f);
        }
        else
        {
            transform.rotation = Quaternion.LookRotation(-(GetComponent<SpellScript>().owner.transform.position - transform.position).normalized);
        }

        //transform.rotation = Quaternion.LookRotation(-new Vector3(GetComponent<SpellScript>().owner.transform.position.x - transform.position.x, 0, GetComponent<SpellScript>().owner.transform.position.z - transform.position.z).normalized);
        transform.Rotate(Vector3.right * -90f);

        endPosition = transform.position - transform.forward * 0.5f;
        
        transform.position -= transform.forward * 2f;
        material = GetComponent<MeshRenderer>().material;
        
    }
    void FixedUpdate()
    {
        if (!spawnComplete)
        {
            if (Mathf.Abs(Vector3.Distance(transform.position, endPosition)) > 0.1f)
            {
                transform.position = Vector3.Slerp(transform.position, endPosition, 0.8f * Time.deltaTime);
            }
            else
            {
                Destroy(effect.gameObject);
                spawnComplete = true;
            }
        }
        else if(startDissolve)
        {
            GetComponent<Collider>().enabled = false;
            float currentHeight = material.GetFloat("_CutOffHeight");
            if (Mathf.Abs(currentHeight - dissolveTargetValue) > 0.1f) material.SetFloat("_CutOffHeight", Mathf.Lerp(currentHeight, dissolveTargetValue, Time.deltaTime));
            else
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Projectile") && other.GetComponent<SpellScript>().owner.CompareTag("Enemy"))
        {
            startDissolve = true;
        }
    }
}
