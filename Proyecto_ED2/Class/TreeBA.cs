using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nancy.Json;


namespace Proyecto_ED2.Class
{

    public class TreeBA<T> where T : IComparable
    {

        internal TreeBANode<T> root;
        internal int maxNodeSize;
        internal int rootSize;
        public static List<T> st = new List<T>();
        private List<object> TreeDisk = new List<object>();
        JavaScriptSerializer Serializer = new JavaScriptSerializer();
        public string Path { get; set; }

        //Builder
        public TreeBA(int m)
        {
            this.root = null;
            this.maxNodeSize = m - 1;
            this.rootSize = (int)(2 * (Math.Floor((double)(2 * m - 1) / 3)) + 1);
        }

        //Method for Insert into the node
        public virtual bool Insert(TreeBANode<T> node, T element)
        {
            //T[] elementList = node.keys;
            List<T> elementList = new List<T>();
            int r = 0;
            while (node.keys[r] != null)
            {
                elementList.Add(node.keys[r]);
                r++;
            }
            int numElements;
            T[] minimalList;

            if (node == root)
            {
                minimalList = new T[elementList.Count() + 1];
                numElements = rootSize;
            }
            else
            {
                minimalList = new T[elementList.Count() + 1];
                numElements = maxNodeSize;
            }

            for (int i = 0; i < elementList.Count(); i++)
            {
                minimalList[i] = elementList[i];
            }

            minimalList[minimalList.Length - 1] = element;
            Array.Sort(minimalList);
            node.keys = new T[numElements];
            for (int x = 0; x < numElements; x++)
            {
                if (x < minimalList.Length)
                {
                    node.keys[x] = minimalList[x];
                }
                else
                {
                    node.keys[x] = default(T);
                }
            }

            return true;
        }

        //Method for find Parent in the tree
        public virtual TreeBANode<T> FParent(TreeBANode<T> searchNode)
        {
            return FParent(searchNode, root);
        }

        //Method for Find parent in the tree
        public virtual TreeBANode<T> FParent(TreeBANode<T> searchNode, TreeBANode<T> @base)
        {
            if (searchNode == null || @base == null || searchNode == @base)
            {
                return null;
            }

            bool isChild = false;

            for (int x = 0; x < @base.child.Length; x++)
            {
                if (@base.child[x] == searchNode)
                {
                    isChild = true;
                }
            }

            if (!isChild)
            {
                for (int x = 0; x < @base.child.Length; x++)
                {
                    TreeBANode<T> possibleParent = FParent(searchNode, @base.child[x]);
                    if (possibleParent != null)
                    {
                        return possibleParent;
                    }
                }
                return null;
            }
            else
            {
                return @base;
            }
        }

        public virtual void RDistribute(TreeBANode<T> node)
        {
            int numNodes = 0; //ConvertNodeToInt(node).length;
            int minimumSize = (int)(Math.Floor((double)(maxNodeSize * 2 - 1) / 3));
            bool hasMoved;
            do
            {
                //System.out.println("Iteration");
                hasMoved = false;
                for (int x = 0; x < node.child.Length; x++)
                {
                    if (maxNodeSize - SpacesLNode(node.child[x]) < minimumSize)
                    {
                        T newRoot = ConvertNodeToInt(node.child[x - 1])[ConvertNodeToInt(node.child[x]).Length - 1];
                        T oldRoot = ConvertNodeToInt(node)[x - 1];
                        DFNode(node.child[x - 1], ConvertNodeToInt(node.child[x - 1])[ConvertNodeToInt(node.child[x - 1]).Length - 1]);
                        DFNode(node, oldRoot);
                        Insert(node, newRoot);
                        Insert(node.child[x], oldRoot);
                    }
                }
            } while (hasMoved == true);
        }

        //public virtual bool DFNode(TreeBANode<T> node, T elementt)
        //{
        //  return (DFNode(node, (elementt)));
        //}

