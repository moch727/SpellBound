using UnityEngine;

public class BoxScript : MonoBehaviour, IInteractable
{
    public Transform point;
    [SerializeField] GameObject includedItem;

    [SerializeField] GameObject itemObject;
    public void interact(PlayerScript player)
    {
        GetComponent<Animator>().enabled = true;
    }

    public void RevealItem()
    {
        itemObject.SetActive(true);
        itemObject.GetComponent<ItemScript>().item = includedItem;
        GetComponent<Collider>().enabled = false;
    }
}
