using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class Camera_Controler : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera _camera;
    // Start is called before the first frame update
    void Start()
    {
        _camera.m_Lens.OrthographicSize = 4;
    }

   
}
