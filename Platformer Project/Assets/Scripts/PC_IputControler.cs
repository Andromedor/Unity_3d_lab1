using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Controler))]
public class PC_IputControler: MonoBehaviour
{
    DateTime _strikeClickTime;
    Controler _playreController;
    
    float _move;
    bool _jumper;
    bool _сeiling;
    bool _canAttack;
    // Start is called before the first frame update
    void Start()
    {
       
        _playreController = GetComponent<Controler>();
    }

    // Update is called once per frame
    void Update()
    {
        _move = Input.GetAxis("Horizontal");
        if (Input.GetButtonUp("Jump"))
        {
           
            _jumper = true;
        }
        // _сeiling = Input.GetKey(KeyCode.C);
        _сeiling = Input.GetKey(KeyCode.C);

        if (Input.GetKey(KeyCode.E))
        {
            _playreController.StaryCasting();
        }
        if (!IsPointerOverUI())
        {
            if (Input.GetButtonDown("Fire1"))
            {

                _strikeClickTime = DateTime.Now;
                _canAttack = true;
            }
            if (Input.GetButtonUp("Fire1"))
            {

                float holdtime = (float)(DateTime.Now - _strikeClickTime).TotalSeconds;
                if (_canAttack)
                    _playreController.StartStriker(holdtime);
                _canAttack = false;


                //   _animator.SetTrigger("Attack1");

            }
        }
        if((DateTime.Now -_strikeClickTime).TotalSeconds >= _playreController.ChargeTime * 2 && _canAttack)
        {
        
            _playreController.StartStriker(_playreController.ChargeTime);
           _canAttack = false;
        }
    }
    private void FixedUpdate()
    {
        _playreController.Move(_move, _jumper, _сeiling);
        _jumper = false;
        
    }

    private bool IsPointerOverUI() => EventSystem.current.IsPointerOverGameObject();

}
