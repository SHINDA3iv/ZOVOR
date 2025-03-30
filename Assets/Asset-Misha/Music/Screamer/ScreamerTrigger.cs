using UnityEngine;

public class ScreamerTrigger : MonoBehaviour
{
    public AudioClip screamerSound; // Звук скримера
    public Transform spawnPoint; // Где создаётся звук
    public float volume = 1f; // Громкость
    public float soundDuration = 3f; // Время проигрывания
    public GameObject tree; // Дерево, которое будет падать
    private Animator treeAnimator; // Ссылка на Animator дерева
    private bool isTreeFallen = false; // Проверка, упало ли дерево

    private void Start()
    {
        // Получаем Animator компонента дерева
        treeAnimator = tree.GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Воспроизводим звук скримера
            ScreamerManager.instance.PlayScreamer(screamerSound, spawnPoint.position, volume, soundDuration);
            //Debug.Log("Скример активирован!");

            // Делаем так, чтобы дерево падало
            if (tree != null && !isTreeFallen)
            {
                //Debug.Log("Дерево падает!");
                FallTree();
                isTreeFallen = true; // Дерево упало, чтобы не повторить
            }

            // Удаляем сам триггер
            Destroy(gameObject);
        }
    }

    private void FallTree()
    {
        // Запускаем анимацию падения дерева только при входе в триггер
        if (treeAnimator != null)
        {
            treeAnimator.SetTrigger("Fall"); // Запускаем триггер анимации
        }

    }
}
