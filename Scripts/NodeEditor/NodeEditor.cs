using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Linq;

public class NodeEditor : EditorWindow {

    [SerializeField]
    private List<BaseNode> windows = new List<BaseNode>();
    [SerializeField]
    private List<int> nodeID = new List<int>();
    [SerializeField]
    private List<GameObject> allGO = new List<GameObject>();

    // the nodeCounter to make sure every node has a unique ID
    private int nodeCounter;

    // Store the mousePosition ( current event e.mousePosition
    private Vector2 mousePos;

    // The selected node
    private BaseNode selectedNode;

    // Are we in transitionMode
    private bool makeTransitionMode = false;

    

    public GUISkin _skin;
     
    // Array to store all the GameObjects in the scene
    private GameObject[] _sceneGO;

    // Are we loading the canvas
    private bool _isLoading = false;

    
    // Floats to change the size of our node windows
    private float _windowWidth = 250f;
    private float _windowHeight = 120f;

    // Vars to create the dirty canvas moving
    private Vector2 _initialPos;
    private bool _setPosition;


    [MenuItem("Cinematics Manager/Node Editor")]

    static void ShowEditor()
    {

        NodeEditor editor = EditorWindow.GetWindow<NodeEditor>();

                   

    }

    void Update()
    {
       
        Repaint();

    
    }

