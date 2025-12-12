using UnityEngine;
using UnityEngine.AI;

public class CrowdWalker : MonoBehaviour
{
    public Transform[] waypoints;
    private int currentIndex = 0;

    private NavMeshAgent agent;
    private Animator animator;

    public float idleTime = 1f;
    private float timer = 0f;

    private enum State
    {
        Walking,
        TurningLeft,
        IdleAfterTurn,
        TurningRight,
        IdleAtSpecial37,
        TurnAfter37,
        Leave
    }

    private State currentState = State.Walking;

    CustomerSpawnPoint customerSpawnPoint;
    GameplaySystem gameplaySystem;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        customerSpawnPoint = GameObject.Find("CustomerSpawnPoint").GetComponent<CustomerSpawnPoint>();
        gameplaySystem = GameObject.Find("GameplaySystem").GetComponent<GameplaySystem>();

        if (waypoints.Length > 0)
        {
            animator.SetBool("isWalking", true);
            animator.Play("walk");
            GoToNextPoint();
        }
    }

    void Update()
    {
        switch (currentState)
        {
            case State.TurningLeft:
                timer += Time.deltaTime;
                if (timer >= 0.1f)
                {
                    timer = 0f;
                    animator.SetBool("isWalking", false);
                    animator.Play("idle1");
                    currentState = State.IdleAfterTurn;
                }
                break;

            case State.IdleAfterTurn:
                timer += Time.deltaTime;
                if (timer >= idleTime)
                {
                    timer = 0f;
                    transform.Rotate(0f, 90f, 0f); // Face original direction
                    animator.SetBool("isWalking", true);
                    animator.Play("walk");
                    currentState = State.Walking;
                    GoToNextPoint();
                }
                break;

            case State.IdleAtSpecial37:
                timer += Time.deltaTime;
                if (timer >= 5f) // special 5 second idle
                {
                    timer = 0f;
                    transform.Rotate(0f, 180f, 0f); // Turn around
                    animator.SetBool("isWalking", true);
                    animator.Play("walk");
                    currentState = State.Walking;
                    GoToNextPoint();
                }
                break;
            case State.Leave:
                agent.SetDestination(customerSpawnPoint.spawnPoint.position);
                if (Vector3.Distance(transform.position, customerSpawnPoint.spawnPoint.position) < 1f)
                {
                    Destroy(this.gameObject);
                }
                break;
        }

        if (currentState != State.Walking) return;

        if (!agent.pathPending && agent.remainingDistance < 1f)
        {
            currentIndex = Random.Range(0, waypoints.Length);

            if (timer >= idleTime && Random.value < 0.2f || !gameplaySystem.IsShiftActive() || currentIndex == 43)
            {
                Debug.Log("Customer decided to leave!");
                currentState = State.Leave;
                return;
            }

            // If we go past the last waypoint, loop to the start
            if (currentIndex >= waypoints.Length)
            {
                currentIndex = 0;
            }

            // Special 37th waypoint logic
            if (currentIndex == 37)
            {
                animator.SetBool("isWalking", false);
                animator.Play("idle1");
                currentState = State.IdleAtSpecial37;
                timer = 0f;
                return;
            }

            // Normal idle logic at every 6th waypoint (except 37)
            if ((currentIndex != 0) && (currentIndex % 6 == 0))
            {
                transform.Rotate(0f, -90f, 0f); // Turn left
                currentState = State.TurningLeft;
                timer = 0f;
                return;
            }

            // Just go to the next point
            GoToNextPoint();
        }
    }

    void GoToNextPoint()
    {
        if (waypoints.Length == 0) return;
        agent.destination = waypoints[currentIndex].position;
    }
}
