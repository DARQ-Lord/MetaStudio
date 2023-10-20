using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PenDrawer : MonoBehaviour
{
    [Header("Pen Properties")]
    public Transform tip;
    public Material drawingMaterial;
    public Material tipMaterial;
    [Range(0.01f, 0.1f)]
    public float penWidth = 0.01f;
    public Color[] penColors;

    [Header("Hands & Grabbable")]
    public OVRGrabber rightHand;
    public OVRGrabber leftHand;
    public OVRGrabbable grabbable;
    public List<LineRenderer> lines;    
    private LineRenderer currentDrawing;
    private int index;
    private int currentColorIndex;

    private void Start()
    {
        currentColorIndex = 0;
        tipMaterial.color = penColors[currentColorIndex];
    }

    private void Update()
    {
        bool isGrabbed = grabbable.isGrabbed;
        print(isGrabbed);
        if (isGrabbed && OVRInput.Get(OVRInput.Button.SecondaryIndexTrigger) )
        {
            print("Draw Called");
            Draw();
        }
        else if (currentDrawing != null)
        {
            lines.Add(currentDrawing);
            currentDrawing = null;
        }
        else if (OVRInput.GetDown(OVRInput.Button.One))
        {
            SwitchColor();
        }
        else if (OVRInput.GetDown(OVRInput.Button.Two))
        {
            if (penWidth+0.01f<= 0.1f)
            {
                penWidth += 0.01f;
            }
            else
            {
                penWidth = 0.01f;

            }
        }
    }

    private void Draw()
    {
        if (currentDrawing == null)
        {
            index = 0;
            print("currentDrawing == null");
            currentDrawing = new GameObject().AddComponent<LineRenderer>();
            var new_mat = new Material(Shader.Find("Unlit/Color"));
            new_mat.color = penColors[index];
            new_mat.SetColor("_Color", penColors[currentColorIndex]);
            currentDrawing.material = new_mat;
            currentDrawing.startWidth = currentDrawing.endWidth = penWidth;
            currentDrawing.startColor = currentDrawing.endColor = penColors[currentColorIndex];
            currentDrawing.positionCount = 1;
            currentDrawing.SetPosition(0, tip.position);
            print(currentDrawing.GetPosition(0));
            print(currentDrawing.isVisible);
            print(currentDrawing.positionCount);

        }
        else
        {
            var currentPos = currentDrawing.GetPosition(index);
            if (Vector3.Distance(currentPos, tip.position) > 0.01f)
            {
                index++;
                currentDrawing.positionCount = index + 1;
                currentDrawing.SetPosition(index, tip.position);
                print("Vector3.Distance(currentPos, tip.position) > 0.01f");
                print(currentDrawing.GetPosition(index));

            }
        }
    }

    private void SwitchColor()
    {
        if (currentColorIndex == penColors.Length - 1)
        {
            currentColorIndex = 0;
        }
        else
        {
            currentColorIndex++;
        }
        tipMaterial.color = penColors[currentColorIndex];
    }

    public void Reset()
    {
        foreach (var line in lines)
        {
            Destroy(line);
        }
    }
}