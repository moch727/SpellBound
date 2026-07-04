using UnityEngine;
using UnityEngine.UI;

public class PointBar : MonoBehaviour
{
    public float maxValue, width, height;

    private Image image;
    private RectTransform rect;
    void Awake()
    {
        image = GetComponent<Image>();
        rect = GetComponent<RectTransform>();
    }

    public void setMaxValue(float maxValue)
    {
      this.maxValue = maxValue;
    }


    public void setValue(float currentValue)
    {
        //image.fillAmount = Mathf.MoveTowards(image.fillAmount,currentValue / maxValue, 2*Time.deltaTime);
        rect.localScale = new Vector3(Mathf.MoveTowards(rect.localScale.x, currentValue / maxValue, 2 * Time.deltaTime),1,1);
    }

}
