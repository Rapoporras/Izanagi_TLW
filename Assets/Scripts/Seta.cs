using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class Seta : MonoBehaviour
{
    [SerializeField] private Animator _anim;
    [SerializeField] private bool _esporas;
    [SerializeField] private bool _fall;
    private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {

                if(_fall)
                    _anim.SetBool("Fall", true);
            }
        }
}
