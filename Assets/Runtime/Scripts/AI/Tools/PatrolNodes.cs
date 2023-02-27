using System.Collections.Generic;
using System.Linq; 
using UnityEngine;
using UnityEditor;
using UnityEngine.AI;

namespace RPG_Project.AI.Tools
{
    [RequireComponent(typeof(NavMeshAgent))] // Require a NavMeshAgent component
    public class PatrolNodes : MonoBehaviour
    {
        // References
        [SerializeField] private NavMeshAgent navMeshAgent; // Reference to the NavMeshAgent component
        [SerializeField] private GameObject nodeContainer; // patrol point container

        // Nodes
        [SerializeField] private List<GameObject> nodes; // patrol nodes list
        [SerializeField] private List<string> nodesNames; // patrol point names list

        // Warp agent
        [SerializeField] private int warpAgentIndex; // index of the patrol point to warp the agent to
        [SerializeField] private Vector3 teleportAgentPosition; // teleport the agent to a custom position

        // Wait time between nodes
        [SerializeField] private bool normalWaitTime; // normal wait time between nodes
        [SerializeField] private float waitTime; // wait time (in seconds) at each nodes
        [SerializeField] private float waitTimer; // wait timer (countdown)
        [SerializeField] private bool randomWaitTime; // random wait time between nodes
        [SerializeField] private float minWaitTime; // minimum wait time
        [SerializeField] private float maxWaitTime; // maximum wait time

        // Move nodes
        [SerializeField] private int moveNodeIndex; // index of the patrol point to move

        // Loop through nodes
        [SerializeField] private int loopIndex; // loop index
        [SerializeField] private int loopCounter; // count loop repetitions
        [SerializeField] private bool loopCount; // loop count
        [SerializeField] private bool loop; // loop patrol nodes
        [SerializeField] private bool loopRandom; // loop "random" patrol nodes
        [SerializeField] private bool loopPingPong; // loop "pingpong" patrol nodes

        // Rename nodes
        [SerializeField] private bool renameNodes = true; // rename patrol nodes after delete

        // Pause patrol
        [SerializeField] private bool pausePatrol; // pause patrol

        public bool PausePatrol
        {
            get { return pausePatrol; }
            set { pausePatrol = value; }
        }

        private void Awake()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
        }

        private void Reset()
        {
            nodes = new List<GameObject>(); // initialize the nodes list
            nodesNames = new List<string>(); // initialize the nodesName list

            nodeContainer = GameObject.Find(gameObject.name + " [Patrol Nodes]"); // find the patrol point container
            DestroyImmediate(nodeContainer); // destroy the node container
        }

        private void Update()
        {
            SetPatrolType(); // set patrol type
        }

        private void AddNode()
        {
            // create node container if it doesn't exist
            if (nodeContainer == null)
            {
                nodeContainer = new GameObject(gameObject.name + " [Patrol Nodes]"); // create a new gameobject
                nodeContainer.transform.parent = transform.parent; // set the parent of the node container
            }

            nodes.Add(new GameObject("Position " + nodes.Count, typeof(PatrolNodesGizmo))); // add a new game object with a "PatrolNodesGizmo" component
            nodes.Last().transform.SetParent(nodeContainer.transform); // set the parent of the newly added node to the node container
            nodes.Last().transform.position = transform.position; // set the position of the newly added node to the position of the agent
            nodesNames.Add("Position " + nodesNames.Count); // add a new name to the nodesNames list
        }

        private void GroundAllNodes()
        {
            foreach (GameObject p in nodes)
            {
                if (NavMesh.SamplePosition(p.transform.position, out NavMeshHit hit, Mathf.Infinity, NavMesh.AllAreas))
                {
                    p.transform.position = hit.position; // set position of the node to the position of the hit
                }
            }
        }

        private void WarpAgentToNode(int index)
        {
            if(navMeshAgent == null)
            {
                navMeshAgent = GetComponent<NavMeshAgent>();
            }

            navMeshAgent.Warp(nodes[index].transform.position); // teleport the agent to a node
        }

        private void WarpAgentToCustomPos()
        {
            if (navMeshAgent == null)
            {
                navMeshAgent = GetComponent<NavMeshAgent>();
            }

            navMeshAgent.Warp(teleportAgentPosition); // teleport the agent to a position
        }

