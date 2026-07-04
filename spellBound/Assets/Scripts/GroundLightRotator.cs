using UnityEngine;

public class GroundLightRotator : MonoBehaviour
{
    [SerializeField] GameObject main;
    private bool rotated;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Terrain") && !rotated)
        {
            main.transform.up = collision.contacts[0].normal;
            rotated = true;
        }
    }
}
