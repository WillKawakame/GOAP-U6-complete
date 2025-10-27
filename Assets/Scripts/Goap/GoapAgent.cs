using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SubGoal
{
    public Dictionary<string, int> sGoals;
    // Se o objetivo deve ser removido ao ser alcan�ado
    public bool remove;
    public SubGoal(string sName, int iCost, bool remove)
    {
        sGoals = new Dictionary<string, int>();
        sGoals.Add(sName, iCost);
        this.remove = remove;
    }
}
public class GoapAgent : MonoBehaviour
{
    public List<GoapAction> actions = new List<GoapAction>();               // Lista de a��es
    public Dictionary<SubGoal, int> goals = new Dictionary<SubGoal, int>(); // Lista de objetivos
    public GoapAction currentAction;                                        // A��o atual
    public Queue<GoapAction> actionQueue;                                   // Fila de a��es
    SubGoal currentGoal;                                                    // Objetivo atual
    GoapPlanner planner;                                                    // Planejador
    bool invoked = false;                                                   // Se a a��o foi invocada    
    public WorldStates personalStates = new WorldStates();                  // Estados pessoais
    public GoapInventory inventory = new GoapInventory();                   // Invent�rio do agente

    public FeedbackType currentFeedback { get; set; } = FeedbackType.None;

    protected virtual void Start()
    {
        GoapAction[] goapActions = GetComponents<GoapAction>();
        foreach (GoapAction act in goapActions)
        {
            actions.Add(act);
        }
    }

    void CompleteAction()
    {
        currentAction.performingAction = false;
        currentAction.PostPerform();
        invoked = false;
    }

    void LateUpdate()
    {
        if (currentAction != null && currentAction.performingAction)
        {
            // O agente possui um path e chegou no destino?
            float distanceToTarget = Vector3.Distance(currentAction.target.transform.position, transform.position);
            // Debug.Log(distanceToTarget);
            if (distanceToTarget <= 2f)
            {
                if (!invoked)
                {
                    //    (Isso evita que o agente olhe para cima ou para baixo)
                    Vector3 targetLookPosition = currentAction.target.transform.position;
                    targetLookPosition.y = transform.position.y;
                    transform.LookAt(targetLookPosition);

                    Debug.Log($"Iniciando ação '{currentAction.actionName}' com Duração: {currentAction.duration}");
                    StartCoroutine(PerformActionDuration(currentAction)); 
                    invoked = true;
                }
            }
            // Se o agente n�o estiver no destino setado inicialmente, setamos o destino
            else if (currentAction.agent.destination != currentAction.target.transform.position)
            {
                currentAction.agent.SetDestination(currentAction.target.transform.position);
            }
            return;
        }
        if (planner == null || actionQueue == null)
        {
            planner = new GoapPlanner();
            // Usamos o linq para sortir a lista por grau de prioridade
            var sortedGoals = from entry in goals orderby entry.Value descending select entry;
            foreach (KeyValuePair<SubGoal, int> sg in sortedGoals)
            {
                actionQueue = planner.ActionPlan(actions, sg.Key.sGoals, personalStates);
                if (actionQueue != null)
                {
                    currentGoal = sg.Key;
                    break;
                }
            }
        }
        // Se a fila de a��es n�o for nula e n�o tiver a��es
        if (actionQueue != null && actionQueue.Count == 0)
        {
            // Se a meta atual for removida
            if (currentGoal.remove)
            {
                // Removemos a meta
                goals.Remove(currentGoal);
            }
            // Zeramos as vari�veis
            planner = null;
        }
        if (actionQueue != null && actionQueue.Count > 0)
        {
            currentAction = actionQueue.Dequeue();
            if (currentAction.PrePerform())
            {
                if (currentAction.target == null && currentAction.targetTag != "")
                {
                    currentAction.target = GameObject.FindWithTag(currentAction.targetTag);
                }
                if (currentAction.target != null)
                {
                    currentAction.performingAction = true;
                    currentAction.agent.SetDestination(currentAction.target.transform.position);
                }
            }
            else { actionQueue = null; }
        }
    }

    private IEnumerator PerformActionDuration(GoapAction action)
    {
        // 1. Espera pela duração definida na ação
        yield return new WaitForSeconds(action.duration);

        // 2. Completa a ação (lógica que estava em CompleteAction)
        action.performingAction = false;
        action.PostPerform();

        // 3. Reseta o 'invoked' para que a próxima ação possa ser executada
        invoked = false;
    }
}