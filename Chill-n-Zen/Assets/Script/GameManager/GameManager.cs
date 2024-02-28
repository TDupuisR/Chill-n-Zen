using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using NaughtyAttributes;
using System.Runtime.CompilerServices;

namespace GameManagerSpace
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;
        public static LibraryItem libraryItems;
        public static AudioManager audioManager;
        public static SaveData saveData;

        [SerializeField] LibraryItem _libraryItems;
        [SerializeField] AudioManager _audioManager;
        [SerializeField] SaveData _saveData;
        [SerializeField] GameObject _loadingScreen;

        private void OnValidate()
        {
            if (_libraryItems == null)
                Debug.LogError(" (error : 1x1) No Library Items Script assigned ", _libraryItems);
            if (_loadingScreen == null)
                Debug.LogError(" (error : 1x2) No loading screen assigned ", _loadingScreen);
            if (_saveData == null)
                Debug.LogError(" (error : 1x2) No save data assigned ", _saveData);
        }

        private void OnEnable() { DontDestroyOnLoad(gameObject); }

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
            saveData = _saveData;
        }

        public void ChangeScene(int sceneIndex)
        {
            if (_loadingScreen != null) _loadingScreen.SetActive(true);
            else
            {
                Debug.LogError(" (error : 1x2) No loading screen assigned ", _loadingScreen);
            }

            StartCoroutine(AsyncLoadScnene(sceneIndex));
        }
        IEnumerator AsyncLoadScnene(int sceneIndex)
        {
            yield return null;

            AsyncOperation LoadSceneOperation = SceneManager.LoadSceneAsync(sceneIndex);
            LoadSceneOperation.allowSceneActivation = false;

            while (!LoadSceneOperation.isDone)
            {
                if (LoadSceneOperation.progress >= 0.9f)
                {
                    LoadSceneOperation.allowSceneActivation = true;
                }

                yield return new WaitForFixedUpdate();
            }
        }

    }

    public static class GMStatic
    {
        //Tag for furnitures identification//
        public enum tagRoom { Null, Other, Bedroom, Livingroom, Kitchen }
        public enum tagType { Null, Furniture, Object, Mural, Ceiling, Carpet }
        public enum tagStyle { Null, Vintage, Disco, Kitch, Modern, Futuristic }

        //Tag for furnitures technical identification//
        public enum tagUsage { Null, Bed, Sink, Storage, Table, Seat, Entertainement, Oven, Fridge, Mirror, Decoration, Light }
        public enum constraint { None, Front, Seat, Bed }
    }
}

