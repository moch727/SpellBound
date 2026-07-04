using UnityEngine;

public class GateScript : MonoBehaviour
{
    public GameObject[] objectives;
    [SerializeField] Vector3 targetLocation;

    [HideInInspector]
    public bool objectivesComplete = false;
    void FixedUpdate()
    {
        if (objectives.Length > 0)
        {
            bool iteration = true;
            for (int i = 0; i < objectives.Length; i++)
            {
                if (objectives[i] != null)
                {
                    iteration = false;
                }
            }
            objectivesComplete = iteration;
        }

        if (objectivesComplete)
        {
            transform.position = Vector3.Slerp(transform.position, targetLocation, 0.5f * Time.deltaTime);
            //if(transform.position == targetLocation) Destroy(gameObject);
        }
            
    }
}
