// using UnityEngine;

// namespace SangheeCat{

//     public class ZoomController : MonoBehaviour
//     {

//         [System.Serializable]
//         public class ZoomLayer
//         {
//             public GameObject layerObject;
//             public float zoomFactor;
//         }
//         public ZoomLayer[] layers;
        
//         private Camera _cam;
//         float startSize;

//         void Start()
//         {
//             _cam = Camera.main;
//             startSize = _cam.orthographicSize;
//         }

//         void FixedUpdate () {
//             foreach (var layer in layers)
//             {
//                 float zoom = _cam.orthographicSize / startSize;
//                 layer.layerObject.transform.localScale *= zoom * layer.zoomFactor;
//             }
//         }
//     }

// }