        public virtual bool DFNode(TreeBANode<T> node, T element)
        {
            T[] elementList = ConvertNodeToInt(node);
            int numElements;
            T[] minimalList;

            if (node == root)
            {
                minimalList = new T[rootSize - SpacesLNode(node) - 1];
                numElements = rootSize;
            }
            else
            {
                minimalList = new T[maxNodeSize - SpacesLNode(node) - 1];
                numElements = maxNodeSize;
            }
            int i = 0;
            int j = 0;
            while (j < elementList.Length && i < minimalList.Length)
            {
                if (!elementList[j].Equals(element))
                {
                    minimalList[i] = elementList[j];
                    i++;
                }
                j++;
            }

            Array.Sort(minimalList);

            return true;
        }

        //public virtual bool ElmentI(T element)
        //{
        //  return ElmentI((element));
        //}

        public virtual TreeBANode<T> FTInsert(T element)
        {
            int rootReferences = 0;
            if (root.child != null)
            {
                for (int x = 0; x < root.child.Length; x++)
                {
                    if (root.child[x] != null)
                    {
                        rootReferences++;
                    }
                }
            }

            if (SpacesLNode(root) > 0 && rootReferences == 0)
            {
                return root;
            }
            else
            {
                return FTInsert(element, root);
            }
        }

        public virtual TreeBANode<T> FTInsert(T element, TreeBANode<T> node)
        {
            if (node != null)
            {
                int i = 1;
                for (; i <= ConvertNodeToInt(node).Length && ConvertNodeToInt(node)[i - 1].CompareTo(element) < 0; i++)
                    if ((i > ConvertNodeToInt(node).Length || ConvertNodeToInt(node)[i - 1].CompareTo(element) > 0) && node.child != null && node.child.Length >= i)
                    {
                        if (node.child != null && node.child[i - 1] != null)
                        {
                            return FTInsert(element, node.child[i - 1]);
                        }
                        else
                        {
                            return node;
                        }
                    }
                    else
                    {
                        return node;
                    }
            }
            else
            {
                return null;
            }
            return node; //ver
        }

        public virtual bool ElmentI(T element)
        {
            if (root == null)
            {
                root = new TreeBANode<T>(element, rootSize);
                root.initializeChild();
                return true;
            }
            else
            {
                TreeBANode<T> nodeToInsertInto = FTInsert(element);
                if (SpacesLNode(nodeToInsertInto) == 0)
                {
                    if (FTInsert(element) == root)
                    {
                        return SRInsert(element);
                    }
                    else
                    {
                        if (SHSpace(nodeToInsertInto))
                        {
                            return ReOElementI(element, nodeToInsertInto);
                        }
                        else
                        {
                            return SNInsert(FTInsert(element), element);
                        }
                    }
                }
                else
                {
                    return Insert(FTInsert(element), element);
                }
            }
        }

        public virtual bool ReOElementI(T element, TreeBANode<T> node)
        {
            TreeBANode<T> parent = FParent(node);
            int x = 0;
            for (; x < parent.child.Length && parent.child[x] != node; x++)
            {
                ;
            }
            if (x > 0 && SpacesLNode(parent.child[x - 1]) > 0)
            {
                T shiftKey = ConvertNodeToInt(node)[0];
                T temp = ConvertNodeToInt(parent)[x - 1];
                DFNode(parent, temp);
                DFNode(node, ConvertNodeToInt(node)[0]);
                Insert(parent, shiftKey);
                Insert(node, element);
                Insert(parent.child[x - 1], temp);
            }
            else if (x < maxNodeSize - 1 && SpacesLNode(parent.child[x + 1]) > 0)
            {

            }
            return true;
        }

        public virtual bool SHSpace(TreeBANode<T> node)
        {
            int x = 0;
            TreeBANode<T> parent = FParent(node);
            for (; x < parent.child.Length && parent.child[x] != node; x++)
            {
                ;
            }


            if (x == parent.child.Length)
            {
                return false;
            }

            if (x == 0)
            {
                if (parent.child[1] != null)
                {
                    return (SpacesLNode(parent.child[1]) > 0);
                }
            }
            else if (x == parent.child.Length - 1 || parent.child[x + 1] == null)
            {

                if (parent.child[x - 1] != null)
                {
                    return (SpacesLNode(parent.child[x - 1]) > 0);
                }
            }
            else
            {
                if (parent.child[x - 1] != null && parent.child[x + 1] != null)
                {
                    return (SpacesLNode(parent.child[x - 1]) > 0 || SpacesLNode(parent.child[x + 1]) > 0);
                }
            }
            return false;
        }

