using TMPro;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public AIScript meleeAIScript;

    public PointBar bar;
    public GameObject textMeshPro;

    [HideInInspector]
    public Camera cam;
    void Awake()
    {
        //cam = Camera.main;
        if (meleeAIScript != null)
        {
            bar.setMaxValue(meleeAIScript.maxHealth);
            //if (textMeshPro != null) textMeshPro.GetComponent<TextMeshProUGUI>().text = meleeAIScript.name;
        }


    }

    public void SetMax()
    {
        if (meleeAIScript != null) bar.setMaxValue(meleeAIScript.maxHealth);
        if (textMeshPro != null) textMeshPro.GetComponent<TextMeshProUGUI>().text = meleeAIScript.enemyName; 
    }
    void LateUpdate()
    {
        if (meleeAIScript != null) bar.setValue(meleeAIScript.health);
        else gameObject.SetActive(false);



        if (cam != null && meleeAIScript.enemyName == "") transform.rotation = Quaternion.LookRotation(transform.position - cam.transform.position);
    }
}
