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
        public static BudgetManager budgetManager;

        [SerializeField] LibraryItem _libraryItems;
        [SerializeField] AudioManager _audioManager;
        [SerializeField] SaveData _saveData;
        [SerializeField] BudgetManager _budgetManager;
        [SerializeField] GameObject _loadingScreen;

        private void OnValidate()
        {
            if (_libraryItems == null)
                Debug.LogError(" (error : 1x1) No Library Items Script assigned ", _libraryItems);
            if (_loadingScreen == null)
                Debug.LogError(" (error : 1x2) No loading screen assigned ", _loadingScreen);
            if (_saveData == null)
                Debug.LogError(" (error : 1x3) No save data assigned ", _saveData);
            if (_budgetManager == null)
                Debug.LogError(" (error : 1x4) No budget manager assigned ", _budgetManager);
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
            budgetManager = _budgetManager;
        }

        public void ChangeScene(int sceneIndex)
        {
            if (_loadingScreen != null) _loadingScreen.SetActive(true);
            else
            {
                Debug.LogError(" (error : 1x5) No loading screen assigned ", _loadingScreen);
            }

            StartCoroutine(AsyncLoadScnene(sceneIndex));
        }
        IEnumerator AsyncLoadScnene(int sceneIndex)
        {
            yield return null;

            AsyncOperation loadSceneOperation = SceneManager.LoadSceneAsync(sceneIndex);
            loadSceneOperation.allowSceneActivation = false;

            while (!loadSceneOperation.isDone)
            {
                if (loadSceneOperation.progress >= 0.9f)
                {
                    loadSceneOperation.allowSceneActivation = true;
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
        public enum constraint { None, Front, Seat, Chair, Bed }

        //Tag for Items GameObjects
        public enum State { Placed, Moving, Waiting }
    }
}

