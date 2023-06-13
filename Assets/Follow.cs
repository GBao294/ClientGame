using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
    public Transform objectToFollow;
    RectTransform rectTransform;
    // Start is called before the first frame update
    private void Awake()
    {
       // rectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        //if (objectToFollow != null)
        //{
        //    rectTransform.position = objectToFollow.position;
        //}
    }
}
