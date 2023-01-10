using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fade : MonoBehaviour
{
    [SerializeField]
    public bool visible = true;

    private CanvasGroup cg;

    private void Start()
    {
        cg = GetComponent<CanvasGroup>();
    }

    //// Update is called once per frame
    //void Update()
    //{
    //    if(visible && cg.alpha <= 0)
    //    {
    //        StartCoroutine(DoFadeIn());
    //    }
    //    else if(!visible && cg.alpha >= 1)
    //    {
    //        StartCoroutine(DoFadeOut());
    //    }
    //}

    public IEnumerator DoFadeOut()
    {
        while (cg.alpha > 0)
        {
            cg.alpha -= Time.deltaTime / 2;
            yield return null;
        }
        cg.interactable = false;
        yield return null;
    }

    public IEnumerator DoFadeIn()
    {
        while (cg.alpha < 1)
        {
            cg.alpha += Time.deltaTime / 2;
            yield return null;
        }
        cg.interactable = false;
        yield return null;
    }
}
