using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;


public class inventory : MonoBehaviour
{
    public List<item> items;

    [SerializeField]
    private Transform slotParent;
    [SerializeField]
    private slot[] slots_1;
    [SerializeField]
    private slot[] slots_2;
    public int a = 0;
    public int b = 0;

    public GameObject leftBtn;
    public AudioSource booksound;
    public GameObject page;

    public static inventory Instance;


    void Awake()
    {
        FreshSlot_1();
        FreshSlot_2();
        leftBtn.GetComponent<Button>().onClick.AddListener(PreSlot);
        leftBtn.SetActive(false);
        Instance = this;
    }
    void Update()
    {
        
        FreshSlot_1();
        FreshSlot_2();
    }
    public void FreshSlot_1()
    {
        int i = a;
        for (; i < items.Count && i < slots_1.Length+a; i++)
        {
            slots_1[i%8].item = items[i];
        }
        for (; i < slots_1.Length+a; i++)
        {
            slots_1[i%8].item = null;
        }
    }

    public void FreshSlot_2()
    {
        int i = b;
        for (; i < items.Count && i < slots_2.Length + b; i++)
        {
            slots_2[i % 8].item = items[i];
        }
        for (; i < slots_2.Length + b; i++)
        {
            slots_2[i % 8].item = null;
        }
    }

    public void NextSlot()
    {
        plusA();
        Invoke("plusB", 1.3f);
        leftBtn.SetActive(true);
    }
    public void PreSlot()
    {
        if(b >= 8)
        {
            booksound.Play();
            page.GetComponent<Animator>().Play("l_bt");
            Invoke("minusA", 1.3f);
            minusB();
        }
    }
    public void plusA()
    {
        a += 8;
    }
    public void minusA()
    {
        a -= 8;
    }
    public void plusB()
    {
        b += 8;
    }
    public void minusB()
    {
        b -= 8;
    }

    public void AddItem(item _item)
    {
        items.Add(_item);
        items.Distinct().ToList();
        foreach(item item in items)
        {
            Debug.Log(item.itemName);
        }
    }

}
