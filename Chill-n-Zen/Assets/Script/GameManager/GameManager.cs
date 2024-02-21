using UnityEngine;

namespace GameManagerSpace
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;
        public static LibraryItem libraryItems;

        [SerializeField] LibraryItem _libraryItems;

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
            libraryItems = _libraryItems;
        }
    }

    public static class GMStatic
    {
        //Tag for furnitures identification//
        public enum tagRoom {Null, Other, Bedroom, Livingroom, Kitchen }
        public enum tagType {Null, Furniture, Object, Mural, Ceiling }
        public enum tagStyle {Null, Vintage, Disco, Kitch, Modern, Futuristic }

        //Tag for furnitures technical identification//
        public enum tagUsage {Null, Bed, Sink, Storage, Table, Seat, Entertainement, Oven, Fridge, Mirror, Decoration, Light }
    }
}

