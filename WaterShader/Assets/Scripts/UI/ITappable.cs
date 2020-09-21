using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface ITappable
{
    bool Tappable2D { get; set; } // true: Plane / 2D Collider, false: 3D Model, 3D Collider
    GameObject GOTapRef { get; set; }
    void OnInitialize();

    void OnTap(Touch t, Vector3 p);
    void OnTap(Touch t, Vector3 p, float d);

    void OnTapWasLetGo();

    void OnDoubleTap();

}
