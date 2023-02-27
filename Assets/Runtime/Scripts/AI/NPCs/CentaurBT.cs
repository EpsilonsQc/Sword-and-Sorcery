using System.Collections.Generic;
using RPG_Project.AI.BehaviourTree;
using RPG_Project.AI.Tasks;

namespace RPG_Project.AI.Monsters
{
    public class CentaurBT : Tree
    {
        protected override Node SetupTree()
        {
            Node root = new Selector(new List<Node>
            {
                new Sequence(new List<Node>
                {
                new Dead(this.transform),
                new CheckFOVRange(this.transform),
                new StopPatrol(this.transform),
                new FaceTarget(this.transform),
                }),

                new Sequence(new List<Node>
                {
                new Dead(this.transform),
                new Patrol(this.transform),
                }),
            });

            return root;
        }
    }
}