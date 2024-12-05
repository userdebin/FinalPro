using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private StateMachine stateMachine;
    private NavMeshAgent agent;

    private Vector3 lastKnownPos;
    private Vector3 LastKnownPos { get => lastKnownPos; set => lastKnownPos = value; }

    public NavMeshAgent Agent { get => agent; }

    // Debugging purpose
    [SerializeField]
    private string currentState;

    public Path path;

    private bool isStunned = false; // Status apakah musuh sedang stun
    private float stunDuration = 2f; // Durasi stun

    void Start()
    {
        stateMachine = GetComponent<StateMachine>();
        agent = GetComponent<NavMeshAgent>();
        stateMachine.Initialise();
    }

    void Update()
    {
        if (isStunned) return; // Jika musuh stun, hentikan semua aktivitas

        CheckFOV();

        if (stateMachine.activeState != null)
        {
            stateMachine.activeState.Perform();
        }
    }

    private void CheckFOV()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            Vector3 directionToPlayer = player.transform.position - transform.position;
            float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);

            if (angleToPlayer <= 45f && directionToPlayer.magnitude <= 10f) // Adjust FOV angle and range
            {
                stateMachine.ChangeState(stateMachine.searchState); // Trigger SearchState
            }
        }
    }

    public void StunEnemy()
    {
        if (isStunned) return; // Cegah stun jika musuh sudah terstun
        StartCoroutine(StunCoroutine());
    }

    private IEnumerator StunCoroutine()
    {
        isStunned = true;
        agent.isStopped = true; // Hentikan pergerakan NavMeshAgent
        Debug.Log("Enemy stunned!");

        yield return new WaitForSeconds(stunDuration);

        isStunned = false;
        agent.isStopped = false; // Lanjutkan pergerakan NavMeshAgent
        Debug.Log("Enemy recovered from stun!");
    }
}
