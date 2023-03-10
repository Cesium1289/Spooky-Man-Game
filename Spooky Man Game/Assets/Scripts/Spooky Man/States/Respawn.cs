using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Respawn : State
{
    private readonly NavMeshPath path;
    public Respawn(GameObject npc, NavMeshAgent agent, Animator animator, GameObject player, SpookyManUtility utility, SpookyManAudio audio)
          : base(npc, agent, animator, player, utility, audio)
    {
       
           currentState = STATE.WATCH;
        path = new NavMeshPath();
    }

    public override void Enter()
    {
       
        base.Enter();
    }

    public override void Update()
    {
        SpawnAroundPlayer();
        Debug.Log("going to stalk state from respawn!");
        nextState = new Stalk(npc, agent, animator, player, utility, audio);
        currentStage = STAGE.EXIT;
        
    }

    public override void Exit()
    {
       
    }

    /*TODO: create a spawn location Slender can go to if player gets on unwalkable navmesh spot.
     * Game will freeze and try to spawn slender if this happens!
     * 
     */
    private void SpawnAroundPlayer()
    {
        Vector3 randomPos;
        bool foundValidSpawn = false;

        do
        {
            randomPos = Random.insideUnitSphere;
            randomPos.Normalize();
            randomPos *= Random.Range(50, 70);
            randomPos += player.transform.position;


            if (NavMesh.SamplePosition(randomPos, out NavMeshHit hit, 1000, NavMesh.AllAreas))
            {
                float distance = Vector3.Distance(player.transform.position, hit.position);
                if (distance < 70 && distance > 50)
                {
                    agent.Warp(hit.position);
                    agent.SetDestination(player.transform.position);
                    agent.CalculatePath(player.transform.position, path);
                    utility.spawnPoint = hit.position;
                    //check if there is a valid path from agent to player
                    if (path.status == NavMeshPathStatus.PathComplete)
                    {
                        foundValidSpawn = true;
                        Debug.Log("Spawning " + distance + " away!");
                    }
                }
            }
        } while (!foundValidSpawn);
    }
}
