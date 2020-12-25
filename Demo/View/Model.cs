using Godot;
using Hopper.Controller;
using Hopper.Utils.Vector;
using System.Collections.Generic;
using System.Linq;

namespace Hopper.View
{
    public class Model<T> : IModel<T> where T : SceneEnt, new()
    {
        private Node2D m_node;
        private List<IViewSieve> m_sieves;
        public IReadOnlyList<ISieve> Sieves => m_sieves;


        public Model(Node2D prefab, params IViewSieve[] sieves)
        {
            m_node = prefab;
            m_sieves = sieves.ToList();
            m_sieves.Sort((a, b) => a.Weight - b.Weight);
        }

        public T Instantiate(IntVector2 pos, IntVector2 orientation)
        {
            var obj = (Node2D)m_node.Duplicate();
            m_node.GetParent().AddChild(obj);
            obj.Visible = true;
            var scent = new T();
            scent.m_node = obj;
            scent.SetInitialPosition(pos);
            scent.SetInitialOrientation(orientation);
            return scent;
        }
    }
}