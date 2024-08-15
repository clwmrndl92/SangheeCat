using UnityEngine;

namespace SangheeCat{

    public class ParallaxController : MonoBehaviour
    {

        [System.Serializable]
        public class ParallaxLayer
        {
            public GameObject layerObject;
            public float parallaxFactor;
            [HideInInspector]
            public float length;
            [HideInInspector]
            public float startPosX;

            public float zoomFactor;
            [HideInInspector]
            public Vector3 startSize;
        }

        public ParallaxLayer[] layers;
        
        float startZoomSize;
        Camera _cam;

        void Start()
        {
            _cam = Camera.main;
            startZoomSize = Camera.main.orthographicSize;
            
            foreach (var layer in layers)
            {
                layer.startPosX = layer.layerObject.transform.position.x;
                layer.length = layer.layerObject.GetComponent<SpriteRenderer>().bounds.size.x;

                layer.startSize = layer.layerObject.transform.localScale;
                
            }
        }

        void LateUpdate () {
            float zoom = _cam.orthographicSize / startZoomSize;
            foreach (var layer in layers)
            {
                float dist = CharactorController.speed * 4f * layer.parallaxFactor * Time.deltaTime;
                
                layer.layerObject.transform.position = new Vector3(layer.layerObject.transform.position.x - dist, layer.layerObject.transform.position.y, layer.layerObject.transform.position.z);
                if (Mathf.Abs(layer.layerObject.transform.position.x - layer.startPosX) >= layer.length)
                    layer.layerObject.transform.position = new Vector3(layer.startPosX, layer.layerObject.transform.position.y, layer.layerObject.transform.position.z);

                
                layer.layerObject.transform.localScale =  layer.startSize + layer.startSize * (zoom-1) * layer.zoomFactor;
            }
        }
    }

}