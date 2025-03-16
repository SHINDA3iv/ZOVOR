using UnityEngine;

public class SocketTriggerFuse : MonoBehaviour
{
    // Ссылка на основной менеджер логики
    public FusePanelLogic fusePanelLogic;

    private void OnTriggerEnter(Collider other)
    {
        // Проверяем, что в сокет вставлен предохранитель
        if (other.CompareTag("Fuse"))
        {
            Debug.Log("Предохранитель вставлен в сокет: " + this.name);

            // Получаем коллайдер на объекте предохранителя
            Collider fuseCollider = other.GetComponent<Collider>();

            // Если коллайдер найден, отключаем его
            if (fuseCollider != null)
            {
                fuseCollider.enabled = false;
                Debug.Log("Collider отключен для объекта: " + other.name);
            }
            else
            {
                Debug.LogWarning("Collider не найден на объекте: " + other.name);
            }

            // Вызываем метод из основного менеджера логики
            if (fusePanelLogic != null)
            {
                fusePanelLogic.OnFusePlaced(other.gameObject, this.gameObject);
            }
            else
            {
                Debug.LogError("FusePanelLogic не назначен!");
            }
        }
    }
}