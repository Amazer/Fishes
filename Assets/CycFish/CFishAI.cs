using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CFishAI : MonoBehaviour
{
    public float turnSpeed = 4f;

    public enum ActionState { Swimming, Escape, Float, Feed, FeedOver }
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
    [SerializeField]
    private FeedFlag feed;

    private FoodAI _foodTarget;


    void Start()
    {
        EventMgr<GameEvent>.instance.AddListener(GameEvent.Feed, OnFeed);
        EventMgr<GameEvent>.instance.AddListener(GameEvent.FoodDestroy, OnFoodDestroy);
        _tr = transform;
        _tr.parent = Tank.instance.gameObject.transform;
        _animator = _tr.GetComponentInChildren<Animator>();
        move = new MoveFlag(_tr);
        speed = new SpeedFlag(_tr);
        rota = new RotateFlag(_tr);
        feed = new FeedFlag(_tr);
        RandomBorn();
        SpeedOver();
    }
    private void OnDestroy()
    {
        EventMgr<GameEvent>.instance.RemListener(GameEvent.Feed, OnFeed);
    }

    // Update is called once per frame
    void Update()
    {
        float deltaTime = Time.deltaTime;
        feed.Update(deltaTime);
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
        turnSpeed = Random.Range(2f, 5f);
        tarTime = Random.Range(1, 5f);
        float dis = Random.Range(1, 2);
        tarDir.y = 0;
        Vector3 tarPos = _tr.position + tarDir * dis;
        if (Tank.instance.InTank(tarPos))
        {
            tarDistance = dis;
        }
        else
        {
            tarPos = Tank.instance.InTankPos(tarPos);
            tarDistance = Vector3.Distance(_tr.position, tarPos);
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
        turnSpeed = Random.Range(4f, 10f);

        float tarX = Tank.instance.RandomX();
        float tarZ = Tank.instance.RandomZ();
        float angle = Random.Range(-35, 35);
        float tanAngle = Mathf.Tan(angle * Mathf.Deg2Rad);
        float tarY = tarX * tanAngle;
        if (tarY < Tank.instance.minHeightPos)
        {
            tarY = Tank.instance.minHeightPos;
        }
        else if (tarY > Tank.instance.maxHeightPos)
        {
            tarY = Tank.instance.maxHeightPos;
        }

        tarTime = Random.Range(5f, 10f);
        tarPos = new Vector3(tarX, tarY, tarZ);
        tarDir = tarPos - _tr.position;
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
        _tr.transform.position = new Vector3(Tank.instance.RandomX(), Tank.instance.RandomY(), Tank.instance.RandomZ());
        tarDir = _tr.transform.forward;
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

        if (Random.Range(0, 5) > 2)
        {
            Escape_SpeedUp();
        }
        else
        {
            Escape_Turn();
        }
    }
    private void Escape_Turn()
    {
        turnSpeed = Random.Range(4f, 10f);

        float tarX = Tank.instance.RandomX();
        float tarZ = Tank.instance.RandomZ();
        float angle = Random.Range(-35, 35);
        float tanAngle = Mathf.Tan(angle * Mathf.Deg2Rad);
        float tarY = tarX * tanAngle;
        if (tarY < Tank.instance.minHeightPos)
        {
            tarY = Tank.instance.minHeightPos;
        }
        else if (tarY > Tank.instance.maxHeightPos)
        {
            tarY = Tank.instance.maxHeightPos;
        }

        tarTime = Random.Range(3f, 5f);
        tarPos = new Vector3(tarX, tarY, tarZ);
        tarDir = tarPos - _tr.position;
        tarDistance = tarDir.magnitude;
        tarDir.Normalize();
        speed.SetVarMinSpeed(Random.Range(0f, 2f));
        speed.SetVarSpeedConfig(Random.Range(0.02f, 0.1f), Random.Range(0.2f, 0.5f));
        speed.StartVarSpeed(tarDistance, tarTime, SpeedOver);
        rota.SetRotate(turnSpeed, turnSpeed, tarDir);
        move.Move(true, tarDir);

    }
    private void Escape_SpeedUp()
    {
        float dis = Random.Range(10f, 20f);
        tarPos = _tr.position + tarDir * dis;
        if (!Tank.instance.InTank(tarPos))
        {
            float deltaX = Mathf.Abs(_tr.position.x - tarPos.x);
            if (tarPos.x < Tank.instance.minWidthPos)
            {
                tarPos.x = _tr.position.x + deltaX;
            }
            else if (tarPos.x > Tank.instance.maxWidthPos)
            {
                tarPos.x = _tr.position.x - deltaX;
            }

            float deltaY = Mathf.Abs(_tr.position.y - tarPos.y);
            if (tarPos.y < Tank.instance.minHeightPos)
            {
                tarPos.y = _tr.position.y + deltaY;
            }
            else if (tarPos.y > Tank.instance.maxHeightPos)
            {
                tarPos.y = _tr.position.y - deltaY;
            }

            float deltaZ = Mathf.Abs(_tr.position.z - tarPos.z);
            if (tarPos.z < Tank.instance.minDepthPos)
            {
                tarPos.z = _tr.position.z + deltaZ;
            }
            else if (tarPos.z > Tank.instance.maxDepthPos)
            {
                tarPos.z = _tr.position.z - deltaZ;
            }
        }
        tarDir = tarPos - _tr.position;
        tarDir.Normalize();
        tarDistance = (tarPos - _tr.position).magnitude;

        turnSpeed = Random.Range(4f, 10f);
        tarTime = Random.Range(1f, 3f);
        tarDir.Normalize();
        speed.SetVarMinSpeed(Random.Range(0f, 2f));
        speed.SetVarSpeedConfig(Random.Range(0.02f, 0.1f), Random.Range(0.2f, 0.5f));
        speed.StartVarSpeed(tarDistance, tarTime, SpeedOver);
        rota.SetRotate(turnSpeed, turnSpeed, tarDir);
        move.Move(true, tarDir,SpeedOver);


    }

    private void Feed()
    {
        _lastState = _curState;
        _curState = ActionState.Feed;
        turnSpeed = Random.Range(4f, 10f);
        tarPos = _foodTarget.transform.position - Tank.instance.transform.position;
        tarDir = tarPos - _tr.position;
        tarDistance = tarDir.magnitude;
        tarDir.Normalize();
        speed.StartSpeedUp(Random.Range(3f, 10f), Random.Range(6f, 10f));
        rota.SetRotateToTarget(turnSpeed, turnSpeed, _foodTarget.transform);
        move.MoveToDynamicTarget(_foodTarget.transform, null);
    }
    private void FeedOver(bool getFood)
    {
        _lastState = _curState;
        _curState = ActionState.FeedOver;
        float dis = Random.Range(10f, 20f);
        tarDir = _tr.forward;
        turnSpeed = Random.Range(4f, 10f);
        tarTime = Random.Range(1f, 3f);
        tarDir.Normalize();
        speed.StartSpeedDown(Random.Range(1f, 4f), Random.Range(1f, 2f),SpeedOver);
        rota.SetRotate(turnSpeed, turnSpeed, tarDir);
        move.Move(true, tarDir,SpeedOver);
    }

    private void OnFeed(object[] param)
    {

        if (!feed.feeding)
        {
            // 寻找距离最近的食物，并且冲过去
            FoodMgr.instance.Nearest(_tr.position, out _foodTarget);
            if (_foodTarget != null)
            {
                feed.StartFeed(_foodTarget, FeedOver);
                Feed();
            }
        }
    }

    private void OnFoodDestroy(object[] param)
    {
        OnFeed(null);// 再寻找其他的目标
    }
}
