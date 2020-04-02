using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamaraHandle : MonoBehaviour
{
    public Camera can;
    public GameObject ground;
    public float sizeCan;
    public float auxPos;
    int sizeAuxY;
    int sizeAuxX;
    public GameObject yAuxBase;
    public GameObject xAuxBase;
    public GameObject []yAux;
    public GameObject[] xAux;
    // Start is called before the first frame update
    void Start()
    {
        sizeCan = 5;
        sizeAuxY = 40;
        sizeAuxX = 22;
        yAux = new GameObject[sizeAuxY];
        xAux = new GameObject[sizeAuxX];
        for (int i = 0; i < sizeAuxY; i++)
        {
            yAux[i] = Instantiate(yAuxBase, ground.transform);
        }
        for (int i = 0; i < sizeAuxX; i++)
        {
            xAux[i] = Instantiate(xAuxBase, ground.transform);
        }
    }

    // Update is called once per frame
    void Update()
    {
        ground.transform.localScale = new Vector3(1* sizeCan / 5, 1* sizeCan / 5, 1);
        can.orthographicSize = sizeCan;
        
        if (Input.GetAxis("Mouse ScrollWheel") > 0f && sizeCan>1) // forward
        {
            sizeCan--;
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f) // backwards
        {
            sizeCan++;
        }
        if(sizeCan<1)
        {
            sizeCan = 1;
        }
        handlePositionAuxs();
    }

    void handlePositionAuxs()
    {
        if (sizeCan < 5) auxPos = sizeCan / 5;
        else if (sizeCan < 40) auxPos = (int)(sizeCan / 5);
        else auxPos = (int)(sizeCan / 10);
        for (int i = 0; i < sizeAuxY; i++)
        {
            float pos = i < (sizeAuxY / 2) ? (i - (sizeAuxY / 2))*auxPos : (i - (sizeAuxY / 2)+1) * auxPos;
            yAux[i].transform.GetChild(1).GetComponent<TMPro.TextMeshPro>().text = pos.ToString();
            yAux[i].transform.position = new Vector3(pos, yAux[i].transform.position.y, yAux[i].transform.position.z);
        }
        for (int i = 0; i < sizeAuxX; i++)
        {
            float pos = i < (sizeAuxX / 2) ? (i - (sizeAuxX / 2)) * auxPos : (i - (sizeAuxX / 2) + 1) * auxPos;
            xAux[i].transform.GetChild(1).GetComponent<TMPro.TextMeshPro>().text = pos.ToString();
            xAux[i].transform.position = new Vector3(xAux[i].transform.position.x, pos, xAux[i].transform.position.z);
        }
    }
}
