using UnityEngine;

public class EnemyCombat : MonoBehaviour
{
    private AIScript aiScript;
    private EnemyAnimator enemyAnimator;

    [System.Serializable]
    public class AttackData
    {
        public string attackName;
        public int animID;
        public int attackRange;
        public int attackDamage;

        public MeleeHitbox meleeDamageBox;
    }

    [SerializeField] AttackData[] attackList;
    //[HideInInspector] 
    private AttackData currentAttack = null;

    //For melee enemies
    [SerializeField] MeleeHitbox damageBox;

    //For ranged enemies
    [HideInInspector] public GameObject spellObject;
    [SerializeField] Transform attachPoint; //Where the spell is generated
    private float maxAttackRange;

    public float baseXP;
    public float combatWalkSpeed;

    void Awake()
    {
        aiScript = GetComponent<AIScript>();
        enemyAnimator = GetComponent<EnemyAnimator>();

        maxAttackRange = attackList[attackList.Length - 1].attackRange;
    }
    public float getCombatWalkSpeed()
    {
        return combatWalkSpeed;
    }
    public float getMaxAttackRange()
    {
        return maxAttackRange;
    }
    public void setCurrentAttack(AttackData ad)
    {
        currentAttack = ad;
    }
    public AttackData getCurrentAttack()
    {
        return currentAttack;
    }
    public AttackData selectAttack(float distanceToTarget, AIScript.AIBehaviour type)
    {
        int validAttack = 0;
        int shortestAttack = 0;

        if (attackList.Length == 1)
        {
            validAttack = 1;
        }
        else
        {
            while (validAttack < attackList.Length)
            {
                if (attackList[validAttack].attackRange > distanceToTarget) break;
                validAttack++;
            }


            if (type == AIScript.AIBehaviour.Ranged && distanceToTarget > 3f)
            {
                while (attackList[shortestAttack].attackRange < 3f)
                {
                    shortestAttack++;
                }
            }
        }
        currentAttack = attackList[UnityEngine.Random.Range(shortestAttack, validAttack)];
        return currentAttack;
    }

    public void SetAttackDamageBox(bool enable)
    {
        currentAttack.meleeDamageBox.setDamageBox(enable);
    }
    public GameObject getSpell()
    {
        return spellObject;
    }
    public void CreateSpell(GameObject o)
    {
        if(attachPoint != null) attachPoint.rotation = Quaternion.identity;
        spellObject = GameObject.Instantiate(o, attachPoint.position, Quaternion.identity, attachPoint);
        spellObject.GetComponent<SpellScript>().owner = gameObject;

        if (o.GetComponent<SpellLocomotionScript>() != null)
        {
            attachPoint.rotation = Quaternion.LookRotation((aiScript.target.transform.position - transform.position).normalized);
            spellObject.GetComponent<SpellScript>().owner = attachPoint.gameObject;
        }
    }
    public void FireSpell()
    {
        if (spellObject != null) spellObject.GetComponent<SpellScript>().shoot();
    }

    public void dropXP()
    {
        aiScript.target.GetComponent<PlayerCombatComponent>().XP += baseXP; //Multiply by level to scale
    }
}
