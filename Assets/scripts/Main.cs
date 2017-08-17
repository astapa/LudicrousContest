using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    public Queue<int[]> playerStack;
    public Queue<int[]> enemyStack;

    public GameObject player;
    public GameObject enemy;
    public GameObject playerSprite;
    public GameObject enemySprite;
    PlayerProperties pprops;
    EnemyProperties eprops;
    BoxCollider2D playerHurtbox;
    BoxCollider2D enemyHurtbox;
    public GameObject hitbox;
    public GameObject counterbox;

    private int dx;
    private int dy;
    private bool lr;
    private bool playerReady;
    private LinkedList<GameObject> tmpBoxes;
    private GameObject[] ui;

    // Use this for initialization
    void Start ()
    {
        dx = 0;
        dy = 0;
        lr = true;
        playerReady = true;
        playerStack = new Queue<int[]> ();
        enemyStack = new Queue<int[]> ();
        tmpBoxes = new LinkedList<GameObject> ();
        pprops = player.GetComponent<PlayerProperties> ();
        eprops = enemy.GetComponent<EnemyProperties> ();
        pprops.statusEffects = new LinkedList<int> ();
        eprops.statusEffects = new LinkedList<int> ();
        playerHurtbox = player.GetComponent<BoxCollider2D> ();
        enemyHurtbox = enemy.GetComponent<BoxCollider2D> ();
        ui = GameObject.FindGameObjectsWithTag ("HideableUi");
        foreach (GameObject g in ui)
        {
            g.SetActive (false);
        }
        StartCoroutine (doPhysics ());
    }

    IEnumerator doPhysics ()
    {
        while (true)
        {
            Debug.Log ("entered infinite loop");
            player.transform.position.Set (0, 0, 0);
            foreach (GameObject g in tmpBoxes)
            {
                Destroy (g);
            }
            yield return new WaitForSeconds (3);
            // perform moves
            int[] playerMove;
            if (playerStack.Count == 0)
            {
                Debug.Log ("stack empty");
                foreach (GameObject g in ui)
                {
                    Debug.Log ("activated object");
                    g.SetActive (true);
                }
                playerReady = false;
                while (!playerReady)
                {
                    Debug.Log ("entered other loop");
                    continue;
                }
                foreach (GameObject g in ui)
                {
                    g.SetActive (false);
                }
            }
            playerMove = playerStack.Dequeue ();
            if (enemyStack.Count == 0)
            {
                enemyStack.Enqueue (new int[] { 0 });
            }
            int[] enemyMove = enemyStack.Dequeue ();

            int punchyoffset = 5;
            float aurayoffset = 8.5f;
            int auraSideyoffset = 4;
            int auraYsize = 8;

            if (pprops.isCrouched)
            {
                punchyoffset = 3;
                aurayoffset = 4.5f;
                auraSideyoffset = 2;
                auraYsize = 4;
            }
            switch (playerMove [0])
            {
            case 1: // punch
                GameObject h = Instantiate (hitbox, player.transform);
                Vector3 newPos = new Vector3 (3, punchyoffset, 0);
                Vector3 newScale = new Vector3 (2, 2, 1);
                if (playerMove [1] == 0)
                    newPos.x = -3;
                h.transform.localPosition = newPos;
                h.transform.localScale = newScale;
                tmpBoxes.AddLast (h);
                break;
            case 2: // aura
                GameObject hitboxUp = Instantiate (hitbox, player.transform);
                GameObject hitboxLeft = Instantiate (hitbox, player.transform);
                GameObject hitboxRight = Instantiate (hitbox, player.transform);
                GameObject hitboxDown = Instantiate (hitbox, player.transform);
                GameObject counterboxDown = Instantiate (counterbox, player.transform);
                GameObject counterboxUp = Instantiate (counterbox, player.transform);
                GameObject counterboxLeft = Instantiate (counterbox, player.transform);
                GameObject counterboxRight = Instantiate (counterbox, player.transform);

                Vector3 upPos = new Vector3 (0, aurayoffset, 0);
                Vector3 downPos = new Vector3 (0, -0.5f, 0);
                Vector3 leftPos = new Vector3 (-2.5f, auraSideyoffset, 0);
                Vector3 rightPos = new Vector3 (2.5f, auraSideyoffset, 0);
                Vector3 hscale = new Vector3 (4, 1, 1);
                Vector3 vscale = new Vector3 (1, auraYsize, 1);

                hitboxDown.transform.localPosition = downPos;
                hitboxDown.transform.localScale = hscale;
                hitboxUp.transform.localPosition = upPos;
                hitboxUp.transform.localScale = hscale;
                hitboxLeft.transform.localPosition = leftPos;
                hitboxLeft.transform.localScale = vscale;
                hitboxRight.transform.localPosition = rightPos;
                hitboxRight.transform.localScale = vscale;
                counterboxDown.transform.localPosition = downPos;
                counterboxDown.transform.localScale = hscale;
                counterboxUp.transform.localPosition = upPos;
                counterboxUp.transform.localScale = hscale;
                counterboxLeft.transform.localPosition = leftPos;
                counterboxLeft.transform.localScale = vscale;
                counterboxRight.transform.localPosition = rightPos;
                counterboxRight.transform.localScale = vscale;
                tmpBoxes.AddLast (hitboxUp);
                tmpBoxes.AddLast (hitboxDown);
                tmpBoxes.AddLast (hitboxLeft);
                tmpBoxes.AddLast (hitboxRight);
                tmpBoxes.AddLast (counterboxUp);
                tmpBoxes.AddLast (counterboxDown);
                tmpBoxes.AddLast (counterboxLeft);
                tmpBoxes.AddLast (counterboxRight);
                break;
            case 3: // move
                if (!pprops.isCrouched)
                {
                    pprops.xvelocity += playerMove [1];
                    pprops.yvelocity += playerMove [2];
                }
                break;
            case 4: // crouch
                if (pprops.isCrouched)
                {
                    playerHurtbox.size = new Vector2 (4, 8);
                    playerHurtbox.offset = new Vector2 (0, 4);
                    playerSprite.transform.localPosition = new Vector3 (0, 4, 0);
                    playerSprite.transform.localScale = new Vector3 (1, 2, 1);
                } else
                {
                    playerHurtbox.size = new Vector2 (4, 4);
                    playerHurtbox.offset = new Vector2 (0, 2);
                    playerSprite.transform.localPosition = new Vector3 (0, 2, 0);
                    playerSprite.transform.localScale = new Vector3 (1, 1, 1);
                }
                pprops.isCrouched = !pprops.isCrouched;
                break;
            default:
                break;
            }
            // check collisions
            // apply effects
            // apply velocity
            // apply friction
            yield return new WaitForSeconds (3);
        }
    }

    public void updateDX (string s)
    {
        dx = int.Parse (s);
    }

    public void updateDY (string s)
    {
        dy = int.Parse (s);
    }

    public void updateLR ()
    {
        lr = !lr;
    }

    public void stackPunch ()
    {
        int data = 0;
        if (lr)
            data = 1;
        playerStack.Enqueue (new int[] { 1, data });
    }

    public void stackAura ()
    {
        playerStack.Enqueue (new int[] { 2 });
    }

    public void stackMove ()
    {
        playerStack.Enqueue (new int[] { 3, dx, dy });
    }

    public void stackCrouch ()
    {
        playerStack.Enqueue (new int[] { 4 });
    }

    public void stackIdle ()
    {
        playerStack.Enqueue (new int[] { 0 });
    }

    public void submit ()
    {
        if (playerStack.Count >= pprops.reactionTime)
            playerReady = true;
    }
}
