using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using NaughtyAttributes;

namespace GameManagerSpace
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;
        public static LibraryItem libraryItems;
        public static AudioManager audioManager;
        public static SaveData saveData;
        public static BudgetManager budgetManager;
        public static RequestManager requestManager;
        public static ColorData colorData;
        public static LevelManager levelManager;

        [SerializeField] LibraryItem _libraryItems;
        [SerializeField] AudioManager _audioManager;
        [SerializeField] SaveData _saveData;
        [SerializeField] BudgetManager _budgetManager;
        [SerializeField] GameObject _loadingScreen;
        [SerializeField] RequestManager _requestManager;
        [SerializeField] ColorData _colorData;
        [SerializeField] LevelManager _levelManager;

        private LoadingAnimation _loadingScript;

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
            if (_requestManager == null)
                Debug.LogError(" (error : 1x5) No request manager assigned ", _requestManager);
            if (_levelManager == null)
                Debug.LogError(" (error : 1x5) No level manager assigned ", _levelManager);
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
            requestManager = _requestManager;
            colorData = _colorData;
            levelManager = _levelManager;

            _loadingScript = _loadingScreen.GetComponent<LoadingAnimation>();
        }

        public void ChangeScene(int sceneIndex)
        {
            if (_loadingScreen != null && _loadingScript != null) _loadingScreen.SetActive(true);
            else
            {
                Debug.LogError(" (error : 1x6) No loading screen assigned ", _loadingScreen);
            }

            StartCoroutine(AsyncLoadScnene(sceneIndex));
        }
        IEnumerator AsyncLoadScnene(int sceneIndex)
        {
            StartCoroutine(_loadingScript.TransitionLoading(2000f, 0f, false));
            do
            {
                yield return new WaitForFixedUpdate();
            } while (!_loadingScript.AnimationFinished);

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

            StartCoroutine(_loadingScript.TransitionLoading(0f, -2000f, true));
        }

        [Button] private void TestLoading() { StartCoroutine(_loadingScript.TransitionLoading(2000f, 0f, false)); }
    }

    public static class GMStatic
    {
        //Tag for furnitures identification//
        public enum tagRoom { Null, Other, Bedroom, Livingroom, Kitchen }
        public enum tagType { Null, Furniture, Object, Mural, Ceiling, Carpet }
        public enum tagMaterial { Null, Wood, Plywood, Fabric }

        //Tag for furnitures technical identification//
        public enum tagUsage { Null, Bed, Sink, Storage, Library, Table, Top, Desk, Workdesk, Seat, Chair, TV, Oven, Fridge, Mirror, Decoration, Window, Light }
        public enum constraint { None, Front, Seat, Chair, Bed }

        //Tag for Items GameObjects
        public enum State { Placed, Moving, Waiting }

        //Request Types
        [System.Serializable]
        public struct Request
        {
            public List<requestObj> obj;
            public List<requestUsage> usage;
            public List<requestColor> color;
            public List<requestMaterial> material;
            public List<requestProximity> proximity;
            public List<requestFreeSpace> freeSpace;
        }

        [System.Serializable]
        public struct requestObj
        {
            public List<Item> itemRequested;
            public bool needAll;
            public int nbRequested;
            public string phraseClient;
            public string solution;
        }
        [System.Serializable]
        public struct requestUsage
        {
            public List<tagUsage> usageRequested;
            public bool needAll;
            public int nbRequested;
            public string phraseClient;
            public string solution;
        }
        [System.Serializable]
        public struct requestColor
        {
            public Color colorRequested;
            public int nbRequested;
            public string phraseClient;
            public string solution;

        }
        [System.Serializable]
        public struct requestMaterial
        {
            public tagMaterial materialRequested;
            public int nbRequested;
            public string phraseClient;
            public string solution;
        }
        [System.Serializable]
        public struct requestProximity
        {
            public tagUsage closeFromRequested;
            public List<tagUsage> closeToRequested;
            public bool isNotProxi;
            public int nbRequested;
            public string phraseClient;
            public string solution;
        }
        [System.Serializable]
        public struct requestFreeSpace
        {
            public int nbRequested;
            public string phraseClient;
            public string solution;
        }
    }
}

