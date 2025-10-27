using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
// Alterar a classe para ser abstrata
public abstract class GoapAction : MonoBehaviour
{    
    public string actionName = "Action"; // Nome da ação
    public float cost = 1f;              // Custo da ação (tempo, recursos, etc.)
    public GameObject target;            // Local que a ação deve ser executada
    public string targetTag;             // Se não houver target, busca por tag
    public float duration = 0f;          // Tempo que a ação leva para ser concluída
    public NavMeshAgent agent;           // Agente de navegação
    // WorldState é usado para setar as condicoes e efeitos no Inspector
    public WorldState[] preConditions; // Condições prévias para executar a ação
    public WorldState[] afterEffects;  // Efeitos após a execução da ação
    // Os dicionários são usados para facilitar a manipulação das condições e efeitos
    public Dictionary<string, int> preconditions;
    public Dictionary<string, int> effects;
    public WorldStates agentStates; // Estado do agente
    public bool performingAction = false; // Se a ação está sendo executada
    public GoapInventory inventory; // Inventário do agente

    // Construtor para inicializar os dicionários
    public GoapAction()
    {
        preconditions = new Dictionary<string, int>();
        effects = new Dictionary<string, int>();
    }

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        if(preConditions != null)
        {
            foreach(WorldState w in preConditions)
            {
                preconditions.Add(w.key, w.value);
            }
        }
        if(afterEffects != null)
        {
            foreach (WorldState w in afterEffects)
            {
                effects.Add(w.key, w.value);
            }
        }
        inventory = GetComponent<GoapAgent>().inventory;
        agentStates = GetComponent<GoapAgent>().personalStates;
    }
    // Placeholder para acionar a ação sem testar
    public bool IsAchievable()
    {
        return true;
    }
    // Verifica se a ação é possível dado um conjunto de condições
    public bool IsAchievableGiven(Dictionary<string, int> conditions)
    {
        foreach (KeyValuePair<string, int> p in preconditions)
        {
            // Se a condição não está nos estados, retorna falso
            if (!conditions.ContainsKey(p.Key))
                return false;
        }
        return true;
    }
    // Os métodos abstratos abaixo são semelhantes ao Enter e Exit do FSM
    // PrePerform é chamado quando a ação está prestes a ser executada
    public abstract bool PrePerform();
    // PostPerform é chamado quando a ação foi concluída
    public abstract bool PostPerform();
}
