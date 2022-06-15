﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_MSFireSpirit : SC_RPGUnit
{
    [SerializeField] GameObject _skillObject;
    Transform skillPos;

    void Awake()
    {

        InitData(SC_PublicDefine.eUnitName.FireSpirit);
        _aniCtrl = transform.GetComponent<Animator>();
        originPos = transform.position;
        skillPos = transform.GetChild(0).GetComponent<Transform>().transform;
        RAniC = _aniCtrl.runtimeAnimatorController;
    }

    void Update()
    {
        if (_isAction)
        {
            transform.position = Vector3.MoveTowards(transform.position, attPos, Time.deltaTime * 10f);

            if (attPos == transform.position)
            {
                if (_nowSkillTurn > 0)
                {
                    ChangeAniToAction(SC_PublicDefine.eActionState.ATTACK);
                }
                else
                {
                    ChangeAniToAction(SC_PublicDefine.eActionState.SKILL);
                    _nowSkillTurn = _skillTurn;
                }
                _isAction = false;
            }
        }
    }
    public override void ChangeAniToAction(SC_PublicDefine.eActionState state)
    {
        switch (state)
        {
            case SC_PublicDefine.eActionState.IDEL:
                _aniCtrl.SetBool("IsRun", false);
                break;
            case SC_PublicDefine.eActionState.RUN:
                _aniCtrl.SetBool("IsRun", true);
                break;
            case SC_PublicDefine.eActionState.ATTACK:
                SC_SoundControlManager._instance.EffectSoundPlay(SC_PublicDefine.eSoundTrack.FireSpiritAttack);
                _aniCtrl.SetTrigger("Attack");
                break;
            case SC_PublicDefine.eActionState.SKILL:
                SC_SoundControlManager._instance.EffectSoundPlay(SC_PublicDefine.eSoundTrack.FireSpiritSkill);
                _aniCtrl.SetTrigger("Skill");
                break;
            case SC_PublicDefine.eActionState.HIT:
                _aniCtrl.SetTrigger("Hit");
                break;
            case SC_PublicDefine.eActionState.DEAD:
                _isdead = true;
                SC_SoundControlManager._instance.EffectSoundPlay(SC_PublicDefine.eSoundTrack.FireSpiritDeath);
                _aniCtrl.SetBool("IsDead", true);
                break;
        }
    }

    public override void FindTime(int SelectAction)
    {
        switch (SelectAction)
        {
            case 0:
                FindAniPlayTime("MSFireSpirit_ATTACK");
                break;
            case 2:
                FindAniPlayTime("MSFireSpirit_SKILL");
                break;
            default:
                AniPlayTime = 0.5f;
                break;
        }
    }
    protected override void OverrideSkillPos()
    {
        attPos = SC_RPGInGameManager._instance._skillPos.transform.position;
    }

    void ShootSkillObject()
    {
        Instantiate(_skillObject, skillPos.position, Quaternion.identity, transform);
    }
}
