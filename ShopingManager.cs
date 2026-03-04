using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ShopingManager : MonoBehaviour
{
    public static ShopingManager Instance { get; private set; }

    [SerializeField] private UnityEvent clockClicked;

    private Text moneyUI;

    [SerializeField] private int money = 0;

    public int Money
    {
        get => money;
        set
        {
            money = value;
            moneyUI.text = "$" + money;
        }
    }

    private void Awake()
    {
        Instance = this;

        moneyUI = transform.Find("Money").GetComponentInChildren<Text>();
    }

    private void Start()
    {
        Money = money;
    }

    public void OnItemClicked(int index)
    {
        if (index == 0)
        {
            if (Money >= 25)
            {
                Money -= 25;
                clockClicked?.Invoke();
            }
        }
        else if (index == 1)
        {
            if (Money >= 50)
            {
                Money -= 50;
                for (int i = 0; i < 3; i++)
                    BlockManager.Instance.RemoveBlock();
            }
        }
    }
}
