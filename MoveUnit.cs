//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using DG.Tweening;
//using System;

//public class MoveUnit : MonoBehaviour
//{
//    private float movementTime = .5f;

//    private List<Node> movementPath;
//    private int pathIndex;
//    [SerializeField] private Unit ownerUnit;

//    private Action doneMoving;



//    //ACTIONS
//    public void MoveOneSpace(Node destinationNode, Action action)
//    {
//        doneMoving = action;
//        transform.DOMoveX(destinationNode.transform.position.x, movementTime).SetEase(Ease.InOutSine);
//        transform.DOMoveY(1f, movementTime / 2f).SetEase(Ease.InOutSine).OnComplete(ReturnJump);
//        transform.DOMoveZ(destinationNode.transform.position.z, movementTime).SetEase(Ease.InOutSine).OnComplete(MoveDone);
//    }

//    public void LoadInNewPath(List<Node> thePath, Action whenComplete)
//    {
//        movementPath = new List<Node>(thePath);
//        pathIndex = 1;
//        doneMoving = whenComplete;
//        StartCoroutine(MoveUnitAction());
//    }
//    IEnumerator MoveUnitAction()
//    {
//        if (ownerUnit.canFly)
//        {
//            if (movementPath[pathIndex].currentUnit != null)
//            {
//                pathIndex += 1;
//            }
//        }

//        yield return new WaitForSeconds(0);
//        Vector3 nextHexLocation = movementPath[pathIndex].transform.position;

//        yield return new WaitForSeconds(0);    

//        transform.DOMoveX(nextHexLocation.x, movementTime).SetEase(Ease.InOutSine).OnComplete(CheckIfPathComplete);
//        transform.DOMoveY(.7f, movementTime / 2f).SetEase(Ease.InOutSine).OnComplete(ReturnJump);
//        transform.DOMoveZ(nextHexLocation.z, movementTime).SetEase(Ease.InOutSine);
//    }

//    private void MoveDone()
//    {
//        if (doneMoving != null)
//        {
//            doneMoving();
//        }
//    }

//    private void ReturnJump()
//    {
//        transform.DOMoveY(0f, movementTime / 2f).SetEase(Ease.InOutSine);
//    }

//    private void CheckIfPathComplete()
//    {
        

//        //pathIndex += 1;
//        //if (pathIndex >= movementPath.Count)
//        //{
//        //    //When move is finished
//        //    ownerUnit.gm.AfterMove();
//        //    doneMoving();
//        //    return;
//        //}
//        //StartCoroutine(MoveUnitAction());
//    }

//}
