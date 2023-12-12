using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class AiPath : MonoBehaviour
{
    [SerializeField] private Tilemap tile;

    public Tilemap Tile { get => tile; private set => tile = value; }

    [ContextMenu("Add Nodes")]
    void AddNodes()
    {
        if(Tile == null)
        {
            Debug.Log("Please place a tilemap in your scene");
            return;
        }

        Vector3 origine = Tile.origin;
        origine.x += Tile.cellSize.x / 2;
        origine.y += Tile.cellSize.y / 2;

        Vector3 bounds = Tile.size;
        bounds.x -= Mathf.Abs(origine.x);
        bounds.y -= Mathf.Abs(origine.y);

        for (int u = 0; u < Mathf.Abs(origine.y) + Mathf.Abs(bounds.y); u++)
        {
            for (int i = 0; i < Mathf.Abs(origine.x) + Mathf.Abs(bounds.x); i++)
            {
                Vector3 pos = new Vector3(origine.x + i, origine.y + u, 0);
                if (!Tile.HasTile(new Vector3Int(Tile.origin.x + i, Tile.origin.y + u, 0)) && Tile.HasTile(new Vector3Int(Tile.origin.x + i, Tile.origin.y + u - 1, 0)))
                {
                    CreateNodes(pos);
                }
            }
        }
    }

    [ContextMenu("Make Link")]
    void MakeLink()
    {
        if (Tile == null)
        {
            Debug.Log("Please place a tilemap in your scene");
            return;
        }
        List<Node> AllNodes = new List<Node>();
        AllNodes = GetComponentsInChildren<Node>().ToList();
        if (AllNodes.Count == 0)
        {
            Debug.Log("Please create a node");
            return;
        }
        foreach (var node in AllNodes)
        {
            IndividualConnection(node, AllNodes);
        }
    }

    void IndividualConnection(Node node, List<Node> AllNodes)
    {
        Vector3 pos = node.transform.position;
        foreach (var n in AllNodes)
        {
            if (n == node || !n.enabled || n == null)
            {
                continue;
            }
            if (node.ConnectedTo.Contains(n))
            {
                continue;
            }

            Vector3 target = n.transform.position;

            if (Mathf.Abs(pos.y - target.y) < .1f)
            {
                bool obstacle = false;
                bool ground = true;

                int x = Mathf.Abs((int)(pos.x - target.x));
                int signal = pos.x < target.x ? 1 : -1;

                Vector3 tilePos = pos;
                tilePos.x -= tile.cellSize.x / 2;
                tilePos.y -= tile.cellSize.y / 2;

                for (int i = 1; i < x; i++)
                {
                    int variationX = i * signal;
                    if (tile.HasTile(new Vector3Int((int)tilePos.x + variationX, (int)tilePos.y, (int)tilePos.z)))
                    {
                        obstacle = true;
                        break;
                    }
                }
                for (int i = 0; i <= x; i++)
                {
                    int variationX = i * signal;
                    if (!tile.HasTile(new Vector3Int((int)tilePos.x + variationX, (int)tilePos.y - 1, (int)tilePos.z)))
                    {
                        ground = false;
                        break;
                    }
                }
                if ((!obstacle && ground))
                {
                    node.ConnectedTo.Add(n);
                }
            }
            else if (Mathf.Round(Mathf.Abs(pos.x - target.x)) == 1 && Mathf.Round(Mathf.Abs(pos.y - target.y)) == 1)
            {
                node.ConnectedTo.Add(n);
                node.AddToJump(n);
            }

        }
    }
    void CreateNodes(Vector3 position)
    {
        List<Node> AllNodes = FindObjectsOfType<Node>().ToList();

        foreach (var n in AllNodes)
        {
            if (n.transform.position == position)
            {
                return;
            }
        }

        GameObject node = Node.CreateNode();
        node.transform.parent = transform;
        node.transform.position = position;
    }
}
