using UnityEngine;
using UnityEngine.UI;

public class PointBar : MonoBehaviour
{
    public float value, maxValue, width, height;

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
        image.fillAmount = Mathf.MoveTowards(image.fillAmount,currentValue / maxValue, 2*Time.deltaTime);
        //float newWidth = (currentValue/maxValue) * width;
        //Debug.Log(width);
        //if (newWidth != width) rect.sizeDelta = new Vector2(Mathf.MoveTowards(width, newWidth, 2f * Time.deltaTime), height);


    }

}
