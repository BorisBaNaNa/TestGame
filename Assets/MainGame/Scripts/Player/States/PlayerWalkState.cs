using System.Collections;
using UnityEngine;

public class PlayerWalkState : IState
{
    private bool IsMovedToTargetPos
        => Vector3.Distance(_player.transform.position, _targetMovePos) < 0.5f;

    private readonly PlayerStateMachine _stateMachine;
    private readonly PlayerController _player;
    private Transform _targetMove;
    private Vector3 _targetMovePos;

    public PlayerWalkState(PlayerStateMachine playerStateMachine)
    {
        _stateMachine = playerStateMachine;
        _player = _stateMachine.Player;
    }

    public void Enter()
    {
        FindAndSetNewTargetMove();

        _player.StartCoroutine(WaitAndStartWalk(2f));
    }

    public void Exit()
    {
        _player.SpawnPoint = _targetMovePos - Vector3.right * 10;
        Object.Destroy(_targetMove.gameObject);
        _player.CurrentAction = null;
        //_player.TargetMove.gameObject.SetActive(false);
    }

    IEnumerator WaitAndStartWalk(float time)
    {
        yield return new WaitForSeconds(time);

        _player.CurrentAction = MoveToTargetPos;
    }

    private void MoveToTargetPos()
    {
        _targetMovePos = _targetMove.position;
        _targetMovePos.y = _player.transform.position.y;

        Vector3 moveDir = _targetMovePos - _player.transform.position;
        Vector3 motion = moveDir.normalized * _player.MoveSpeed * Time.deltaTime;

        _player.CharacterController.Move(motion);
        RotateOnTarget(moveDir);

        if (IsMovedToTargetPos)
            _stateMachine.StateSwitch<PlayerAttackState>();
    }

    private void RotateOnTarget(Vector3 dir)
    {
        Quaternion _targetRotation = Quaternion.LookRotation(dir);
        _player.transform.rotation = Quaternion.RotateTowards(_player.transform.rotation, _targetRotation, 0.5f);
    }


    public void FindAndSetNewTargetMove()
    {
        GameObject[] attackPoints = GameObject.FindGameObjectsWithTag("AttackPoint");

        float minDist = float.MaxValue;
        Transform closestPoint = null;
        foreach (GameObject attackPoint in attackPoints)
        {
            float dist = Vector3.Distance(attackPoint.transform.position, _player.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                closestPoint = attackPoint.transform;
            }
        }
        _targetMove = closestPoint;
    }
}
