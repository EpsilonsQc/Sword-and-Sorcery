using UnityEngine;

namespace RPG_Project.AI.BehaviourTree
{
    public abstract class Tree : MonoBehaviour
    {
        private Node _root = null;

        protected void Awake()
        {
            _root = SetupTree();
        }

        private void Update()
        {
            if (_root != null)
            {
                _root.Evaluate();
            }
        }

        protected abstract Node SetupTree();
    }
}
