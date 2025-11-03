using UnityEngine;
using UnityEngine.UI;

public class NoteScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private const KeyCode REDKEY = KeyCode.Alpha1;
    private const KeyCode GREENKEY = KeyCode.Alpha2;
    private const KeyCode BLUEKEY = KeyCode.Alpha3;

    public Sprite redImage;
    public Sprite greenImage;
    public Sprite blueImage;

    private KeyCode noteInput;

    public int color = 0;
    void Start()
    {
    }

    public void setColor(int color)
    {
        this.color = color;
        if (color == 0)
        {
            noteInput = REDKEY;
            gameObject.GetComponent<Image>().sprite = redImage; //directly access component for it to work
        }
        else if (color == 1)
        {
            noteInput = GREENKEY;
            gameObject.GetComponent<Image>().sprite = greenImage;
        }
        else if (color == 2)
        {
            noteInput = BLUEKEY;
            gameObject.GetComponent<Image>().sprite = blueImage;
        }
    }

    public KeyCode getKey()
    {
        return noteInput;
    }
}
