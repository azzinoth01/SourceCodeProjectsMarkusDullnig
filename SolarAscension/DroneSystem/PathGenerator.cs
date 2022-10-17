using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PathGenerator {

    public Queue<(WaypointInfo, WaypointInfo, uint)> PathQueueing;

    private Coroutine _currentCoroutine;
    private uint _iD;
    public int yieldCounter;

    public event Action<(uint, PathInfo)> calcFinished;
    WaypointValue[] _array;
    Heap<WaypointValue> _openList;
    HashSet<WaypointValue> _closedList;


    public PathGenerator() {
        _iD = 0;

        PathQueueing = new Queue<(WaypointInfo, WaypointInfo, uint)>();
        _array = new WaypointValue[1000000];
        _openList = new Heap<WaypointValue>(_array);
        _closedList = new HashSet<WaypointValue>();

        //_currentCoroutine = DroneSystem.Instanz.StartCoroutine(AStarPathCalcNumerator(start, end));
    }
    public uint AddNewPathToQueue(WaypointInfo start, WaypointInfo end) {

        _iD = _iD + 1;
        if (_iD == 0) {
            _iD = _iD + 1;
        }

        PathQueueing.Enqueue((start, end, _iD));
        if (_currentCoroutine == null) {
            (WaypointInfo, WaypointInfo, uint) pair = PathQueueing.Dequeue();
            _currentCoroutine = DroneSystem.Instanz.StartCoroutine(AStarPathCalcNumerator(pair.Item1, pair.Item2, pair.Item3));
        }


        return _iD;
    }

    public IEnumerator AStarPathCalcNumerator(WaypointInfo start, WaypointInfo end, uint returnId) {
        yield return null;
        yieldCounter = 0;


        _openList.Clear();
        _closedList.Clear();




        WaypointValue endingValue = new WaypointValue(0, 0);

        endingValue.node = end;

        int xDif = (int)MathF.Abs(start.pos.x - end.pos.x);
        int yDif = (int)MathF.Abs(start.pos.y - end.pos.y);
        int zDif = (int)MathF.Abs(start.pos.z - end.pos.z);

        endingValue.HValue = (xDif + yDif + zDif) * DroneSystemInfo.Instanz.Basevalue;



        _openList.Add(endingValue);


        WaypointValue curentValue = null;
        int maxiterationCount = 10000;

        while (_openList.Count != 0) {


            curentValue = null;

            curentValue = _openList.RemoveFirst();

            _closedList.Add(curentValue);

            if (curentValue.node == start) {
                break;
            }



            WaypointInfo[] succesorList = new WaypointInfo[6];

            succesorList[0] = CheckWaypointConnections(curentValue.node, Vector3Int.right);
            succesorList[1] = CheckWaypointConnections(curentValue.node, Vector3Int.left);
            succesorList[2] = CheckWaypointConnections(curentValue.node, Vector3Int.up);
            succesorList[3] = CheckWaypointConnections(curentValue.node, Vector3Int.down);
            succesorList[4] = CheckWaypointConnections(curentValue.node, Vector3Int.forward);
            succesorList[5] = CheckWaypointConnections(curentValue.node, Vector3Int.back);


            foreach (WaypointInfo connection in succesorList) {


                if (connection.state == WaypointState.blocked) {

                    continue;
                }
                int gValueModifier = 0;
                if ((connection.state == WaypointState.moveOut && connection.pos.Equals(start.pos) == false) || (connection.state == WaypointState.onlyMoveIn && connection.pos.Equals(end.pos) == false)) {
                    gValueModifier = 500;
                }

                WaypointConnection waypointConnection = null;
                int moveDirectionModifier = 1;

                foreach (WaypointConnection con in curentValue.node.InGoingConnections) {
                    if (con.connectTo == connection) {
                        waypointConnection = con;
                        moveDirectionModifier = 0;
                        break;
                    }
                }
                if (waypointConnection == null) {
                    foreach (WaypointConnection con in curentValue.node.OutGoingConnections) {
                        if (con.connectTo == connection) {
                            waypointConnection = con;
                            moveDirectionModifier = 500 * DroneSystemInfo.Instanz.Basevalue;
                            break;
                        }
                    }
                }
                WaypointValue succesor = new WaypointValue(0, 0);
                succesor.node = connection;


                if (_closedList.Contains(succesor)) {
                    continue;
                }



                if (!_openList.Contains(succesor)) {


                    if (waypointConnection != null) {
                        succesor.GValue = (curentValue.GValue + (waypointConnection.value * moveDirectionModifier) + gValueModifier);

                    }
                    else {

                        succesor.GValue = (curentValue.GValue + ((connection.CurrentValue + curentValue.movedOnPathsCount) * moveDirectionModifier) + gValueModifier);
                        if (curentValue.node.InGoingConnections.Count != 0) {
                            curentValue.movedOnPathsCount = curentValue.movedOnPathsCount + 1;

                        }
                    }
                    succesor.parent = curentValue;
                    xDif = (int)MathF.Abs(start.pos.x - connection.pos.x);
                    yDif = (int)MathF.Abs(start.pos.y - connection.pos.y);
                    zDif = (int)MathF.Abs(start.pos.z - connection.pos.z);

                    succesor.HValue = (xDif + yDif + zDif) * DroneSystemInfo.Instanz.Basevalue;
                    _openList.Add(succesor);

                }


            }



            maxiterationCount = maxiterationCount - 1;
            if (maxiterationCount < 0) {

                maxiterationCount = 500;
                yieldCounter = yieldCounter + 1;

                if (yieldCounter > 2000) {
                    if (PathQueueing.Count != 0) {
                        (WaypointInfo, WaypointInfo, uint) pair = PathQueueing.Dequeue();
                        _currentCoroutine = DroneSystem.Instanz.StartCoroutine(AStarPathCalcNumerator(pair.Item1, pair.Item2, pair.Item3));

                    }
                    else {
                        _currentCoroutine = null;
                    }
                    calcFinished?.Invoke((returnId, null));
                    yieldCounter = 0;
                    yield break;
                }


                yield return null;



            }
        }

        if (curentValue.node != start) {

            if (PathQueueing.Count != 0) {
                (WaypointInfo, WaypointInfo, uint) pair = PathQueueing.Dequeue();
                _currentCoroutine = DroneSystem.Instanz.StartCoroutine(AStarPathCalcNumerator(pair.Item1, pair.Item2, pair.Item3));

            }
            else {
                _currentCoroutine = null;
            }
            calcFinished?.Invoke((returnId, null));
            yield break;
        }






        List<WaypointInfo> pathing = new List<WaypointInfo>();


        while (curentValue.parent != null) {


            pathing.Add(curentValue.node);
            curentValue = curentValue.parent;
        }
        pathing.Add(curentValue.node);


        PathInfo pathInfo = new PathInfo(pathing);


        calcFinished?.Invoke((returnId, pathInfo));


        if (PathQueueing.Count != 0) {
            (WaypointInfo, WaypointInfo, uint) pair = PathQueueing.Dequeue();
            _currentCoroutine = DroneSystem.Instanz.StartCoroutine(AStarPathCalcNumerator(pair.Item1, pair.Item2, pair.Item3));

        }
        else {
            _currentCoroutine = null;
        }


    }


    public WaypointInfo CheckWaypointConnections(WaypointInfo point, Vector3Int direction) {
        WaypointInfo ways;


        if (DroneSystemInfo.Instanz.Waypointgrid.TryGetValue(point.pos + direction, out ways) == false) {
            ways = new WaypointInfo(point.pos + direction, DroneSystemInfo.Instanz.Basevalue);

            if (DroneSystemInfo.Instanz.Player.BuildingGrid.LockGrid.IsLocked(new GridCoordinate(ways.pos.x, ways.pos.y, ways.pos.z)) == true) {
                ways.state = WaypointState.blocked;



            }



        }

        return ways;


    }
}
