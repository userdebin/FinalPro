using UnityEngine;

public class SearchState : BaseState
{
    public float searchSpeed = 3.5f; // Movement speed while searching
    public float maxSearchTime = 10f; // Maximum time to search
    private float searchTimer;

    public override void Enter()
    {
        Debug.Log("Entering SearchState");
        searchTimer = 0f;
        enemy.Agent.speed = searchSpeed; // Set movement speed for search
    }

    public override void Perform()
    {
        SearchForPlayer();
    }

    public override void Exit()
    {
        Debug.Log("Exiting SearchState");
    }

    private void SearchForPlayer()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            Vector3 playerPosition = player.transform.position;

            // Move towards the player's last known position
            enemy.Agent.SetDestination(playerPosition);

            // Transition to AttackState if in range
            if (Vector3.Distance(enemy.transform.position, playerPosition) <= 2f) // Adjust detection range
            {
                stateMachine.ChangeState(new AttackState());
            }
        }

        // Stop searching after maxSearchTime
        searchTimer += Time.deltaTime;
        if (searchTimer >= maxSearchTime)
        {
            stateMachine.ChangeState(stateMachine.patrolState); // Return to patrol
        }
    }
}
