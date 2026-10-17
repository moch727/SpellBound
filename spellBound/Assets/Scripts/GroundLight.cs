using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class GroundLight : MonoBehaviour
{
    [SerializeField] int maxlightlvl;
    [SerializeField] float duration;
    [SerializeField] float fadeTime;
    public float fadeSpeed;
    private float timeElapsed;
    [HideInInspector]
    public float fadePercent;

    private Light attachedlight;
    private SphereCollider sCollider;

    public float radii;

    //[SerializeField] float LPINTERVAL;
    public GameObject attached;

    public bool playerEntered;
    void Awake()
    {
        attachedlight = GetComponent<Light>();
        sCollider = GetComponent<SphereCollider>();

        radii = attachedlight.range;

        //sCollider.radius = radii * 2 / 3;
        sCollider.radius = 3.5f;

        fadeSpeed = attachedlight.intensity / duration;
    }

    private void Update()
    {
        timeElapsed += Time.deltaTime;
        fadePercent = 1 -  timeElapsed / duration;
        
        attachedlight.intensity -= fadeSpeed * Time.deltaTime;
        if (sCollider.radius > 1)
        {
            sCollider.radius = fadePercent * 3.5f; //radii
        }
        attachedlight.range = fadePercent * 4.5f;

        if (attachedlight.intensity <= 0)
        {
            sCollider.radius = 0f;
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        transform.up = collision.contacts[0].normal;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerEntered = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerEntered = false;
        }
    }
}
