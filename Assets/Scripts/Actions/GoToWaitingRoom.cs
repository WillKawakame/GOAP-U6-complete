public class GoToWaitingRoom : GoapAction
{
    public override bool PrePerform()
    {
        return true;
    }
    public override bool PostPerform()
    {
        GoapWorld.Instance.GetWorld().UpdateState("atWaitingRoom", 1);
        GoapWorld.Instance.AddPatient(gameObject);
        agentStates.UpdateState("atHospital", 1);
        return true;
    }
}
