using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HandlePoints : MonoBehaviour
{
    public Transform target;
    public InputField func;
    public GameObject pointUIBase;

    public List<GameObject> pointsUI;
    public GameObject contentBtn;
    
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButton(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.tag == "Pivot")
                {
                    target = hit.transform; ;
                }
            }
        }
        if(Input.GetMouseButtonUp(0))
        {
            target = null;
        }
        if (target != null)
        {
            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            target.transform.position = pos;
        }
    }
    public void addPoint()
    {
        pointsUI.Add(Instantiate(pointUIBase, contentBtn.transform));
    }

    public void updateUIPoints(List<GameObject> basePoisition)
    {
        for (int i = 0; i < pointsUI.Count; i++)
        {
            InputField xIf = pointsUI[i].transform.GetChild(1).GetComponent<InputField>();
            InputField yIf = pointsUI[i].transform.GetChild(2).GetComponent<InputField>();
            if(!xIf.isFocused && !yIf.isFocused)
            {
                xIf.text = basePoisition[i].transform.position.x.ToString();
                yIf.text = basePoisition[i].transform.position.y.ToString();
            }
        }
    }
    public void reloadPositionByUiX(string posiX)
    {
        float x;
        try
        {
            x = float.Parse(posiX);
        }
        catch (System.Exception)
        {
            x = 0;
        }
        target.transform.position = new Vector2(x, target.transform.position.y);
        target = null;
    }

    public void reloadPositionByUiY(string posiY)
    {
        float y;
        try
        {
            y = float.Parse(posiY);
        }
        catch (System.Exception)
        {
            y = 0;
        }
        target.transform.position = new Vector2(target.transform.position.x,y);
        target = null;
    }
}
