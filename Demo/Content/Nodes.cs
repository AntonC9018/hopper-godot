namespace Hopper
{
    public class Nodes
    {
        public Godot.Node2D player;
        public Godot.Node2D enemy;
        public Godot.Node2D wall;
        public Godot.Node2D tile;
        public Godot.Node2D chest;
        public Godot.Node2D droppedItem;
        public Godot.Node2D bomb;
        public Godot.Node2D explosion;
        public Godot.Node2D water;
        public Godot.Node2D ice;
        public Godot.Node2D bounceTrap;
        public Godot.Node2D barrier;
        public Godot.Node2D knipper;
        public Godot.Node2D testBoss;
        public Godot.Node2D whelp;
        public Godot.Node2D laserBeamHead;
        public Godot.Node2D laserBeamBody;
        public Godot.Node2D _default;

        public Nodes(Godot.Node parent)
        {
            player = parent.GetNode<Godot.Node2D>(new Godot.NodePath("candace4"));
            enemy = parent.GetNode<Godot.Node2D>(new Godot.NodePath("enemy"));
            wall = parent.GetNode<Godot.Node2D>(new Godot.NodePath("crate"));
            tile = parent.GetNode<Godot.Node2D>(new Godot.NodePath("tile"));
            chest = parent.GetNode<Godot.Node2D>(new Godot.NodePath("chest"));
            droppedItem = parent.GetNode<Godot.Node2D>(new Godot.NodePath("droppedtestitem"));
            bomb = parent.GetNode<Godot.Node2D>(new Godot.NodePath("bomb"));
            explosion = parent.GetNode<Godot.Node2D>(new Godot.NodePath("explosion"));
            water = parent.GetNode<Godot.Node2D>(new Godot.NodePath("WaterTile"));
            ice = parent.GetNode<Godot.Node2D>(new Godot.NodePath("tile"));
            bounceTrap = parent.GetNode<Godot.Node2D>(new Godot.NodePath("trap"));
            barrier = parent.GetNode<Godot.Node2D>(new Godot.NodePath("side_power_wall"));
            knipper = parent.GetNode<Godot.Node2D>(new Godot.NodePath("knipper"));
            testBoss = parent.GetNode<Godot.Node2D>(new Godot.NodePath("legot"));
            whelp = parent.GetNode<Godot.Node2D>(new Godot.NodePath("whelp"));
            laserBeamHead = parent.GetNode<Godot.Node2D>(new Godot.NodePath("beam"));
            laserBeamBody = parent.GetNode<Godot.Node2D>(new Godot.NodePath("beam"));
            _default = parent.GetNode<Godot.Node2D>(new Godot.NodePath("default"));
        }

    }
}