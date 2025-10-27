public class Patient : GoapAgent
{
    protected override void Start()
    {
        base.Start();
        // Criando um objetivo chamado isWaiting
        SubGoal s1 = new SubGoal("isWaiting", 1, true);
        // Adicionando o objetivo na lista e definindo a prioridade
        goals.Add(s1, 3);
        // É mais importante ser curado do que esperar
        SubGoal s2 = new SubGoal("isTreated", 1, true);
        goals.Add(s2, 5);
        SubGoal s3 = new SubGoal("isHome", 1, true);
        goals.Add(s3, 5);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
