using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

public class Block : MonoBehaviour
{
    [HideInInspector] public int Index = -2;
    [HideInInspector] public bool IsDisappearing = false;
    [HideInInspector] public bool IsAppearing = false;

    private Material material;

    public string MaterialName
    {
        get => material.name;
    }

    private void Awake()
    {
        material = GetComponent<MeshRenderer>().sharedMaterial;
    }

    private void OnMouseEnter()
    {
        if (IsAppearing || IsDisappearing) return;
        if (BlockManager.Instance.CurrentSelectionIndex == Index) return; 
        transform.localScale = Vector3.one;
    }

    private void OnMouseOver()
    {
        if (IsAppearing || IsDisappearing) return;
        if (BlockManager.Instance.CurrentSelectionIndex == Index) return;
        if (transform.localScale != Vector3.one) transform.localScale = Vector3.one;
    }

    private void OnMouseExit()
    {
        if (IsAppearing || IsDisappearing) return;
        if (BlockManager.Instance.CurrentSelectionIndex == Index) return;
        transform.localScale = Vector3.one * 0.8f;
    }

    private void OnMouseDown()
    {
        if (IsAppearing || IsDisappearing) return;
        if (BlockManager.Instance.CurrentSelectionIndex == Index) return;
        BlockManager.Instance.CurrentSelectionIndex = Index;
    }

    public void Appear()
    {
        IsAppearing = true;
        gameObject.SetActive(true);
        transform.DOScale(0.8f, 0.2f).onComplete += () => IsAppearing = false;
    }

    public void Disappear()
    {
        IsDisappearing = true;
        transform.DOScale(0f, 0.2f).onComplete += () =>
        {
            IsDisappearing = false;
            gameObject.SetActive(false);
        };
    }
}
