using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Util //useful generic methods
{
    public static void SetLayerRecursively(GameObject _Obj, int _NewLayer)
    {
        if(_Obj == null)
        {
            return;
        }
        _Obj.layer = _NewLayer;
        foreach(Transform _Child in _Obj.transform)
        {
            if(_Child == null)
            {
                continue;
            }
            SetLayerRecursively(_Child.gameObject, _NewLayer);
        }       
        
    }
}
