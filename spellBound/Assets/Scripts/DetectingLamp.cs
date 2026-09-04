using UnityEngine;

public class DetectingLamp : MonoBehaviour
{

    [SerializeField] DetectingLamp[] lamps;

    public bool triggered = false;

    public GateScript gate;

    [SerializeField] GameObject fireEffect;
    private bool triggerEvent;
    private float delay = 2f;
    private float count;

    private void Awake()
    {
        if(fireEffect != null) fireEffect.SetActive(false);
    }
    private void Update()
    {
        if (triggerEvent)
        {
            count += Time.deltaTime;
            if (count > delay)
            {
                gate.objectivesComplete = true;
                triggerEvent = false;
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Projectile") && other.GetComponent<SpellScript>().owner.CompareTag("Player"))
        {
            if(fireEffect != null) fireEffect.SetActive(true);
            triggered = true;

            bool complete = true;
            for (int i = 0; i < lamps.Length; i++)
            {
                if (!lamps[i].triggered)
                {
                    complete = false;
                    break;
                }
            }

            triggerEvent = complete;
        }
    }
}
