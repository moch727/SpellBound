using UnityEngine;

public class FurnaceScript : MonoBehaviour
{
    [SerializeField] GameObject lightEffect;

    public bool lit;

    private void Awake()
    {
        lightEffect.SetActive(false);
    }
    private void FixedUpdate()
    {
        if(lit) lightEffect.SetActive(true);
    }
}
