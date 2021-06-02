using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField]
    private GameObject _loadingScreen;

    [SerializeField]
    private Image _progressBar;

    [SerializeField]
    private Image _background;

    [SerializeField]
    private TMP_Text _loadingText;

    [SerializeField]
    private TMP_Text _tipText;

    [SerializeField]
    private CanvasGroup _tipCanvasGroup;

    [SerializeField]
    private string[] _tips;

    [SerializeField]
    private Sprite[] _backgrounds;

    private List<AsyncOperation> _scenesLoading = new List<AsyncOperation>();

    private float _totalSceneProgress;

    private int _tipCount;

    public void LoadGame()
    {
        _background.sprite = _backgrounds[Random.Range(0, _backgrounds.Length)];

        _loadingScreen.SetActive(true);

        StartCoroutine(GenerateTips());

        _scenesLoading.Add(SceneManager.UnloadSceneAsync(1));

        StartCoroutine(LoadSceneAsync());

        //_scenesLoading.Add(SceneManager.LoadSceneAsync(2, LoadSceneMode.Additive));

        //StartCoroutine(GetSceneLoadProgress());
    }

    public IEnumerator LoadSceneAsync()
    {
        var operation = SceneManager.LoadSceneAsync(2, LoadSceneMode.Additive);

        operation.allowSceneActivation = false;

        while (!operation.isDone)
        {
            _progressBar.fillAmount = operation.progress;

            _loadingText.text = $"Loading Environment: {operation.progress * 100f}%";

            if (operation.progress >= 0.9f)
            {
                operation.allowSceneActivation = true;
            }

            yield return null;
        }

        _loadingScreen.SetActive(false);
    }

    public IEnumerator GetSceneLoadProgress()
    {
        for (var i = 0; i < _scenesLoading.Count; i++)
        {
            while (!_scenesLoading[i].isDone)
            {
                _totalSceneProgress = 0f;

                foreach (var operation in _scenesLoading)
                {
                    _totalSceneProgress += operation.progress;

                    /*
                    if (operation.progress >= 0.9f)
                    {
                        _loadingText.text = "Press [SPACE] to continue";

                        if (Input.GetKeyDown(KeyCode.Space))
                        {
                            _scenesLoading[i].allowSceneActivation = true;
                        }
                    }
                    */
                }

                _totalSceneProgress = _totalSceneProgress / _scenesLoading.Count * 100f;

                _progressBar.fillAmount = _totalSceneProgress;

                _loadingText.text = $"Loading Environment: {_totalSceneProgress}%";

                yield return null;
            }
        }

        _loadingScreen.SetActive(false);
    }

    public IEnumerator GenerateTips()
    {
        _tipCount = Random.Range(0, _tips.Length);

        _tipText.text = _tips[_tipCount];

        while (_loadingScreen.activeInHierarchy)
        {
            yield return new WaitForSeconds(3f);

            LeanTween.alphaCanvas(_tipCanvasGroup, 0f, 0.5f);

            yield return new WaitForSeconds(0.5f);

            _tipCount++;

            if (_tipCount >= _tips.Length)
            {
                _tipCount = 0;
            }

            _tipText.text = _tips[_tipCount];

            LeanTween.alphaCanvas(_tipCanvasGroup, 1f, 0.5f);
        }
    }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        Instance = this;

        SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);
    }
}