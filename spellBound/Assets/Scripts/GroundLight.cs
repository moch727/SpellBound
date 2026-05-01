using UnityEngine;

public class GroundLight : MonoBehaviour
{
    [SerializeField] static int maxNumOfLights;
    private static int numOfLights = 0; //Scales off of player lp amount?

    [SerializeField] int maxlightlvl;
    [SerializeField] float duration;
    [SerializeField] float fadeTime;
    [SerializeField] float fadeSpeed;
    private float timeElapsed;
    private float fadePercent;

    private Light light;
    private SphereCollider sCollider;

    public float radii;
    private float lvlLength;
    void Awake()
    {
        numOfLights++;
        light = GetComponent<Light>();
        sCollider = GetComponent<SphereCollider>();

        radii = light.range;
        sCollider.radius = radii;

        lvlLength = radii / maxlightlvl; //Distance between each level 
        fadeSpeed = light.intensity / duration;
    }

    private void Update()
    {
        timeElapsed += Time.deltaTime;
        fadePercent = 1 -  timeElapsed / duration;
        //Debug.Log(fadePercent);
        //if(timeElapsed >= duration)
        //{
        light.intensity -= fadeSpeed * Time.deltaTime;
        //light.range = fadePercent * radii;
        //}
        //For sudden light fade out


        if (light.intensity <= 0)
        {
            Destroy(gameObject);
        }

    }

    public int getlightLevel(float distance) 
    {
        if(fadePercent > 0.1f)
        {
            int lightlvl = maxlightlvl - Mathf.FloorToInt(distance / (lvlLength*fadePercent));
            if (lightlvl < maxlightlvl) return lightlvl;
            else return maxlightlvl;
        }
        return 0;
    }

}
