using UnityEngine;

public class EnvironmentGroundLight : MonoBehaviour
{
    [SerializeField] bool recoverLP;
    [SerializeField] float interval;
    [SerializeField] float lpAmount;

    public GameObject replacedSpell;
    private float timeElapsed;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerCombatComponent combat = other.GetComponent<PlayerCombatComponent>();

            if (recoverLP)
            {
                timeElapsed += Time.deltaTime;
                if (timeElapsed >= interval) //1 is lp recovery interval
                {
                    if (combat.health < combat.maxHealth) combat.health += 1;
                    else if (combat.LP < combat.maxLP) combat.LP += 1; //combat.maxExtraLP
                    timeElapsed = 0f;
                }
            }
        }
    }
}
