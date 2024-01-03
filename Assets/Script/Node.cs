using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Node : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI m_Num, m_NumPencil;
    [SerializeField] GameObject m_Node;

    //Values
    public const int SELECT = 0;
    public const int WRONG = 1;
    public const int ROWCOL = 2;
    public const int CLEAR = 3;

    //Methods
    public DataNode dataNode;

    public void SetData(DataNode data)
    {
        //Neu la 0 thi khong hien thi
        dataNode = data;
        m_NumPencil.text = "";
        if (data.num > 0)
        {
            m_Num.text = data.num.ToString();
           //Debug.Log(data.num);
        }
        else
        {
            m_Num.text = "";
        }
    }

    public void SetNum(int num)
    {
        dataNode.num = num;
        m_Num.text = num.ToString();

        DataInput data = new DataInput();
        data.id = dataNode.id;
        data.value = num;

        Board.instance.lstDataInput.Add(data);
    }

    public void ClearPencil()
    {
        m_NumPencil.text = "";
    }

    public void SetPencil(int num)
    {
        m_NumPencil.text = num.ToString();
    }

    public void DoClickNode()
    {
        Board.instance.dataCurrentNode = dataNode;
        Board.instance.SelectNode();
        SetColorNode(SELECT);
    }

    public void SetColorText(bool check)
    {
        if (check)
        {
            m_Num.color = new Color32(0, 0, 0, 225);
        }
        else
        {
            m_Num.color = new Color32(255, 0, 0, 225);
        }
    }

    public void SetColorNode(int color)
    {
        switch (color)
        {
            case SELECT:
                {
                   m_Node.GetComponent<Image>().color = new Color32(148, 255, 236, 255);
                    break;
                }
            case WRONG:
                {
                    m_Node.GetComponent<Image>().color = new Color32(255, 0, 15, 255);
                    break;
                }
            case ROWCOL:
                {
                    m_Node.GetComponent<Image>().color = new Color32(107, 248, 133, 255);
                    break;
                }
            case CLEAR:
                {
                    m_Node.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
                    break;
                }
        }
    }

    // Start is called before the first frame update

    //void Start()
    //{

    //}

    //// Update is called once per frame
    //void Update()
    //{

    //}
}

public class DataNode
{
    public int num, addNum, hintNum;
    public bool canEdit;
    //Moi node deu do 1 box quan ly -> boxId
    public int id, boxId;
    public int x, y;
}
