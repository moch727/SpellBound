using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] AIScript meleeAIScript;

    public PointBar bar;

    [HideInInspector]
    public Camera cam;
    void Awake()
    {
        //cam = Camera.main;
        if(meleeAIScript != null) bar.setMaxValue(meleeAIScript.health);
    }

    void LateUpdate()
    {
        if (meleeAIScript != null) bar.setValue(meleeAIScript.health);


        if(cam != null) transform.rotation = Quaternion.LookRotation(transform.position - cam.transform.position);
    }
}