        //public virtual bool DElement(T element)
        //{
        //  return DElement(new int?(element));
        //}

        public virtual bool DElement(T element)
        {
            TreeBANode<T> node = BASerch(element);

            if (node == null)
            {
                return false;
            }

            int minimumSize = (int)(Math.Floor((double)(maxNodeSize * 2 - 1) / 3));

            bool haschild = false;
            if (node.child != null)
            {
                for (int x = 0; x < node.child.Length; x++)
                {
                    if (node.child[x] != null)
                    {
                        haschild = true;
                    }
                }
            }

            if (node != null && node != root)
            {
                if (haschild == false && ConvertNodeToInt(node).Length > minimumSize)
                {
                    DFNode(node, element);
                }
                else
                {
                    if (SHSpare(node))
                    {
                        return ReODelete(element, node);
                    }
                    else
                    {
                        return MDelete(element, node);
                    }
                }
            }
            return false;
        }

        public virtual bool ReODelete(T element, TreeBANode<T> node)
        {
            int minimumSize = (int)(Math.Floor((double)(maxNodeSize * 2 - 1) / 3));
            TreeBANode<T> parent = FParent(node);
            int x = 0;
            for (; x < parent.child.Length && parent.child[x] != node; x++)
            {
                ;
            }

            if (x > 0 && CountSpacesFill(parent.child[x - 1]) > minimumSize)
            {
                T shiftKey = ConvertNodeToInt(node)[0];
                T temp = ConvertNodeToInt(parent)[x - 1];

                DFNode(parent, temp);

                DFNode(node, ConvertNodeToInt(node)[0]);

                Insert(parent, shiftKey);
                Insert(node, element);
                Insert(parent.child[x - 1], temp);
            }
            else if (x < maxNodeSize - 1 && CountSpacesFill(parent.child[x + 1]) > 0)
            {
                T shiftKey = ConvertNodeToInt(parent.child[x + 1])[0];
                T temp = ConvertNodeToInt(parent)[x];

                DFNode(parent, temp);
                DFNode(parent.child[x + 1], shiftKey);
                DFNode(node, ConvertNodeToInt(node)[x]);

                Insert(parent, shiftKey);
                Insert(node, temp);
            }
            return true;
        }
        public virtual bool MDelete(T element, TreeBANode<T> node)
        {


            TreeBANode<T> parent = FParent(node);

            int numChild = -1;

            for (int x = 0; x < parent.child.Length; x++)
            {
                if (parent.child[x] == node)
                {
                    numChild = x;
                }
            }

            if (numChild == -1)
            {
                return false;
            }

            DFNode(node, element);
            T[] elementList = new T[maxNodeSize];
            T[] nodeContents = ConvertNodeToInt(node);

            for (int x = 0; x < nodeContents.Length; x++)
            {
                elementList[x] = nodeContents[x];
            }

            int counter = 0;
            TreeBANode<T> sibling = null;
            TreeBANode<T> newNode = new TreeBANode<T>(maxNodeSize);
            int? newParent;

            if (numChild == 0)
            {
                sibling = parent.child[numChild + 1];
            }
            else if (numChild == maxNodeSize - 1)
            {
                sibling = parent.child[numChild - 1];
            }
            else if (numChild > 0 && SpacesLNode(parent.child[numChild - 1]) >= ConvertNodeToInt(node).Length)
            {
                sibling = parent.child[numChild - 1];
            }
            else if (numChild < maxNodeSize - 1 && SpacesLNode(parent.child[numChild + 1]) >= ConvertNodeToInt(node).Length)
            {
                sibling = parent.child[numChild - 1];
            }

            for (int x = nodeContents.Length; x < maxNodeSize; x++)
            {
                elementList[x] = ConvertNodeToInt(sibling)[counter];
                counter++;

            }
            Array.Sort(elementList);
            for (int x = 0; x < elementList.Length; x++)
            {
                Insert(newNode, elementList[x]);
            }

            if (sibling == parent.child[numChild + 1])
            {
                parent.child[numChild] = newNode;
                T temp = ConvertNodeToInt(parent)[numChild];
                DFNode(parent, ConvertNodeToInt(parent)[numChild]);

                Insert(newNode, temp);

            }
            else
            {
                parent.child[numChild - 1] = newNode;
                T temp = ConvertNodeToInt(parent)[numChild];
                DFNode(parent, ConvertNodeToInt(parent)[numChild - 1]);
                Insert(newNode, temp);
            }
            return true;
        }

