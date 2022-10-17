using System.Collections;
using UnityEngine;


public class Drone : MonoBehaviour {

    public WaypointInfo _moveToWaypoint;
    public WaypointInfo _previousMoveToWaypoint;
    private Vector3 _position;
    private bool _waypointReached;
    public bool _canMoveTowardsNextWaypoint;
    private bool _useReturnPath;
    private bool _useInsidePath;
    public PathInfo path;
    public PathInfo returnPath;
    public PathInfo _currentPath;
    public int _pathIndex;
    public DroneModul startModul;
    public DroneModul endModul;
    public DroneModul origin;

    public bool stopMovement;
    public float stopMovementTime;

    private GameObject _droneVisualisation;
    public float _currentSpeed;


    private bool _increaseSpeed;

    private bool _droneFlagedForDestruction;
    private bool _droneFlagedForSleep;
    private uint _ticksWithOutMoving;
    public int PathIndex {


        set {
            _pathIndex = value;
        }
    }

    public bool DroneFlagedForSleep {

        set {
            _droneFlagedForSleep = value;
        }
    }


    // Start is called before the first frame update
    void Start() {

        _droneVisualisation = transform.GetChild(0).gameObject;
        _ticksWithOutMoving = 0;
        _droneFlagedForDestruction = false;

        _waypointReached = true;
        _canMoveTowardsNextWaypoint = true;
        _useReturnPath = true;
        _useInsidePath = true;
        _currentPath = origin.InsidePath;


        stopMovement = false;
        stopMovementTime = 0;

        if (_currentPath != null) {
            _currentPath.drones.Add(this);
        }
        if (path != null) {
            path.allDronesUsingThisPath.Add(this);
        }
        if (returnPath != null) {
            returnPath.allDronesUsingThisPath.Add(this);
        }
        _currentSpeed = DroneSystemInfo.Instanz.DroneSpeed;


        Vector3Int speedPos = path.path[(int)(path.path.Count * 0.1f)].pos;

        if (DroneSystemInfo.Instanz.DroneSpeedRadius < speedPos.magnitude) {
            DroneSystemInfo.Instanz.DroneSpeedRadius = speedPos.magnitude;
        }




    }



