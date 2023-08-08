using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Accessibility;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class LoadingScreen : MonoBehaviour
{
    public static LoadingScreen Instance { get; set; }


    public Slider slider;

    public Text number;

    public bool isReleaseLoading = false;

    public Animator pig;

    public Animator text;
    // Start is called before the first frame update
    
    void Awake()
    {
        Instance = this;
        if (Instance != this)
        {
            //Debug.LogError("Loadiungs screen this != Instance.");
            return;
        }
        DontDestroyOnLoad(gameObject.transform.parent);
       
    }
    public void startAniamtorText()
    {
        text.Play("textAnimation");
    }



    // Update is called once per frame

    public void playAnimationPig()
    {
        pig.Play("piganimation");
    }

    public void StartLoadingScreen(string nameScene)
    {
        gameObject.SetActive(true);
        number.text = "0%";
        Animator animator = GetComponent<Animator>();
        animator.Play("loadingscreen", 0, 0);
        StartCoroutine(LoadScreen(nameScene));
    }
    IEnumerator LoadScreen(string NameScene)
    {
        yield return new WaitForSeconds(1f);
        AsyncOperation operation = SceneManager.LoadSceneAsync(NameScene);
        while (!operation.isDone)
        {
            float processValue = Mathf.Clamp01(operation.progress / .9f) * 100f;
       
            int intValue = (int)processValue;
            number.text = intValue.ToString() + "%";
            slider.value = processValue;
            yield return null;
        }
        Animator animator = GetComponent<Animator>();
        animator.Play("back", 0, 0);
        yield return new WaitForSeconds(1.5f);
        gameObject.SetActive(false);
    }


    public void StartLoadingScreenFirst(string nameScene , bool isDone)
    {
        gameObject.SetActive(true);
        number.text = "0%";
        Animator animator = GetComponent<Animator>();
        animator.Play("loadingscreen",0,0);
        StartCoroutine(LoadScreenFirst(nameScene , isDone));
    }
    //delay tam thoi them 5s de doi bien load nhan vat
    IEnumerator LoadScreenFirst(string NameScene , bool isDone_)
    {
        yield return new WaitForSeconds(1f);
        AsyncOperation operation = SceneManager.LoadSceneAsync(NameScene);
        while (!operation.isDone && isDone_ == false)
        {
            float processValue = Mathf.Clamp01(operation.progress / .9f) * 100f;
            //Debug.Log(processValue);
            int intValue = (int)processValue;
            number.text = intValue.ToString() + "%";
            slider.value = processValue;
            yield return null;
        }
      
        yield return new WaitForSeconds(3f);
        Animator animator = GetComponent<Animator>();
        animator.Play("back", 0, 0);
        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);
    }

    public void StartLoadingScreenDie(float duration)
    {
        gameObject.SetActive(true);
        number.text = "0%";
        Animator animator = GetComponent<Animator>();
        animator.Play("loadingscreen", 0, 0);
        StartCoroutine(CountdownScreen(duration));
        
    }

    IEnumerator CountdownScreen(float duration)
    {
        yield return new WaitForSeconds(1f);
        float startTime = Time.time;
        float endTime = startTime + duration;

        while (Time.time < endTime)
        {
            float progress = Mathf.Clamp01((Time.time - startTime) / duration);
            int intValue = (int)(progress * 100);
            number.text = intValue.ToString() + "%";
            slider.value = progress;
            yield return null;
        }
        Animator animator = GetComponent<Animator>();
        animator.Play("back", 0, 0);
        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);
    }


    public void FlexLoadingScreen(string NameScene, bool isDone_)
    {
        gameObject.SetActive(true);
        number.text = "0%";
        Animator animator = GetComponent<Animator>();
        animator.Play("loadingscreen", 0, 0);
        StartCoroutine(FlexLoading(NameScene , isDone_));
        isReleaseLoading= true;
        Debug.Log("----");
    }

    IEnumerator FlexLoading(string NameScene, bool isDone_)
    {
        yield return new WaitForSeconds(1f);

        // Start loading the scene asynchronously
        AsyncOperation operation = SceneManager.LoadSceneAsync(NameScene);

        // Wait until the scene loading progress reaches 90%
        while (!operation.isDone && isDone_ == false && isReleaseLoading)
        {
            float processValue = Mathf.Clamp01(operation.progress / 0.9f) * 50f;
            int intValue = (int)processValue;
            number.text = intValue.ToString() + "%";
            slider.value = processValue / 100f;
            Debug.Log(slider.value + "slider value" + isDone_ + "---" + isReleaseLoading);
            yield return null;
        }
        Debug.Log(isReleaseLoading);
        // Switch to the second part of the loading
        float startTime = Time.time;
        float duration = 1;
        float endTime = startTime + duration;

        while (Time.time < endTime)
        {
            float progress = Mathf.Clamp01((Time.time - startTime) / duration);
            slider.value = progress + 0.5f;
            int intValue = (int)(slider.value * 100);
            number.text = intValue.ToString() + "%";
            yield return null;
        }

        Animator animator = GetComponent<Animator>();
        animator.Play("back", 0, 0);
        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);
    }


    public void FlexLoadingScreenWithoutBool(string NameScene)
    {
        gameObject.SetActive(true);
        number.text = "0%";
        Animator animator = GetComponent<Animator>();
        animator.Play("loadingscreen", 0, 0);
        StartCoroutine(FlexLoadingWithoutBool(NameScene));
        isReleaseLoading = true;
        Debug.Log("----");
    }

    IEnumerator FlexLoadingWithoutBool(string NameScene)
    {
        yield return new WaitForSeconds(1f);

        // Start loading the scene asynchronously
        AsyncOperation operation = SceneManager.LoadSceneAsync(NameScene);

        // Wait until the scene loading progress reaches 90%
        while (!operation.isDone && isReleaseLoading)
        {
            float processValue = Mathf.Clamp01(operation.progress / 0.9f) * 50f;
            int intValue = (int)processValue;
            number.text = intValue.ToString() + "%";
            slider.value = processValue / 100f;
         
            yield return null;
        }
        
        // Switch to the second part of the loading
        float startTime = Time.time;
        float duration = 1;
        float endTime = startTime + duration;

        while (Time.time < endTime)
        {
            float progress = Mathf.Clamp01((Time.time - startTime) / duration);
            slider.value = progress + 0.5f;
            int intValue = (int)(slider.value * 100);
            number.text = intValue.ToString() + "%";
            yield return null;
        }

        Animator animator = GetComponent<Animator>();
        animator.Play("back", 0, 0);
        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);
    }


}
