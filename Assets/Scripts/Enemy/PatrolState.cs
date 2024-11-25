using System.Collections;
using UnityEngine;

public class PatrolState : BaseState
{
    public int waypointIndex; // Index waypoint saat ini
    public float waitTime = 2f; // Waktu tunggu di setiap waypoint
    private bool isWaiting = false; // Status apakah musuh sedang menunggu

    public override void Enter()
    {
        Debug.Log("Entering PatrolState");
        MoveToNextWaypoint();
    }

    public override void Perform()
    {
        PatrolCycle();
    }

    public override void Exit()
    {
        Debug.Log("Exiting PatrolState");
    }

    private void PatrolCycle()
    {
        if (enemy.Agent.remainingDistance < 0.2f && !enemy.Agent.pathPending && !isWaiting)
        {
            // Musuh mencapai waypoint, mulai menunggu
            enemy.StartCoroutine(WaitAtWaypoint());
        }
    }

    private IEnumerator WaitAtWaypoint()
    {
        isWaiting = true;
        enemy.Agent.isStopped = true; // Hentikan NavMeshAgent sementara
        yield return new WaitForSeconds(waitTime);
        isWaiting = false;
        enemy.Agent.isStopped = false; // Lanjutkan NavMeshAgent
        MoveToNextWaypoint();
    }

    private void MoveToNextWaypoint()
    {
        // Tentukan waypoint berikutnya
        if (waypointIndex < enemy.path.waypoints.Count - 1)
        {
            waypointIndex++;
        }
        else
        {
            waypointIndex = 0; // Kembali ke waypoint pertama
        }

        // Set tujuan NavMeshAgent ke waypoint berikutnya
        enemy.Agent.SetDestination(enemy.path.waypoints[waypointIndex].position);
    }
}
