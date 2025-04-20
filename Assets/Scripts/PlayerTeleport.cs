using UnityEngine;

public class PlayerTeleport : MonoBehaviour
{
    public Transform monster; // Ссылка на монстра
    public Vector3 teleportPosition; // Координаты для телепортации
    public float detectionRange = 1f; // Радиус, в котором срабатывает телепорт

    private void Update()
    {
        if (monster == null) return;

        float distance = Vector3.Distance(transform.position, monster.position);

        if (distance <= detectionRange)
        {
            TeleportPlayer();
        }
    }

    private void TeleportPlayer()
    {
        transform.position = teleportPosition;
        Debug.Log("Игрок получил удар и был телепортирован!");
    }
}
