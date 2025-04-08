using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherController : MonoBehaviour
{
    public Archer archer;
    public Camera mainCamera;
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            archer.StartAim();
        }
        if (Input.GetMouseButton(0))
        {
            archer.tensionPoint = MousePosition();
        }
        if (Input.GetMouseButtonUp(0))
        {
            archer.EndAim();
        }
    }
    Vector2 MousePosition()
    {
        return mainCamera.ScreenToWorldPoint(Input.mousePosition);
    }
}
