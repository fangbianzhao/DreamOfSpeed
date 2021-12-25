using UnityEngine;
using System.Collections;
using Newtonsoft.Json;

public class EnemyAttack : MonoBehaviour
{
    public float timeBetweenAttacks = 1f;
    public int attackDamage = 10;


    private NetworkHost networkHost;
    Animator anim;
    ArrayList players;
    EnemyHealth enemyHealth;
    bool playerInRange;
    float timer;
    private GameObject player;
    public PlayerHealth playerHealth;

    void Awake ()
    {
        players = new ArrayList();
        player = GameObject.FindGameObjectWithTag("Player");
        playerHealth = player.GetComponent<PlayerHealth>();
        enemyHealth = GetComponent<EnemyHealth>();
        anim = GetComponent <Animator> ();
        networkHost = NetworkHost.GetInstance();
    }


    void OnTriggerEnter (Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            players.Add(other.gameObject);
            playerInRange = true;
        }
    }


    void OnTriggerExit (Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            if(players.Contains(other.gameObject))
            {
                players.Remove(other.gameObject);
            }
            playerInRange = false;
        }      
    }


    void Update ()
    {
        timer += Time.deltaTime;

        if(timer >= timeBetweenAttacks && players.Count > 0  && !enemyHealth.isDead)
        //if(playerInRange && timer>=timeBetweenAttacks)
        {
            Attack ();
        }

        // if(playerHealth.currentHealth <= 0)
        // {
        //    anim.SetTrigger ("PlayerDead");
        // }
    }


    void Attack ()
    {
        
        //playerHealth.TakeDamage(attackDamage);

        timer = 0f;
        foreach( GameObject player in players)
        {
             PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
             if (playerHealth.currentHealth > 0)
             {
                 // send enemy attack msg
                 SendMonsterAttackMsg(enemyHealth.monsterID, GameSettings.playerID, attackDamage);
             }
        }

    }
     void SendMonsterAttackMsg(int monsterID, int playerID, int monsterDamage)
     {
         ClientMonsterAttackMsg clientMonsterAttackMsg = new ClientMonsterAttackMsg();
         clientMonsterAttackMsg.monsterID = monsterID;
         clientMonsterAttackMsg.playerID = playerID;
         clientMonsterAttackMsg.monsterDamage = monsterDamage;

         string monsterAttackJson = JsonConvert.SerializeObject(clientMonsterAttackMsg);

         byte[] msg = MessageHandler.SetClientMsg(
            NetworkSettings.MONSTER_ENTITY_SERVICE_ID,
            NetworkSettings.MONSTER_ENTITY_ATTACK_CMD,
            monsterAttackJson);

         StartCoroutine(networkHost.SendBytesMessage(msg));
     }

}
