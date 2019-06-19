using UnityEngine;
using System.Collections;
using ClearWindow;
using System;

public class ResultPopup : MonoBehaviour, OnClickListener {

    public Sprite failSprite;

    private SpriteRenderer sr;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    public void OnClick()
    {
        if (GameManager.Instance) GameManager.Instance.Status = GameStatus.PLAYING;

        Destroy(gameObject);
    }

    public void SetSuccess(bool success)
    {
        sr.sprite = failSprite;
    }
}
