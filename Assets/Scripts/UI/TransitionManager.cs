using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionManager : MonoBehaviour
{
    public bool TransitionInOnSceneLoad { get; private set; }

    public static TransitionManager transitionManager;

    [SerializeField] GameObject transitionObject;
    readonly Vector3 bottomPos = new Vector3(0, -1700, 0);
    readonly Vector3 topPos = new Vector3(0, 1700, 0);
    [SerializeField] float transitionSpeed = 10;
    [SerializeField] float waitTime = 0.1f;

    [SerializeField] AudioFile splashIn;
    [SerializeField] AudioFile splashOut;

    bool transitioning = false;

    public void Awake()
    {
        if (transitionManager == null || transitionManager == this) 
        {
            transitionManager = this;
            DontDestroyOnLoad(this.gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void OnSceneLoaded(Scene sceneName, LoadSceneMode arg1)
    {
        if (TransitionInOnSceneLoad)
        {
            TransitionIn();
        }
    }

    public void TransitionOut(string newScene)
    {
        if (transitioning) { return; }
        StartCoroutine(TransitionOutRoutine(newScene));
    }

    public void TransitionIn()
    {
        StartCoroutine(TransitionInRoutine());
    }

    

    private IEnumerator TransitionOutRoutine(string newScene)
    {
        transitioning = true;
        GameObject canvas = Instantiate(transitionObject);
        RectTransform imageRect = canvas.transform.GetChild(0).GetComponent<RectTransform>();
        imageRect.anchoredPosition = topPos;

        SoundEffectManager.sfxmanager.PlayOneShot(splashOut);

        while (imageRect.anchoredPosition.y > 0)
        {
            imageRect.anchoredPosition -= new Vector2(0, transitionSpeed * Time.deltaTime);
            yield return null;
        }

        yield return new WaitForSeconds(waitTime);

        TransitionInOnSceneLoad = true;
        SceneManager.LoadScene(newScene);
    }   

    private IEnumerator TransitionInRoutine()
    {
        Debug.Log("here");
        GameObject canvas = Instantiate(transitionObject);
        RectTransform imageRect = canvas.transform.GetChild(0).GetComponent<RectTransform>();
        imageRect.anchoredPosition = Vector3.zero;

        SoundEffectManager.sfxmanager.PlayOneShot(splashIn);

        while (imageRect.anchoredPosition.y > bottomPos.y)
        {
            imageRect.anchoredPosition -= new Vector2(0, transitionSpeed * Time.deltaTime);
            yield return null;
        }

        TransitionInOnSceneLoad = false;
        Destroy(canvas);
        transitioning = false;
    }
}