    void OnGUI()
    {
  

        GUI.skin = _skin;
        //Handles.BeginGUI();

        GameObject[] _tmpGO = GameObject.FindGameObjectsWithTag("Node").OrderBy(go => go.name).ToArray();
        _sceneGO = GameObject.FindGameObjectsWithTag("Node").OrderBy(go => go.name).ToArray();



        ////////////////////////////////////////////////////////////////////////////////////
        //
        //                                  LOADING FROM THE SCENE
        //
        // We check to see if there are any gameobjects in the scene with the tag 'Node'
        // If there are we:
        //              Create a new Node based on the animation type ( ReturnAnim() )
        //              Draw a new Rect and place it at the positions we get from the NodeObject component
        //              Set the WayPoint ( GameObject )
        //              We add the node to the windows List
        //              We add the current GameObject from the scene in our allGO List
        //              We set the nodeID which we get from nodeCounter var
        //              We add the nodeID ( nodeCounter ) to the nodeID list
        //              We add 1 to the nodeCounter
        ////////////////////////////////////////////////////////////////////////////////////

        if (GUILayout.Button("Load from scene"))
        {
            _isLoading = true;
            // _sceneGO = GameObject.FindGameObjectsWithTag("Node").OrderBy(go => go.name).ToArray();

            for (int i = 0; i < _sceneGO.Length; i++)
            {
                if (_sceneGO[i].GetComponent<NodeObject>().ReturnAnim() == "Start")
                {
                    AnimationStartNode animStartNode = new AnimationStartNode();
                    animStartNode.windowRect = new Rect(_sceneGO[i].GetComponent<NodeObject>().ReturnPosX(), _sceneGO[i].GetComponent<NodeObject>().ReturnPosY(), _windowWidth, _windowHeight);

                    animStartNode.SetWayPoint(_sceneGO[i].GetComponent<NodeObject>().ReturnWayPoint());

                    windows.Add(animStartNode);
                    allGO.Add(_sceneGO[i]);

                    animStartNode.SetID(nodeCounter);
                    nodeID.Add(nodeCounter);



                    nodeCounter++;
                }
                if (_sceneGO[i].GetComponent<NodeObject>().ReturnAnim() == "Idle")
                {
                    IdleNode idleNode = new IdleNode();
                    idleNode.windowRect = new Rect(_sceneGO[i].GetComponent<NodeObject>().ReturnPosX(), _sceneGO[i].GetComponent<NodeObject>().ReturnPosY(), _windowWidth, _windowHeight);

                    windows.Add(idleNode);
                    allGO.Add(_sceneGO[i]);

                    idleNode.SetIdle(_sceneGO[i].GetComponent<NodeObject>().ReturnIdleWait());
                    if(_sceneGO[i].GetComponent<NodeObject>().ReturnAudio() != null)
                    {

                        idleNode.SetSound("Yes");
                        idleNode.SetAudio(_sceneGO[i].GetComponent<NodeObject>().ReturnAudio());

                    }

                    idleNode.SetID(nodeCounter);
                    nodeID.Add(nodeCounter);
                    nodeCounter++;
                }
                if (_sceneGO[i].GetComponent<NodeObject>().ReturnAnim() == "Walk")
                {
                    WalkNode walkNode = new WalkNode();
                    walkNode.windowRect = new Rect(_sceneGO[i].GetComponent<NodeObject>().ReturnPosX(), _sceneGO[i].GetComponent<NodeObject>().ReturnPosY(), _windowWidth, _windowHeight);

                    walkNode.SetWayPoint(_sceneGO[i].GetComponent<NodeObject>().ReturnWayPoint());

                    if (_sceneGO[i].GetComponent<NodeObject>().ReturnAudio() != null)
                    {

                        walkNode.SetSound("Yes");
                        walkNode.SetAudio(_sceneGO[i].GetComponent<NodeObject>().ReturnAudio());

                    }

                    windows.Add(walkNode);
                    allGO.Add(_sceneGO[i]);

                    walkNode.SetID(nodeCounter);
                    nodeID.Add(nodeCounter);
                    nodeCounter++;
                }
                if (_sceneGO[i].GetComponent<NodeObject>().ReturnAnim() == "Run")
                {
                    RunNode runNode = new RunNode();
                    runNode.windowRect = new Rect(_sceneGO[i].GetComponent<NodeObject>().ReturnPosX(), _sceneGO[i].GetComponent<NodeObject>().ReturnPosY(), _windowWidth, _windowHeight);

                    runNode.SetWayPoint(_sceneGO[i].GetComponent<NodeObject>().ReturnWayPoint());


                    if (_sceneGO[i].GetComponent<NodeObject>().ReturnAudio() != null)
                    {

                        runNode.SetSound("Yes");
                        runNode.SetAudio(_sceneGO[i].GetComponent<NodeObject>().ReturnAudio());

                    }

                    windows.Add(runNode);
                    allGO.Add(_sceneGO[i]);

                    runNode.SetID(nodeCounter);
                    nodeID.Add(nodeCounter);
                    nodeCounter++;
                }

                if (_sceneGO[i].GetComponent<NodeObject>().ReturnAnim() == "CombatIdle")
                {
                    CombatIdleNode combatIdleNode = new CombatIdleNode();
                    combatIdleNode.windowRect = new Rect(_sceneGO[i].GetComponent<NodeObject>().ReturnPosX(), _sceneGO[i].GetComponent<NodeObject>().ReturnPosY(), _windowWidth, _windowHeight);

                    combatIdleNode.SetIdle(_sceneGO[i].GetComponent<NodeObject>().ReturnIdleWait());


                    if (_sceneGO[i].GetComponent<NodeObject>().ReturnAudio() != null)
                    {

                        combatIdleNode.SetSound("Yes");
                        combatIdleNode.SetAudio(_sceneGO[i].GetComponent<NodeObject>().ReturnAudio());

                    }

                    windows.Add(combatIdleNode);
                    allGO.Add(_sceneGO[i]);

                    combatIdleNode.SetID(nodeCounter);
                    nodeID.Add(nodeCounter);
                    nodeCounter++;
                }

                if (_sceneGO[i].GetComponent<NodeObject>().ReturnAnim() == "CustomNode")
                {
                    CustomNode customNode = new CustomNode();
                    customNode.windowRect = new Rect(_sceneGO[i].GetComponent<NodeObject>().ReturnPosX(), _sceneGO[i].GetComponent<NodeObject>().ReturnPosY(), _windowWidth, _windowHeight);
                    if (_sceneGO[i].GetComponent<NodeObject>().ReturnCustomAnimType() == "Idle")
                    {
                        customNode.SetIdle(_sceneGO[i].GetComponent<NodeObject>().ReturnIdleWait());
                    }
                    else if (_sceneGO[i].GetComponent<NodeObject>().ReturnCustomAnimType() == "Walk" || _sceneGO[i].GetComponent<NodeObject>().ReturnCustomAnimType() == "Run" || _sceneGO[i].GetComponent<NodeObject>().ReturnCustomAnimType() == "Ranged")
                    {

                        customNode.SetWayPoint(_sceneGO[i].GetComponent<NodeObject>().ReturnWayPoint());
                        if (_sceneGO[i].GetComponent<NodeObject>().ReturnAudio() != null)
                        {

                            customNode.SetSound("Yes");
                            customNode.SetAudio(_sceneGO[i].GetComponent<NodeObject>().ReturnAudio());

                        }

                    }
                }
                    if (_sceneGO[i].GetComponent<NodeObject>().ReturnAnim() == "Ranged")
                    {
                        RangedNode rangedNode = new RangedNode();
                        rangedNode.windowRect = new Rect(_sceneGO[i].GetComponent<NodeObject>().ReturnPosX(), _sceneGO[i].GetComponent<NodeObject>().ReturnPosY(), _windowWidth, _windowHeight);
                        
                       
                            rangedNode.SetWayPoint(_sceneGO[i].GetComponent<NodeObject>().ReturnWayPoint());
                            if (_sceneGO[i].GetComponent<NodeObject>().ReturnAudio() != null)
                            {

                                rangedNode.SetSound("Yes");
                                rangedNode.SetAudio(_sceneGO[i].GetComponent<NodeObject>().ReturnAudio());

                            }


                        windows.Add(rangedNode);
                        allGO.Add(_sceneGO[i]);

                        rangedNode.SetID(nodeCounter);
                        nodeID.Add(nodeCounter);
                        nodeCounter++;
                    }

            }

        }

        // We try to redraw the existing connections between nodes
        // if the _sceneGO is not empty AND there is at least 1 node in the windows list
        //      Go through all the objects
        //      Safety check to see if the specific _sceneGO object is not null
        //      If the outputID is greater than 0 ( else we get a circle since the curve will be drawn back to its origin
        //      Draw the curve from the current window to the window that is stated in the outputID from the NodeObject attached to itself

        if (_sceneGO != null && windows.Count > 0)
        {
            for (int j = 0; j < _sceneGO.Length; j++)
            {
                if (_sceneGO[j] != null)
                {

                    if (_sceneGO[j].GetComponent<NodeObject>().ReturnOutputID() > 0)
                    {
                        DrawNodeCurve(windows[j].windowRect, windows[_sceneGO[j].GetComponent<NodeObject>().ReturnOutputID()].windowRect);
                        Repaint();
                    }
                }
                else
                {
                    Debug.Log("the array _sceneGO is empty");
                }
            }
        }
        else
        {
            
            Debug.Log("Window counting error");
        }

        /////////////////////////////////////////////////////////////////////////
        //                          CLEARING THE CANVAS
        //
        // Clear Canvas
        // Get all the GameObjects that are asociated with a node
        // DestroyImmediate -> Editor function -> Destroy does not work in the editor
        //
        // Clear all the lists
        /////////////////////////////////////////////////////////////////////////

        if (GUILayout.Button("Clear Canvas"))
        {


            for (int i = 0; i < _tmpGO.Length; i++)
            {
                DestroyImmediate(_tmpGO[i]);
            }

            windows.Clear();
            allGO.Clear();
            nodeCounter = 0;


        }


        Event e = Event.current;
        mousePos = e.mousePosition;

        // If Right button and we are not in transitionMode ( drawing a curve )

        if (e.button == 1 && !makeTransitionMode)
        {
            _isLoading = false;
            // Have we clicked
            if (e.type == EventType.mouseDown)
            {
                bool clickedOnWindow = false;
                int selectIndex = -1;

                for (int i = 0; i < windows.Count; i++)
                {
                    // Check to see if we clicked on our selected node (itself)
                    if (windows[i].windowRect.Contains(mousePos))
                    {

                        selectIndex = i;
                        clickedOnWindow = true;
                        break;
                    }
                }


                // If we have not clicked on a window but still right clicked we want to Menu to appear to add new nodes

                if (!clickedOnWindow)
                {
                    GenericMenu menu = new GenericMenu();

                    //     menu.AddItem(new GUIContent("Add Input Node"), false, ContextCallback, "inputNode");
                    //menu.AddItem(new GUIContent("Add Output Node"), false, ContextCallback, "outputNode");
                    //     menu.AddItem(new GUIContent("Add Calculation Node"), false, ContextCallback, "calcNode");
                    //     menu.AddItem(new GUIContent("Add Comparison Node"), false, ContextCallback, "compNode");
                    //     menu.AddSeparator(" ");
                    //     menu.AddItem(new GUIContent("Add GameObject Node"), false, ContextCallback, "goActive");
                    //     menu.AddItem(new GUIContent("Add Distance Node"), false, ContextCallback, "goDistance");
                    //     menu.AddItem(new GUIContent("Add Timer Node"), false, ContextCallback, "timerNode");
                    //     menu.AddSeparator(" ");
                    menu.AddDisabledItem(new GUIContent("[ Cinematics ]"));
                    menu.AddItem(new GUIContent("Add Animation Start Node"), false, ContextCallback, "animStartNode");
                    //  menu.AddItem(new GUIContent("Add Animation Node"), false, ContextCallback, "animNode");
                    menu.AddItem(new GUIContent("Add Idle Node"), false, ContextCallback, "idleNode");
                    menu.AddItem(new GUIContent("Add Walk To Node"), false, ContextCallback, "walkNode");
                    menu.AddItem(new GUIContent("Add Run To Node"), false, ContextCallback, "runNode");
                    menu.AddItem(new GUIContent("Add Combat Idle Node"), false, ContextCallback, "combatIdleNode");
                    menu.AddItem(new GUIContent("Add Custom Animation Node"), false, ContextCallback, "customNode");
                    menu.AddDisabledItem(new GUIContent("[ ATTACK ANIMATION ]"));
                    menu.AddItem(new GUIContent("Add Ranged Attack Node"), false, ContextCallback, "rangedNode");

                    menu.AddDisabledItem(new GUIContent("[ AUDIO ]"));

                    menu.AddItem(new GUIContent("Add Sound Track"), false, ContextCallback, "soundtrackNode");
                    

                    menu.ShowAsContext();
                    e.Use();

                }

                else
                {
                    // If we have clicked on a itself show the different menu
                    GenericMenu menu = new GenericMenu();
                    menu.AddItem(new GUIContent("Make Transition"), false, ContextCallback, "makeTransition");
                    menu.AddSeparator(" ");
                    menu.AddItem(new GUIContent("Delete Node"), false, ContextCallback, "deleteNode");

                    menu.ShowAsContext();
                    e.Use();

                }

            }
        }

        // If we have clicked on makeTransition 

        else if (e.button == 0 && e.type == EventType.mouseDown && makeTransitionMode)
        {

            bool clickedOnWindow = false;
            int selectIndex = -1;

            for (int i = 0; i < windows.Count; i++)
            {
                // Get the current window we have selected ( FROM: Node )
                if (windows[i].windowRect.Contains(mousePos))
                {

                    selectIndex = i;
                    clickedOnWindow = true;
                    break;
                }
            }

            // If we have clicked on a window that is not the window we have selected ( TO: node )
            if (clickedOnWindow && !windows[selectIndex].Equals(selectedNode))
            {

                // Set the input in the TO node
                windows[selectIndex].SetInput((BaseInputNode)selectedNode, mousePos);
                //Debug.Log("From node: " + selectedNode + " to: " + windows[selectIndex]);
                //Debug.Log(selectedNode.ReturnID());
                //Debug.Log(windows[selectIndex].ReturnID());

                // In our FROM node we set the outputID from the TO node
                allGO[selectedNode.ReturnID()].GetComponent<NodeObject>().setOutputID(windows[selectIndex].ReturnID());


                makeTransitionMode = false;
                selectedNode = null;

            }

            if (!clickedOnWindow)
            {
                makeTransitionMode = false;
                selectedNode = null;
            }

            e.Use();
        }

        // If we have clicked on a window but we are NOT in transitionMode
        // We set the node we have clicked to nodeToChange
        else if (e.button == 0 && e.type == EventType.mouseDown && !makeTransitionMode)
        {
            bool clickedOnWindow = false;
            int selectIndex = -1;

            for (int i = 0; i < windows.Count; i++)
            {
                if (windows[i].windowRect.Contains(mousePos))
                {

                    selectIndex = i;
                    clickedOnWindow = true;
                    break;
                }
            }

            if (clickedOnWindow)
            {
                BaseInputNode nodeToChange = windows[selectIndex].ClickedOnInput(mousePos);
                if (nodeToChange != null)
                {
                    selectedNode = nodeToChange;
                    makeTransitionMode = true;
                }
            }
        }

        // If we are in transitionMode and clicked on a node 

        if (makeTransitionMode && selectedNode != null)
        {
            // Draw the Bezier Curve
            Rect mouseRect = new Rect(e.mousePosition.x, e.mousePosition.y, 10, 10);
            DrawNodeCurve(selectedNode.windowRect, mouseRect);

            Repaint();
        }

        foreach (BaseNode n in windows)
        {
            n.DrawCurves();
        }

        BeginWindows();
        // GUILayout.BeginScrollView();

        for (int i = 0; i < windows.Count; i++)
        {
            windows[i].windowRect = GUI.Window(i, windows[i].windowRect, DrawNodeWindow, windows[i].windowTitle);
            
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////
        //                                    DIRTY CANVAS MOVING               
        // 
        // Scaling with the GUI.matrix does not really work and is pretty nasty we create a workaround
        //
        //          If we are dragging with the scrollWheel
        //          If the initial position is not equal to the current mousePosition
        //          If the bool _setPosition is false
        //          Set the _initialPos to our current mousePosition
        //          Set the bool _setPosition to true ( so we only set it ONCE per drag )
        //          Calculate the difference between the initial position and the current mousePosition
        //                          The difference is used to offset the nodes
        //          Get ALL the windows in our current windows List
        //          Move them on x and y
        /////////////////////////////////////////////////////////////////////////////////////////////////////


        if (e.button == 2 && e.type ==  EventType.MouseDrag)
        {
            if (_initialPos != e.mousePosition)
            {
               
                if(!_setPosition)
                {
                    _initialPos = e.mousePosition;
                    _setPosition = true;
                }

                Vector2 _offSet = _initialPos - e.mousePosition;

                for (int i = 0; i < windows.Count; i++)
                {
                    if (_initialPos.x < e.mousePosition.x)
                    {
                        windows[i].windowRect.x -= _offSet.x;
                        _setPosition = false;
                       
                    }
                    if (_initialPos.x > e.mousePosition.x)
                    {
                        windows[i].windowRect.x -= _offSet.x;
                        _setPosition = false;
                       
                    }
                    if (_initialPos.y < e.mousePosition.y)
                    {
                        windows[i].windowRect.y -= _offSet.y;
                        _setPosition = false;
                    }
                    if (_initialPos.y > e.mousePosition.y)
                    {
                        windows[i].windowRect.y -= _offSet.y;
                        _setPosition = false;
                    }
                    
                }
            }
        }

        EndWindows();

    
        
    }


    // Draw the node as a window

    void DrawNodeWindow(int id)
    {
     
        windows[id].DrawWindow();

        // set it dragable
        GUI.DragWindow();

        if(id != null)
        {
            
            allGO[id].GetComponent<NodeObject>().setPosition(windows[id].windowRect.x, windows[id].windowRect.y);
        }
     

    }

    // ContextCallBack()
    // Called by the OnGUI() to create a new Menu item
    // clb = Callback


    void ContextCallback(object obj)
    {
        string clb = obj.ToString();
        GameObject _parent = GameObject.FindGameObjectWithTag("NodeParent");

        if(clb.Equals("inputNode"))
        {

            InputNode inputNode = new InputNode();
            inputNode.windowRect = new Rect(mousePos.x, mousePos.y, _windowWidth, _windowHeight);

            windows.Add(inputNode);


            //Create a new empty gameobject to store in the scene 
            GameObject newNode = new GameObject();

                newNode.AddComponent<NodeObject>();
                newNode.GetComponent<NodeObject>().setNodeID(nodeCounter);
                newNode.GetComponent<NodeObject>().setPosition(mousePos.x, mousePos.y);
                 inputNode.SetID(nodeCounter);

            // make it a child of "Node"
            newNode.transform.parent = _parent.transform;

            // Name the newly made gameobject with Node + n
            newNode.name = "Node" + nodeCounter;

            // give it the tag Node
            newNode.tag = "Node";

            // Add the counter to the nodeID list to keep track of everything
            nodeID.Add(nodeCounter);

            // Add 1 to the nodecounter
            nodeCounter++;

           
         }
        else if(clb.Equals("outputNode"))
        {
            OutputNode outputNode = new OutputNode();
            outputNode.windowRect = new Rect(mousePos.x, mousePos.y, _windowWidth, _windowHeight);

            windows.Add(outputNode);

            GameObject newNode = new GameObject();

                newNode.AddComponent<NodeObject>();
                newNode.GetComponent<NodeObject>().setNodeID(nodeCounter);
                newNode.GetComponent<NodeObject>().setTitle(outputNode.windowTitle);
            newNode.GetComponent<NodeObject>().setPosition(mousePos.x, mousePos.y);

            outputNode.SetID(nodeCounter);

            newNode.transform.parent = _parent.transform;
            newNode.name = "Node" + nodeCounter;
            newNode.tag = "Node";
            nodeID.Add(nodeCounter);
            nodeCounter++;

           
        }

        else if (clb.Equals("calcNode"))
        {
            CalcNode calcNode = new CalcNode();
            calcNode.windowRect = new Rect(mousePos.x, mousePos.y, _windowWidth, _windowHeight);
            windows.Add(calcNode);

            GameObject newNode = new GameObject();

                newNode.AddComponent<NodeObject>();
                newNode.GetComponent<NodeObject>().setNodeID(nodeCounter);
                newNode.GetComponent<NodeObject>().setTitle(calcNode.windowTitle);
            newNode.GetComponent<NodeObject>().setPosition(mousePos.x, mousePos.y);

            calcNode.SetID(nodeCounter);

            newNode.transform.parent = _parent.transform;
            newNode.name = "Node" + nodeCounter;
            newNode.tag = "Node";
            nodeID.Add(nodeCounter);
            nodeCounter++;

         
        }

        else if(clb.Equals("compNode"))
        {
            ComparisonNode compNode = new ComparisonNode();
            compNode.windowRect = new Rect(mousePos.x, mousePos.y, _windowWidth, _windowHeight);

            windows.Add(compNode);

            GameObject newNode = new GameObject();

                newNode.AddComponent<NodeObject>();
                newNode.GetComponent<NodeObject>().setNodeID(nodeCounter);
                newNode.GetComponent<NodeObject>().setTitle(compNode.windowTitle);
            newNode.GetComponent<NodeObject>().setPosition(mousePos.x, mousePos.y);

            compNode.SetID(nodeCounter);

            newNode.transform.parent = _parent.transform;
            newNode.name = "Node" + nodeCounter;
            newNode.tag = "Node";
            nodeID.Add(nodeCounter);
            nodeCounter++;

         


        }
        else if(clb.Equals("goActive"))
        {
            GameObjectActive goNode = new GameObjectActive();
            goNode.windowRect = new Rect(mousePos.x, mousePos.y, _windowWidth, _windowHeight);

            windows.Add(goNode);

            GameObject newNode = new GameObject();

                newNode.AddComponent<NodeObject>();
                newNode.GetComponent<NodeObject>().setNodeID(nodeCounter);
                newNode.GetComponent<NodeObject>().setTitle(goNode.windowTitle);
            newNode.GetComponent<NodeObject>().setPosition(mousePos.x, mousePos.y);

            goNode.SetID(nodeCounter);

            newNode.transform.parent = _parent.transform;
            newNode.name = "Node" + nodeCounter;
            nodeID.Add(nodeCounter);
            nodeCounter++;

          
        }

        else if(clb.Equals("goDistance"))
        {
            GameObjectDistance goDist = new GameObjectDistance();
            goDist.windowRect = new Rect(mousePos.x, mousePos.y, _windowWidth, _windowHeight);

            windows.Add(goDist);

            GameObject newNode = new GameObject();

                newNode.AddComponent<NodeObject>();
                newNode.GetComponent<NodeObject>().setNodeID(nodeCounter);
                newNode.GetComponent<NodeObject>().setTitle(goDist.windowTitle);
            newNode.GetComponent<NodeObject>().setPosition(mousePos.x, mousePos.y);

            goDist.SetID(nodeCounter);

            newNode.transform.parent = _parent.transform;
            newNode.name = "Node" + nodeCounter;
            nodeID.Add(nodeCounter);
            nodeCounter++;

           
        }

        else if(clb.Equals("timerNode"))
        {
            TimerNode timerNode = new TimerNode();
            timerNode.windowRect = new Rect(mousePos.x, mousePos.y, _windowWidth, _windowHeight);

            windows.Add(timerNode);

            GameObject newNode = new GameObject();

                newNode.AddComponent<NodeObject>();
                newNode.GetComponent<NodeObject>().setNodeID(nodeCounter);
                newNode.GetComponent<NodeObject>().setTitle(timerNode.windowTitle);
            newNode.GetComponent<NodeObject>().setPosition(mousePos.x, mousePos.y);

            timerNode.SetID(nodeCounter);

            newNode.transform.parent = _parent.transform;
            newNode.name = "Node" + nodeCounter;
            nodeID.Add(nodeCounter);
            nodeCounter++;

           
        }

        else if(clb.Equals("animStartNode"))
        {
            AnimationStartNode animStartNode = new AnimationStartNode();
            animStartNode.windowRect = new Rect(mousePos.x, mousePos.y, _windowWidth, _windowHeight);

            windows.Add(animStartNode);

            GameObject newNode = new GameObject();

                newNode.AddComponent<NodeObject>();
                newNode.GetComponent<NodeObject>().setNodeID(nodeCounter);
                newNode.GetComponent<NodeObject>().setTitle(animStartNode.windowTitle);
                newNode.GetComponent<NodeObject>().setAnimation("Start");
            newNode.GetComponent<NodeObject>().setPosition(mousePos.x, mousePos.y);

            animStartNode.SetID(nodeCounter);

            newNode.transform.parent = _parent.transform;
            newNode.name = "Node" + nodeCounter;
            newNode.tag = "Node";
            nodeID.Add(nodeCounter);
            allGO.Add(newNode);
            nodeCounter++;

            
        }

        else if (clb.Equals("animNode"))
        {
            AnimationNode animNode = new AnimationNode();
            animNode.windowRect = new Rect(mousePos.x, mousePos.y, _windowWidth, _windowHeight);

            windows.Add(animNode);

            GameObject newNode = new GameObject();

                newNode.AddComponent<NodeObject>();
                newNode.GetComponent<NodeObject>().setNodeID(nodeCounter);
                newNode.GetComponent<NodeObject>().setTitle(animNode.windowTitle);
            newNode.GetComponent<NodeObject>().setPosition(mousePos.x, mousePos.y);

            animNode.SetID(nodeCounter);

            newNode.transform.parent = _parent.transform;
            newNode.name = "Node" + nodeCounter;
            nodeID.Add(nodeCounter);
            allGO.Add(newNode);
            nodeCounter++;

          
        }

        else if(clb.Equals("idleNode"))
        {
            IdleNode idleNode = new IdleNode();
            idleNode.windowRect = new Rect(mousePos.x, mousePos.y, _windowWidth, _windowHeight);
            idleNode.SetID(nodeCounter);
            windows.Add(idleNode);

            GameObject newNode = new GameObject();
            

                newNode.AddComponent<NodeObject>();
                newNode.GetComponent<NodeObject>().setNodeID(nodeCounter);
                newNode.GetComponent<NodeObject>().setTitle(idleNode.windowTitle);
                newNode.GetComponent<NodeObject>().setAnimation("Idle");
            newNode.GetComponent<NodeObject>().setPosition(mousePos.x, mousePos.y);

            idleNode.SetID(nodeCounter);

            newNode.transform.parent = _parent.transform;
            newNode.name = "Node" + nodeCounter;
            newNode.tag = "Node";
            nodeID.Add(nodeCounter);
            allGO.Add(newNode);
            nodeCounter++;

           
        }

        else if(clb.Equals("walkNode"))
        {
            WalkNode walkNode = new WalkNode();
            walkNode.windowRect = new Rect(mousePos.x, mousePos.y, _windowWidth, _windowHeight);

            windows.Add(walkNode);

            GameObject newNode = new GameObject();

                newNode.AddComponent<NodeObject>();
                newNode.GetComponent<NodeObject>().setNodeID(nodeCounter);
                newNode.GetComponent<NodeObject>().setTitle(walkNode.windowTitle);
                newNode.GetComponent<NodeObject>().setAnimation("Walk");
            newNode.GetComponent<NodeObject>().setPosition(mousePos.x, mousePos.y);

            walkNode.SetID(nodeCounter);

            newNode.transform.parent = _parent.transform;
            newNode.name = "Node" + nodeCounter;
            newNode.tag = "Node";
            nodeID.Add(nodeCounter);
            allGO.Add(newNode);
            nodeCounter++;

        }

        else if(clb.Equals("runNode"))
        {
            RunNode runNode = new RunNode();
            runNode.windowRect = new Rect(mousePos.x, mousePos.y, _windowWidth, _windowHeight);

            windows.Add(runNode);

            GameObject newNode = new GameObject();

                newNode.AddComponent<NodeObject>();
                newNode.GetComponent<NodeObject>().setNodeID(nodeCounter);
                newNode.GetComponent<NodeObject>().setTitle(runNode.windowTitle);
                newNode.GetComponent<NodeObject>().setAnimation("Run");
            newNode.GetComponent<NodeObject>().setPosition(mousePos.x, mousePos.y);

            runNode.SetID(nodeCounter);

            newNode.transform.parent = _parent.transform;
            newNode.name = "Node" + nodeCounter;
            newNode.tag = "Node";
            nodeID.Add(nodeCounter);
            allGO.Add(newNode);
            nodeCounter++;

         
        }

        else if(clb.Equals("combatIdleNode"))
        {
            CombatIdleNode combatIdleNode = new CombatIdleNode();
            combatIdleNode.windowRect = new Rect(mousePos.x, mousePos.y, _windowWidth, _windowHeight);

            windows.Add(combatIdleNode);

            GameObject newNode = new GameObject();


                newNode.AddComponent<NodeObject>();
                newNode.GetComponent<NodeObject>().setNodeID(nodeCounter);
                newNode.GetComponent<NodeObject>().setTitle(combatIdleNode.windowTitle);
                newNode.GetComponent<NodeObject>().setAnimation("CombatIdle");
            newNode.GetComponent<NodeObject>().setPosition(mousePos.x, mousePos.y);

            combatIdleNode.SetID(nodeCounter);
                

            newNode.transform.parent = _parent.transform;
            newNode.name = "Node" + nodeCounter;
            newNode.tag = "Node";
            nodeID.Add(nodeCounter);
            allGO.Add(newNode);
            nodeCounter++;

           
        }

        else if(clb.Equals("customNode"))
        {
            CustomNode customNode = new CustomNode();
            customNode.windowRect = new Rect(mousePos.x, mousePos.y, _windowWidth, _windowHeight + 100);

            windows.Add(customNode);

            GameObject newNode = new GameObject();

            newNode.AddComponent<NodeObject>();
            newNode.GetComponent<NodeObject>().setNodeID(nodeCounter);
            newNode.GetComponent<NodeObject>().setTitle(customNode.windowTitle);
            newNode.GetComponent<NodeObject>().setAnimation("CustomNode");
            newNode.GetComponent<NodeObject>().setPosition(mousePos.x, mousePos.y);

            customNode.SetID(nodeCounter);


            newNode.transform.parent = _parent.transform;
            newNode.name = "Node" + nodeCounter;
            newNode.tag = "Node";
            nodeID.Add(nodeCounter);
            allGO.Add(newNode);
            nodeCounter++;
        }

        else if (clb.Equals("rangedNode"))
        {
            RangedNode rangedNode = new RangedNode();
            rangedNode.windowRect = new Rect(mousePos.x, mousePos.y, _windowWidth, _windowHeight);

            windows.Add(rangedNode);

            GameObject newNode = new GameObject();

            newNode.AddComponent<NodeObject>();
            newNode.GetComponent<NodeObject>().setNodeID(nodeCounter);
            newNode.GetComponent<NodeObject>().setTitle(rangedNode.windowTitle);
            newNode.GetComponent<NodeObject>().setAnimation("Ranged");
            newNode.GetComponent<NodeObject>().setPosition(mousePos.x, mousePos.y);

            rangedNode.SetID(nodeCounter);


            newNode.transform.parent = _parent.transform;
            newNode.name = "Node" + nodeCounter;
            newNode.tag = "Node";
            nodeID.Add(nodeCounter);
            allGO.Add(newNode);
            nodeCounter++;

        }

        else if(clb.Equals("makeTransition"))
        {
            bool clickedOnWindow = false;
            int selectIndex = -1;

            for (int i = 0; i < windows.Count; i++)
            {
                if (windows[i].windowRect.Contains(mousePos))
                {

                    selectIndex = i;
                    clickedOnWindow = true;
                    break;
                }
            }
            if(clickedOnWindow)
            {
                selectedNode = windows[selectIndex];
                makeTransitionMode = true;
            }
        }

        else if(clb.Equals("deleteNode"))
        {
            bool clickedOnWindow = false;
            int selectIndex = -1;

            for (int i = 0; i < windows.Count; i++)
            {
                if (windows[i].windowRect.Contains(mousePos))
                {

                    selectIndex = i;
                    clickedOnWindow = true;
                    break;
                }
            }

            if(clickedOnWindow)
            {

                
                BaseNode selNode = windows[selectIndex];
                DestroyImmediate(allGO[selectIndex].gameObject);
                windows.RemoveAt(selectIndex);

                nodeID.RemoveAt(selectIndex);

                
                allGO.RemoveAt(selectIndex);

               

                foreach(BaseNode n in windows)
                {
                    n.NodeDeleted(selNode);
                }
            }
        }
    }
 
    public static void DrawNodeCurve(Rect start, Rect end)
    {

        Vector3 startPos = new Vector3(start.x + start.width / 2, start.y + start.height / 2, 0);
        Vector3 endPos = new Vector3(end.x + end.width / 2, end.y + end.height / 2, 0);

        Vector3 startTan = startPos + Vector3.right * 50;
        Vector3 endTan = endPos + Vector3.left * 50;
        Color shadowCol = new Color(0, 0, 0, .06f);


        for (int i = 0; i < 3; i++)
        {
            Handles.DrawBezier(startPos, endPos, startTan, endTan, shadowCol, null, (i + 1) * 5);
        }

        Handles.DrawBezier(startPos, endPos, startTan, endTan, Color.black, null, 1);
        

    }

    
}
