using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_ED2.Class
{
    public class Node<T> : IEquatable<Node<T>>
    {
        public Node<T> next = null;
        public TreeBANode<T> elem;

        public Node(TreeBANode<T> node)
        {
            elem = node;
        }

        public bool Equals(Node<T> other)
        {
            return this.next.Equals(other.next);
        }
    }
}
