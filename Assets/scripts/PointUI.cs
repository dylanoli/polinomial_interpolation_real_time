using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PointUI : MonoBehaviour
{
    public static int idG;
    public int id;
    public Text title;
    public InputField xIf;
    public InputField yIf;
    public Button removeBtn;
    // Start is called before the first frame update
    void Start()
    {
        id = idG;
        title.text = "Point " + id;
        idG++;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void changePosX(string text)
    {
        HandlePoints handlePoints = GameObject.Find("Main Camera").GetComponent<HandlePoints>();
        int index = handlePoints.pointsUI.FindIndex(x => x.GetComponent<PointUI>().id == id);
        handlePoints.target = GameObject.Find("Main Camera").GetComponent<Interpolation>().points[index].transform;
        handlePoints.reloadPositionByUiX(text);
    }

    public void changePosY(string text)
    {
        HandlePoints handlePoints = GameObject.Find("Main Camera").GetComponent<HandlePoints>();
        int index = handlePoints.pointsUI.FindIndex(x => x.GetComponent<PointUI>().id == id);
        handlePoints.target = GameObject.Find("Main Camera").GetComponent<Interpolation>().points[index].transform;
        handlePoints.reloadPositionByUiY(text);
    }

    public void removerPoint()
    {
        HandlePoints handlePoints = GameObject.Find("Main Camera").GetComponent<HandlePoints>();
        Interpolation interpolation = GameObject.Find("Main Camera").GetComponent<Interpolation>();
        int index = handlePoints.pointsUI.FindIndex(x => x.GetComponent<PointUI>().id == id);
        interpolation.removePoint(index);
        verifyCanRemove();
    }

    public static void verifyCanRemove()
    {

        HandlePoints handlePoints = GameObject.Find("Main Camera").GetComponent<HandlePoints>();
        if (handlePoints.pointsUI.Count == 1)
        {
            handlePoints.pointsUI[0].GetComponent<PointUI>().removeBtn.transform.localScale = Vector2.zero;
        }
        else
        {
            handlePoints.pointsUI[0].GetComponent<PointUI>().removeBtn.transform.localScale = Vector2.one;
        }
    }
}
