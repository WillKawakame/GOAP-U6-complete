using UnityEngine; 

public class GiveFeedback : GoapAction
{
    private const string PCTAG = "GamingPC";

    private GoapAgent goapAgent;

    void Start()
    {
        goapAgent = GetComponent<GoapAgent>();
    }

    public override bool PrePerform()
    {
        GameObject pc = inventory.FindItemWithTag(PCTAG);

        if (pc != null)
        {
            Computer pcStatus = pc.GetComponent<Computer>();
            if (pcStatus != null && goapAgent != null)
            {
                //...
                pcStatus.ReceiveFeedback(goapAgent.currentFeedback);
                goapAgent.currentFeedback = FeedbackType.None;

                // PARA ESTA LINHA:
                pcStatus.SetState(Computer.ComputerState.ShowFeedback);
            }

            inventory.RemoveItem(pc);
            GoapWorld.Instance.AddPC(pc);
            GoapWorld.Instance.GetWorld().UpdateState("FreePC", 1);
        }

        return true;
    }

    public override bool PostPerform()
    {
        agentStates.RemoveState("gameTested");
        agentStates.AddState("feedbackGiven", 1);
        return true;
    }
}