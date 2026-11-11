using UnityEngine;

public interface IInteractable
{
    void interact(PlayerScript player);
}
public class ItemScript : MonoBehaviour, IInteractable
{
    public GameObject item;

    public void interact(PlayerScript player)
    {
        GetComponent<ParticleSystem>().Stop();
        player.addSpell(item);
        Destroy(gameObject);
    }
}
