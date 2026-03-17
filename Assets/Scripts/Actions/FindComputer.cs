public class FindComputer : GoapAction
{
    private const string PCTAG = "GamingPC";

    public override bool PrePerform()
    {
        target = GoapWorld.Instance.RemovePC();

        if (target == null)
        {
            return false;
        }

        inventory.AddItem(target);

        GoapWorld.Instance.GetWorld().UpdateState("FreePC", -1);

        return true;
    }

    public override bool PostPerform()
    {
        agentStates.AddState("hasPC", 1);
        return true;
    }
}