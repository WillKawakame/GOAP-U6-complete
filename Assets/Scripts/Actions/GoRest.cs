public class GoRest : GoapAction
{
    public override bool PrePerform()
    {
        return true;
    }
    public override bool PostPerform()
    {
        agentStates.RemoveState("exhausted");
        return true;
    }
}
