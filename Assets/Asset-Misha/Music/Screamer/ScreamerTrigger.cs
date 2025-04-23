using UnityEngine;

public class ScreamerTrigger : MonoBehaviour
{
    public AudioClip screamerSound; // ���� ��������
    public Transform spawnPoint; // ��� �������� ����
    public float volume = 1f; // ���������
    public float soundDuration = 3f; // ����� ������������
    public GameObject tree; // ������, ������� ����� ������
    private Animator treeAnimator; // ������ �� Animator ������
    private bool isTreeFallen = false; // ��������, ����� �� ������

    private void Start()
    {
        // �������� Animator ���������� ������
        treeAnimator = tree.GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // ������������� ���� ��������
            ScreamerManager.instance.PlayScreamer(screamerSound, spawnPoint.position, volume, soundDuration);
            //Debug.Log("������� �����������!");

            // ������ ���, ����� ������ ������
            if (tree != null && !isTreeFallen)
            {
                //Debug.Log("������ ������!");
                FallTree();
                isTreeFallen = true; // ������ �����, ����� �� ���������
            }

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

    }
}
