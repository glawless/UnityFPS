using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;

public class EnemyAI : MonoBehaviour
{
    public float lookRadius = 10f;
    public float attackRadius = 5f;
    public float walkPointRange = 2f;
    public LayerMask whatIsGround;
    bool walkPointSet;
    private Vector3 walkPoint;

    public Transform target;
    NavMeshAgent agent;

    // Attacking
    public float timeBetweenAttacks;
    public float shootForce;
    public float upwardShootForce;
    bool alreadyAttacked;
    public GameObject bullet;
    public Transform gunTip;
    public float damage = 5;

    //Animation
    public bool isWalking, isAttacking;

    //Networking
    private PhotonView PV;


    private void Start()
    {
        PV = GetComponent<PhotonView>();
        agent = GetComponent<NavMeshAgent>();
        alreadyAttacked = false;
    }

    private void Update()
    {
        if (PV.IsMine)
        {
            isAttacking = false;
            isWalking = false;
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

            if (target == null)
            {
                if (players != null && players.Length != 0)
                {
                    int ranNum = Random.Range(0, players.Length);
                    target = players[ranNum].transform;
                }
            }
            if (target != null)
            {
                float distance = Vector3.Distance(target.position, transform.position);

                if (distance <= lookRadius)
                {
                    agent.SetDestination(target.position);
                    isAttacking = false;
                    isWalking = true;

                    if (distance <= agent.stoppingDistance)
                    {
                        // Face the target
                        FaceTarget();
                    }
                    if (distance <= attackRadius)
                    {
                        isAttacking = true;
                        isWalking = false;
                        AttackTarget();
                        PV.RPC("AttackTarget", RpcTarget.All);
                    }
                }
                else
                {
                    target = null;
                    isAttacking = false;
                    isWalking = true;
                    Patrol();
                }
            }
            else
            {
                target = null;
                isAttacking = false;
                isWalking = true;
                Patrol();
            }
        }
    }

    private void Patrol()
    {
        if (!walkPointSet) SetWalkPoint();

        if (walkPointSet)
        {
            agent.SetDestination(walkPoint);
        }

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        if (distanceToWalkPoint.magnitude <= 2f)
        {
            walkPointSet = false;
        }
    }


    private void SetWalkPoint()
    {
        // Calculate a random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
        {
            walkPointSet = true;
        }
    }


    void FaceTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);

    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
        Gizmos.DrawWireSphere(transform.position, attackRadius);

    }

    [PunRPC]
    private void AttackTarget()
    {
        //Make sure the enemy doesnt move while attacking
        agent.SetDestination(transform.position);

        transform.LookAt(target);
        transform.rotation *= Quaternion.Euler(0, -1.5f, 0);


        if (!alreadyAttacked)
        {
            // Add attack code here
            GameObject currentBullet = Instantiate(bullet, gunTip.position, Quaternion.identity);

            currentBullet.GetComponent<Rigidbody>().AddForce(transform.forward * shootForce, ForceMode.Impulse);
            currentBullet.GetComponent<Rigidbody>().AddForce(transform.up * upwardShootForce, ForceMode.Impulse);
            currentBullet.GetComponent<Bullet>().SetDamage(damage);

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }


    private void ResetAttack()
    {
        alreadyAttacked = false;
    }










}
