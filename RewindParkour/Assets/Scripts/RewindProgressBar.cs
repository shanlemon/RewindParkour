using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class RewindProgressBar : MonoBehaviour
{

    [SerializeField] private Slider slider;
    
    private WarpPosition warpPosition;

    private void Start() {
        warpPosition = Managers.Player.GetComponent<WarpPosition>();
    }

    private void Update() {
        slider.value = warpPosition.WarpFillPercentage;
    }
    
}
