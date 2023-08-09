using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageArea : MonoBehaviour
{
    [SerializeField] GameObject _bullet;

    public void SpawnDMGArea()
    {
        Instantiate(_bullet);
    }
}
