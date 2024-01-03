using SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Board : MonoBehaviour
{

    //Fields
    [SerializeField] TextMeshProUGUI m_TextMode, m_TextCountDown;
    [SerializeField] GameObject m_PencilOn, m_PencilOff;
    [SerializeField] GameObject m_PopupSelectMode;

    //Values

    public List<Node> lstNode = new List<Node>();

    private bool isUndoing = false;

    private const int EASY = 0;
    private const int MEDIUM = 1;
    private const int HARD = 2;
    private const int EXPERT = 3;

    public static Board instance;

    public DataNode dataCurrentNode;

    private bool pencil = false;
    private bool isPause = false;

    private float timeCountdown;

    public List<DataInput> lstDataInput = new List<DataInput>();

    //Methods

    public void TimeCountDown()
    {
        if (!isPause)
        {
            timeCountdown += Time.deltaTime;
            m_TextCountDown.text = string.Format("{0:00}:{1:00}:{2:00}",Mathf.FloorToInt(timeCountdown/3600), Mathf.FloorToInt(timeCountdown/60%60), 
                                                                                                                Mathf.FloorToInt(timeCountdown%60));
        }
    }

    public void DoClickNum(int num = 1)
    {
        foreach(Node n in lstNode)
        {
            if(n.dataNode.id == dataCurrentNode.id)
            {
                if(n.dataNode.canEdit == true)
                {
                    if(pencil == false)
                    {
                        n.SetNum(num);
                        CheckError(n, num);
                    }
                    else
                    {
                        n.SetPencil(num);
                    }
                }
            }
        }
    }

    public void DoClickPause()
    {
        isPause = !isPause;
    }

    public void DoClickErase()
    {
        foreach(Node n in lstNode)
        {
            if(n.dataNode.id == dataCurrentNode.id)
            {
                if(n.dataNode.canEdit == true)
                {
                    dataCurrentNode.num = 0;
                    n.SetData(dataCurrentNode);
                    n.ClearPencil();
                }
            }
        }
    }

    public void Restart()
    {
        lstDataInput.Clear();
        timeCountdown = 0;
        foreach(Node n in lstNode)
        {
            if(n.dataNode.canEdit == true)
            {
                n.dataNode.num = 0;
                n.SetData(n.dataNode);
                n.ClearPencil();
            }
        }
        DoClickClosePopup();
    }

    public void DoClickPencil()
    {
        pencil = !pencil;
        m_PencilOn.SetActive(pencil);
        m_PencilOff.SetActive(!pencil);
    }

    public void DoClickOpenPopup()
    {
        m_PopupSelectMode.SetActive(true);
    }

    public void DoClickClosePopup()
    {
        m_PopupSelectMode.SetActive(false);
    }

    public void SelectNode()
    {
        foreach(Node n in lstNode)
        {
            if(dataCurrentNode != null)
            {
                if(n.dataNode.y == dataCurrentNode.y || n.dataNode.x == dataCurrentNode.x || n.dataNode.boxId == dataCurrentNode.boxId)
                {
                    n.SetColorNode(Node.ROWCOL);
                }
                else
                {
                    n.SetColorNode(Node.CLEAR);
                }
                if(n.dataNode.num != 0)
                {
                    if(n.dataNode.num == dataCurrentNode.num)
                    {
                        n.SetColorNode(Node.SELECT);
                    }
                }
            }
            else
            {
                n.SetColorNode(Node.CLEAR);
            }
        }
    }

    public void CheckError(Node node, int num)
    {
        List<Node> listCheckError = new List<Node>();
        foreach(Node n in lstNode)
        {
            if (n.dataNode.id == dataCurrentNode.id) continue;
            if(n.dataNode.y == dataCurrentNode.y || n.dataNode.x == dataCurrentNode.x || n.dataNode.boxId == dataCurrentNode.boxId && n.dataNode.num == dataCurrentNode.num)
            {
                listCheckError.Add(n);
            }
        }

        foreach(Node o in listCheckError)
        {
            if(o.dataNode.num == num)
            {
                node.SetColorText(false);
                break;
            }
            else
            {
                node.SetColorText(true);
            }
        }
    }

    public void DoClickUndo()
    {
        int count = lstDataInput.Count;
        isUndoing = true;
        if(count == 0)
        {
            isUndoing = false;
            return;
        }
        foreach(Node o in lstNode)
        {
            if(o.dataNode.id == lstDataInput[count - 1].id)
            {
                o.dataNode.num = 0;
                o.SetData(o.dataNode);
                CheckError(o, o.dataNode.num);
                dataCurrentNode = o.dataNode;
                o.ClearPencil();
                SelectNode();
            }
            if(count > 1)
            {
                if(o.dataNode.id == lstDataInput[count - 2].id)
                {
                    o.dataNode.num = lstDataInput[count - 2].value;
                    o.SetData(o.dataNode);
                    CheckError(o, o.dataNode.num);
                    dataCurrentNode = o.dataNode;
                    o.ClearPencil();
                    SelectNode();
                }
            }
        }
        isUndoing = false;
        lstDataInput.RemoveAt(lstDataInput.Count - 1);
    }

    public void DoClickNewGame()
    {
        lstDataInput.Clear();
        timeCountdown = 0;
        ParseData(EASY);
    }

    public void ClearBoard()
    {
        if(lstNode.Count != 0)
        {
            foreach(Node o in lstNode)
            {
                if(o.dataNode.canEdit == true)
                {
                    o.dataNode.num = 0;
                    o.SetData(o.dataNode);
                    o.ClearPencil();
                    dataCurrentNode = null;
                    SelectNode();
                    timeCountdown = 0;
                }
            }
        }
    }


    public void ParseData(int modeGame)
    {
        lstDataInput.Clear();
        dataCurrentNode = null;

        int lvl = UnityEngine.Random.Range(0, 200);

        string level = "Level_" + lvl;

        string mode = "";

        switch(modeGame)
        {
            case EASY:
                {
                    mode = "easy/";
                    m_TextMode.text = "easy";
                    timeCountdown = 0;
                    break;
                }
            case MEDIUM:
                {
                    mode = "medium/";
                    m_TextMode.text = "medium";
                    timeCountdown = 0;
                    break;
                }
            case HARD:
                {
                    mode = "hard/";
                    m_TextMode.text = "hard";
                    timeCountdown = 0;
                    break;
                }
            case EXPERT:
                {
                    mode = "expert/";
                    m_TextMode.text = "expert";
                    timeCountdown = 0;
                    break;
                }
        }

        string path = "DB/" + mode + level;

        Debug.Log(path);

        string dataParse = Resources.Load<TextAsset>(path).ToString();

        Debug.Log(dataParse);

        List<DataNode> lstDataNode = new List<DataNode>();

        //JSONNode neu la Object
        JSONNode jN = JSON.Parse(dataParse);

        //JSONArray neu la Array
        //Lay Array tu duong dan o tren
        JSONArray jA = jN["blocks"].AsArray;

        //Trong Array co rat nhieu mang object
        for(int i = 0; i < jA.Count; i ++)
        {
            JSONNode jB = jA[i].AsObject;
            DataNode dn = new DataNode();
            //Neu la string thi .Value
            dn.id = i;
            dn.num = jB["num"].AsInt;
            dn.canEdit = jB["canEdit"].AsBool;
            dn.addNum = jB["addNum"].AsInt;

            //Tinh box
            int _x = -1;
            int _y = -1;

            if (i % 9 <= 2) _x = 0;
            else if (2 < (i % 9) && (i % 9) < 6) _x = 1;
            else _x = 2;

            if (i / 9 <= 2) _y = 0;
            else if (2 < (i / 9) && (i / 9) < 6) _y = 1;
            else _y = 2;

            //Co 9 box tu 0 -> 8. Box cuoi cung la 8 (toi da)
            dn.boxId = _y * 3 + _x;

            dn.x = i / 9;
            dn.y = i % 9;

            lstDataNode.Add(dn);

        }

        for(int i = 0; i < lstNode.Count; i ++)
        {
            lstNode[i].SetData(lstDataNode[i]);
        }

        m_PopupSelectMode.SetActive(false);

    }

    public void Awake()
    {
        instance = this;
        m_PopupSelectMode.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        timeCountdown = 0;
        ParseData(EASY);
        m_PopupSelectMode.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        TimeCountDown();
    }
}

public class DataInput
{
    public int id;
    public int value;
}