        public virtual TreeBANode<T> BASerch(T key)
        {
            return BASerch(key, root);
        }

        public virtual bool SHSpare(TreeBANode<T> node)
        {
            int x = 0;
            int minSize = (int)(Math.Floor((double)(maxNodeSize * 2 - 1) / 3));
            TreeBANode<T> parent = FParent(node);
            for (; x < parent.child.Length && parent.child[x] != node; x++)
            {
                ;
            }


            if (x == parent.child.Length)
            {
                return false;
            }

            if (x == 0)
            {
                if (parent.child[1] != null)
                {
                    return (CountSpacesFill(parent.child[1]) > minSize);
                }
            }
            else if (x == parent.child.Length - 1 || parent.child[x + 1] == null)
            {
                if (parent.child[x - 1] != null)
                {
                    return (CountSpacesFill(parent.child[x - 1]) > minSize);
                }
            }
            else
            {
                if (parent.child[x - 1] != null && parent.child[x + 1] != null)
                {
                    return (CountSpacesFill(parent.child[x - 1]) > minSize || CountSpacesFill(parent.child[x + 1]) > minSize);
                }
            }
            return false;
        }

        public virtual TreeBANode<T> BASerch(T key, TreeBANode<T> node)
        {
            if (node != null)
            {
                int i = 1;
                for (; i <= ConvertNodeToInt(node).Length && ConvertNodeToInt(node)[i - 1].CompareTo(key) < 0; i++)
                {
                    ;
                }
                if (i > ConvertNodeToInt(node).Length || ConvertNodeToInt(node)[i - 1].CompareTo(key) > 0)
                {
                    return BASerch(key, node.child[i - 1]);
                }
                else
                {
                    return node;
                }
            }
            else
            {
                return null;
            }
        }

        public virtual string search(T element)
        {
            return search((element));
        }

        //public virtual string search(T element)
        //{
        //  return search(element, root);
        //}
        public virtual T[] search(T element, TreeBANode<T> node)
        {
            if (node != null)
            {

                int i = 1;
                for (; i <= ConvertNodeToInt(node).Length && element.CompareTo(ConvertNodeToInt(node)[i - 1]) > 0; i++) ;

                if (i > ConvertNodeToInt(node).Length || ConvertNodeToInt(node)[i - 1].CompareTo(element) > 0)
                {
                    if (node.child != null && node.child.Length > 0 && i < node.child.Length - 1)
                    {
                        return search(element, node.child[i - 1]);
                    }
                    else
                    {
                        return node.keys;
                    }
                }
                else
                {
                    return node.keys;
                }
            }
            else
            {
                return node.keys;
            }
        }

        public virtual int height()
        {
            return height(root);
        }

        public virtual int height(TreeBANode<T> node)
        {
            if (node == null)
            {
                return 0;
            }

            if (node.child == null)
            {
                return 1;
            }

            int[] heights = new int[node.child.Length];

            for (int x = 0; x < node.child.Length; x++)
            {
                heights[x] = height(node.child[x]);
            }
            int maxHeight = -1;
            for (int x = 0; x < node.child.Length; x++)
            {
                if (heights[x] > maxHeight)
                {
                    maxHeight = heights[x];
                }
            }
            return 1 + maxHeight;
        }

        public virtual int fullness()
        {
            if (root == null)
            {
                return 0;
            }

            return (int)((double)(CountSpacesFill()) / CountSpaceTree() * 100);
        }

