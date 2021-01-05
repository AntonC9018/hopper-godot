using Godot;

namespace Hopper
{
    public class Nodes
    {
        public Node2D player;
        public Node2D enemy;
        public Node2D wall;
        public Node2D tile;
        public Node2D chest;
        public Node2D droppedItem;
        public Node2D bomb;
        public Node2D explosion;
        public Node2D water;
        public Node2D iceCube;
        public Node2D iceFloor;
        public Node2D bounceTrap;
        public Node2D barrier;
        public Node2D knipper;
        public Node2D testBoss;
        public Node2D whelp;
        public Node2D laserBeamHead;
        public Node2D laserBeamBody;
        public Node2D danger;
        public Node2D _default;

        private Node parent;

        public Node2D GetNode(string str)
        {
            return parent.GetNode<Node2D>(new NodePath(str));
        }

        public Nodes(Node parent)
        {
            this.parent = parent;

            player = GetNode("candace4");
            enemy = GetNode("enemy");
            wall = GetNode("crate");
            tile = GetNode("tile");
            chest = GetNode("chest");
            droppedItem = GetNode("droppedtestitem");
            bomb = GetNode("bomb");
            explosion = GetNode("explosion");
            water = GetNode("WaterTile");
            iceCube = GetNode("icecube");
            iceFloor = GetNode("icefloor");
            bounceTrap = GetNode("trap");
            barrier = GetNode("side_power_wall");
            knipper = GetNode("knipper");
            testBoss = GetNode("legot");
            whelp = GetNode("whelp");
            laserBeamHead = GetNode("beam");
            laserBeamBody = GetNode("beam");
            danger = GetNode("danger");
            _default = GetNode("default");
        }

    }
}