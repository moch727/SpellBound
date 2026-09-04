using System.Runtime.CompilerServices;
using UnityEngine;

public class WaveSrcScript : MonoBehaviour
{
    public GolemScript golemOwner;

    public Vector3 targetLocation;
    [SerializeField] ParticleSystem particleEffect;

    [SerializeField] GameObject hitEffect;

    //public bool destroyed;

    public void InitiateSrc()
    {
        ParticleSystem.MainModule main = particleEffect.main;
        main.startLifetime = golemOwner.waveDuration;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<SpellScript>() != null && other.GetComponent<SpellScript>().owner.CompareTag("Player"))
        {
            ParticleSystem.MainModule main = particleEffect.main;
            main.simulationSpeed = 5f;
            GameObject.Instantiate(hitEffect, transform.position, transform.rotation);

            GameObject.Destroy(gameObject);
        }
    }
}
