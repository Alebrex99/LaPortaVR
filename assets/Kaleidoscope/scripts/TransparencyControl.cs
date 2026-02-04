using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransparencyControl : MonoBehaviour
{
    [Range(0.0f, 1.0f)]
    public float _currentAlpha = 0.0f;
    private Renderer[] _renderers;
    
    // Start is called before the first frame update
    void Start()
    {
        _renderers = GetComponentsInChildren<Renderer>();
        foreach (Renderer r in _renderers)
        {
            FadeThis(r, _currentAlpha);
        }
    }
    public IEnumerator AlphaRoutine(float a, float b, float duration)
    {
        float timer = 0;
        while (timer <= duration)
        {
            _currentAlpha = Mathf.Lerp(a, b, timer / duration);
            foreach (Renderer r in _renderers)
            {
                FadeThis(r, _currentAlpha);
            }
            timer += Time.deltaTime;
            yield return null;
        }
        _currentAlpha = b;
        foreach (Renderer r in _renderers)
        {
            FadeThis(r, b);
        }
    }
    private void FadeThis(Renderer r, float inputAlpha)
    {
        Material mat = r.material;
        Material newMat = new Material(Shader.Find("Universal Render Pipeline/Unlit"));
        
 
        // Set surface type to Transparent
        mat.SetFloat("_Surface", 1.0f);
 
        // Set Blending Mode to Alpha
        mat.SetFloat("_Blend", 0.0f);    
 
        // Set alpha
        mat.color = new Color(mat.color.r, mat.color.g, mat.color.b, inputAlpha);  
               
        newMat = mat;
        r.material = newMat;
    }
}
