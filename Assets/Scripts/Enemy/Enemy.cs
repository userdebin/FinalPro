//Enemy.cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;



public class Enemy : MonoBehaviour
{
    private StateMachine stateMachine;
    private NavMeshAgent agent;

    private Vector3 lastKnownPos;
    private Vector3 LastKnownPos { get => lastKnownPos; set => lastKnownPos = value; }

    public NavMeshAgent Agent { get => agent; }

    //just for debugging purpose
    [SerializeField]
    private string currentState;

    public Path path;


    void Start()
    {
        stateMachine = GetComponent<StateMachine>();
        agent = GetComponent<NavMeshAgent>();
        stateMachine.Initialise();
    }

    void Update()
    {
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
}