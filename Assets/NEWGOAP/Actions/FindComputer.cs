public class FindComputer : GoapAction
{
    // A tag que o GOAP vai usar para o recurso
    private const string PCTAG = "GamingPC";

    public override bool PrePerform()
    {
        // 1. Tenta pegar um PC do mundo (similar ao seu RemovePatient/RemoveCubicle)
        target = GoapWorld.Instance.RemovePC();

        if (target == null)
        {
            // Se não houver PC livre, a ação falha.
            return false;
        }

        // 2. Captura o PC livre e o adiciona ao inventário do agente
        inventory.AddItem(target);

        // 3. Atualiza o estado do mundo para dizer que há menos um PC livre.
        GoapWorld.Instance.GetWorld().UpdateState("FreePC", -1);

        return true;
    }

    public override bool PostPerform()
    {
        // NOVO: Informa ao estado pessoal do agente que ele conseguiu um PC.
        // Isso servirá de pré-condição para a ação TestGame.
        agentStates.AddState("hasPC", 1);
        return true;
    }
}