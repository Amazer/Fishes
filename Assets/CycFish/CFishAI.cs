using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CFishAI : MonoBehaviour
{
    public float turnSpeed = 4f;

    public enum ActionState { Idle = 0, Turn, Swimming,Escape }
    public float tarTime;
    public float _curSpeed;
    public float tarSpeed;
    public float floatSpeed;
    public Vector3 tarDir;
    public Vector3 tarPos;
    public float _curTime;
    public ActionState _curState;

    public float _idleEndTime;
    public float _turnEndTime;
    public Animator _animator;

    private Transform _tr;
    // Use this for initialization

    void Start()
    {

        _tr = transform;
        _animator = _tr.GetComponentInChildren<Animator>();
        RandomBorn();
        GetTarget();
    }

    // Update is called once per frame
    void Update()
    {

        if (_curState == ActionState.Idle)
        {
            if (Time.realtimeSinceStartup < _idleEndTime)
            {
                _tr.rotation = Quaternion.Slerp(_tr.rotation, Quaternion.LookRotation(tarDir), turnSpeed * Time.deltaTime);
                tarPos = _tr.position + _tr.forward * tarSpeed * Time.deltaTime;
                UpdateAnimatorSpeed(tarSpeed);
                if (inTank(tarPos))
                {
                    _tr.position = tarPos;
                }
                else
                {
                    _tr.position = _tr.position - _tr.forward * tarSpeed * Time.deltaTime;
                }
            }
            else
            {
                GetTarget();
            }

        }
        else if (_curState == ActionState.Turn)
        {

            if(Time.realtimeSinceStartup < _turnEndTime)
            {
                _tr.rotation = Quaternion.Slerp(_tr.rotation, Quaternion.LookRotation(tarDir), turnSpeed * Time.deltaTime);
                tarPos = _tr.position + _tr.forward * floatSpeed * Time.deltaTime;
                UpdateAnimatorSpeed(tarSpeed);
                if (inTank(tarPos))
                {
                    _tr.position = tarPos;
                }
                else
                {
                    _tr.position = _tr.position - _tr.forward * floatSpeed * Time.deltaTime;
                }

            }
            else
            {
                _curState = ActionState.Swimming;
                turnSpeed = Random.Range(0.5f, 4f);
                Update();

            }
        }
        else if (_curState == ActionState.Swimming)
        {
            _curTime += Time.deltaTime;
            float factor = _curTime / tarTime;
            if (factor < 0.33f)
            {

                _curSpeed = Mathf.Lerp(floatSpeed, tarSpeed, _curTime / (tarTime * 0.33f));
            }
            else if (factor > 0.66f)
            {
                _curSpeed = Mathf.Lerp(tarSpeed, floatSpeed, (factor - 0.66f) / 0.33f);
            }
            UpdateAnimatorSpeed(_curSpeed);
            tarPos = _tr.position + tarDir * _curSpeed * Time.deltaTime;
            if (inTank(tarPos))
            {
                _tr.position = tarPos;
                _tr.rotation = Quaternion.Slerp(_tr.rotation, Quaternion.LookRotation(tarDir), turnSpeed * Time.deltaTime);
                //                _tr.rotation = Quaternion.LookRotation(tarDir);
                if (_curTime > tarTime)
                {
//                    Idle();
                    GetTarget();
                }
            }
            else
            {
//                Idle();

                GetTarget();
            }
        }
        else if(_curState == ActionState.Escape)
        {
            _curTime += Time.deltaTime;
            float factor = _curTime / tarTime;
            if (factor < 0.13f)
            {
                _curSpeed = Mathf.Lerp(floatSpeed, tarSpeed, _curTime / (tarTime * 0.13f));
            }
            else if (factor > 0.66f)
            {
                _curSpeed = Mathf.Lerp(tarSpeed, floatSpeed, (factor - 0.66f) / 0.33f);
            }
            UpdateAnimatorSpeed(_curSpeed);
            tarPos = _tr.position + tarDir * _curSpeed * Time.deltaTime;
            if (inTank(tarPos))
            {
                _tr.position = tarPos;
                _tr.rotation = Quaternion.Slerp(_tr.rotation, Quaternion.LookRotation(tarDir), turnSpeed * Time.deltaTime);
                if (_curTime > tarTime)
                {
                    GetTarget();
                }
            }
            else
            {
                GetTarget();
            }

        }

    }
    private void UpdateAnimatorSpeed(float speed)
    {
        if(!_animator)
        {
            return;
        }
        if (speed > 1.5f)
        {
            speed = 1.5f;
        }
        else if (speed < 0.2f)
        {
            speed = 0.2f;
        }
        _animator.speed = speed;

    }
    private void Idle()
    {
        _idleEndTime = Time.realtimeSinceStartup + Random.Range(1f, 3f);
        //        _idleTimeEndTime = Time.realtimeSinceStartup + Random.Range(5f, 10f);
        _curState = ActionState.Idle;
        tarSpeed = Random.Range(-0.5f, 0.5f);
        turnSpeed = Random.Range(0.2f, 1f);
        float random = Random.Range(1, 10);
        if (random > 3)
        {
            tarDir = new Vector3(_tr.eulerAngles.x, 0, _tr.eulerAngles.z);
        }

    }
    private void GetTarget()
    {
        _curState = ActionState.Turn;

        float turnTime = Random.Range(0.1f, 2f);
        _turnEndTime = Time.realtimeSinceStartup + turnTime;
        float rangeLimit = 3;
        floatSpeed = Random.Range(-1f, 1f);
        turnSpeed =turnTime * Random.Range(0.5f, 1f);
        tarTime = Random.Range(5f, 10f);
        float deltaY = Random.Range(-rangeLimit, rangeLimit);
        float tarY = _tr.position.y + deltaY;
        if (tarY < Tank.instance.height.x)
        {
            tarY = Tank.instance.height.x;
        }
        else if (tarY > Tank.instance.height.y)
        {
            tarY = Tank.instance.height.y;
        }
        float fixedMaxWidth = Tank.instance.width.y - 3f * rangeLimit;
        float tarX = Random.Range(Tank.instance.width.x, fixedMaxWidth);
        if (tarX > fixedMaxWidth * 0.5f)
        {
            tarX = tarX + 3 * rangeLimit;
        }
        tarPos = new Vector3(tarX, tarY, Random.Range(Tank.instance.depth.x, Tank.instance.depth.y));
        tarDir = tarPos - _tr.position;
        tarSpeed = tarDir.magnitude / tarTime * 1.5f;
        tarDir.Normalize();
//        turnSpeed = Random.Range(0.5f, 4f);
        _curTime = 0f;
        float random = Random.Range(0, 10);
        if(random>=7f)
        {
            _curState = ActionState.Swimming; 
            turnSpeed = Random.Range(0.5f, 4f);
        }

    }
    private bool inTank(Vector3 position)
    {
        if (position.x < Tank.instance.width.x || position.x > Tank.instance.width.y)
        {
            return false;
        }
        if (position.y < Tank.instance.height.x || position.y > Tank.instance.height.y)
        {
            return false;
        }
        if (position.z < Tank.instance.depth.x || position.z > Tank.instance.depth.y)
        {
            return false;
        }
        return true;
    }
    private void RandomBorn()
    {
        _tr.transform.position = new Vector3(Tank.instance.RandomX(), Tank.instance.RandomY(), Tank.instance.RandomZ());
    }
    public void Escape()
    {
        _curState = ActionState.Escape;

        float turnTime = Random.Range(0.1f, 0.5f);
        _turnEndTime = Time.realtimeSinceStartup + turnTime;
        float rangeLimit = 3;
        floatSpeed = Random.Range(-1f, 1f);
        turnSpeed = Random.Range(5f, 10f);
        tarTime = Random.Range(5f, 10f);
        float deltaY = Random.Range(-rangeLimit, rangeLimit);
        float tarY = _tr.position.y + deltaY;
        if (tarY < Tank.instance.height.x)
        {
            tarY = Tank.instance.height.x;
        }
        else if (tarY > Tank.instance.height.y)
        {
            tarY = Tank.instance.height.y;
        }
        float fixedMaxWidth = Tank.instance.width.y - 3f * rangeLimit;
        float tarX = Random.Range(Tank.instance.width.x, fixedMaxWidth);
        if (tarX > fixedMaxWidth * 0.5f)
        {
            tarX = tarX + 3 * rangeLimit;
        }
        tarPos = new Vector3(tarX, tarY, Random.Range(Tank.instance.depth.x, Tank.instance.depth.y));
        tarDir = tarPos - _tr.position;
        tarSpeed = tarDir.magnitude / tarTime * 1.5f;
        tarDir.Normalize();
//        turnSpeed = Random.Range(0.5f, 4f);
        _curTime = 0f;
    }
}
