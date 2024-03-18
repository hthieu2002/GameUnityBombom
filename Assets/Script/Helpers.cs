using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Helpers
{
    #region Biến toàn cục
    public LayerMask[] blockLayer;
    #endregion

    public class Node
    {
        private Node parent;
        private Vector2 location, action;
        public float h, g, f;

        // Hàm khởi tạo
        public Node(Node parent = null, Vector2 location = default(Vector2), Vector2 action = default(Vector2))
        {
            this.Parent = parent;
            this.Location = location;
            this.Action = action;

            h = 0f;
            g = 0f;
            f = 0f;
        }

        // Hàm get set
        public Node Parent { get => parent; set => parent = value; }
        public Vector2 Location { get => location; set => location = value; }
        public Vector2 Action { get => action; set => action = value; }
    }

    public List<Node> getPath(Node node)
    {
        List<Node> path = new List<Node>();

        while (node.Parent != null)
        {
            path.Add(node);
            node = node.Parent;
        }

        return path;
    }

    public List<Vector2> getPath_Action(List<Node> path)
    {
        List<Vector2> actions = new List<Vector2>();
        foreach (Node node in path)
        {
            actions.Add(node.Action);
        }

        return actions;
    }

    public List<Dictionary<(int, int), Vector2>> get_free_neighbors(Vector2 location)
    {
        #region Biến
        int x = (int)location.x;
        int y = (int)location.y;

        List<Dictionary<(int, int), Vector2>> neighbors = new List<Dictionary<(int, int), Vector2>>();
        neighbors.Add(new Dictionary<(int, int), Vector2> { { (x - 1, y), Vector2.left } });
        neighbors.Add(new Dictionary<(int, int), Vector2> { { (x + 1, y), Vector2.right } });
        neighbors.Add(new Dictionary<(int, int), Vector2> { { (x, y + 1), Vector2.up } });
        neighbors.Add(new Dictionary<(int, int), Vector2> { { (x, y - 1), Vector2.down } });
        List<Dictionary<(int, int), Vector2>> free_neighbors = new List<Dictionary<(int, int), Vector2>>();
        #endregion

        foreach (var neighbor in neighbors)
        {
            foreach (var tile in neighbor.Keys)
            {
                // Biến khóa của dictionary thành Vector2 (điểm xung quanh)
                Vector2 poisition = new Vector2(tile.Item1, tile.Item2);

                // Kiểm tra xem điểm đó có phải là tường hoặc bom hay không
                if (!Physics2D.OverlapCircle(poisition, 0.48f, blockLayer[0]))
                {
                    free_neighbors.Add(neighbor);
                }
            }

        }

        return free_neighbors;
    }

    public float manhattan_distance(Vector2 start, Vector2 playerPosion)
    {
        float distance = Mathf.Abs(start.x - playerPosion.x) + Mathf.Abs(start.y - playerPosion.y);
        return distance;
    }

    // Giải thuật A*
    public List<Node> Astar(Vector2 start, Vector2 playerPosion, int sizeMap)
    {
        playerPosion.x = Mathf.Round(playerPosion.x);
        playerPosion.y = Mathf.Round(playerPosion.y);
        List<Node> path;

        List<Node> open_list = new List<Node>();
        List<Node> close_list = new List<Node>();

        open_list.Add(new Node(null, start, default(Vector2)));
        // Thoát khỏi vòng lặp khi không tìm thấy đường nào để đi
        int max_loop = sizeMap;
        int counter = 0;
        while (counter <= max_loop && open_list.Count > 0)
        {
            // Sử dụng FIFO
            // Tìm node được đưa vào đầu tiên
            Node curr_node = open_list[0];
            int curr_index = 0;

            for (int i = 0; i < open_list.Count; i++)
            {
                Node node = open_list[i];
                if (node.f < curr_node.f)
                {
                    curr_node = node;
                    curr_index = i;
                }
            }

            //Kiểm tra xem điểm đó có phải người chơi không
            if (curr_node.Location == playerPosion)
            {
                path = getPath(curr_node);
                return path;
            }

            // current = phần tử vào đầu tiên của open_list (xóa phần tử đó ra khỏi open)
            // thêm current vào close_list
            open_list.RemoveAt(curr_index);
            close_list.Add(curr_node);

            // Lấy các ô ở cạnh bên
            List<Dictionary<(int, int), Vector2>> neighbors = get_free_neighbors(curr_node.Location);

            List<Node> neighbor_nodes = new List<Node>();
            foreach (var neighbor in neighbors)
            {
                foreach (var item in neighbor.Keys)
                {
                    Vector2 location = new Vector2(item.Item1, item.Item2);
                    neighbor_nodes.Add(new Node(null, location, neighbor[item]));
                }
            }

            // Xét các node neighbor của current
            foreach(Node neighbor in neighbor_nodes)
            {
                // Các trạng thái sử dụng trong vòng for
                bool in_closed = false;
                bool in_open = false;

                // cost = g(current) + movementcost(current, neighbor)
                float cost = curr_node.g + 1f;

                // Nếu neighbor ở trong open_list và có cost nhỏ hơn g(neighbor)
                // Xóa neighbor ra khỏi open_list, vì ta đã tìm thấy đường tốt hơn
                for(int i = 0; i<open_list.Count; i++)
                {
                    if(neighbor.Location == open_list[i].Location && cost < neighbor.g)
                    {
                        open_list.RemoveAt(i);
                        in_open = true;
                    }
                }

                // Nếu neighbor ở trong close_list và có cost nhỏ hơn g(neighbor)
                // Xóa neighbor ra khỏi close_list
                for (int i = 0; i < close_list.Count; i++)
                {
                    if (neighbor.Location == close_list[i].Location && cost < neighbor.g)
                    {
                        close_list.RemoveAt(i);
                        in_closed = true;
                    }
                }

                // Nếu neighbor không ở trong open_list hoặc close_list
                // Đặt g(neighbor) vào cost
                // Thêm neighbor vào open_list
                // Set hàm đơi ưu tiên thành g(neighbor) + h(neighbor)
                // Set nút cha của neighbor vào current
                if(!in_closed && !in_open)
                {
                    neighbor.g = cost;
                    open_list.Add(neighbor);
                    neighbor.h = manhattan_distance(neighbor.Location, playerPosion);
                    neighbor.f = neighbor.g + neighbor.h;
                    neighbor.Parent = curr_node;
                }
            }

            counter++;
        }
        return null;
    }
}
