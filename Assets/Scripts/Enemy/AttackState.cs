using System.Collections;
using UnityEngine;

public class AttackState : BaseState
{
    public float attackRange = 1.5f; // Distance to attack player

    public override void Enter()
    {
        Debug.Log("Entering AttackState");
    }

    public override void Perform()
    {
        AttackPlayer();
    }

    public override void Exit()
    {
        Debug.Log("Exiting AttackState");
    }

    private void AttackPlayer()
    {
        // Ensure enemy is close enough to attack
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null && Vector3.Distance(enemy.transform.position, player.transform.position) <= attackRange)
        {
            PlayerSettings playerSettings = player.GetComponent<PlayerSettings>();
            if (playerSettings != null)
            {
                playerSettings.TakeDamageServerRpc(playerSettings.currentHealth); // Deal one-hit kill
                Debug.Log("Player killed!");
            }
        }
        else
        {
            stateMachine.ChangeState(stateMachine.searchState); // Go back to searching
        }
    }
}
