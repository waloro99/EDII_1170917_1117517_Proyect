using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_ED2.Class
{

    public class TreeBANode<T> : IEquatable<TreeBANode<T>>
    {
        public T[] keys;
        public TreeBANode<T>[] child = null;
        public int Nodemax;
        public T Element { get; set; }

        //builder
        public TreeBANode(int m)
        {
            keys = new T[m];
            for (int x = 0; x < m; x++)
                keys[x] = keys[x];
            Nodemax = m;
            initializeChild();
        }


        public bool Equals(TreeBANode<T> other)
        {
            return this.keys.Equals(other.keys);
        }

        public TreeBANode(T element, int m)
        {
            keys = new T[m];
            for (int x = 0; x < m; x++)
                keys[x] = keys[x];
            Nodemax = m;
            initializeChild();

            Element = element;
            Nodemax = m;
            keys[0] = element;
            for (int x = 1; x < m; x++)
                keys[x] = keys[x];
            initializeChild();
        }

        public virtual void initializeChild()
        {
            if (child == null)
                child = new TreeBANode<T>[Nodemax];

            for (int x = 0; x < Nodemax; x++)
                child[x] = null;
        }


    }

}
