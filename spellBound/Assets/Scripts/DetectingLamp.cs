using UnityEngine;

public class DetectingLamp : MonoBehaviour
{
    private static int numOfLamps;

    [SerializeField] int lampsToTrigger;
    private static int lampsTriggered = 0;

    public GateScript gate;

    [SerializeField] GameObject fireEffect;

    private void Awake()
    {
        fireEffect.SetActive(false);
        numOfLamps = lampsToTrigger;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Projectile") && other.GetComponent<SpellScript>().owner.CompareTag("Player"))
        {
            fireEffect.SetActive(true);
            lampsTriggered++;

            Debug.Log(numOfLamps);
            Debug.Log(lampsTriggered);
            gate.objectivesComplete = (lampsTriggered >= numOfLamps);
        }
    }
}
