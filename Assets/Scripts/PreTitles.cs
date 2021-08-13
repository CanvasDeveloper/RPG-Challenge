using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreTitles : MonoBehaviour
{
    [SerializeField]private float timeIntro = 2f;
    private void Start() {
        StartCoroutine(Delay());
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(timeIntro);
        FadeController.Instance.NextScene();
    }
}
