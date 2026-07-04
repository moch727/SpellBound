using UnityEngine;

public class DetectionScript : MonoBehaviour
{
    private Rigidbody rb;
    private Vector3 initialPosition;

    
    [SerializeField] LayerMask layersToHit;
    public float aggroRange; //Max distance for ai to be aggro'd towards player
    [SerializeField] bool enableLineOfSight;

    //[HideInInspector]
    public bool detected = false;
    public bool isVisible = false;
    public GameObject target;
    
    public SphereCollider runbackDetector; //Planned for entities with line of sight : When going back to their initial "watching area", they will have a sphere range where they can detect the player

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        //if (gameObject.CompareTag("LineOfSight"))
        //{
        //    runbackDetector = GetComponent<SphereCollider>();
        //    initialPosition = transform.position;

        //}
    }
    private void FixedUpdate()
    {

        isVisible = target != null ? inLineOfSight() && Physics.Raycast(transform.position, ((target.transform.position + Vector3.up) - transform.position).normalized, out RaycastHit hit, aggroRange, layersToHit) && hit.collider.CompareTag("Player") : false;
        
        if (target != null && Vector3.Distance(transform.position, target.transform.position) >= aggroRange) //remove vertical distance difference
        {
            detected = false;
            target = null;
        }

        if(runbackDetector != null && transform.position == initialPosition) runbackDetector.enabled = false; 
    }

    private bool inLineOfSight()
    {
        if (enableLineOfSight)
        {
            Quaternion r = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation((target.transform.position - transform.position).normalized), 360f);
            return Mathf.Abs(transform.rotation.eulerAngles.y - r.eulerAngles.y) < 90;
        }
        return true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) //Physics.Raycast(transform.position, (other.transform.position - transform.position).normalized, out RaycastHit hit, radius) && hit.collider.CompareTag("Player"
        {
            detected = true;
            target = other.gameObject;
        }
    }

    //private void OnTriggerStay(Collider other)
    //{
    //    Physics.Raycast(transform.position, (other.gameObject.transform.position - transform.position).normalized, out RaycastHit hit, aggroRange);
    //    Debug.Log(hit.collider.tag);
    //    if (target == null && other.CompareTag("Player") && hit.collider.CompareTag("Player")) //Physics.Raycast(transform.position, (other.transform.position - transform.position).normalized, out RaycastHit hit, radius) && hit.collider.CompareTag("Player"
    //    {
    //        detected = true;
    //        target = other.gameObject;
    //    }
    //}
}
