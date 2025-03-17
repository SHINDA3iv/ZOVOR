using System.Collections.Generic;
using UnityEngine;

public class FusePanelLogic : MonoBehaviour
{
    private int placedFusesCount = 0;
   
    public void OnFusePlaced(GameObject fuse, GameObject socket)
    {
        placedFusesCount++;
        Debug.Log("Вставлен предохранитель. Текущее количество: " + placedFusesCount);

        if (placedFusesCount == 4)
        {
            OpenDoor();
        }
    }

    private void OpenDoor()
    {
        Debug.Log("Дверь открывается...");
    }

}