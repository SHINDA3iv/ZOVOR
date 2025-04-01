using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T_Zombie : MonoBehaviour
{
    public AudioClip screamerSound; // Звук скримера
    public Transform spawnPoint; // Где создаётся звук
    public float volume = 1f; // Громкость
    public float soundDuration = 3f; // Время проигрывания
    public GameObject tree; // Дерево, которое будет падать
    private Animator treeAnimator; // Ссылка на Animator дерева
    private bool isTreeFallen = false; // Проверка, упало ли дерево
    [SerializeField] private float fallDestroyTime = 5f; // Время удаления дерева

    private void Start()
    {
        // Получаем Animator компонента дерева
        treeAnimator = tree.GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MainCamera"))
        {
            tree.SetActive(true);
            // Делаем так, чтобы дерево падало
            if (tree != null && !isTreeFallen)
            {
                //Debug.Log("Дерево падает!");
                FallTree();
                isTreeFallen = true; // Дерево упало, чтобы не повторить
            }
            // Воспроизводим звук скримера
            ScreamerManager.instance.PlayScreamer(screamerSound, spawnPoint.position, volume, soundDuration);
            //Debug.Log("Скример активирован!");

            

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

        Destroy(tree, fallDestroyTime); // Удаляем дерево через заданное время
    }
}

