using UnityEngine;

namespace GameManagerSpace
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;


        private void Awake()
        {
            if (Instance == null)
            {
                if (Instance != null) Destroy(gameObject);
                Instance = this;
            }
            else
            {
                Debug.LogError(" (error : 1x0) Too many GameManager instance ", gameObject);
            }
        }
    }

    public static class GMStatic
    {
        //Tag for furnitures identification//
        public enum tagRoom { Other, Bedroom, Livingroom, Kitchen }
        public enum tagType { Furniture, Object, Mural, Ceiling }
        public enum tagStyle { Vintage, Disco, Kitch, Modern, Futuristic }

        //Tag for furnitures technical identification//
        public enum tagUsage { Bed, Sink, Storage, Table, Seat, Entertainement, Oven, Fridge, Mirror, Decoration, Light }
    }
}

