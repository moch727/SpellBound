using UnityEngine;

public class LightConstructor : MonoBehaviour
{
    [SerializeField] GameObject lightPrefab;
    private void OnTriggerEnter(Collider other)
    {
         if (other.gameObject.CompareTag("Terrain") && lightPrefab != null)
        {
                GameObject lightObject = GameObject.Instantiate(lightPrefab, transform.position, Quaternion.identity);
                lightObject.GetComponent<GroundLight>().attached = other.gameObject;
                GetComponent<SpellScript>().owner.GetComponent<PlayerCombatComponent>().addLight(lightObject);
        }
    }
}
