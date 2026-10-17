using System.Collections;
using UnityEngine;

public class FireBarrageScript : MonoBehaviour
{
    [SerializeField] GameObject projectile;

    //TODO: Make it so you can control the projectile travel distance in this script, so you can time it with the spell circle shrink to prevent errors with the projectile remaining but the circle being destroyed
    [SerializeField] ParticleSystem effect;

    [SerializeField] Transform minTransform;
    [SerializeField] Transform maxTransform;

    private SpellScript spellScript;

    private Vector3 targetLocation;

    public float FIREINTERVAL;
    public float DURATION;
    private bool enableFire = true;

    private void Start()
    {
        spellScript = GetComponent<SpellScript>();

        ParticleSystem.MainModule main = effect.main;
        effect.Stop();
        main.duration = DURATION;
        main.startLifetime = DURATION;
        effect.Play();

        targetLocation = spellScript.owner.GetComponent<PlayerScript>().cam.transform.position + spellScript.owner.GetComponent<PlayerScript>().cam.transform.forward * 3f;
        transform.position = targetLocation;
        transform.parent = null;

        Vector3 lookPosition = spellScript.owner.transform.position;
        lookPosition.y = targetLocation.y;
        transform.LookAt(lookPosition, Vector3.up);
        transform.Rotate(Vector3.up * 180f);

        StartCoroutine(FireProjectile());
        StartCoroutine(Duration());
    }
    IEnumerator FireProjectile()
    {
        while (enableFire)
        {
            GameObject spellObject;
            float x = Random.Range(minTransform.position.x, maxTransform.position.x);
            float y = Random.Range(minTransform.position.y, maxTransform.position.y);
            Vector3 randomPosition = new Vector3(x, y, transform.position.z);

            spellObject = GameObject.Instantiate(projectile, randomPosition, transform.rotation);
            spellObject.transform.position += transform.forward * 1f;
            spellObject.GetComponent<SpellScript>().owner = gameObject;

            spellObject.GetComponent<SpellScript>().shoot();
            yield return new WaitForSeconds(FIREINTERVAL);
        }

    }
    IEnumerator Duration()
    {
        yield return new WaitForSeconds(DURATION); //Give time for all the projectiles to complete their fire
        enableFire = false;

        yield return new WaitForSeconds(2.5f);
        Destroy(gameObject);
    }
}
