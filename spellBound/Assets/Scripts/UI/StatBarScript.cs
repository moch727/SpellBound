using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatBarScript : MonoBehaviour
{
    [SerializeField] UIManager manager;

    [SerializeField] TextMeshProUGUI value;

    [SerializeField] Button UP;
    [SerializeField] Button DOWN;

    //public void ChangeStat(int change)
    //{
    //    value.text = (int.Parse(value.text) + change).ToString();
    //    manager.availablePoints -= change;
    //}

    private void Update()
    {
        //UP.interactable = !(manager.availablePoints <= 0);
        //DOWN.interactable = !manager.HasMaxPoints();

        //value.text = (manager.).ToString();
    }
}
