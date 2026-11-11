using System.Collections;
using UnityEngine;

public class DiscoConstruct : MonoBehaviour
{
    [SerializeField] GameObject projectile;

    public float FIREINTERVAL;
    public float DURATION;
    private bool stopFire = false;

    private float totalTime;
    private float timeElapsed;

    private void Start()
    {
        this.enabled = false;

        StartCoroutine(LaunchProjectiles());
        StartCoroutine(SpellTerminate());
    }
    void Update()
    {
        transform.rotation = Random.rotation;
        // timeElapsed += Time.deltaTime;
        // totalTime += Time.deltaTime;
        // if (totalTime > DURATION)
        // {
        //     Destroy(gameObject);
        // }
        // else if (timeElapsed > FIREINTERVAL)
        // {
        //     GameObject spellObject;
        //     spellObject = GameObject.Instantiate(projectile, transform.position, transform.rotation);
        //     spellObject.transform.position += transform.forward * 1f;
        //     spellObject.GetComponent<SpellScript>().owner = gameObject;

        //     spellObject.GetComponent<SpellScript>().shoot();

        //     timeElapsed = 0;
        // }
    }

    IEnumerator LaunchProjectiles()
    {
        while (!stopFire)
        {
            GameObject spellObject;
            spellObject = GameObject.Instantiate(projectile, transform.position, transform.rotation);
            spellObject.transform.position += transform.forward * 1f;
            spellObject.GetComponent<SpellScript>().owner = gameObject;

            spellObject.GetComponent<SpellScript>().shoot();

            yield return new WaitForSeconds(FIREINTERVAL);
        }
    }
    IEnumerator SpellTerminate()
    {
        yield return new WaitForSeconds(DURATION);
        stopFire = true;
        Destroy(gameObject);
    }
}
