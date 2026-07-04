using UnityEngine;

public class BoxScript : MonoBehaviour
{
    public Transform point;
    [SerializeField] GameObject includedItem;

    [SerializeField] GameObject itemObject;
    public void OpenBox()
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
