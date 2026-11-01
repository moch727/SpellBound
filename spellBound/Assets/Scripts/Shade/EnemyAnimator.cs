using UnityEngine;

public class EnemyAnimator : MonoBehaviour
{
    private AIScript aiScript;
    private EnemyCombat enemyCombat;
    private Animator animator;
    void Awake()
    {
        aiScript = GetComponent<AIScript>();
        enemyCombat = GetComponent<EnemyCombat>();
        animator = GetComponent<Animator>();        
    }

    public void animateSpawn()
    {
        if (!animator.GetBool("Spawned")) animator.SetBool("Spawned", true);
    }

    public void animateWalk(bool enable)
    {
        animator.SetBool("Walk", enable);
    }

    public void setWalkType(int type)
    {
        animator.SetInteger("WalkNum", type);
    }

    public void animateAttack()
    {
        animator.SetBool("Walk", false);
        animator.SetInteger("AttackNum", enemyCombat.getCurrentAttack().animID);
        animator.SetTrigger("Attack");
    }

    public void animateDeath()
    {
        animator.SetBool("Walk", false);
        animator.ResetTrigger("Attack");
        animator.SetBool("Death", true);
    }

    //Animation Events
    public void ResetState()
    {
        aiScript.canMove = true;
        aiScript.canRotate = true;
        aiScript.state = AIScript.AIState.None;
        // if (attachPoint != null) attachPoint.rotation = Quaternion.identity;
        enemyCombat.setCurrentAttack(null);
        aiScript.agent.enabled = true;
    }
    public void StartMoving()
    {
        aiScript.canMove = true;
    }
    public void PrepareAttack(GameObject o)
    {
        aiScript.canMove = false;
        aiScript.canRotate = false;
        aiScript.agent.ResetPath();
        if (o != null)
        {
            enemyCombat.CreateSpell(o);
        }
    }
    public void Fire()
    {
        enemyCombat.FireSpell();
    }
    public void EnableDamagebox()
    {
        enemyCombat.SetAttackDamageBox(true);
    }
    public void DisableDamagebox()
    {
        enemyCombat.SetAttackDamageBox(false);
    }
    public void Terminate()
    {
        aiScript.animateMud();
        if (aiScript.hasMud())
        {
            aiScript.target.GetComponent<PlayerCombatComponent>().XP += enemyCombat.baseXP; //Multiply by level to scale
            Destroy(gameObject);
        }
        //if (mud != null) mud.SetActive(true);
        //Destroy(gameObject);
    }
}
