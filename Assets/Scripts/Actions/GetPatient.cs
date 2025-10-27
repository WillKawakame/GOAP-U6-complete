using UnityEngine;
public class GetPatient : GoapAction
{
    GameObject resource;
    public override bool PrePerform()
    {
        target = GoapWorld.Instance.RemovePatient();
        // Se não houver paciente, falha
        if(target == null) { return false; }
        // Captura o cubículo livre
        resource = GoapWorld.Instance.RemoveCubicle();
        // Se existir um cubículo livre, reserva-o
        if(resource != null) { inventory.AddItem(resource); }
        else
        {
            GoapWorld.Instance.AddPatient(target);
            target = null;
            return false;
        }
        GoapWorld.Instance.GetWorld().UpdateState("FreeCubicle", -1);
        return true;
    }
    public override bool PostPerform()
    {
        GoapWorld.Instance.GetWorld().UpdateState("atWaitingRoom", -1);
        if (target) target.GetComponent<GoapAgent>().inventory.AddItem(resource);
        return true;
    }
}
