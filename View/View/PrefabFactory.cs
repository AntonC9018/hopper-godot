﻿using Godot;
using System.Collections.Generic;

namespace Hopper.View
{
    public static class PrefabFactory
    {
        public static Dictionary<Object, Node2D> PrefabList;
        
        public static Node2D GeneratePrefab (int id)
        {
            // generate main node with generic event receiver
            var root = new Node2D();

            // additional stuff (animators, sprites, etc)
            // link the stuff
            // add the stuff in the prefab list

            return root;
        }
    }
}