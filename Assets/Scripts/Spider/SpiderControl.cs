using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IA;
using UnityEditor.Experimental.GraphView;

public class SpiderControl : MonoBehaviour
{
    public enum States { IDLE, FOLLOW, ATTACK, RETURN }
    private EventFSM<States> _myFsm;

    private Spider spider;
    private NodePoint _goalNode;
    private List<NodePoint> pathList;

    private void Awake()
    {
        spider = GetComponent<Spider>();  //asignar de forma correcta, no por get component


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
        };

        idle.OnUpdate += () =>
        {

            if (Input.GetKeyDown(KeyCode.F))  //cambiar esto a if ve al player
                SendInputToFSM(States.FOLLOW);

        };

        //RETURN
        returning.OnEnter += x =>
        {
            _goalNode = spider.wayPoints[0].GetComponent<NodePoint>();

            pathList = spider.ConstructPathAStar(spider.transform.position, _goalNode);
            
        };

        returning.OnUpdate += () =>
        {
            spider.Move(pathList);

            if (Input.GetKeyDown(KeyCode.F))  //cambiar esto a if ve al player
                SendInputToFSM(States.FOLLOW);

            if (spider.current >= pathList.Count)
            {
                spider.current = 0;
                SendInputToFSM(States.IDLE);
            }
        };

        returning.OnExit += x =>
        {

        };

        //FOLLOW
        following.OnEnter += x =>
        {
            _goalNode = spider.wayPoints[0].GetComponent<NodePoint>();  //cambiar por el node en el que se encuentra el player en el momento de ser detectado.
                                                                        //Al ser detectado agarrar el nodo mas cercano y pasarlo por aca

            pathList = spider.ConstructPathAStar(spider.transform.position, _goalNode);
        };
        following.OnUpdate += () =>
        {
            spider.Move(pathList);

            if (Input.GetKeyDown(KeyCode.R) && spider.current >= pathList.Count)  //cambiar el key r a if DEJA DE VER al player o dejart solo al completar el count
                SendInputToFSM(States.RETURN);

            if (Input.GetKeyDown(KeyCode.A))  //cambiar el key down A a if esta en el rango de ataque al player
                SendInputToFSM(States.ATTACK);
        
        };

        following.OnFixedUpdate += () =>
        {
        };

        following.OnExit += x =>
        {
        };

        //ATTACK
        attacking.OnEnter += x =>
        {
            spider.Attack(); 
        };
        attacking.OnUpdate += () =>
        {
            if (Input.GetKeyDown(KeyCode.R))  //cambiar esto a if DEJA DE VER al player
                SendInputToFSM(States.RETURN);

            //de ser necesario agregar la opcion de desde el ataque poder volver a encontrar al player por path. balancear con si deja de ver al player
        };
        attacking.OnExit += x =>
        {

        };

        _myFsm = new EventFSM<States>(idle);
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
