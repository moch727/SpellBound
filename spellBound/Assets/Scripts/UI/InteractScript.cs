using TMPro;
using UnityEngine;

public class InteractScript : MonoBehaviour
{
    [HideInInspector]
    public Camera cam;

    void LateUpdate()
    {
        if (cam != null) transform.rotation = Quaternion.LookRotation(transform.position - cam.transform.position);
    }
}
