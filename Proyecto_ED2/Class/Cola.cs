using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_ED2.Class
{

    public class Cola<T>
    {

        public Node<T> root = null;

        public virtual Node<T> dequeue()
        {
            Node<T> temp = root;

            if (root != null)
            {
                root = root.next;
            }

            return temp;
        }

        public virtual void enqueue(Node<T> node)
        {
            enqueue(node.elem);
        }

        public virtual void enqueue(TreeBANode<T> node)
        {
            if (root == null)
            {
                root = new Node<T>(node);
                return;
            }

            Node<T> nodePointer = root;
            while (nodePointer.next != null)
            {
                nodePointer = nodePointer.next;
            }
            nodePointer.next = new Node<T>(node);
        }

        public virtual bool Empty
        {
            get
            {
                return (root == null);
            }
        }


    }

}
