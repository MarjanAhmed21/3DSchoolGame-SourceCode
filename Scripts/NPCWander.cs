using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCWander : MonoBehaviour
{
    private NavMeshAgent agent;
    public bool isWandering = false;

    Animator anim;

    public float maxRotationSpeed;

    public float idleDurationMin;
    public float idleDurationMax;

    public float moveDurationMin;
    public float moveDurationMax;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();

        StartCoroutine(IdleToWander());


    }

    IEnumerator IdleToWander()
    {
        // Wait for a random idle duration between 5 to 10 seconds
        float idleDuration = Random.Range(idleDurationMin, idleDurationMax);
        yield return new WaitForSeconds(idleDuration);

        // Transition to wandering state
        isWandering = true;
        float moveDuration = Random.Range(moveDurationMin, moveDurationMax);
        float startTime = Time.time;

        while (isWandering && (Time.time - startTime < moveDuration))
        {
            // Find a random point on the NavMesh surface to move to
            Vector3 randomPoint = RandomNavMeshPoint(transform.position, 10f, -1);
            agent.SetDestination(randomPoint);

            anim.SetFloat("Blend", 1f);

            // Check if the agent is currently moving
            if (agent.velocity.sqrMagnitude > Mathf.Epsilon)
            {
                // Get the desired rotation based on the velocity
                Quaternion desiredRotation = Quaternion.LookRotation(agent.velocity.normalized);

                // Smoothly rotate the agent towards the desired rotation
                transform.rotation = Quaternion.RotateTowards(transform.rotation, desiredRotation, maxRotationSpeed * Time.deltaTime);
            }

            // Wait until the agent reaches the destination or is stuck
            while (agent.pathPending || (agent.remainingDistance > agent.stoppingDistance))
            {
                yield return null;
            }

            // Stop wandering after reaching the destination
            isWandering = false;
            anim.SetFloat("Blend", 0f);

            // Wait before starting idle-to-wander transition again
            StartCoroutine(IdleToWander());
        }

    }

    // Find a random point on the NavMesh surface within a specified range
    private Vector3 RandomNavMeshPoint(Vector3 origin, float distance, int layermask)
    {
        Vector3 randomDirection = Random.insideUnitSphere * distance;
        randomDirection += origin;
        NavMeshHit navHit;
        NavMesh.SamplePosition(randomDirection, out navHit, distance, layermask);
        return navHit.position;
    }

}
