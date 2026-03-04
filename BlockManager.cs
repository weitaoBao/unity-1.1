using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

public class BlockManager : MonoBehaviour
{
    public static BlockManager Instance { get; private set; }

    [SerializeField] private UnityEvent blockDisappearing;

    private int currentSelectionIndex = -1;
    private Block[] blockArray;
    private bool isRotating = false;
    private Transform child;

    public int CurrentSelectionIndex
    {
        get => currentSelectionIndex;
        set
        {
            if (currentSelectionIndex != -1)
            {
                if (blockArray[value].MaterialName == blockArray[currentSelectionIndex].MaterialName)
                {
                    blockArray[value].Disappear();
                    blockArray[currentSelectionIndex].Disappear();
                    blockDisappearing?.Invoke();
                    ShopingManager.Instance.Money += 10;
                    currentSelectionIndex = -1;
                    return;
                }
                else blockArray[currentSelectionIndex].transform.localScale = Vector3.one * 0.8f;
            }
            currentSelectionIndex = value;
            blockArray[currentSelectionIndex].transform.localScale = Vector3.one;
        }
    }

    private void Awake()
    {
        Instance = this;

        blockArray = GetComponentsInChildren<Block>();
        for (int i = 0; i < blockArray.Length; i++)
            blockArray[i].Index = i;
        child = transform.GetChild(0);
    }

    public void Update()
    {
        if (isRotating) return;

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            isRotating = true;
            Debug.Log("Ç°" + Vector3.right);
            Vector3 axis = child.InverseTransformDirection(Vector3.right);
            Debug.Log("ºó" + axis);
            child.DOLocalRotateQuaternion(child.localRotation * Quaternion.AngleAxis(90f, axis), 0.5f).onComplete += () => isRotating = false;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            isRotating = true;
            Vector3 axis = child.InverseTransformDirection(Vector3.right);
            child.DOLocalRotateQuaternion(child.localRotation * Quaternion.AngleAxis(-90f, axis), 0.5f).onComplete += () => isRotating = false;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            isRotating = true;
            transform.DOLocalRotateQuaternion(transform.localRotation * Quaternion.AngleAxis(90f, Vector3.up), 0.5f).onComplete += () => isRotating = false;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            isRotating = true;
            transform.DOLocalRotateQuaternion(transform.localRotation * Quaternion.AngleAxis(-90f, Vector3.up), 0.5f).onComplete += () => isRotating = false;
        }
    }

    public void RemoveBlock()
    {
        string tmpMaterialName = "";
        foreach (var tmp in blockArray)
        {
            if (tmpMaterialName == "" && tmp.gameObject.activeSelf && !tmp.IsDisappearing)
            {
                if (tmp.Index == currentSelectionIndex) currentSelectionIndex = -1;
                tmp.Disappear();
                tmpMaterialName = tmp.MaterialName;
            }
            else if (tmpMaterialName == tmp.MaterialName && tmp.gameObject.activeSelf && !tmp.IsDisappearing)
            {
                tmp.Disappear();
                blockDisappearing?.Invoke();
                return;
            }
        }
    }

    public bool ResetBlock()
    {
        string tmpMaterialName = "";
        foreach (var tmp in blockArray)
        {
            if (tmpMaterialName == "" && !tmp.gameObject.activeSelf && !tmp.IsAppearing)
            {
                tmp.Appear();
                tmpMaterialName = tmp.MaterialName;
            }
            else if (tmpMaterialName == tmp.MaterialName && !tmp.gameObject.activeSelf && !tmp.IsAppearing)
            {
                tmp.Appear();
                return true;
            }
        }

        return false;
    }
}
