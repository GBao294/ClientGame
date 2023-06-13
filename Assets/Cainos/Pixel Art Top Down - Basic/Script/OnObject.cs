using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnObject : MonoBehaviour
{
    [SerializeField] public GameObject model;
    // Start is called before the first frame update
    private void OnDisable()
    {
        if (model.activeSelf == false)
        {
            model.SetActive(true);
            //GetComponent<CountDown>().RestartCountdown();
        }
    }

}
