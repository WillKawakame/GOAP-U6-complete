using UnityEngine; // Necessário para Random

public class TestGame : GoapAction
{
    private const string PCTAG = "GamingPC";

    private GoapAgent goapAgent;

    void Start()
    {
        goapAgent = GetComponent<GoapAgent>();
    }
    // ---------------------------------

    public override bool PrePerform()
    {
        target = inventory.FindItemWithTag(PCTAG);
        if (target == null) return false;

        Computer pcStatus = target.GetComponent<Computer>();
        if (pcStatus != null)
        {
            pcStatus.SetState(Computer.ComputerState.InUse);
        }
        // ---------------------------------------------

        return true;
    }

    public override bool PostPerform()
    {
        agentStates.RemoveState("hasPC");
        agentStates.AddState("gameTested", 1);

        if (goapAgent != null)
        {
            // Gera um feedback aleatório (Good, Bad, ou Buggy)
            int randomFeedback = Random.Range(1, 4); // Gera 1, 2, ou 3
            goapAgent.currentFeedback = (FeedbackType)randomFeedback;

            Debug.Log($"Tester gerou feedback: {goapAgent.currentFeedback}");
        }
        // ------------------------------------------

        return true;
    }
}