using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UpdateWorld : MonoBehaviour
{
    public Text states;
    void LateUpdate()
    {
        Dictionary<string, int> wStates = GoapWorld.Instance.GetWorld().GetStates();
        states.text = "";
        foreach (KeyValuePair<string, int> s in wStates)
        {
            states.text += s.Key + ": " + s.Value + "\n";
        }
    }
}