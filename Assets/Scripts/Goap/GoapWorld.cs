// GoapWorld.cs (Modificado para seleção aleatória de PC)

using System.Collections.Generic;
using UnityEngine;

public sealed class GoapWorld
{
    // Instância singleton
    private static readonly GoapWorld instance = new GoapWorld();
    // Estado global do mundo
    private static WorldStates world;
    // Fila de pacientes
    private static Queue<GameObject> patients;
    // Fila de cubiculos
    private static Queue<GameObject> cubicles;

    // MODIFICADO: Trocamos a Fila (Queue) por uma Lista (List)
    private static List<GameObject> gamingPcs;

    // Construtor estático para inicializar o estado do mundo
    static GoapWorld()
    {
        world = new WorldStates();
        patients = new Queue<GameObject>();
        cubicles = new Queue<GameObject>();

        // MODIFICADO: Inicializa a Lista de PCs
        gamingPcs = new List<GameObject>();

        // Popula a a fila de cubículos com os objetos na cena
        GameObject[] cubs = GameObject.FindGameObjectsWithTag("Cubicle");
        foreach (GameObject c in cubs) { cubicles.Enqueue(c); }

        // Informa ao mundo a quantidade de cubículos livres
        if (cubicles.Count > 0) { world.AddState("FreeCubicle", cubicles.Count); }

        // MODIFICADO: Popula a LISTA de PCs de jogos com objetos na cena
        GameObject[] pcs = GameObject.FindGameObjectsWithTag("GamingPC"); 
        foreach (GameObject pc in pcs) 
        { 
            // MODIFICADO: Usa .Add() em vez de .Enqueue()
            gamingPcs.Add(pc); 
        }

        // MODIFICADO: Informa ao mundo a quantidade de PCs livres (usando .Count)
        if (gamingPcs.Count > 0) { world.AddState("FreePC", gamingPcs.Count); } 
    }

    // Construtor privado para evitar instância externa
    private GoapWorld() { }
    // Propriedade para acessar a instância singleton
    public static GoapWorld Instance { get { return instance; } }
    // Método para acessar o estado global do mundo
    public WorldStates GetWorld() { return world; }

    // Mtodos para gerenciar a fila de pacientes
    public void AddPatient(GameObject patient)
    {
        patients.Enqueue(patient);
    }
    public GameObject RemovePatient()
    {
        if (patients.Count <= 0) return null;
        return patients.Dequeue();
    }
    // Gerencia a fila de cubículos
    public void AddCubicle(GameObject cubicle)
    {
        cubicles.Enqueue(cubicle);
    }
    public GameObject RemoveCubicle()
    {
        if (cubicles.Count <= 0) return null;
        return cubicles.Dequeue();
    }

    // --- MÉTODOS DE PC MODIFICADOS ---

    // Gerencia a LISTA de PCs de jogos
    public void AddPC(GameObject pc)
    {
        // MODIFICADO: Usa .Add() em vez de .Enqueue()
        gamingPcs.Add(pc);
    }

    public GameObject RemovePC()
    {
        // MODIFICADO: Lógica de remoção aleatória
        if (gamingPcs.Count <= 0) return null;

        // 1. Pega um índice aleatório (de 0 até o número de PCs na lista)
        int randomIndex = Random.Range(0, gamingPcs.Count);
        
        // 2. Pega o PC nesse índice
        GameObject pc = gamingPcs[randomIndex];
        
        // 3. Remove o PC da lista (para que outro agente não pegue o mesmo)
        gamingPcs.RemoveAt(randomIndex);
        
        // 4. Retorna o PC aleatório
        return pc;
    }
}