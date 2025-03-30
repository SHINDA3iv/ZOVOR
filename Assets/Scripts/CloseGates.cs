using System.Collections.Generic;
using UnityEngine;

public class CloseGates : MonoBehaviour
{
    public List<GameObject> triggerObjects = new List<GameObject>();
    public GameObject gate1;
    public GameObject gate2;

    public float closedGateYPosition = 0f;

    public GameObject xrOrigin;

    private void OnTriggerEnter(Collider other)
    {
        // Проверяем, что коллизия произошла с объектом xrOrigin
        if (other.gameObject == xrOrigin)
        {
            CloseGatesDoor();


            RemoveAllTriggersAndColliders();
        }
    }

    private void CloseGatesDoor()
    {
        // Закрываем ворота, изменяя их позицию по оси Y
        gate1.transform.position = new Vector3(gate1.transform.position.x, closedGateYPosition, gate1.transform.position.z);
        gate2.transform.position = new Vector3(gate2.transform.position.x, closedGateYPosition, gate2.transform.position.z);
    }

    private void RemoveAllTriggersAndColliders()
    {
        // Удаляем все триггеры из списка
        foreach (var obj in triggerObjects)
        {
            if (obj != null)
            {
                Destroy(obj);
            }
        }
        triggerObjects.Clear();
    }

}