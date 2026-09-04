using UnityEngine;

public class MudSplashScript : MonoBehaviour
{
    [SerializeField] float duration;
    private float currentTime;

    [SerializeField] float tickDamage;
    [SerializeField] float tickSpeed;
    private float tickCounter;

    private ParticleSystem m_splash;
    void Start()
    {
        m_splash = GetComponent<ParticleSystem>();
    }
    void Update()
    {
        if (m_splash.time >= 0.21f)
        {
            if(currentTime <= duration)
            {
                m_splash.Pause();
                currentTime += Time.deltaTime;
            }
            else
            {
                m_splash.Play();
                GetComponent<Collider>().enabled = false;
            }

        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            tickCounter += Time.deltaTime;
            if (tickCounter >= tickSpeed)
            {
                other.GetComponent<PlayerCombatComponent>().reduceHealth((int) tickDamage);
                tickCounter = 0f;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) tickCounter = 0f;
    }
}