        public virtual string breadthFirst()
        {
            string treeString = "";

            Cola<T> queue = new Cola<T>();

            Node<T> nodePointer = new Node<T>(root);

            if (nodePointer != null)
            {
                queue.enqueue(nodePointer);
                while (!queue.Empty)
                {
                    nodePointer = queue.dequeue();
                    Console.WriteLine(nodePointer.elem.keys);
                    treeString += nodePointer.elem.keys;
                    if (nodePointer.elem.child != null)
                    {
                        for (int x = 0; x < nodePointer.elem.child.Length; x++)
                        {
                            if (nodePointer.elem.child[x] != null)
                            {
                                queue.enqueue(nodePointer.elem.child[x]);
                            }
                        }
                    }
                }
            }
            return treeString;
        }

        public virtual string TreeString
        {
            get
            {
                return getTreeString(root);
            }
        }

        public virtual string getTreeString(TreeBANode<T> node)
        {
            if (node == null)
            {
                return "";
            }

            string nodeString = "";

            for (int x = 0; x < ConvertNodeToInt(node).Length; x++)
            {
                nodeString += getTreeString(node.child[x]);
                nodeString += ConvertNodeToInt(node)[x];
            }
            nodeString += TreeString;
            return nodeString;
        }

        public virtual bool SRInsert(T element)
        {
            if (root == null)
            {
                return false;
            }

            T[] rootArray = new T[ConvertNodeToInt(root).Length + 1];
            T[] currentElements = ConvertNodeToInt(root);

            for (int x = 0; x < currentElements.Length; x++)
            {
                rootArray[x] = currentElements[x];
            }
            rootArray[rootArray.Length - 1] = element;
            Array.Sort(rootArray);

            int numLeft = (int)(Math.Ceiling((double)(currentElements.Length) / 2));

            TreeBANode<T> leftNode = new TreeBANode<T>(maxNodeSize);
            TreeBANode<T> rightNode = new TreeBANode<T>(maxNodeSize);

            for (int x = 0; x < numLeft; x++)
            {
                Insert(leftNode, rootArray[x]);
            }

            //string nullString = "";
            for (int x = 0; x < rootSize; x++)
            {
                //nullString += "[]";
                root.keys[x] = default(T);
            }

            //root.keys = null;

            Insert(root, rootArray[numLeft]);

            for (int x = numLeft + 1; x < rootArray.Length; x++)
            {
                Insert(rightNode, rootArray[x]);
            }
            root.child[0] = leftNode;
            root.child[1] = rightNode;
            return true;
        }
        public virtual bool SNInsert(TreeBANode<T> node, T element)
        {
            TreeBANode<T> parent = FParent(node);

            int numChild = -1;

            for (int x = 0; x < parent.child.Length; x++)
            {
                if (parent.child[x] == node)
                {
                    numChild = x;
                }
            }

            if (numChild == -1)
            {
                return false;
            }
            //System.out.println("Parent is " + ConvertNodeToInt(parent)[numChild-1]);
            T[] elementList = new T[maxNodeSize + 1];
            T[] nodeContents = ConvertNodeToInt(node);

            for (int x = 0; x < maxNodeSize; x++)
            {
                elementList[x] = nodeContents[x];
            }
            elementList[elementList.Length - 1] = element;
            Array.Sort(elementList);

            TreeBANode<T> newNodeLeft = new TreeBANode<T>(maxNodeSize);
            TreeBANode<T> newNodeRight = new TreeBANode<T>(maxNodeSize);
            int splitter = (int)(Math.Floor((2 * (maxNodeSize + 1) - 1) / (double)(3)));
            Insert(parent, elementList[splitter]);

            for (int x = 0; x < splitter; x++)
            {
                Insert(newNodeLeft, elementList[x]);
            }
            for (int x = splitter + 1; x < elementList.Length; x++)
            {
                Insert(newNodeRight, elementList[x]);
            }
            //System.out.println("Num child " + numChild);

            parent.child[numChild] = newNodeLeft;
            if (numChild + 1 < parent.child.Length)
            {
                parent.child[numChild + 1] = newNodeRight;
            }
            //RDistribute(parent);
            return true;
        }

