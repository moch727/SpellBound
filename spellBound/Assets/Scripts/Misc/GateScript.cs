using UnityEngine;

public class GateScript : MonoBehaviour
{
    public GameObject[] objectives;
    private Animator animator;

    [Header("CutScene")]
    [SerializeField] Camera cam;
    [SerializeField] ScreenFXController screenFXController;
    [SerializeField] PlayerScript player;
    private string transitionCam = "CutScene";
    private bool startFade;
    private bool cutSceneComplete;

    [HideInInspector]
    public bool objectivesComplete = false;
    private bool startMoving;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        animator.enabled = false;
        if(cam != null) cam.enabled = false;
        
    }
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

        if (objectivesComplete && !cutSceneComplete)
        {
            if(cam == null) //No cutscene set for this gate
            {
                animator.enabled = true;
                cutSceneComplete = true;
            }
            else
            {
                if (!startFade)
                {
                    screenFXController.EnableFadeInOut();
                    startFade = true;
                }
                if (screenFXController.state == ScreenFXController.ScreenState.FadingOut && startFade)
                {
                    animator.enabled = true;
                    if (transitionCam.CompareTo("CutScene") == 0)
                    {
                        player.currentAction = PlayerScript.Action.CutScene;
                        player.ResetAnimation();
                        player.cam.SetActive(false);
                        cam.enabled = true;
                        player.UIManager.GetComponent<Canvas>().enabled = false;
                    }
                    else
                    {
                        player.currentAction = PlayerScript.Action.None;
                        player.cam.SetActive(true);
                        cam.enabled = false;
                        player.UIManager.GetComponent<Canvas>().enabled = true;
                        startFade = true;
                        cutSceneComplete = true;
                    }
                }
            }
        }   
    }

    public void TriggerFade(string transitionCam)
    {
        this.transitionCam = transitionCam;
        startFade = false;
    }
}
