using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static UIController Instance;

    private Camera cam;
    [SerializeField] private Image imgAim;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        cam = Camera.main;
    }

    public void SetTargetHUD()
    {
        imgAim.enabled = !imgAim.enabled;
    }
}
