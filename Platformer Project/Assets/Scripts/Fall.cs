using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fall : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private int _damage;
    private Rigidbody2D _rb;
     private Controler controler;
    private PlayerItems _player;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }
  //  private void OnTriggerEnter2D(Collider2D collision)
  //  {
      
  //      if (collision.gameObject.tag.Equals("Floor") && Controler.Rigidbody2D.velocity.y <1)
    //    {
   //        
            
                //_player.TakeTanage(_damage);
            
   //         Debug.Log("Damage");
     //   }
  //  }
}
