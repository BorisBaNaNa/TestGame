using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamagePanel : MonoBehaviour
{
    [SerializeField]
    private float Speed = 5f;
    [SerializeField]
    private float LifeTime = 1.5f;

    private float _currentTime;
    private TextMeshProUGUI textUGUI;
    private Camera _camera;

    private void Awake()
    {
        textUGUI = GetComponentInChildren<TextMeshProUGUI>();
        _currentTime = LifeTime;
        _camera = Camera.main;

        Destroy(gameObject, LifeTime);
    }

    private void Update()
    {
        MoveToUp();

        textUGUI.alpha = _currentTime / LifeTime;
        _currentTime -= Time.deltaTime;
    }

    private void MoveToUp()
    {
        transform.position = Vector3.MoveTowards(transform.position, transform.position + Vector3.up * 5f, Speed * Time.deltaTime); ;
    }

    public void SetText(string text, bool isCrit)
    {
        if (isCrit)
        {
            text += "!!!";
            textUGUI.color = Color.red;
        }
        textUGUI.text = text;
    }
}
