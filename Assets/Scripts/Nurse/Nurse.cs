using UnityEngine;
public class Nurse : GoapAgent
{
    protected override void Start()
    {
        base.Start();
        // Criando um objetivo chamado treatPatient
        SubGoal s1 = new SubGoal("treatPatient", 1, false);
        // Adicionando o objetivo na lista e definindo a prioridade
        goals.Add(s1, 3);
        // Criando o objetido de descansar
        SubGoal s2 = new SubGoal("rested", 1, false);
        goals.Add(s2, 1);
        Invoke(nameof(GetTired), Random.Range(15, 20));

    }
    void GetTired()
    {
        personalStates.UpdateState("exhausted", 0);
        Invoke(nameof(GetTired), Random.Range(15, 20));
    }
}