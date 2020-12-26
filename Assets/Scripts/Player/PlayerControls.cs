using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayerControls : MonoBehaviour
{
    public static bool DoingAnim;
    public float ObjectRange;
    Build buildScript;
    bool cancelPaused;

    void Start()
    {
        buildScript = GameObject.Find("Farm handler").GetComponent<Build>();
    }

    void Update()
    {      
        if (Input.GetAxis("Speed") != 0) PlayerMovement.IsRunning = true;
        else PlayerMovement.IsRunning = false;

        if (Input.GetKeyDown(KeyCode.B)) TriggerBuild();

        if (Input.GetKeyDown(KeyCode.T)) TriggerTutorial();

        if (Input.GetKeyDown(KeyCode.X)) ThrowItem();

        if (Input.GetKeyDown(KeyCode.O)) OpenLetter();

        if (Input.GetKeyDown(KeyCode.K)) TakeScreenshot();

        if (Input.GetKeyDown(KeyCode.L)) OpenLog();

        if (Input.GetAxisRaw("Cancel") != 0) TriggerCancel(); // If doing something, stop it, else, pause the game
        else cancelPaused = false;

        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject() && !buildScript.enabled && !DoingAnim)
        {
            if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) TriggerActionOneHard();
            else TriggerActionOne();
        }
        else if (Input.GetMouseButtonDown(1) && !EventSystem.current.IsPointerOverGameObject() && !buildScript.enabled)
        {
            if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) TriggerActionTwoHard();
            else TriggerActionTwo();
        }
    }

    public void TriggerTutorial()
    {
        GameObject tutorial = GameObject.Find("UI").transform.Find("Tutorial").gameObject;
        if (tutorial.activeSelf) tutorial.SetActive(false);
        else tutorial.SetActive(true);
    }

    public void TriggerBuild()
    {
        if (!buildScript.enabled && Inventory.Data.ObjectInHand is BuildableObject)
        {
            BuildableObject bo = (BuildableObject)Inventory.Data.ObjectInHand;
            Vector2 pos = new Vector2(Mathf.Round(Camera.main.ScreenToWorldPoint(Input.mousePosition).x * 4.0f) / 4.0f, Mathf.Round(Camera.main.ScreenToWorldPoint(Input.mousePosition).y * 4.0f) / 4.0f);
            buildScript.StartBuild(Instantiate(Resources.Load<GameObject>("Objects/" + bo.Name), pos, Quaternion.Euler(0, 0, 0)), bo);
        }
    }

    public void TriggerCancel()
    {
        if (!cancelPaused)
        {
            cancelPaused = true;
            if (buildScript.enabled) buildScript.CancelBuild();
            else if (Time.timeScale == 0)
            {
                Options.ShowOptions(false);
                Time.timeScale = 1;
            }
            else
            {
                Time.timeScale = 0;
                Options.ShowOptions(true);
            }
        }
    }

    public void TriggerActionOne()
    {
        Vector2 mousePos = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
        if (Physics2D.OverlapPoint(mousePos, 1 << LayerMask.NameToLayer("Pickable")))
        {
            GameObject objectToMove = Physics2D.OverlapPoint(mousePos, 1 << LayerMask.NameToLayer("Pickable")).gameObject;
            if (!objectToMove.CompareTag("House") && Vector2.Distance(transform.position, objectToMove.transform.position) <= ObjectRange) buildScript.StartBuild(objectToMove);
            
        }
        else if (Physics2D.OverlapPoint(mousePos, 1 << LayerMask.NameToLayer("Floor")))
        {
            GameObject floor = Physics2D.OverlapPoint(mousePos, 1 << LayerMask.NameToLayer("Floor")).gameObject;
            if (Inventory.Data.ObjectInHand is Shovel)
            {
                Transform tParent = floor.transform.Find("Vertices");
                List<GameObject> floorsToUpdate = null;
                string floorName = "None";
                foreach (Transform t in tParent)
                {                            
                    Vertex v = VertexSystem.VertexFromPosition(t.transform.position);
                    if (v != null)
                    {
                        if (v.Floor == "Composite tile")
                        {
                            floorsToUpdate = Floor.GetCollidingFloors(floor);
                            floorName = v.Floor;
                        }
                        v.Floor = "None";
                    }
                }
                Destroy(floor);

                if (floorName != "None")
                {
                    foreach (GameObject f in floorsToUpdate)
                    {
                        Floor.UpdateFloorSprite(f, floorName);
                    }
                }
            }
        }
        else if (Physics2D.OverlapPoint(mousePos, 1 << LayerMask.NameToLayer("Plowed soil")))
        {
            GameObject plowedsoil = Physics2D.OverlapPoint(mousePos, 1 << LayerMask.NameToLayer("Plowed soil")).gameObject;
            if (Inventory.Data.ObjectInHand != null)
            {                    
                if (Vector2.Distance(transform.position, plowedsoil.transform.position) <= ObjectRange) plowedsoil.GetComponent<PlowedSoil>().UseTool();
            }
            else plowedsoil.GetComponent<PlowedSoil>().TakeDripBottle();
        }
        else if (Physics2D.OverlapPoint(mousePos, 1 << LayerMask.NameToLayer("Farm land")))
        {
            GameObject farmLand = Physics2D.OverlapPoint(mousePos, 1 << LayerMask.NameToLayer("Farm land")).gameObject;
            if (Inventory.Data.ObjectInHand is Hoe && Vector2.Distance(transform.position, farmLand.transform.position) <= ObjectRange)
            {     
                bool plowable = true;
                foreach (Transform t in farmLand.transform.Find("Vertices"))
                {
                    Vertex v = VertexSystem.VertexFromPosition(t.transform.position);
                    if (v.State != VertexState.Available) plowable = false;
                }
                
                if (plowable)
                {
                    PlayerControls.DoingAnim = true;
                    StartCoroutine(PlayerControls.DoAnim("Hoe", (Vector2)farmLand.transform.position));
                    Master.RunSoundStatic(((Tool)Inventory.Data.ObjectInHand).Clips[0]);

                    GameObject plowedSoil = Instantiate(Resources.Load<GameObject>("Farm/Plowed soil"), farmLand.transform.position, Quaternion.Euler(0, 0, 0));
                    Transform ground = GameObject.Find("Farm").transform;
                    plowedSoil.transform.SetParent(ground);
                    MapSystem.Data.FarmLands.Remove(farmLand.transform.position);
                    Destroy(farmLand);
                    Farm.PlowedSoils.Add(plowedSoil.GetComponent<PlowedSoil>());

                    Farm.UpdateFarm(plowedSoil);

                    foreach (Collider2D col in Physics2D.OverlapCircleAll(plowedSoil.transform.position, 1.5f, 1 << LayerMask.NameToLayer("Plowed soil")))
                    {
                        Farm.UpdateFarm(col.gameObject);
                    }

                    foreach (Transform t in plowedSoil.transform.Find("Vertices"))
                    {
                        Vertex v = VertexSystem.VertexFromPosition(t.transform.position);
                        v.State = VertexState.Occuppied;
                    }
                }
            }
        }
    }

    public void TriggerActionOneHard()
    {
        Vector2 mousePos = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
        if (Physics2D.OverlapPoint(mousePos, 1 << LayerMask.NameToLayer("Pickable")))
        {
            GameObject objectClicked = Physics2D.OverlapPoint(mousePos, 1 << LayerMask.NameToLayer("Pickable")).gameObject;
            if (Vector2.Distance(transform.position, objectClicked.transform.position) <= ObjectRange) ObjectsHandler.Data.Objects.Find(x => x.Model == objectClicked).ActionOne();
        }
    }

    public void TriggerActionTwo()
    {
        Vector2 mousePos = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
        if (Physics2D.OverlapPoint(mousePos, 1 << LayerMask.NameToLayer("Pickable")))
        {
            GameObject objectClicked = Physics2D.OverlapPoint(mousePos, 1 << LayerMask.NameToLayer("Pickable")).gameObject;
            if (Vector2.Distance(transform.position, objectClicked.transform.position) <= ObjectRange) ObjectsHandler.Data.Objects.Find(x => x.Model == objectClicked).ActionTwo();
        }
    }

    public void TriggerActionTwoHard()
    {
        Vector2 mousePos = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
        if (Physics2D.OverlapPoint(mousePos, 1 << LayerMask.NameToLayer("Pickable")))
        {
            GameObject objectClicked = Physics2D.OverlapPoint(mousePos, 1 << LayerMask.NameToLayer("Pickable")).gameObject;
            if (Vector2.Distance(transform.position, objectClicked.transform.position) <= ObjectRange) ObjectsHandler.Data.Objects.Find(x => x.Model == objectClicked).ActionTwoHard();
        }  
    }

    public static IEnumerator DoAnim(string anim, Vector2 pos)
    {
        GameObject player = GameObject.Find("Player");
        Vector2 distance = new Vector2(pos.x - player.transform.position.x,
                                        pos.y - player.transform.position.y);
        
        if (Mathf.Abs(distance.x) > Mathf.Abs(distance.y))
        {   
            if (distance.x < 0)
            {
                anim += " left";
                PlayerMovement.LastDirection = "AnimLeft";
            }
            else
            {
                anim += " right";
                PlayerMovement.LastDirection = "AnimRight";
            }
        }
        else
        {
            if (distance.y < 0)
            {
                anim += " down";
                PlayerMovement.LastDirection = "AnimDown";
            }
            else 
            {
                anim += " up";
                PlayerMovement.LastDirection = "AnimUp";
            }
        }

        player.GetComponent<Animator>().SetTrigger(anim);
        yield return new WaitForSeconds(0.25f);
        player.GetComponent<Animator>().ResetTrigger(anim);
        DoingAnim = false;
    }

    public void ThrowItem()
    {
        if (Inventory.Data.ObjectInHand == null) return;
        if (buildScript.enabled) buildScript.CancelBuild();

        GameObject item = Instantiate(Resources.Load<GameObject>("Item"), new Vector2(transform.position.x, transform.position.y), transform.rotation);
        item.GetComponent<SpriteRenderer>().sprite = Inventory.Data.ObjectInHand.GetUISprite();
        item.GetComponent<Item>().ItemObject = Inventory.Data.ObjectInHand;

        Inventory.Data.ObjectInHand.WorldPosition = item.transform.position;
        ObjectsHandler.Data.Objects.Add(Inventory.Data.ObjectInHand);

        Inventory.RemoveObject();
    }

    public void OpenLetter()
    {
        if (!(Inventory.Data.ObjectInHand is Letter)) return;

        if (UI.Elements["Letter"].activeSelf) // Close letter
        {
            UI.Elements["Letter"].SetActive(false);
            return;
        }

        Letter letter = (Letter)Inventory.Data.ObjectInHand;
        letter.Read = true;
        letter.TranslationKey = "LetterOpen";
        Inventory.ChangeObject();
        UI.Elements["Letter"].GetComponent<Image>().sprite = UI.Sprites[letter.Type];
        UI.Elements["Letter title"].GetComponent<Text>().text = letter.Title;
        UI.Elements["Letter body"].GetComponent<Text>().text = letter.Body;
        UI.Elements["Letter signature"].GetComponent<Text>().text = letter.Signature;
        UI.Elements["Letter"].SetActive(true);
    }

    public void TakeScreenshot()
    {
        Camera camera = transform.Find("Camera").gameObject.GetComponent<Camera>();
        RenderTexture rt = new RenderTexture(Options.Data.Width, Options.Data.Height, 24);
        camera.targetTexture = rt;
        Texture2D screenShot = new Texture2D(Options.Data.Width, Options.Data.Height, TextureFormat.RGB24, false);
        camera.Render();
        RenderTexture.active = rt;
        screenShot.ReadPixels(new Rect(0, 0, Options.Data.Width, Options.Data.Height), 0, 0);
        camera.targetTexture = null;
        RenderTexture.active = null;
        Destroy(rt);
        byte[] bytes = screenShot.EncodeToPNG();
        string filename = string.Format("{0}/Screenshots/Vivaio {1} ({2}).png", Options.Data.DataPath, Master.GameVersion, System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"));
        System.IO.File.WriteAllBytes(filename, bytes);
    }

    public void OpenLog()
    {
        if (UI.Elements["Log"].activeSelf)
        {
            CloseLog();
            return;
        }
        
        string logText = "Vivaio " + Master.GameVersion + " " + Master.GameEdition + " Edition (" + Master.VersionDate + ")\n\n";

        if (Master.Data.Log.Count == 0)
        {
            logText += "No errors found!";
        }
        else
        {
            foreach (string l in Master.Data.Log)
            {
                logText += l + "\n\n";
            }
        }

        UI.Elements["Log text"].GetComponent<Text>().text = logText;
        UI.Elements["Log"].SetActive(true);
    }

    public void CloseLog()
    {
        UI.Elements["Log"].SetActive(false);
    }
}