
public class Tester : GoapAgent
{
    protected override void Start()
    {
        base.Start();

        SubGoal s1 = new SubGoal("isHome", 1, true);

        goals.Add(s1, 1);
    }
}