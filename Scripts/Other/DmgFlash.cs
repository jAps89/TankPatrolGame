/*****************************************************************************
* Project: CMN5201gpr-0322-Game
* File   : Rotateable.cs
* Date   : 17.04.22
* Author : Jan Apsel (JA)
*
* These coded instructions, statements, and computer programs contain
* proprietary information of the author and are protected by Federal
* copyright law. They may not be disclosed to third parties or copied
* or duplicated in any form, in whole or in part, without the prior
* written consent of the author.
*
* History:
*   22.4.22 JA created 
******************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DmgFlash : MonoBehaviour
{
    [SerializeField] DmgFlash[] mirrorSprites;
    SpriteRenderer _spriteRenderer;

    public SpriteRenderer SpriteRenderer { get { return _spriteRenderer; } }

    [SerializeField] float _intervaMin = 0.02f, _intervaMax = 0.04f;
    [SerializeField] float _time = 0.20f;
    [SerializeField] bool _finalState = true; 

    [SerializeField] Transform _movePos;
    [SerializeField] float _lerpSpeed;

    [SerializeField] EntityStats _entityStats;

    [SerializeField] bool _isDestroyOnDeath = true;
    [SerializeField] bool _canBeKilled = true;

    public delegate void RoutineDone();
    public event RoutineDone OnRoutineDone;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        if (_entityStats != null)
        {
            _entityStats.OnHealthDamageTaken += DamageFlash;
            _entityStats.OnArmorDamageTaken += DamageFlash;
            if(_canBeKilled)
                _entityStats.OnDeath += DeathFlash;
        }
            
    }

    [SerializeField] bool test;

    private void FixedUpdate()
    {
        if (test)
        {
            test = false;
            StartCoroutine(Flash(_intervaMin, _intervaMax, _time, _finalState));
            foreach (DmgFlash sprite in mirrorSprites)
            {
                sprite.StartFlash(_finalState);
            }
        }
    }
    bool _isDeath;
    public void DeathFlash()
    {
        _isDeath = true;
        if(_isDestroyOnDeath)
            OnRoutineDone += DestroySelf;
        StartCoroutine(Flash(_intervaMin, _intervaMax, 3f, false));
    }
    void DamageFlash(float foo, bool bar)
    {
        StartCoroutine(Flash(_intervaMin, _intervaMax, _time, true));
    }
    void DestroySelf()
    {
        Destroy(transform.root.gameObject);
    }

    public void StartFlash()
    {
        StartCoroutine(Flash(_intervaMin, _intervaMax, _time, _finalState));
    }
    public void StartFlash(bool finalState)
    {
        StartCoroutine(Flash(_intervaMin, _intervaMax, _time, finalState));
    }
    public void StartFlash(bool finalState, float time)
    {
        StartCoroutine(Flash(_intervaMin, _intervaMax, time, finalState));
    }

    IEnumerator Flash(float intervalMin, float intervalMax, float time, bool finalStatus)
    {
        if (mirrorSprites!=null)
            foreach (DmgFlash sprite in mirrorSprites)
                sprite.gameObject.SetActive(true);

        while (time > 0)
        {
            float interval = Random.Range(intervalMin, intervalMax);
            time -= interval;
            _spriteRenderer.enabled = !_spriteRenderer.enabled;
            yield return new WaitForSeconds(interval);
        }
        _spriteRenderer.enabled = finalStatus;

        if(mirrorSprites!=null)
            foreach(DmgFlash sprite in mirrorSprites)
                sprite.gameObject.SetActive(finalStatus);

        if(_isDeath) OnRoutineDone?.Invoke();
        yield return null;
    }
    IEnumerator Move()
    {
        Vector3 startPos = transform.position;
        Vector3 endPos = _movePos.position;
        float currLerp = 0.1f;
        while(currLerp < 1f)
        {
            transform.position = Vector3.Lerp(startPos, endPos, currLerp);
            currLerp += _lerpSpeed;
            yield return new WaitForFixedUpdate();
        }
        while (currLerp > 0f)
        {
            transform.position = Vector3.Lerp(startPos, endPos, currLerp);
            currLerp -= _lerpSpeed;
            yield return new WaitForFixedUpdate();
        }
        transform.position = startPos;
    }
}