        public virtual int SpacesLNode(TreeBANode<T> node)
        {
            if (node != null)
            {
                int contador = 0;

                for (int i = 0; i < node.keys.Length; i++)
                {
                    if (node.keys[i] != null)
                    {
                        contador++;
                    }
                }
                if (contador == node.keys.Length)
                {
                    return 0;
                }
                else
                {
                    return node.keys.Length - 1;
                }

            }
            else
            {
                return 0;
            }
        }

        public virtual int CountSpaceTree()
        {
            return CountSpaceTree(root);
        }
        public virtual int CountSpaceTree(TreeBANode<T> node)
        {
            int currentNodeSpaces = node.keys.Length - 1;
            int childNodeSpaces = 0;
            if (node.child != null && node.child.Length > 0)
            {
                for (int i = 0; i < maxNodeSize; i++)
                {
                    if (node.child[i] != null)
                    {
                        childNodeSpaces += (int)(node.child[i].keys.Length - 1) / 2;
                    }
                }
            }
            return currentNodeSpaces + childNodeSpaces;
        }

        public virtual int CountSpacesFill()
        {
            return CountSpacesFill(root);
        }
        public virtual int CountSpacesFill(TreeBANode<T> node)
        {
            if (node == null)
            {
                return 0;
            }
            int spacesFilled = 0;
            int childSpacesFilled = 0;
            if (node == root)
            {
                spacesFilled = rootSize - SpacesLNode(node);
            }
            else
            {
                spacesFilled = maxNodeSize - SpacesLNode(node);
            }

            if (node.child != null && node.child.Length > 0)
            {
                for (int i = 0; i < maxNodeSize; i++)
                {
                    if (node.child[i] != null)
                    {
                        childSpacesFilled += maxNodeSize - SpacesLNode(node.child[i]);
                    }
                }
            }
            return spacesFilled + childSpacesFilled;
        }

        //editar
        public virtual T[] ConvertNodeToInt(TreeBANode<T> node)
        {
            int i = 0;
            if (node == null)
            {
                return null;
            }
            T[] splitNum = node.keys;
            return splitNum;
            //splitNum = splitNum.Replace("[", "");
            //splitNum = splitNum.Replace("]", " ");

            //var st = new StreamTokenizer(splitNum);

            //T[] numbers = new T[st.Count()];
            //while (st.Count > 1)
            //{
            //  numbers[i] = splitNum[i];
            //  i++;
            //}
            //T[] valueArray = null;
            //int notNullCounter = 0;

            //for (int x = 0; x < numbers.Length; x++)
            //{
            //  if (!string.ReferenceEquals(numbers[x], " "))
            //  {
            //      notNullCounter++;
            //  }
            //}

            //valueArray = new T[notNullCounter];
            //for (int x = 0; x < valueArray.Length; x++)
            //{

            ////    valueArray[x] = numbers[x];

            //}

        }


        public void InsertDisk(T element)
        {
            TreeDisk.Add(element);
            string path = Path + "Example.txt"; //Path de creacion del arbol en disco
            if (!File.Exists(path))
            {
                using (var tw = new StreamWriter(path, true))
                {
                    tw.WriteLine(ToJSON(TreeDisk));
                }
            }
            else if (File.Exists(path))
            {
                TreeDisk.Clear();
                string arbol = ReadDisk(path);
                Object[] obj = (Object[])Serializer.Deserialize<Object>(arbol);

                for (int i = 0; i < obj.Length; i++)
                {
                    TreeDisk.Add(obj[i]);
                }

                TreeDisk.Add(element);
                File.Delete(path);
                using (var tw = new StreamWriter(path, true))
                {
                    tw.WriteLine(ToJSON(TreeDisk));
                }
            }
        }

        private string ReadDisk(string path)
        {

            string arbol = "";
            using (StreamReader reader = new StreamReader(path))
            {
                arbol = reader.ReadToEnd();
            }

            return arbol;
        }

        private string ToJSON(object obj)
        {
            return Serializer.Serialize(obj);
        }
    }

}
