using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DistanceSlider : MonoBehaviour
{
    public ZombieCommand zombieCommand;
    public Slider progressSlider;

    private void Start()
    {
        progressSlider.maxValue = 1f; // 슬라이더 최대값 설정
        progressSlider.value = 0;
    }
    void Update()
    {
        progressSlider.value = zombieCommand.GetProgress();
    }
}
