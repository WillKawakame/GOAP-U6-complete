// ComputerStatus.cs (Versão Final Corrigida)

using System.Collections; // NECESSÁRIO PARA COROUTINES
using UnityEngine;
using UnityEngine.UI;

// (O enum FeedbackType deve existir de um script anterior)
public enum FeedbackType { None, Good, Bad, Buggy }

public class Computer : MonoBehaviour
{
    public enum ComputerState
    {
        Off,
        Waiting,  // Ocioso, ícone desligado
        InUse,
        ShowFeedback, // Mostrando o ícone de feedback temporariamente
        Broken
    }

    [Header("Controle do Inspector")]
    public bool isBroken = false;
    [Tooltip("Tempo (em segundos) que o ícone de feedback fica visível")]
    public float feedbackDisplayTime = 5.0f; // Tempo para mostrar o feedback

    [Header("Componentes de UI")]
    public Image statusIcon;

    [Header("Sprites de Estado e Feedback")]
    public Sprite inUseSprite;
    public Sprite brokenSprite;
    [Space(10)]
    public Sprite goodFeedbackSprite;
    public Sprite badFeedbackSprite;
    public Sprite buggyFeedbackSprite;

    [Header("Feedback Recebido")]
    [SerializeField]
    private FeedbackType lastReceivedFeedback = FeedbackType.None;

    private ComputerState currentState;

    void Start()
    {
        if (isBroken)
        {
            SetState(ComputerState.Broken);
            return;
        }
        
        if (statusIcon == null)
        {
            Debug.LogWarning("Componente 'statusIcon' (Image) não foi linkado em: " + gameObject.name);
            this.enabled = false;
            return;
        }
        UpdateStateFromInspector();
    }

    void Update()
    {
        if (statusIcon == null) return;
        UpdateStateFromInspector();
    }

    // O agente chama esta função para ENTREGAR o feedback
    public void ReceiveFeedback(FeedbackType feedback)
    {
        lastReceivedFeedback = feedback;
        Debug.Log($"Computador {gameObject.name} recebeu feedback: {feedback}");
    }

    // Função principal que controla os ícones
    public void SetState(ComputerState newState)
    {
        // Impede que o estado seja alterado para algo que não seja "Waiting"
        // enquanto o feedback está sendo mostrado.
        if (currentState == ComputerState.ShowFeedback && newState != ComputerState.Waiting)
        {
            // Se um novo agente sentar (InUse) enquanto o feedback está visível,
            // permita a mudança e pare a coroutine.
            if (newState == ComputerState.InUse)
            {
                StopAllCoroutines(); // Para a contagem regressiva de feedback
            }
            else
            {
                return; // Ignora a mudança
            }
        }

        currentState = newState;
        statusIcon.enabled = true; // Habilita o ícone por padrão

        switch (currentState)
        {
            case ComputerState.InUse:
                statusIcon.sprite = inUseSprite;
                break;

            case ComputerState.Broken:
                statusIcon.enabled = true;
                statusIcon.sprite = brokenSprite;
                break;

            case ComputerState.Off:
                statusIcon.enabled = false;
                break;

            // --- NOVO ESTADO DEDICADO ---
            case ComputerState.ShowFeedback:
                ShowFeedbackIcon(); // Mostra o ícone de feedback
                // Inicia o timer para limpar o feedback
                StartCoroutine(ClearFeedbackState());
                break;

            // --- ESTADO DE ESPERA CORRIGIDO ---
            case ComputerState.Waiting:
                // SEU PEDIDO ORIGINAL: "se ele estiver na espera o sprite fique desativado"
                lastReceivedFeedback = FeedbackType.None; // Limpa o feedback
                statusIcon.enabled = false; // Desativa o ícone
                break;
        }
    }

    // Nova Coroutine para fazer a transição de ShowFeedback -> Waiting
    private IEnumerator ClearFeedbackState()
    {
        // Espera o tempo definido
        yield return new WaitForSeconds(feedbackDisplayTime);

        // Se o estado ainda for ShowFeedback (e não foi interrompido 
        // por um novo usuário), mude para Waiting.
        if (currentState == ComputerState.ShowFeedback)
        {
            SetState(ComputerState.Waiting);
        }
    }

    // Função interna para decidir qual ícone de feedback mostrar
    private void ShowFeedbackIcon()
    {
        statusIcon.enabled = true; // Garante que está visível
        switch (lastReceivedFeedback)
        {
            case FeedbackType.Good:
                statusIcon.sprite = goodFeedbackSprite;
                break;
            case FeedbackType.Bad:
                statusIcon.sprite = badFeedbackSprite;
                break;
            case FeedbackType.Buggy:
                statusIcon.sprite = buggyFeedbackSprite;
                break;
            case FeedbackType.None:
            default:
                // Se por algum motivo não houver feedback,
                // apenas desativa o ícone.
                statusIcon.enabled = false;
                break;
        }
    }

    private void UpdateStateFromInspector()
    {
        if (currentState == ComputerState.InUse || currentState == ComputerState.ShowFeedback)
        {
            return;
        }

        else if (isBroken) SetState(ComputerState.Broken);
        else SetState(ComputerState.Waiting);
    }

    public ComputerState GetCurrentState()
    {
        return currentState;
    }
}