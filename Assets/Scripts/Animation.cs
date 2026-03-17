using UnityEngine;
using UnityEngine.AI;

public class NavAgentAnimator : MonoBehaviour
{
    public NavMeshAgent agent;
    public Animator animator;

    private readonly int isWalkingHash = Animator.StringToHash("IsWalking");

    void Update()
    {
        float currentSpeed = agent.velocity.magnitude;
        bool isWalking = currentSpeed > 0.1f;
        animator.SetBool(isWalkingHash, isWalking);
    }
}