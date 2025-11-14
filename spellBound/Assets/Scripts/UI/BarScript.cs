using System.Collections;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem.Controls;

public class BarScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float validDistance = 10f; //Distance allowed for success

    private bool timed = true;
    private bool startDetection = false;
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (startDetection)
        {
            transform.Translate(transform.right * moveSpeed * Time.deltaTime);
        }

    }

    public void setDetection(bool detection)
    {
        startDetection = detection;
    }
    public bool checkInputTiming(Vector3 targetNote)
    {
        Debug.Log("Triggered");
        if (Mathf.Abs(transform.position.x - targetNote.x) < validDistance) //Distance is lower than valid, register as timed
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
