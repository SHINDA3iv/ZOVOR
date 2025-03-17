//using UnityEngine;
//using UnityEngine.XR.Interaction.Toolkit;

//public class SocketManager : MonoBehaviour
//{
//    private MyXRSocketInteractor socketInteractor;

//    private void Start()
//    {
//        socketInteractor = GetComponent<MyXRSocketInteractor>();
//    }

//    private void OnEnable()
//    {
//        // Подписываемся на события входа и выхода объекта из сокета
//        socketInteractor.selectEntered.AddListener(OnObjectPlaced);
//        socketInteractor.selectExited.AddListener(OnObjectRemoved);
//    }

//    private void OnDisable()
//    {
//        // Отписываемся от событий
//        socketInteractor.selectEntered.RemoveListener(OnObjectPlaced);
//        socketInteractor.selectExited.RemoveListener(OnObjectRemoved);
//    }

//    private void OnObjectPlaced(SelectEnterEventArgs args)
//    {
//        // Получаем объект, который был помещен в сокет
//        HandleXRGrabInteractable interactable = args.interactableObject.transform.GetComponent<HandleXRGrabInteractable>();

//        if (interactable != null)
//        {
//            // Проверяем, находится ли объект уже в другом сокете
//            if (interactable.IsInAnotherSocket(socketInteractor.transform))
//            {
//                Debug.LogWarning("Object is already in another socket. This socket is not available.");
//                socketInteractor.SetAvailability(false); // Делаем этот сокет недоступным
//                return;
//            }

//            // Помещаем объект в сокет
//            interactable.PlaceInSocket(socketInteractor.transform);

//            // Делаем все остальные сокеты недоступными для этого объекта
//            MyXRSocketInteractor[] otherSockets = FindObjectsOfType<MyXRSocketInteractor>();
//            foreach (var otherSocket in otherSockets)
//            {
//                if (otherSocket != socketInteractor)
//                {
//                    otherSocket.SetAvailability(false);
//                }
//            }
//        }
//    }

//    private void OnObjectRemoved(SelectExitEventArgs args)
//    {
//        // Получаем объект, который был удален из сокета
//        HandleXRGrabInteractable interactable = args.interactableObject.transform.GetComponent<HandleXRGrabInteractable>();

//        if (interactable != null)
//        {
//            // Удаляем объект из сокета
//            interactable.RemoveFromSocket();

//            // Делаем все сокеты доступными снова
//            MyXRSocketInteractor[] allSockets = FindObjectsOfType<MyXRSocketInteractor>();
//            foreach (var socket in allSockets)
//            {
//                socket.SetAvailability(true);
//            }
//        }
//    }
//}