using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    private void Start()
    {
        GameManager.instance.platform = gameObject;
        GameManager.instance.CreateSimulation();
    }
}
