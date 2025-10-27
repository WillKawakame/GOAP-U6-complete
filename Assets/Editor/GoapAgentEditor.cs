using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(GoapAgentVisual))]
[CanEditMultipleObjects]
public class GoapAgentVisualEditor : Editor 
{
    void OnEnable() { }
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        serializedObject.Update();
        GoapAgentVisual agent = (GoapAgentVisual) target;
        GUILayout.Label("Name: " + agent.name);
        GUILayout.Label("Current Action: " + agent.gameObject.GetComponent<GoapAgent>().currentAction);
        GUILayout.Label("Actions: ");
        foreach (GoapAction a in agent.gameObject.GetComponent<GoapAgent>().actions)
        {
            string pre = "";
            string eff = "";

            foreach (KeyValuePair<string, int> p in a.preconditions)
                pre += p.Key + ", ";
            foreach (KeyValuePair<string, int> e in a.effects)
                eff += e.Key + ", ";

            GUILayout.Label("====  " + a.actionName + "(" + pre + ")(" + eff + ")");
        }
        GUILayout.Label("Goals: ");
        foreach (KeyValuePair<SubGoal, int> g in agent.gameObject.GetComponent<GoapAgent>().goals)
        {
            GUILayout.Label("---: ");
            foreach (KeyValuePair<string, int> sg in g.Key.sGoals)
                GUILayout.Label("=====  " + sg.Key);
        }
        serializedObject.ApplyModifiedProperties();
    }
}