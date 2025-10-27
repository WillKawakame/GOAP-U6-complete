using UnityEngine;

public class LookAt : MonoBehaviour
{
    void Start()
    {
        transform.LookAt(Camera.main.transform.position);
    }
}
