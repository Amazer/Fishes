using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CFishAI : MonoBehaviour
{
    public float turnSpeed = 4f;

    public enum ActionState { Idle = 0, Turn, Swimming, Escape, Float }
    public float tarTime;
    public float _curSpeed;
    public float tarSpeed;
    public float floatSpeed;
    public Vector3 tarDir;
    public Vector3 tarPos;
    public float tarDistance;
    public float _curTime;
    public ActionState _curState;
    public ActionState _lastState;

    public Animator _animator;

    private Transform _tr;
    [SerializeField]
    private MoveFlag move;
    [SerializeField]
    private SpeedFlag speed;
    [SerializeField]
    private RotateFlag rota;

    // Use this for initialization

    void Start()
    {
        _tr = transform;
        _tr.parent = Tank.instance.gameObject.transform;
        _animator = _tr.GetComponentInChildren<Animator>();
        move = new MoveFlag(_tr);
        speed = new SpeedFlag(_tr);
        rota = new RotateFlag(_tr);
        RandomBorn();
        Swimming();
    }

    // Update is called once per frame
    void Update()
    {
        float deltaTime = Time.deltaTime;
        speed.Update(deltaTime);
        UpdateAnimatorSpeed(speed.curSpeed);
        move.SetSpeed(speed.curSpeed);
        rota.Update(deltaTime);
        move.Update(deltaTime);
        //        if (_curState == ActionState.Escape)
        //        {
        //            _curTime += Time.deltaTime;
        //            float factor = _curTime / tarTime;
        //            if (factor < 0.13f)
        //            {
        //                _curSpeed = Mathf.Lerp(floatSpeed, tarSpeed, _curTime / (tarTime * 0.13f));
        //            }
        //            else if (factor > 0.66f)
        //            {
        //                _curSpeed = Mathf.Lerp(tarSpeed, floatSpeed, (factor - 0.66f) / 0.33f);
        //            }
        //            UpdateAnimatorSpeed(_curSpeed);
        //            tarPos = _tr.localPosition + tarDir * _curSpeed * Time.deltaTime;
        //            _tr.localPosition = tarPos;
        //            _tr.rotation = Quaternion.Slerp(_tr.rotation, Quaternion.LookRotation(tarDir), turnSpeed * Time.deltaTime);
        //            if (_curTime > tarTime)
        //            {
        //                GetTarget();
        //            }
        //        }

    }
    private void UpdateAnimatorSpeed(float speed)
    {
        if (!_animator)
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

    // 漂浮
    private void Float()
    {
        _curState = ActionState.Float;
        floatSpeed = Random.Range(-1f, 1f);
        float turnTime = Random.Range(0.1f, 2f);
        turnSpeed = turnTime * Random.Range(0.5f, 1f);
        tarTime = Random.Range(1, 5f);
        tarSpeed = Random.Range(-1f, 2f);
        float dis = Random.Range(1, 2);
        tarDir.y = 0;
        Vector3 tarPos = _tr.localPosition + tarDir * dis;
        if (Tank.instance.InTank(tarPos))
        {
            tarDistance = dis;
        }
        else
        {
            tarPos = Tank.instance.InTankPos(tarPos);
            tarDistance = Vector3.Distance(_tr.localPosition, tarPos);
        }
        speed.SetVarMinSpeed(Random.Range(0f, 1f));
        speed.SetVarSpeedConfig(Random.Range(0.1f, 0.5f), Random.Range(0.0f, 0.5f));
        speed.StartVarSpeed(tarDistance, tarTime, SpeedOver);
        rota.SetRotate(turnSpeed, turnSpeed, tarDir);
        move.Move(true, tarDir);
    }
    private void Swimming()
    {
        _curState = ActionState.Swimming;
        float turnTime = Random.Range(0.1f, 2f);
        floatSpeed = Random.Range(-1f, 1f);
        turnSpeed = turnTime * Random.Range(0.5f, 1f);

        float tarX = Tank.instance.RandomX();
        float tarZ = Tank.instance.RandomZ();
        float angle = Random.Range(-35, 35);
        float tanAngle = Mathf.Tan(angle * Mathf.Deg2Rad);
        float tarY = tarX * tanAngle;
        if (tarY < Tank.instance.height.x)
        {
            tarY = Tank.instance.height.x;
        }
        else if (tarY > Tank.instance.height.y)
        {
            tarY = Tank.instance.height.y;
        }

        tarTime = Random.Range(5f, 10f);
        tarPos = new Vector3(tarX, tarY, tarZ);
        tarDir = tarPos - _tr.localPosition;
        tarDistance = tarDir.magnitude;
        tarSpeed = tarDir.magnitude / tarTime;
        tarDir.Normalize();
        _curTime = 0f;
        float random = Random.Range(0, 10);
        if (random >= 7f)
        {
            _curState = ActionState.Swimming;
            turnSpeed = Random.Range(0.5f, 4f);
        }
        speed.SetVarMinSpeed(Random.Range(0f, 2f));
        speed.SetVarSpeedConfig(Random.Range(0.1f, 0.5f), Random.Range(0.2f, 0.5f));
        speed.StartVarSpeed(tarDistance, tarTime, SpeedOver);
        rota.SetRotate(turnSpeed, turnSpeed, tarDir);
        move.Move(true, tarDir);
    }
    private void RandomBorn()
    {
        _tr.transform.localPosition = new Vector3(Tank.instance.RandomX(), Tank.instance.RandomY(), Tank.instance.RandomZ());
    }
    private void MoveOver()
    {
        //        GetTarget();
        Float();
    }
    private void SpeedOver()
    {
        if (Random.Range(0, 10) > 4)
        {

            Swimming();
        }
        else
        {
            Float();
        }
    }
    public void EscapeTest()
    {
        _lastState = _curState;
        _curState = ActionState.Escape;

//        if (_lastState == ActionState.Swimming)
//        {
//
//        }
//        else if (_lastState == ActionState.Float)
//        {
            float turnTime = Random.Range(0.1f, 1f);
            turnSpeed =  Random.Range(4f, 10f);

            float tarX = Tank.instance.RandomX();
            float tarZ = Tank.instance.RandomZ();
            float angle = Random.Range(-35, 35);
            float tanAngle = Mathf.Tan(angle * Mathf.Deg2Rad);
            float tarY = tarX * tanAngle;
            if (tarY < Tank.instance.height.x)
            {
                tarY = Tank.instance.height.x;
            }
            else if (tarY > Tank.instance.height.y)
            {
                tarY = Tank.instance.height.y;
            }

            tarTime = Random.Range(3f, 5f);
            tarPos = new Vector3(tarX, tarY, tarZ);
            tarDir = tarPos - _tr.localPosition;
            tarDistance = tarDir.magnitude;
            tarSpeed = tarDir.magnitude / tarTime;
            tarDir.Normalize();
            speed.SetVarMinSpeed(Random.Range(0f, 2f));
            speed.SetVarSpeedConfig(Random.Range(0.02f, 0.1f), Random.Range(0.2f, 0.5f));
            speed.StartVarSpeed(tarDistance, tarTime, SpeedOver);
            rota.SetRotate(turnSpeed, turnSpeed, tarDir);
            move.Move(true, tarDir);
//        }
    }
    public void Escape()
    {
        _curState = ActionState.Escape;

        float turnTime = Random.Range(0.1f, 0.5f);
        float rangeLimit = 3;
        floatSpeed = Random.Range(-1f, 1f);
        turnSpeed = Random.Range(5f, 10f);
        tarTime = Random.Range(5f, 10f);
        float deltaY = Random.Range(-rangeLimit, rangeLimit);
        float tarY = _tr.localPosition.y + deltaY;
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
        tarDir = tarPos - _tr.localPosition;
        tarDistance = tarDir.magnitude;
        tarSpeed = tarDir.magnitude / tarTime * 1.5f;
        tarDir.Normalize();
        //        turnSpeed = Random.Range(0.5f, 4f);
        _curTime = 0f;
    }
}
