using UnityEngine;

public class HoldLightFire : MonoBehaviour
{
    [SerializeField] GameObject effect;
    // private ParticleSystem fireEffect;
    void Awake()
    {
        // fireEffect = GetComponent<ParticleSystem>();
        // fireEffect.Stop();
    }

    public void setFireEffect(bool play)
    {
        effect.SetActive(play);
    }
}
