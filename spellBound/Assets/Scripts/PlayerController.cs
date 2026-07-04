using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private Rigidbody rb;
    private float movementX;
    private float movementY;
    private float vertical = 0;

    public float speed = 10;
    public TextMeshProUGUI countText;
    public GameObject winTextObject;

    private int count;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        count = 0;
        setCountText();
        winTextObject.SetActive(false);
    }
        
    private void FixedUpdate()
    {   
        Vector3 movement = new Vector3(movementX, 0, movementY);
        rb.AddForce(movement * speed);


        if (gameObject.transform.position.y < -3)
        {
            SceneManager.LoadScene("MiniGame", LoadSceneMode.Single);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PickUp"))
        {
            count++;
            other.gameObject.SetActive(false);
            setCountText();
            if (count >= 9) { 
                winTextObject.SetActive(true);
                Destroy(GameObject.FindGameObjectWithTag("Enemy"));
            }
        }
        else if (other.gameObject.CompareTag("PickUp-"))
        {
            if(count > 0)
            {
                count--;
                setCountText();

            }
            other.gameObject.SetActive(false);
        }
    }

    void setCountText()
    {
        countText.text = "Count : " + count.ToString();
    }

    void OnMove(InputValue movementValue) //called when movement input pressed (i.e wasd, arrow keys)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();
        movementX = movementVector.x;
        movementY = movementVector.y;
    }

    private void OnCollisionEnter(Collision collision) //Doesn't work when no movement input
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Destroy(gameObject);
            winTextObject.gameObject.SetActive(true);
            winTextObject.GetComponent<TextMeshProUGUI>().color = Color.red;
            winTextObject.GetComponent<TextMeshProUGUI>().text = "You Lose!";
        }
        
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Climbable"))
        {
            if ((movementX != 0))
            {
                vertical = Mathf.Abs(movementX);

            }
            else if ((movementY != 0))
            {
                vertical = Mathf.Abs(movementY);

            }
            else
            {
                vertical = 0;
            }


        }
        else if (collision.gameObject.CompareTag("SpeedBoost"))
        {
            speed = 15;
        }
        else
        {
            speed = 10;
            vertical = 0;
        }

        rb.AddForce(Vector3.up * vertical * Time.deltaTime * 25, ForceMode.Impulse);


    }
    // Update is called once per frame
    //void Update()
    //{

    //}
}
