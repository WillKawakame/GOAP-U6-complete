using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class GoapAgentVisual : MonoBehaviour
{
    public GoapAgent thisAgent;

    void Start()
    {
        thisAgent = this.GetComponent<GoapAgent>();
    }
}
