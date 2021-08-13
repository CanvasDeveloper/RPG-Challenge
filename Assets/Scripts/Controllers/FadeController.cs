using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Animator))]
public class FadeController : MonoBehaviour
{
    public static FadeController Instance;
    private Animator anim;
    public bool isFadeCompleted;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this; DontDestroyOnLoad(transform.parent.gameObject); 
        }
        else
        {
            Destroy(transform.parent.gameObject);
        }
    }

    void Start()
    {
        anim = GetComponent<Animator>();    
    }

    public void NextScene()
    {
        Time.timeScale = 1;
        StartCoroutine(NextSceneFade());
    }

    public void ChangeScene(string name)
    {
        Time.timeScale = 1;
        StartCoroutine(ChangeSceneFade(name));
    }

    public void ReloadScene()
    {
        Time.timeScale = 1;
        StartCoroutine(ReloadSceneFade());
    }

    public void Exit()
    {
        StartCoroutine(ExitFade());
    }

    IEnumerator NextSceneFade()
    {
        isFadeCompleted = false;
        anim.SetTrigger("fade");
        yield return new WaitForEndOfFrame();
        yield return new WaitUntil(() => isFadeCompleted);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
        anim.SetTrigger("fade");
        yield return new WaitUntil(() => isFadeCompleted);
    }

    IEnumerator ReloadSceneFade()
    {
        isFadeCompleted = false;
        anim.SetTrigger("fade");
        yield return new WaitForEndOfFrame();
        yield return new WaitUntil(() => isFadeCompleted);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        anim.SetTrigger("fade");
    }

    IEnumerator ChangeSceneFade(string name)
    {
        isFadeCompleted = false;
        anim.SetTrigger("fade");
        yield return new WaitForEndOfFrame();
        yield return new WaitUntil(() => isFadeCompleted);
        SceneManager.LoadScene(name);
        anim.SetTrigger("fade");
    }

    IEnumerator ExitFade()
    {
        isFadeCompleted = false;
        anim.SetTrigger("fade");
        yield return new WaitForEndOfFrame();
        yield return new WaitUntil(() => isFadeCompleted);
        Application.Quit();
    }

    public void OnFadeCompleted()
    {
        isFadeCompleted = true;
    }
}
