using UnityEngine;

public class MudPlane : MonoBehaviour
{
    [SerializeField] GolemScript owner;
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<Rigidbody>().linearDamping = owner.mudLevel * 0.5f;
            if (owner.mudLevel >= 3) other.GetComponent<PlayerCombatComponent>().reduceHealth(other.GetComponent<PlayerCombatComponent>().health);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) other.GetComponent<Rigidbody>().linearDamping = 0f;
    }
}
