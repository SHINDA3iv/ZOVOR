using System.Collections.Generic;
using UnityEngine;

public class FusePanelLogic : MonoBehaviour
{
    // Список всех сокетов (пустых мест для предохранителей)
    public List<GameObject> sockets = new List<GameObject>();

    // Счетчик вставленных предохранителей
    private int placedFusesCount = 0;

    void Start()
    {
        // Проверяем, что сокеты добавлены
        if (sockets.Count == 0)
        {
            Debug.LogError("Список сокетов пуст! Добавьте сокеты в инспекторе.");
        }

        // Настройка коллайдеров сокетов
        foreach (var socket in sockets)
        {
            var collider = socket.GetComponent<Collider>();
            if (collider != null)
            {
                collider.isTrigger = true; // Убедитесь, что коллайдер является триггером
            }
            else
            {
                Debug.LogError("У сокета " + socket.name + " отсутствует коллайдер!");
            }
        }
    }

    // Метод, вызываемый при вставке предохранителя в сокет
    public void OnFusePlaced(GameObject fuse, GameObject socket)
    {
        // Отключаем коллайдер у вставленного предохранителя
        var fuseCollider = fuse.GetComponent<Collider>();
        if (fuseCollider != null)
        {
            fuseCollider.enabled = false;
        }
        else
        {
            Debug.LogError("У предохранителя " + fuse.name + " отсутствует коллайдер!");
        }

        // Увеличиваем счетчик вставленных предохранителей
        placedFusesCount++;

        Debug.Log("Вставлен предохранитель. Текущее количество: " + placedFusesCount);

        if (placedFusesCount == 4)
        {
            Debug.Log("Все предохранители вставлены!");
            OpenDoor();
        }
    }

    // Метод для открытия двери
    private void OpenDoor()
    {
        // Здесь можно добавить логику для поднятия двери
        Debug.Log("Дверь открывается...");
        // Например, анимация или перемещение объекта двери
    }

   
    // Метод для поиска сокета по позиции
    private GameObject FindSocket(Vector3 position)
    {
        foreach (var socket in sockets)
        {
            if (Vector3.Distance(socket.transform.position, position) < 0.1f)
            {
                return socket;
            }
        }
        return null;
    }
}