using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutlineObject : MonoBehaviour
{
    private Transform highlight;
    [Range(0.0f, 10.0f)]
    public float width = 7.0f;

    public void StartOutline()
    {
        if (highlight != null)
        {
            highlight.gameObject.GetComponent<Outline>().enabled = false;
        }
        //assegno ad highLight il gameObject
        highlight = gameObject.transform;
        //cerca di ottenere un componente Outline esistente dal GameObject "highlight"
        Outline outline = highlight.gameObject.GetComponent<Outline>();
        
        if (outline != null)
        {
            outline.enabled = true;
        }
        //se outline è null il componente non esiste
        else
        {
            outline = highlight.gameObject.AddComponent<Outline>();
            outline.enabled = true;
            outline.OutlineColor = Color.cyan;
            outline.OutlineWidth = width;
        }
    }

    public void StopOutline()
    {
        if (highlight != null)
        {
            Outline outline = highlight.gameObject.GetComponent<Outline>();
            if (outline != null)
            {
                outline.enabled = false;
            }
        }
    }
    
}
