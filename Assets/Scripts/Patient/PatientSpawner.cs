using UnityEngine;
public class PatientSpawner : MonoBehaviour
{
    public GameObject patientPrefab;
    public int initialPatientCount = 0;
    void Start()
    {
        for(int i = 0; i < initialPatientCount; i++)
        {
            Instantiate(patientPrefab, transform.position, Quaternion.identity);
        }
        Invoke(nameof(InstantiatePatient), Random.Range(5, 10));
    }
    void InstantiatePatient()
    {
        Instantiate(patientPrefab, transform.position, Quaternion.identity);
        Invoke(nameof(InstantiatePatient), Random.Range(5, 10));
    }
}
