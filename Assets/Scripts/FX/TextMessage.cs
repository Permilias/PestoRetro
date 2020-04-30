using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class TextMessage : FX
{
    float targetHeight;
    public TextMeshPro textMesh;
    public void SetTextAndPlay(string _text, Color _color, float height)
    {
        duration = FXPlayer.Instance.config.textMessageEnterSpeed + FXPlayer.Instance.config.textMessageDuration + FXPlayer.Instance.config.textMessageExitSpeed;
        targetHeight = height;
        textMesh.text = _text;
        textMesh.color = new Color(_color.r, _color.g, _color.b, 0f);
        Play();
    }

    public override void Play()
    {
        base.Play();
        textMesh.transform.localPosition = Vector3.zero;

        textMesh.DOFade(1f, FXPlayer.Instance.config.textMessageEnterSpeed);
        textMesh.transform.DOLocalMove(Vector3.zero + new Vector3(0, targetHeight, 0), FXPlayer.Instance.config.textMessageEnterSpeed).SetEase(FXPlayer.Instance.config.textMessageEnterEase).OnComplete(() =>
        {
            textMesh.transform.DOLocalMove(textMesh.transform.localPosition, FXPlayer.Instance.config.textMessageDuration).OnComplete(() =>
            {
                textMesh.DOFade(0f, FXPlayer.Instance.config.textMessageExitSpeed);
                textMesh.transform.DOLocalMove(textMesh.transform.localPosition + new Vector3(0, FXPlayer.Instance.config.textMessageExitHeight, 0), FXPlayer.Instance.config.textMessageExitSpeed).SetEase(FXPlayer.Instance.config.textMessageExitEase);
            });
        });

    }
}
