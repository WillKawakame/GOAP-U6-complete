using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Node
{
    public Node parent;
    public float cost;
    public Dictionary<string, int> state;
    public GoapAction action;
    public Node(Node parent, float cost, Dictionary<string, int> state, GoapAction action)
    {
        this.parent = parent;
        this.cost = cost;
        this.state = new Dictionary<string, int>(state);
        this.action = action;
    }
    // Considerando tamb�m os estados do agente
    public Node(Node parent, float cost, Dictionary<string, int> state, Dictionary<string, int> agentstate, GoapAction action)
    {
        this.parent = parent;
        this.cost = cost;
        this.state = new Dictionary<string, int>(state);
        foreach (KeyValuePair<string, int> a in agentstate)
            if (!state.ContainsKey(a.Key))
                this.state.Add(a.Key, a.Value);
        this.action = action;
    }
}
public class GoapPlanner
{
    public Queue<GoapAction> ActionPlan(List<GoapAction> actions, Dictionary<string, int> goal, WorldStates states)
    {
        List<GoapAction> usableActions = new List<GoapAction>();
        foreach(GoapAction a in actions)
        {
            if (a.IsAchievable())
            {
                usableActions.Add(a);
            }
        }
        List<Node> leaves = new List<Node>();
        Node start = new Node(null, 0, GoapWorld.Instance.GetWorld().GetStates(), states.GetStates(), null);
        bool success = BuildGraph(start, leaves, usableActions, goal);
        if (!success)
        {
            Debug.Log("N�o foi poss�vel criar o plano");
            return null;
        }
        Node cheapest = null;
        foreach(Node leaf in leaves)
        {
            if (cheapest == null) { cheapest = leaf; }
            else if (leaf.cost < cheapest.cost) { cheapest = leaf; }
        }
        List<GoapAction> result = new List<GoapAction>();
        Node n = cheapest;
        while (n != null)
        {
            if (n.action != null)
            {
                result.Add(n.action); // Adiciona ao final, que � mais eficiente
            }
            n = n.parent;
        }
        // A associa��o da lista com o reverse tem a complecidde O(n)
        Queue<GoapAction> queue = new Queue<GoapAction>(result.AsEnumerable().Reverse());
        // Para exibir as a��es
        Debug.Log("The plan is: ");
        foreach (GoapAction a in queue)
        {
            Debug.Log("Q: " + a.actionName);
        }
        return queue;
    }

    bool BuildGraph(Node parent, List<Node> leaves, List<GoapAction> usableActions, Dictionary<string, int> goal)
    {
        bool foundPath = false;
        foreach (GoapAction action in usableActions)
        {
            if (action.IsAchievableGiven(parent.state))
            {
                Dictionary<string, int> currentState = new Dictionary<string, int>(parent.state);
                foreach (KeyValuePair<string, int> effect in action.effects)
                {
                    if (!currentState.ContainsKey(effect.Key))
                    {
                        currentState.Add(effect.Key, effect.Value);
                    }
                }
                Node node = new Node(parent, parent.cost + action.cost, currentState, action);

                if (GoalAchieved(goal, currentState))
                {
                    leaves.Add(node);
                    foundPath = true;
                }
                else
                {
                    // A cada itera��o, a lista de a��es � reduzida
                    List<GoapAction> subset = ActionSubset(usableActions, action);                    
                    bool found = BuildGraph(node, leaves, subset, goal);
                    if (found) { foundPath = true; }
                }
            }
        }
        return foundPath;
    }

    // Verifica se a meta foi alcan�ada
    bool GoalAchieved(Dictionary<string, int> goal, Dictionary<string, int> state)
    {
        foreach (KeyValuePair<string, int> g in goal)
        {
            // Verifica se a chave NÃO existe OU se o valor é DIFERENTE
            if (!state.ContainsKey(g.Key) || state[g.Key] != g.Value)
            {
                return false;
            }
        }
        return true;
    }

    // Remove a a��o da lista de a��es
    List<GoapAction> ActionSubset(List<GoapAction> actions, GoapAction removeMe)
    {
        List<GoapAction> subset = new List<GoapAction>();
        foreach (GoapAction a in actions)
        {
            // Se a a��o n�o � igual a removeMe, adiciona a a��o na lista
            if (!a.Equals(removeMe)) { subset.Add(a); }
        }
        return subset;
    }
}
