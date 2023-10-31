using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeInOut : MonoBehaviour
{
    [SerializeField]  private CanvasGroup canvasgroup;
    
    private bool fin = false;
    private bool fout = false;
    private bool chamado = false;

    public IEnumerator Espera(float waitTime) {
        yield return new WaitForSeconds(waitTime);
    }

    public void Update() {

        if(fin==true) {
            canvasgroup.alpha += Time.deltaTime; 
            if(canvasgroup.alpha >= 0.7f)
                fin = false;
        }   

        if(fout==true) {    
            canvasgroup.alpha -= Time.deltaTime;
            if(canvasgroup.alpha == 0) {
                fout = false;
                chamado = false;
            }    
        }     
    }

    public IEnumerator initFade() {
        if(chamado == false) {
            fin = true;
            chamado = true;
            yield return new WaitForSeconds(3.0f);
            fout = true;
        }    
    }

    public void Pronto() {
        StartCoroutine(initFade());
    }
}
