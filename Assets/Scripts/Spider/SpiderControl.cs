using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IA;

public class SpiderControl : MonoBehaviour
{
    public enum States { IDLE, FOLLOW, ATTACK, RETURN }
    private EventFSM<States> _myFsm;

    private Spider _spider;
    public NodePoint _goalNode;
    public List<NodePoint> pathList;


    private void Awake()
    {
        _spider = GetComponent<Spider>();  //asignar de forma correcta, no por get component


        var idle = new State<States>("idle");
        var following = new State<States>("following");
        var attacking = new State<States>("attacking");
        var returning = new State<States>("returning");

        //creo las transiciones
        StateConfigurer.Create(idle)
            .SetTransition(States.FOLLOW, following)
            .Done(); //aplico y asigno

        StateConfigurer.Create(following)
            .SetTransition(States.ATTACK, attacking)
            .SetTransition(States.RETURN, returning)
            .Done();

        StateConfigurer.Create(attacking)
            .SetTransition(States.FOLLOW, following)
            .SetTransition(States.RETURN, returning)
            .Done();

        StateConfigurer.Create(returning)
            .SetTransition(States.IDLE, idle)
            .SetTransition(States.FOLLOW, following)
            .Done();

        //IDLE
        idle.OnEnter += x =>
        {
            //Debug.Log("Its Idle");
        };

        idle.OnUpdate += () =>
        {
            //Debug.Log(Vector2.Distance(transform.position, _spider._target.transform.position) + " " + _spider.followArea);
            if(!_spider._target) return;
            if (Vector2.Distance(transform.position, _spider._target.transform.position) <= _spider.followArea &&
            _spider.InSight(transform.position, _spider._target.transform.position))
            {
                SendInputToFSM(States.FOLLOW);
            }


        };

        //RETURN
        returning.OnEnter += x =>
        {
            //Debug.Log("Its Returning");

            _goalNode = _spider.homeNode;

            pathList = _spider.ConstructPathAStar(_spider.transform.position, _goalNode);
        };

        returning.OnUpdate += () =>
        {
            _spider.Move(pathList);

            if (Input.GetKeyDown(KeyCode.F))  //cambiar esto a if ve al player
                SendInputToFSM(States.FOLLOW);

            if (Vector2.Distance(transform.position, _spider._target.transform.position) <= _spider.followArea &&
            _spider.InSight(transform.position, _spider._target.transform.position))
            {
                SendInputToFSM(States.FOLLOW);
            }

            if (_spider.current >= pathList.Count)
            {
                _spider.current = 0;
                SendInputToFSM(States.IDLE);
            }
        };

        returning.OnExit += x =>
        {
            _spider.current = 0;
            pathList.Clear();
        };

        //FOLLOW
        following.OnEnter += x =>
        {
            //Debug.Log("Its Following");
            _spider.Alert(true);
            StartCoroutine(CoroutineWaitForAttack(0.5f));
        };

        following.OnUpdate += () =>
        {
            if (pathList.Count <= 0) return;
            if (_spider.current >= pathList.Count - 1 && Vector2.Distance(transform.position, pathList[_spider.current].transform.position) < 0.15f)
            {
                SendInputToFSM(States.RETURN);
            }

            if (Vector2.Distance(transform.position, _spider._target.transform.position) <= _spider.attackArea)
            {
                SendInputToFSM(States.ATTACK);
            }

            _spider.Move(pathList);
        };

        following.OnFixedUpdate += () =>
        {
        };

        following.OnExit += x =>
        {
            _spider.current = 0;
            pathList.Clear();
        };

        //ATTACK
        attacking.OnEnter += x =>
        {
            //Debug.Log("Its Attacking");

            _spider.Attack();
        };
        attacking.OnUpdate += () =>
        {
            if (_spider.attacked)
            { SendInputToFSM(States.RETURN); }
        };
        attacking.OnExit += x =>
        {
            _spider.attacked = false;
        };

        _myFsm = new EventFSM<States>(idle);
    }

    public IEnumerator CoroutineWaitForAttack(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        _goalNode = _spider.CheckNearestStart(_spider._target.transform.position);
        pathList = _spider.ConstructPathAStar(_spider.transform.position, _goalNode);
        _spider.Alert(false);
        if (pathList.Count <= 0)
        {
            _goalNode = _spider.homeNode;
            pathList = _spider.ConstructPathAStar(_spider.transform.position, _goalNode);
        }
    }
    private void Start()
    {
        SendInputToFSM(States.IDLE);
    }

    private void SendInputToFSM(States inp)
    {
        _myFsm.SendInput(inp);
    }
    private void Update()
    {
        _myFsm.Update();
    }

    private void FixedUpdate()
    {
        _myFsm.FixedUpdate();
    }
}