// using UnityEngine;

// public class CustomerQueue : MonoBehaviour
// {
//     public static CustomerQueue Instance;

//     public Transform[] waitingSpots;

//     private Customer[] occupiedSlots;

//     private void Awake()
//     {
//         if (Instance == null)
//             Instance = this;
//         else
//             Destroy(gameObject);

//         occupiedSlots = new Customer[waitingSpots.Length];
//     }

//     public Transform RequestSpot(Customer customer)
//     {
//         for (int i = 0; i < waitingSpots.Length; i++)
//         {
//             if (occupiedSlots[i] == null)
//             {
//                 occupiedSlots[i] = customer;
//                 return waitingSpots[i];
//             }
//         }
//         return null;
//     }

//     public void ReleaseSpot(Customer customer)
//     {
//         for (int i = 0; i < occupiedSlots.Length; i++)
//         {
//             if (occupiedSlots[i] == customer)
//             {
//                 occupiedSlots[i] = null;
//                 return;
//             }
//         }
//     }
// }