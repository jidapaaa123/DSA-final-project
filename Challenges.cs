namespace Realms;


public class Challenges
{
    private int lastTimeBalanced = 0;
    private List<int> builtInNumbers = new()
    {
        1, 2, 3, 4, 5, 6, 7, 8, 9,
        10, 15, 20, 25, 30, 35, 40, 45, 50,
        55, 65, 75, 85, 95, 105, 115, 125, 135,
        17, 27, 37, 47, 57, 67, 77, 87, 97,
    };
    public int Count { get; set; }

    public Node? Root;

    /// <summary>
    /// Built-in Binary Tree
    /// </summary>
    public Challenges()
    {
        Root = null;
        Count = 0;

        BuildTree();
    }

    private void ClearTree()
    {
        Root = null;
    }

    private void BuildTree()
    {
        foreach (var rootNum in builtInNumbers)
        {
            this.InsertNode(new Node(rootNum));
        }
    }

    private void BuildTree(List<Node> nodes)
    {
        foreach (var node in nodes)
        {
            this.InsertNode(node);
        }
    }

    /// <summary>
    /// Returns closest node
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public Node FindClosest(int key)
    {
        List<Node> options = new();
        InOrderTraversal(Root, options);

        if (options.Count == 0)
        {
            throw new Exception("You've exhausted all the challenges, and you succumb to the darkness");
        }

        Node closest = Root;
        int distance = Math.Abs(key - Root.Data);
        for (int i = 1; i < options.Count; i++)
        {
            var node = options[i];
            int newDistance = Math.Abs(key - node.Data);

            if (newDistance < distance)
            {
                distance = newDistance;
                closest = node;
            }
        }

        return closest;
    }

    public void InOrderTraversal(Node node, List<Node> traversed)
    {
        if (node == null)
        {
            return;
        }

        InOrderTraversal(node.Left, traversed);
        // Console.WriteLine(node.Data);
        traversed.Add(node);
        InOrderTraversal(node.Right, traversed);
    }

    public void InsertNode(Node node)
    {
        Root = InsertNode(Root, node);
        Count++;
    }
    public Node InsertNode(Node currNode, Node node)
    {
        if (currNode == null)
        {
            currNode = node;
            return currNode;
        }

        if (node.Data < currNode.Data)
        {
            currNode.Left = InsertNode(currNode.Left, node);
        }
        else if (node.Data > currNode.Data)
        {
            currNode.Right = InsertNode(currNode.Right, node);
        }

        return currNode;
    }

    public void Delete(Node node)
    {
        Delete(Root, node);
        Count--;
        TreeBalancer();
    }
    public Node Delete(Node currentNode, Node node)
    {
        if (currentNode == null)
        {
            return currentNode;
        }

        if (node.Data < currentNode.Data)
        {
            currentNode.Left = Delete(currentNode.Left, node);
        }
        else if (node.Data > currentNode.Data)
        {
            currentNode.Right = Delete(currentNode.Right, node);
        }
        else // FOUND THE NODE!!
        {
            // Node with one child or none
            if (currentNode.Left == null)
            {
                return currentNode.Right;
            }
            else if (currentNode.Right == null)
            {
                return currentNode.Left;
            }

            // Node with 2 kids: get its successor; smallest in the right
            currentNode.Data = MinValue(currentNode.Right);

            currentNode.Right = Delete(currentNode.Right, currentNode);
        }

        return currentNode;
    }

    public int MinValue(Node currentNode)
    {
        while (currentNode.Left != null)
        {
            currentNode = currentNode.Left;
        }

        return currentNode.Data;
    }

    public void TreeBalancer()
    {
        if (lastTimeBalanced % 5 == 0) // re-does the tree every 5 removes
        {
            List<Node> traversed = new();
            InOrderTraversal(Root, traversed);
            ClearTree();
            BuildTree(traversed);
            lastTimeBalanced = 1;
        }
        else
        {
            while (!IsBalanced(Root, out Node longerTree))
            {
                if (longerTree == Root.Right) // if right is longer, rotate left
                {
                    rotateLeft();
                }
                else
                {
                    rotateRight();
                }
            }
            lastTimeBalanced++;
        }
    }

    private void rotateRight()
    {
        Node oldRoot = Root;
        Node newRoot = Root.Left;
        Node newRootsRight = newRoot.Right;

        Root = newRoot;
        newRoot.Right = oldRoot;
        oldRoot.Left = newRootsRight;
    }

    private void rotateLeft()
    {
        Node oldRoot = Root;
        Node newRoot = Root.Right;
        Node newRootsLeft = newRoot.Left;

        Root = newRoot;
        newRoot.Left = oldRoot;
        oldRoot.Right = newRootsLeft;
    }

    public int TreeHeight(Node root)
    {
        if (root == null)
        {
            return -1;
        }

        int height = 0;
        int heightLeft = height;
        int heightRight = height;
        if (root.Left != null)
        {
            heightLeft = TreeHeightHelper(root.Left, height + 1);
        }

        if (root.Right != null)
        {
            heightRight = TreeHeightHelper(root.Right, height + 1);
        }

        return heightLeft > heightRight ? heightLeft : heightRight;
    }

    public int TreeHeightHelper(Node root, int edges)
    {
        int heightLeft = edges;
        int heightRight = edges;
        if (root.Left != null)
        {
            heightLeft = TreeHeightHelper(root.Left, edges + 1);
        }

        if (root.Right != null)
        {
            heightRight = TreeHeightHelper(root.Right, edges + 1);
        }

        return heightLeft > heightRight ? heightLeft : heightRight;
    }

    public bool IsBalanced(Node current, out Node longerTree)
    {
        longerTree = null; // null unless changed
        if (current == null)
        {
            return true; // it's balanced if the root is empty... lol
        }

        // current node exists, but has no children
        if (current.Left == null && current.Right == null)
        {
            return true;
        }

        int leftHeight = current.Left == null ? 0 : TreeHeightHelper(current.Left, 1);
        int rightHeight = current.Right == null ? 0 : TreeHeightHelper(current.Right, 1);

        if (leftHeight != rightHeight)
        {
            longerTree = leftHeight > rightHeight ? current.Left : current.Right;
        }

        return Math.Abs(leftHeight - rightHeight) <= 1;
    }
}

public class Node
{
    public int Data;
    public Node Left;
    public Node Right;

    public ChallengeType Type;
    public Skill RequiredSkill;
    public int SkillPoints;
    public string RequiredItem;

    /// <summary>
    /// Uses Node Number to randomize challenge & skillPoints
    /// Challenges have both the option of required stat & item
    /// </summary>
    /// <param name="number"></param>
    public Node(int number)
    {
        Data = number;
        SkillPoints = Math.Max(Random.Shared.Next(1, 8), number / 10);

        int rand = Random.Shared.Next(0, 3);

        RequiredSkill = rand switch
        {
            0 => Skill.Intelligence,
            1 => Skill.Strength,
            2 => Skill.Agility,
            _ => throw new Exception("Invalid Skill Type!?!?!?"),
        };

        Type = RequiredSkill switch
        {
            Skill.Intelligence => ChallengeType.Puzzle,
            Skill.Strength => ChallengeType.Combat,
            Skill.Agility => ChallengeType.Trap,
            _ => throw new Exception("Invalid Challenge Type!?!?!?"),
        };

        // randomly picks required item
        RequiredItem = GameInfo.ItemsList[Random.Shared.Next(0, GameInfo.ItemsList.Count)];
    }
}