    // Update is called once per frame
    void Update() {



        if (stopMovement == true || _currentPath == null) {
            return;
        }

        if (_ticksWithOutMoving >= 60) {
            _droneFlagedForDestruction = true;
            // destroy drone imitatly because it is in a deadlock
            if (_ticksWithOutMoving >= 1200) {
                DestroyThisDrone();
                origin.CurrentMaxDroneReduceAmount = origin.CurrentMaxDroneReduceAmount - 1;
                return;
            }
        }


        if (_waypointReached == false && _canMoveTowardsNextWaypoint == true && _moveToWaypoint != null) {
            _ticksWithOutMoving = 0;
            if (_increaseSpeed == true) {

                if (_currentSpeed < 1000) {
                    _currentSpeed = _currentSpeed + 0.5f;
                }

            }


            Vector3 direction;
            float deltaTime = Time.deltaTime;

            direction = _moveToWaypoint.GameObject.transform.position - transform.position;

            if ((direction.magnitude <= _currentSpeed * deltaTime)) {


                _waypointReached = true;
                transform.position = transform.position + direction;
            }
            else {
                transform.position = transform.position + (_currentSpeed * direction.normalized * deltaTime);
            }


            Quaternion toRotate = Quaternion.LookRotation(direction.normalized, Vector3.up);
            toRotate = toRotate * Quaternion.Euler(new Vector3(0, -90, 0));
            _droneVisualisation.transform.rotation = toRotate;


        }
        else {
            _ticksWithOutMoving = _ticksWithOutMoving + 1;
        }

        if (stopMovementTime != 0 && _waypointReached == true) {

            StartCoroutine(ResumeMoveDrone(stopMovementTime));
            stopMovementTime = 0;
            _ticksWithOutMoving = 0;
        }
        else if (_waypointReached == true) {



            if (_increaseSpeed == false && _useInsidePath == false) {
                if (_moveToWaypoint.pos.magnitude >= DroneSystemInfo.Instanz.DroneSpeedRadius) {
                    _increaseSpeed = true;
                }
            }
            else if (_increaseSpeed == true && _useInsidePath == false) {
                if (_moveToWaypoint.pos.magnitude <= DroneSystemInfo.Instanz.DroneSpeedRadius) {
                    _increaseSpeed = false;
                    _currentSpeed = DroneSystemInfo.Instanz.DroneSpeed;
                }
            }


            _ticksWithOutMoving = 0;
            _moveToWaypoint.WaypointReachedByDrone = true;
            _pathIndex = _pathIndex + 1;
            _previousMoveToWaypoint = _moveToWaypoint;
            _waypointReached = false;
            _canMoveTowardsNextWaypoint = false;



            if (_pathIndex < _currentPath.path.Count) {


                WaypointInfo currentWaypoint = _currentPath.path[_pathIndex];


                QueueDrones(_currentPath, currentWaypoint, _pathIndex);


            }
            else {
                _pathIndex = 0;
                _currentPath.drones.Remove(this);
                if (_useInsidePath == true) {
                    _useInsidePath = false;

                    if (_useReturnPath == true) {
                        _useReturnPath = false;
                        _currentPath = path;

                    }
                    else {
                        _currentPath = returnPath;
                        _useReturnPath = true;

                    }
                }
                else {
                    _useInsidePath = true;
                    if (_useReturnPath == true) {
                        _currentPath = startModul.InsidePath;
                        if (_droneFlagedForDestruction == true) {
                            origin.CurrentMaxDroneReduceAmount = origin.CurrentMaxDroneReduceAmount - 1;
                            DestroyThisDrone();
                        }
                        else if (_droneFlagedForSleep == true) {
                            DestroyThisDrone();
                        }

                    }
                    else {
                        _currentPath = endModul.InsidePath;


                    }
                }

                WaypointInfo currentWaypoint = _currentPath.path[_pathIndex];
                QueueDrones(_currentPath, currentWaypoint, _pathIndex);
                _currentPath.drones.Add(this);

            }

        }
    }

    private void QueueDrones(PathInfo usePath, WaypointInfo startPoint, int currentIndex) {

        if (startPoint.DronesInQueue.Count != 0 && startPoint.DronesInQueue.Peek().Item1 == this) {
            return;
        }

        bool bothWayConnection = true;
        int nextIndex = 1;
        WaypointInfo nextWaypoint = null;
        if (usePath.path.Count > currentIndex + nextIndex) {
            nextWaypoint = usePath.path[currentIndex + nextIndex];
        }

        WaypointInfo previousWaypoint = null;



        while (bothWayConnection == true) {

            bothWayConnection = false;
            foreach (WaypointConnection con in startPoint.OutGoingConnections) {
                if (con.connectTo == nextWaypoint) {
                    if (con.state == PathState.bothWays) {
                        bothWayConnection = true;
                        break;
                    }
                    else {
                        break;
                    }
                }
            }


            if (bothWayConnection == false) {
                startPoint.DronesInQueue.Enqueue((this, previousWaypoint, null));
            }
            else {
                startPoint.DronesInQueue.Enqueue((this, previousWaypoint, nextWaypoint));
                previousWaypoint = startPoint;
                startPoint = nextWaypoint;
                nextIndex = nextIndex + 1;
                nextWaypoint = null;
                if (usePath.path.Count > currentIndex + nextIndex) {
                    nextWaypoint = usePath.path[currentIndex + nextIndex];
                }

            }
        }
    }



    public IEnumerator ResumeMoveDrone(float waitTime) {
        stopMovement = true;
        yield return new WaitForSeconds(waitTime);

        stopMovement = false;


    }
    public void DestroyThisDrone() {
        origin.DronesCreated.Remove(this);
        Destroy(gameObject);
    }
}
