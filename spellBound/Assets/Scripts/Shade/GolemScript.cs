using UnityEngine;
using UnityEngine.AI;

public class GolemScript : MonoBehaviour
{
    private AIScript AIScript;
    private Animator animator;

    [SerializeField] int guardChance; //out of 10
    private bool runSelected;

    public float waveDuration;
    private float counter;

    [SerializeField] GameObject mud;
    private Vector3 startPosition;
    public float mudLevel;
    [SerializeField] GameObject waveSrc;

    public WaveSrcScript[] srcList;
    [SerializeField] GameObject stunEffect;

    private ParticleSystem barrierEffect;

    private bool complete;

    //For placing the object in random area
    [SerializeField] Vector3 minPosition;
    [SerializeField] Vector3 maxPosition;
    void Start()
    {
        AIScript = GetComponent<AIScript>();
        animator = GetComponent<Animator>();

        startPosition = mud.transform.position;

        srcList = new WaveSrcScript[3];
    }
    
    public void StartWavePhase()
    {
        ResetWavePhase();

        barrierEffect = AIScript.spellObject.GetComponent<ParticleSystem>();
        for (int i = 0; i < srcList.Length; i++)
        {
            Vector3 randomPosition = transform.position;

            while(Vector3.Distance(transform.position, randomPosition) < 2) //Find a position that is at least 2m away from golem
            {
                float randomX = Random.Range(minPosition.x, maxPosition.x);
                float randomZ = Random.Range(minPosition.z, maxPosition.z);

                randomPosition = new Vector3(randomX, -0, randomZ);
            }


            GameObject src = GameObject.Instantiate(waveSrc, randomPosition, Quaternion.identity);

            srcList[i] = src.GetComponent<WaveSrcScript>();
            srcList[i].golemOwner = this;
            srcList[i].InitiateSrc();
        }
    }

    private void UpdateWavePhase()
    {
        if (barrierEffect.time >= 0.49f && !complete) //check if barrier effect started
        {
            barrierEffect.Pause();

            bool allDestroyed = true;
            for (int i = 0; i < srcList.Length; i++)
            {
                if (srcList[i] != null)
                {
                    allDestroyed = false;
                    break;
                }
            }

            if (allDestroyed)
            {
                //Stun the boss
                counter = waveDuration;

                GameObject.Instantiate(stunEffect, transform.position, Quaternion.identity).transform.localScale = Vector3.one * 2.5f;
                animator.SetTrigger("Stun");
                //GetComponent<NavMeshAgent>().enabled = false; //for root motion
                Destroy(barrierEffect.gameObject);
            }
            else
            {
                counter += Time.deltaTime;
                if (counter >= waveDuration)
                {
                    barrierEffect.Play();
                    animator.SetTrigger("CompleteWave");
                    if (mudLevel < 3) mudLevel++;
                    complete = true;
                }
            }
        }

        if (complete && mudLevel < 3)
        {
            mud.transform.position = Vector3.MoveTowards(mud.transform.position, startPosition + Vector3.up * mudLevel * 1f, 0.3f * Time.deltaTime);
        }
    }

    private void ManageRun()
    {
        if (AIScript.target != null && animator.GetBool("Walk") && !runSelected)
        {
            int randomValue = Random.Range(1, 11);
            if (randomValue <= guardChance)
            {
                animator.SetBool("Guard", true);
                AIScript.defense = 0.5f;
            }
            runSelected = true;
        }

        if(!animator.GetBool("Walk") || AIScript.target == null)
        {
            animator.SetBool("Guard", false);
            AIScript.defense = 0f;
            runSelected = false;
        }
    }
    private void FixedUpdate()
    {
        if (barrierEffect != null)
        {
            UpdateWavePhase();
        }

        ManageRun();
    }

    private void ResetWavePhase()
    {
        barrierEffect = null;
        complete = false;
        counter = 0;

        for (int i = 0; i < srcList.Length; i++)
        {
            srcList[i] = null;
        }
    }
    //public void PrepareAttack(GameObject spellObject)
    //{
    //    Debug.Log("GolemScript");
    //}
}
