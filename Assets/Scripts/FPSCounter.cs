using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FPSCounter : MonoBehaviour
{
    [SerializeField] protected TMP_Text counter;

    private void Start()
    {
        Application.targetFrameRate = 60;
    }
    // Update is called once per frame
    void LateUpdate()
    {
        counter.text = "FPS: " + Mathf.RoundToInt(1 / Time.deltaTime);
    }
}
