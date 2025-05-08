using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class FadeText : MonoBehaviour
{
    [SerializeField] private CanvasGroup myUIGroup;

    [SerializeField] private bool fadeIn = false;
    [SerializeField] private bool fadeOut = false;
    
    public float displayTime = 5f;

    public void ShowUI()
    {
        fadeIn = true;
    }

    public void HideUI()
    {
        fadeOut = true;
    }
    
    void Start()
    {
    //     StartCoroutine(DisplayThenHide());
    }

    // IEnumerator DisplayThenHide(){
    //     yield return new WaitForSeconds(displayTime);
    //     Destroy(gameObject);
    // }

    void Update()
    {
        if (fadeIn){
            if (myUIGroup.alpha < 1 ){
                myUIGroup.alpha += Time.deltaTime;
                if (myUIGroup.alpha >= 1){
                    fadeIn = false;
                }
            }
        }
         if (fadeOut){
            if (myUIGroup.alpha >= 0 ){
                myUIGroup.alpha -= Time.deltaTime;
                if (myUIGroup.alpha == 1){
                    fadeOut = false;
                }
            }
        }
    }
}
