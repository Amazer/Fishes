using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CFishAI : MonoBehaviour
{
    public float turnSpeed = 4f;

    public enum ActionState { Swimming, Escape, Float }
    public float tarTime;
    public float _curSpeed;
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
        SpeedOver();
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
        turnSpeed =  Random.Range(2f, 5f);
        tarTime = Random.Range(1, 5f);
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

        tarTime = Random.Range(5f, 10f);
        tarPos = new Vector3(tarX, tarY, tarZ);
        tarDir = tarPos - _tr.localPosition;
        tarDistance = tarDir.magnitude;
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
    public void Escape()
    {
        Debug.Log("EscapeTest");
        _lastState = _curState;
        _curState = ActionState.Escape;


        if (_lastState == ActionState.Swimming)
        {
            float dis = Random.Range(10f, 20f);
            tarPos = _tr.localPosition + tarDir * dis;
            if(!Tank.instance.InTank(tarPos))
            {
                float deltaX = Mathf.Abs(_tr.localPosition.x - tarPos.x);
                if(tarPos.x<Tank.instance.width.x)
                {
                    tarPos.x = _tr.localPosition.x + deltaX;
                }
                else if(tarPos.x >Tank.instance.width.y)
                {
                    tarPos.x = _tr.localPosition.x - deltaX;
                }

                float deltaY = Mathf.Abs(_tr.localPosition.y - tarPos.y);
                if(tarPos.y<Tank.instance.height.x)
                {
                    tarPos.y = _tr.localPosition.y + deltaY;
                }
                else if(tarPos.y >Tank.instance.height.y)
                {
                    tarPos.y = _tr.localPosition.y - deltaY;
                }

                float deltaZ = Mathf.Abs(_tr.localPosition.z - tarPos.z);
                if(tarPos.z<Tank.instance.depth.x)
                {
                    tarPos.z = _tr.localPosition.z + deltaZ;
                }
                else if(tarPos.z >Tank.instance.depth.y)
                {
                    tarPos.z = _tr.localPosition.z - deltaZ;
                }
            }
            tarDir = tarPos - _tr.localPosition;
            tarDir.Normalize();
            tarDistance = (tarPos - _tr.localPosition).magnitude;

            turnSpeed =  Random.Range(4f, 10f);
            tarTime = Random.Range(1f, 3f);
            tarDir.Normalize();
            speed.SetVarMinSpeed(Random.Range(0f, 2f));
            speed.SetVarSpeedConfig(Random.Range(0.02f, 0.1f), Random.Range(0.2f, 0.5f));
            speed.StartVarSpeed(tarDistance, tarTime, SpeedOver);
            rota.SetRotate(turnSpeed, turnSpeed, tarDir);
            move.Move(true, tarDir);

        }
        else if (_lastState == ActionState.Float)
        {
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
            tarDir.Normalize();
            speed.SetVarMinSpeed(Random.Range(0f, 2f));
            speed.SetVarSpeedConfig(Random.Range(0.02f, 0.1f), Random.Range(0.2f, 0.5f));
            speed.StartVarSpeed(tarDistance, tarTime, SpeedOver);
            rota.SetRotate(turnSpeed, turnSpeed, tarDir);
            move.Move(true, tarDir);
        }
        else if(_lastState== ActionState.Escape)
        {
//            tarDistance = (tarPos - _tr.localPosition).magnitude;
//            if(tarDistance>5f)
//            {
//                turnSpeed *= 2f;
//                tarTime = (tarTime - speed.usedTime) * 0.5f;
//                speed.StartVarSpeed(tarDistance, tarTime, SpeedOver);
//                rota.SetRotate(turnSpeed, turnSpeed, tarDir);
//                move.Move(true, tarDir);
//            }
//            else
//            {
//                _lastState = ActionState.Float;
//                Escape();
//            }
        }
    }
}
