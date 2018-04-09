﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ASL.NavMesh
{
    [System.Serializable]
    public class NavMeshTriangle
    {
        public Vector3 vertex0
        {
            get
            {
                if (m_NodeLists == null || m_NodeLists.Count < 1)
                    return default(Vector3);
                return m_NodeLists[0].vertex0;
            }
        }

        public Vector3 vertex1
        {
            get
            {
                if (m_NodeLists == null || m_NodeLists.Count < 1)
                    return default(Vector3);
                return m_NodeLists[0].vertex1;
            }
        }

        public Vector3 vertex2
        {
            get
            {
                if (m_NodeLists == null || m_NodeLists.Count < 1)
                    return default(Vector3);
                return m_NodeLists[0].vertex2;
            }
        }

        public Vector2 uv0
        {
            get
            {
                if (m_NodeLists == null || m_NodeLists.Count < 1)
                    return default(Vector2);
                return m_NodeLists[0].uv0;
            }
        }

        public Vector2 uv1
        {
            get
            {
                if (m_NodeLists == null || m_NodeLists.Count < 1)
                    return default(Vector2);
                return m_NodeLists[0].uv1;
            }
        }

        public Vector2 uv2
        {
            get
            {
                if (m_NodeLists == null || m_NodeLists.Count < 1)
                    return default(Vector2);
                return m_NodeLists[0].uv2;
            }
        }

        /// <summary>
        /// 节点列表
        /// </summary>
        [SerializeField]
        private List<NavMeshTriangleNode> m_NodeLists;

        /// <summary>
        /// 三角形的AABB包围盒
        /// </summary>
        public Bounds bounds { get { return m_Bounds; } }

        [SerializeField]
        private Bounds m_Bounds;

        public NavMeshTriangle(Vector3 vertex0, Vector3 vertex1, Vector3 vertex2, Vector2 uv0, Vector2 uv1, Vector2 uv2, float area, int maxDepth)
        {
            NavMeshTriangleNode root = new NavMeshTriangleNode(vertex0, vertex1, vertex2, uv0, uv1, uv2);
            m_NodeLists = new List<NavMeshTriangleNode>();
            m_NodeLists.Add(root);

            float maxX = Mathf.Max(vertex0.x, vertex1.x, vertex2.x);
            float maxY = Mathf.Max(vertex0.y, vertex1.y, vertex2.y);
            float maxZ = Mathf.Max(vertex0.z, vertex1.z, vertex2.z);

            float minX = Mathf.Min(vertex0.x, vertex1.x, vertex2.x);
            float minY = Mathf.Min(vertex0.y, vertex1.y, vertex2.y);
            float minZ = Mathf.Min(vertex0.z, vertex1.z, vertex2.z);

            Vector3 si = new Vector3(maxX - minX, maxY - minY, maxZ - minZ);
            if (si.x <= 0)
                si.x = 0.1f;
            if (si.y <= 0)
                si.y = 0.1f;
            if (si.z <= 0)
                si.z = 0.1f;
            Vector3 ct = new Vector3(minX, minY, minZ) + si / 2;

            this.m_Bounds = new Bounds(ct, si);

            //float currentArea = Vector3.Cross(vertex1 - vertex0, vertex2 - vertex0).magnitude*0.5f;
            //m_Depth = Mathf.RoundToInt(currentArea/area*0.25f);
            //if (m_Depth < 0)
            //    m_Depth = 0;
            //if (m_Depth > maxDepth)
            //    m_Depth = maxDepth;
            //m_Depth = 4;

            root.Subdivide(4, m_NodeLists);
        }


        public void GenerateMesh(List<Vector3> vlist, List<int> ilist)
        {
            if (m_NodeLists.Count >= 1)
                m_NodeLists[0].GenerateMesh(m_NodeLists, vlist, ilist);
        }

        public void SamplingFromTexture(Texture2D texture)
        {
            if (m_NodeLists.Count >= 1)
                m_NodeLists[0].SamplingFromTexture(m_NodeLists, texture);
        }

        public void Draw(IPaintingTool tool, bool clear)
        {
            if (m_NodeLists.Count >= 1)
            {
                m_NodeLists[0].Draw(!clear, tool, m_NodeLists);
            }
        }

        public void DrawTriangle()
        {
            if (m_NodeLists.Count >= 1)
                m_NodeLists[0].DrawTriangle(m_NodeLists);
        }
    }
}