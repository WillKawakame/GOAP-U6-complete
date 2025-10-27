public class GoHome : GoapAction
{
    private const string PCTAG = "Home";

    public override bool PrePerform()
    {
        return true;
    }
    public override bool PostPerform()
    {
        Destroy(gameObject, 3f);
        return true;
    }
}
