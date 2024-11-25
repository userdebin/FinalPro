//StateMachine.cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    public BaseState activeState;
    public PatrolState patrolState;
    public SearchState searchState; // Add SearchState
    public AttackState attackState; // Add AttackState

    public void Initialise()
    {
        patrolState = new PatrolState();
        searchState = new SearchState();
        attackState = new AttackState(); // Initialize AttackState
        ChangeState(patrolState);
    }


    void Start()
    {

    }

    void Update()
    {
        if (activeState != null)
        {
            activeState.Perform();
        }
    }

    public void ChangeState(BaseState newState)
    {
        //check if activeState != null
        if (activeState != null)
        {
            //run cleanup on activeState
            activeState.Exit();
        }
        //change to a new State.
        activeState = newState;

        //fail-safe null check to make sure new state wasn't null
        if (activeState != null)
        {
            //setup new state
            activeState.stateMachine = this;
            activeState.enemy = GetComponent<Enemy>();  
            activeState.Enter();
        }
    }
}