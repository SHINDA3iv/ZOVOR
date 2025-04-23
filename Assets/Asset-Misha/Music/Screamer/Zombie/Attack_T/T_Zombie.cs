using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T_Zombie : MonoBehaviour
{
    public AudioClip screamerSound; // ���� ��������
    public Transform spawnPoint; // ��� �������� ����
    public float volume = 1f; // ���������
    public float soundDuration = 3f; // ����� ������������
    public GameObject tree; // ������, ������� ����� ������
    private Animator treeAnimator; // ������ �� Animator ������
    private bool isTreeFallen = false; // ��������, ����� �� ������
    [SerializeField] private float fallDestroyTime = 5f; // ����� �������� ������

    private void Start()
    {
        // �������� Animator ���������� ������
        treeAnimator = tree.GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            tree.SetActive(true);
            // ������ ���, ����� ������ ������
            if (tree != null && !isTreeFallen)
            {
                //Debug.Log("������ ������!");
                FallTree();
                isTreeFallen = true; // ������ �����, ����� �� ���������
            }
            // ������������� ���� ��������
            ScreamerManager.instance.PlayScreamer(screamerSound, spawnPoint.position, volume, soundDuration);
            //Debug.Log("������� �����������!");

            

            // ������� ��� �������
            Destroy(gameObject);
        }
    }

    private void FallTree()
    {
        // ��������� �������� ������� ������ ������ ��� ����� � �������
        if (treeAnimator != null)
        {
            treeAnimator.SetTrigger("Fall"); // ��������� ������� ��������
        }

        Destroy(tree, fallDestroyTime); // ������� ������ ����� �������� �����
    }
}

