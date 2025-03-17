//using UnityEngine;

//public class VRGrabObject : MonoBehaviour
//{
//    private Rigidbody rb;
//    private Transform handTransform;

//    void Start()
//    {
//        rb = GetComponent<Rigidbody>();
//        rb.isKinematic = false; // Убедитесь, что Rigidbody активен
//    }

//    public void Grab(Transform hand)
//    {
//        handTransform = hand;
//        rb.isKinematic = true; // Отключаем физику при захвате
//        transform.SetParent(hand); // Привязываем объект к руке
//    }

//    public void Release()
//    {
//        transform.SetParent(null);
//        rb.isKinematic = false; // Включаем физику при отпускании
//    }

//    void FixedUpdate()
//    {
//        if (handTransform != null)
//        {
//            rb.MovePosition(handTransform.position); // Перемещаем объект через физику
//            rb.MoveRotation(handTransform.rotation);
//        }
//    }
//}