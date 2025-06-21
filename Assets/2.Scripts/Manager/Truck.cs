using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Truck : MonoBehaviour
{
    public static Truck Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private int _zombieContactCount = 0;
    public int zombieContactCount
    {
        get { return _zombieContactCount; }
        set {
            _zombieContactCount = value; 

            if(_zombieContactCount > 0)
            {
               Background.Instance.StopScroll();
            }
            else
            {
                Background.Instance.ResumeScroll();
            }

        }
    }
}
