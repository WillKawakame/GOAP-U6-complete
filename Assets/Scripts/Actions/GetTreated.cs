public class GetTreated : GoapAction
{
    public override bool PrePerform()
    {
        target = inventory.FindItemWithTag("Cubicle");
        return target == null ? false : true;
    }

    public override bool PostPerform()
    {
        GoapWorld.Instance.GetWorld().UpdateState("Treated", 1);
        agentStates.UpdateState("isCured", 1);
        inventory.RemoveItem(target);
        return true;
    }
}
