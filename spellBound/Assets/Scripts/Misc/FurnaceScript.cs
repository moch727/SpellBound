using UnityEngine;

public class FurnaceScript : MonoBehaviour, IInteractable
{
    [SerializeField] GameObject lightEffect;

    private void Awake()
    {
        lightEffect.SetActive(false);
    }
    public void interact(PlayerScript player)
    {
        if(!lightEffect.activeSelf) lightEffect.SetActive(false);
        player.setMenuState();
        player.UIManager.ShowFurnaceBox();
    }
}
