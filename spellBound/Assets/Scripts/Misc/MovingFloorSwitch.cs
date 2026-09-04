using UnityEngine;

public class MovingFloorSwitch : MonoBehaviour
{
    [SerializeField] GameObject[] movingTiles;

    [SerializeField] float tileMoveSpeed;

    [HideInInspector] public bool startMoving = false;

    public Vector3[] tilePositions;

    private void Awake()
    {
        tilePositions = new Vector3[movingTiles.Length];
    }
    void FixedUpdate()
    {

        if (startMoving)
        {
            bool moveComplete = true;
            for (int i = 0; i < movingTiles.Length; i++)
            {
                int tileToMoveTo = 0;
                if (i < movingTiles.Length - 1) tileToMoveTo = i + 1;

                Vector3 targetPosition = tilePositions[tileToMoveTo];
                targetPosition.y = movingTiles[i].transform.position.y;

                //movingTiles[i].transform.position = Vector3.Lerp(movingTiles[i].transform.position, targetPosition, tileMoveSpeed * Time.deltaTime);
                movingTiles[i].transform.position = Vector3.MoveTowards(movingTiles[i].transform.position, targetPosition, tileMoveSpeed * Time.deltaTime);

                if (moveComplete) moveComplete = Vector3.Distance(movingTiles[i].transform.position, targetPosition) < 0.01f;
            }

            if (moveComplete) startMoving = false;
        }
    }

    private void recordTilePositions()
    {
        for(int i = 0;i < movingTiles.Length; i++)
        {
            tilePositions[i] = movingTiles[i].transform.position;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(!startMoving && other.CompareTag("Projectile") && other.GetComponent<SpellScript>().owner.CompareTag("Player"))
        {
            recordTilePositions();
            startMoving = true;
        }
    }
}
