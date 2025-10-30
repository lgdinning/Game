using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PhaseManager : MonoBehaviour
{
    public Camera cam;
    public static PhaseManager phaseManager;
    public GameObject mc;
    public GameObject actionStatus;
    public ActionStatus state;
    public GameObject enemyDisplay;
    public DisplayManager display;
    public List<GameObject> playerPieces;
    public List<GameObject> enemyPieces;
    public List<GameObject> enemiesToDelete;
    public bool playerPhase;
    public bool allyTakedown;
    public bool waiting;

    void Awake()
    {
        phaseManager = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        waiting = false;
        allyTakedown = false;
        playerPhase = true;
        state = actionStatus.GetComponent<ActionStatus>();
        display = enemyDisplay.GetComponent<DisplayManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerPhase && (state.state == 1) && Input.GetKeyDown(KeyCode.Backspace))
        {
            playerPhase = false;
            SetPlayerTurn(false);
            CheckPlayerDone();
        }
    }

    public void UpdateTargetList(GameObject deletedTarget)
    {
        foreach (GameObject e in enemyPieces)
        {
            if (!e.GetComponent<MoveEnemy>().hasMoved)
            {

            }
            //while e.GetComponent<EnemyAttack>()
        }
    }

    public void ReTarget()
    {
        foreach (GameObject e in enemyPieces)
        {
            e.GetComponent<MoveEnemy>().ChooseTargets();
        }
    }
    public void SortTurnOrder()
    {
        ReTarget();
        List<GameObject> notNulls = enemyPieces.Where(x => x.GetComponent<MoveEnemy>().attackHeap.Peek().Count > 0).ToList();
        List<GameObject> nulls = enemyPieces.Where(x => x.GetComponent<MoveEnemy>().attackHeap.Peek().Count < 1).ToList();
        notNulls = notNulls.OrderByDescending(x => x.GetComponent<MoveEnemy>().attackHeap.Peek()[0]).ToList();
        notNulls.AddRange(nulls);
        enemyPieces = notNulls;
    }

    public void ActivateGroup(int gID)
    {

        foreach (GameObject e in enemyPieces)
        {
            EnemyAttack enemyInfo = e.GetComponent<EnemyAttack>();
            if (enemyInfo.groupId == gID)
            {
                enemyInfo.behaviour = "Attacking";
            }
        }
    }

    public void ClearUnclear()
    {
        foreach (GameObject e in enemyPieces)
        {
            if (e.GetComponent<MoveEnemy>().displaying)
            {
                display.UpdateDisplay(e.GetComponent<MoveEnemy>().attackableTiles, false, 1);
                e.GetComponent<MoveEnemy>().UpdateAttackables();
                display.UpdateDisplay(e.GetComponent<MoveEnemy>().attackableTiles, true, 1);
            }
        }
    }

    public void Clear(bool total)
    {
        foreach (GameObject e in enemyPieces)
        {
            if (e.GetComponent<MoveEnemy>().displaying)
            {
                display.UpdateDisplay(e.GetComponent<MoveEnemy>().attackableTiles, false, 1);
            }
            if (total)
            {
                e.GetComponent<MoveEnemy>().displaying = false;
            }
        }
    }

    public void UnClear()
    {
        foreach (GameObject e in enemyPieces)
        {
            if (e.GetComponent<MoveEnemy>().displaying)
            {
                e.GetComponent<MoveEnemy>().UpdateAttackables();
                display.UpdateDisplay(e.GetComponent<MoveEnemy>().attackableTiles, true, 1);
            }
        }
    }

    public void UpdateTarget(int x, int y)
    {
        foreach (GameObject e in enemyPieces)
        {
            e.GetComponent<MoveEnemy>().targetX = x;
            e.GetComponent<MoveEnemy>().targetY = y;
        }
    }

    public void SetPlayerTurn(bool on)
    {
        if (on)
        {
            playerPhase = true;
            foreach (GameObject p in playerPieces)
            {
                p.GetComponent<MoveCharacter>().hasMoved = false;
                p.GetComponent<MeshRenderer>().material = p.GetComponent<MoveCharacter>().unmoved;
            }
        }
        else
        {
            playerPhase = false;
            foreach (GameObject p in playerPieces)
            {
                if (p.GetComponent<MoveCharacter>().isMC)
                {
                    UpdateTarget(p.transform.parent.GetComponent<TileBehaviour>().x, p.transform.parent.GetComponent<TileBehaviour>().y);
                }
                p.GetComponent<MoveCharacter>().hasMoved = true;
                p.GetComponent<MeshRenderer>().material = p.GetComponent<MoveCharacter>().selected;
            }
        }
    }


    public bool CheckPlayerDone()
    {
        foreach (GameObject p in playerPieces)
        {
            if (!p.GetComponent<MoveCharacter>().hasMoved)
            {
                return false;
            }
        }
        SetPlayerTurn(false);
        StartCoroutine(EnemyTurn());
        return true;
    }

    public void SetHasMoved(bool has)
    {
        foreach (GameObject e in enemyPieces)
        {
            e.GetComponent<MoveEnemy>().hasMoved = has;
        }
    }

    IEnumerator EnemyTurn()
    {
        int i = 0;
        foreach (GameObject enemy in enemyPieces)
        {

            enemy.GetComponent<MoveEnemy>().BFS(enemy.transform.parent.GetComponent<TileBehaviour>().x, enemy.transform.parent.GetComponent<TileBehaviour>().y, enemy.GetComponent<MoveEnemy>().movementDistance);
            if ((enemy.GetComponent<MoveEnemy>().targets.Count > 0) && (enemy.GetComponent<EnemyAttack>().behaviour == "Defending"))
            {
                ActivateGroup(enemy.GetComponent<EnemyAttack>().groupId);
            }
        }
        SortTurnOrder();
        while (i < enemyPieces.Count)
        //foreach (GameObject e in enemyPieces)
        {
            GameObject e = enemyPieces[i];
            if (!e.GetComponent<MoveEnemy>().hasMoved)
            {
                int checkIfRemoved = enemyPieces.Count; //Use this to determine if the 
                MoveEnemy eMove = e.GetComponent<MoveEnemy>();
                if (eMove.displaying)
                {
                    eMove.BFS(e.transform.parent.GetComponent<TileBehaviour>().x, e.transform.parent.GetComponent<TileBehaviour>().y, e.GetComponent<MoveEnemy>().movementDistance);
                    //Debug.Log(e.GetComponent<MoveEnemy>().attackableTiles.Count);
                    display.UpdateDisplay(eMove.attackableTiles, false, 1);
                }
                cam.transform.SetParent(eMove.transform, true);
                cam.transform.position = new Vector3(eMove.transform.position.x, cam.transform.position.y, eMove.transform.position.z);
                //cam.transform.position.z = eMove.transform.position.z;
                eMove.DoTurn(e.transform.parent.GetComponent<TileBehaviour>().x, e.transform.parent.GetComponent<TileBehaviour>().y);
                while (eMove.isMoving)
                {
                    yield return null;
                }
                if (eMove.displaying && (checkIfRemoved == enemyPieces.Count))
                {
                    eMove.BFS(e.transform.parent.GetComponent<TileBehaviour>().x, e.transform.parent.GetComponent<TileBehaviour>().y, eMove.movementDistance);
                    display.UpdateDisplay(eMove.attackableTiles, true, 1);
                }
                if (checkIfRemoved == enemyPieces.Count)
                {
                    i += 1;
                }
                if (allyTakedown)
                {
                    SortTurnOrder();
                    i = 0;
                    allyTakedown = false;
                }
                eMove.hasMoved = true;
                waiting = true;
                yield return new WaitForSeconds(0.3f);
            }
        }
        foreach (GameObject e in enemiesToDelete)
        {
            enemyPieces.Remove(e);
        }
        enemiesToDelete = new List<GameObject>();
        SetHasMoved(false);
        SetPlayerTurn(true);
        cam.transform.position = new Vector3(mc.transform.position.x, cam.transform.position.y, mc.transform.position.z);
        cam.transform.parent = null;
        playerPhase = true;
    }

    IEnumerator Wait(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        waiting = false;
    }
}
