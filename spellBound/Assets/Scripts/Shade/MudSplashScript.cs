using UnityEngine;

public class MudSplashScript : MonoBehaviour
{
    [SerializeField] float duration;
    [SerializeField] float tickDamage;
    private float currentTime;

    private ParticleSystem m_splash;
    [SerializeField] GameObject hitbox;
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
                hitbox.GetComponent<Collider>().enabled = false;
            }

        }
    }

    private void OnTriggerStay(Collider other)
    {
        
    }
}