        private void CountLoop()
        {
            if(!Application.isPlaying) { return; } // early out if application is not playing

            if (loopCounter > 0 && navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance && !navMeshAgent.pathPending)
            {
                if(Wait())
                {
                    navMeshAgent.SetDestination(nodes[loopIndex].transform.position); // set the destination of the agent to the next node

                    if (loopIndex < nodes.Count - 1)
                    {
                        loopIndex++; // go to the next node
                    }
                    else // agent reached the last node
                    {
                        loopIndex = 0; // go to the first node
                        loopCounter--; // decrease the loop count by 1
                    }
                }
            }
            else if (loopCounter == 0 && navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance && !navMeshAgent.pathPending)
            {
                if(Wait())
                {
                    navMeshAgent.SetDestination(nodes[0].transform.position); // set the destination of the agent to the first node
                }
            }
        }

        private void Loop()
        {
            if(!Application.isPlaying) { return; } // early out if application is not playing

            if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance && !navMeshAgent.pathPending)
            {
                if(Wait())
                {
                    navMeshAgent.SetDestination(nodes[loopIndex].transform.position); // set the destination of the agent to the next node

                    if (loopIndex < nodes.Count - 1)
                    {
                        loopIndex++;
                    }
                    else
                    {
                        loopIndex = 0;
                    }
                }
            }
        }

        private void RandomLoop()
        {
            if(!Application.isPlaying) { return; } // early out if application is not playing

            if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance && !navMeshAgent.pathPending)
            {
                if(Wait())
                {
                    navMeshAgent.SetDestination(nodes[Random.Range(0, nodes.Count)].transform.position);
                }
            }
        }

        private void PingPongLoop()
        {
            if(!Application.isPlaying) { return; } // early out if application is not playing

            if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance && !navMeshAgent.pathPending)
            {
                if(Wait())
                {
                    navMeshAgent.SetDestination(nodes[loopIndex].transform.position); // set the destination of the agent to the next node

                    if (loopIndex == nodes.Count - 1)
                    {
                        nodes.Reverse(); // reverse the nodes list
                        nodesNames.Reverse(); // reverse the nodesNames list
                        loopIndex = 0; // set the loop index to 0
                    }
                    else
                    {
                        loopIndex++;
                    }
                }
            }
        }

        private void SetPatrolType()
        {
            if((!loopCount && !loop && !loopRandom && !loopPingPong) || pausePatrol)
            {
                return;
            }
            else
            {
                if (loopCount)
                {
                    CountLoop();
                }
                else if (loop && !loopRandom && !loopPingPong)
                {
                    Loop();
                }
                else if (loopRandom)
                {
                    RandomLoop();
                }
                else if (loopPingPong)
                {
                    PingPongLoop();
                }
            }
        }

        private bool Wait()
        {
            if(normalWaitTime || randomWaitTime)
            {
                if (waitTimer > 0)
                {
                    waitTimer -= Time.deltaTime; // countdown
                    return false;
                }
                else if(randomWaitTime)
                {
                    waitTimer = Random.Range(minWaitTime, maxWaitTime); // set the wait timer to a random value between minWaitTime and maxWaitTime
                    return true;
                }
                else
                {
                    waitTimer = waitTime; // reset wait timer
                    return true;
                }
            }
            else
            {
                return true;
            }
        }

        private void MoveNode(bool value, int index)
        {
            if(value)
            {
                nodes[index].transform.position = transform.position; // move selected node to the position of the agent
            }
        }

        private void MoveAllNodes(bool value)
        {
            if(value)
            {
                foreach (GameObject p in nodes)
                {
                    p.transform.position = transform.position; // move all nodes to the position of the agent
                }
            }
        }

        private void RenameAllNodes(bool value)
        {
            if(value)
            {
                // rename all nodes
                for (int i = 0; i < nodes.Count; i++)
                {
                    nodesNames[i] = "Position " + i;
                    nodes[i].name = "Position " + i;
                }
            }
        }

        private void RemoveNode(int index)
        {
            if (nodes[index] != null)
            {
                DestroyImmediate(nodes[index]); // destroy the node game object
                nodesNames.RemoveAt(index); // remove the name of the node from the list
                nodes.RemoveAt(index); // remove the node from the list

                if(nodes == null || nodes.Count == 0)
                {
                    DestroyImmediate(nodeContainer); // destroy the node container
                }
            }
        }

        private void RemoveAllNodes()
        {
            while (nodes.Count > 0)
            {
                RemoveNode(0); // remove all nodes
            }
        }

        [CustomEditor(typeof(PatrolNodes))]
        public class PatrolRoutesEditor : Editor
        {
            public override void OnInspectorGUI()
            {
                PatrolNodes patrolNodes = (PatrolNodes)target;

                #region PATROL_NODES_FOR_NAV_MESH_AGENT
                EditorGUILayout.LabelField("Patrol nodes for NavMeshAgent", EditorStyles.centeredGreyMiniLabel);

                // ADD NEW NODE BUTTON
                if (GUILayout.Button("Add a new node"))
                {
                    patrolNodes.AddNode(); // add a new node
                }

                // GROUND ALL NODES BUTTON
                if(GUILayout.Button("Ground all nodes"))
                {
                    patrolNodes.GroundAllNodes(); // ground all nodes
                }

                EditorGUILayout.Space(5);
                #endregion
                
                #region NODES_LIST
                // NODES LIST
                foreach (GameObject p in patrolNodes.nodes)
                {
                    // INDEX OF THE NODES IN THE LIST
                    int index = patrolNodes.nodes.IndexOf(p);
                    
                    EditorGUI.BeginChangeCheck();
                    EditorGUILayout.BeginHorizontal();

                    // NODE NAME
                    string nodeName = GUILayout.TextField(patrolNodes.nodesNames[index], GUILayout.MaxWidth(EditorGUIUtility.currentViewWidth - 212)); // display name of the node
                    
                    // NODE POSITION
                    Vector3 nodePosition = EditorGUILayout.Vector3Field("", patrolNodes.nodes[index].transform.position, GUILayout.MinWidth(EditorGUIUtility.currentViewWidth * 0.56f)); // display position of the node
    
                    // DELETE BUTTON
                    if (GUILayout.Button(new GUIContent("X", "Delete this node"), GUILayout.MaxWidth(50)))
                    {
                        patrolNodes.RemoveNode(index); // remove the node
                        patrolNodes.RenameAllNodes(patrolNodes.renameNodes); // rename all nodes if "renameNodes" is true
                        break; // prevent the loop to continue (avoid error when deleting a node)
                    }

                    EditorGUILayout.EndHorizontal();
                    if (EditorGUI.EndChangeCheck())
                    {
                        // set the name of the node
                        patrolNodes.nodesNames[index] = nodeName; 
                        p.name = nodeName;

                        // set the position of the node
                        patrolNodes.nodes[index].transform.position = nodePosition; 
                        p.transform.position = nodePosition;
                    }
                }
                #endregion

                #region WARP_AGENT_OPTIONS
                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
                EditorGUILayout.LabelField("Warp agent", EditorStyles.centeredGreyMiniLabel);

                // WARP AGENT
                EditorGUILayout.BeginHorizontal();
                if(GUILayout.Button("Warp agent to node at index [" + patrolNodes.warpAgentIndex + "]"))
                {
                    if(patrolNodes.warpAgentIndex > patrolNodes.nodes.Count - 1)
                    {
                        EditorUtility.DisplayDialog("Index out of range", "The index is out of range (max index is " + (patrolNodes.nodes.Count - 1) + ")." , "OK");
                    }
                    else
                    {
                        patrolNodes.WarpAgentToNode(patrolNodes.warpAgentIndex); // warp the agent to the selected node index
                    }
                }
                
                // WARP AGENT INDEX
                patrolNodes.warpAgentIndex = EditorGUILayout.IntField("", patrolNodes.warpAgentIndex);
                EditorGUILayout.EndHorizontal();

                // WARP AGENT TO CUSTOM POSITION BUTTON
                if(GUILayout.Button("Warp agent to custom position (" + patrolNodes.teleportAgentPosition.x + ", " + patrolNodes.teleportAgentPosition.y + ", " + patrolNodes.teleportAgentPosition.z + ")"))
                {
                    patrolNodes.WarpAgentToCustomPos(); // teleport the agent to the custom position
                }

                // CUSTOM POSITION
                patrolNodes.teleportAgentPosition = EditorGUILayout.Vector3Field("", patrolNodes.teleportAgentPosition);
                #endregion

                #region PATROL_TYPES_OPTIONS
                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
                EditorGUILayout.LabelField("Patrol type", EditorStyles.centeredGreyMiniLabel);

                // LOOP COUNT TOGGLE
                patrolNodes.loopCount = GUILayout.Toggle(patrolNodes.loopCount, "Loop count", "Button");

                if(patrolNodes.loopCount)
                {
                    patrolNodes.loop = false;
                    patrolNodes.loopCounter = EditorGUILayout.IntSlider(new GUIContent("Loop count", "The number of times the agent will loop through the patrol nodes."), patrolNodes.loopCounter, 0, 20);
                }

                // LOOP TOGGLE
                if(patrolNodes.loop = GUILayout.Toggle(patrolNodes.loop, "Loop", "Button"))
                {
                    patrolNodes.loopCount = false;

                    EditorGUILayout.BeginHorizontal();

                    // RANDOM LOOP TOGGLE
                    if (patrolNodes.loopRandom = GUILayout.Toggle(patrolNodes.loopRandom, "Random", "Button"))
                    {
                        patrolNodes.loopPingPong = false;
                    }
                    
                    // PINGPONG LOOP TOGGLE
                    if (patrolNodes.loopPingPong = GUILayout.Toggle(patrolNodes.loopPingPong, "PingPong", "Button"))
                    {
                        patrolNodes.loopRandom = false;
                    }

                    EditorGUILayout.EndHorizontal();
                }
                else
                {
                    patrolNodes.loop = false;
                    patrolNodes.loopRandom = false;
                    patrolNodes.loopPingPong = false;
                }
                #endregion

                #region TIMER_OPTIONS
                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
                EditorGUILayout.LabelField("Timer", EditorStyles.centeredGreyMiniLabel);

                // NORMAL TIMER TOGGLE
                if(patrolNodes.normalWaitTime = GUILayout.Toggle(patrolNodes.normalWaitTime, "Normal", "Button"))
                {
                    patrolNodes.randomWaitTime = false;

                    // WAIT TIME SLIDER
                    patrolNodes.waitTime = EditorGUILayout.IntSlider(new GUIContent("Wait time", "The wait time in second between each node."), (int)patrolNodes.waitTime, 0, 20);
                }

                // RANDOM WAIT TIME TOGGLE
                if(patrolNodes.randomWaitTime = GUILayout.Toggle(patrolNodes.randomWaitTime, "Random", "Button"))
                {
                    patrolNodes.normalWaitTime = false;

                    // MIN WAIT TIME SLIDER
                    patrolNodes.minWaitTime = EditorGUILayout.IntSlider(new GUIContent("Min wait time", "The minimum time in second the agent will wait at each node."), (int)patrolNodes.minWaitTime, 0, 20);

                    // MAX WAIT TIME SLIDER
                    patrolNodes.maxWaitTime = EditorGUILayout.IntSlider(new GUIContent("Max wait time", "The maximum time in second the agent will wait at each node."), (int)patrolNodes.maxWaitTime, 0, 20);
                }
                #endregion

                #region MOVE_NODES
                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
                EditorGUILayout.LabelField("Move nodes", EditorStyles.centeredGreyMiniLabel);

                // MOVE A NODE BUTTON
                EditorGUILayout.BeginHorizontal();
                if(GUILayout.Button("Move node at index [" + patrolNodes.moveNodeIndex + "] to agent"))
                {
                    if(patrolNodes.moveNodeIndex > patrolNodes.nodes.Count - 1)
                    {
                        EditorUtility.DisplayDialog("Index out of range", "The index is out of range (max index is " + (patrolNodes.nodes.Count - 1) + ")." , "OK");
                    }
                    else if (EditorUtility.DisplayDialog("Move selected node to agent", "Are you sure you want to move node [" + patrolNodes.moveNodeIndex + "] to agent location?", "Yes", "No"))
                    {
                        patrolNodes.MoveNode(true, patrolNodes.moveNodeIndex); // move all nodes
                    }
                }

                patrolNodes.moveNodeIndex = EditorGUILayout.IntField("", patrolNodes.moveNodeIndex);
                EditorGUILayout.EndHorizontal();

                // MOVE ALL NODES BUTTON
                if(GUILayout.Button("Move all nodes to agent"))
                {
                    if (EditorUtility.DisplayDialog("Move all nodes to agent", "Are you sure you want to move all nodes to agent location?", "Yes", "No"))
                    {
                        patrolNodes.MoveAllNodes(true); // move all nodes
                    }
                }
                #endregion

                #region NODES_OPTIONS
                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
                EditorGUILayout.LabelField("Nodes options", EditorStyles.centeredGreyMiniLabel);

                // RENAME AFTER DELETE TOGGLE
                patrolNodes.renameNodes = EditorGUILayout.Toggle(new GUIContent("Rename nodes after delete", "Rename all nodes each time you delete a node."), patrolNodes.renameNodes);

                // RENAME ALL NODES BUTTON
                if(GUILayout.Button("Rename all nodes"))
                {
                    if (EditorUtility.DisplayDialog("Rename all nodes", "Are you sure you want to rename all nodes?", "Yes", "No"))
                    {
                        patrolNodes.RenameAllNodes(true); // rename all nodes
                    }
                }

                // DELETE ALL NODES BUTTON
                if (GUILayout.Button("Delete all nodes"))
                {
                    if (EditorUtility.DisplayDialog("Delete all nodes", "Are you sure you want to delete all nodes?", "Yes", "No"))
                    {
                        patrolNodes.RemoveAllNodes(); // remove all nodes
                    }
                }

                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
                #endregion
            }
        }
    }
}
