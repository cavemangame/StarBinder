using System;
using System.Collections.Generic;

using CocosSharp;

namespace StarBinder
{
	// http://wiki.unity3d.com/index.php?title=Triangulator
	// The algorithm supports concave polygons, but not polygons with holes, or multiple polygons at once.

	public class Triangulator
	{
		private List<CCPoint> m_points = new List<CCPoint>();

		public Triangulator (CCPoint[] points) 
		{
			m_points = new List<CCPoint>(points);
		}

		public int[] Triangulate() 
		{
			List<int> indices = new List<int>();

			int n = m_points.Count;
			if (n < 3)
				return indices.ToArray();

			int[] V = new int[n];
			if (Area() > 0) {
				for (int v = 0; v < n; v++)
					V[v] = v;
			}
			else {
				for (int v = 0; v < n; v++)
					V[v] = (n - 1) - v;
			}

			int nv = n;
			int count = 2 * nv;
			for (int m = 0, v = nv - 1; nv > 2; ) {
				if ((count--) <= 0)
					return indices.ToArray();

				int u = v;
				if (nv <= u)
					u = 0;
				v = u + 1;
				if (nv <= v)
					v = 0;
				int w = v + 1;
				if (nv <= w)
					w = 0;

				if (Snip(u, v, w, nv, V)) {
					int a, b, c, s, t;
					a = V[u];
					b = V[v];
					c = V[w];
					indices.Add(a);
					indices.Add(b);
					indices.Add(c);
					m++;
					for (s = v, t = v + 1; t < nv; s++, t++)
						V[s] = V[t];
					nv--;
					count = 2 * nv;
				}
			}

			indices.Reverse();
			return indices.ToArray();
		}

		private double Area () 
		{
			int n = m_points.Count;
			double A = 0.0;
			for (int p = n - 1, q = 0; q < n; p = q++) {
				CCPoint pval = m_points[p];
				CCPoint qval = m_points[q];
				A += pval.X * qval.Y - qval.X * pval.Y;
			}
			return (A * 0.5);
		}

		private double epsilon = 0.000001;

		private bool Snip (int u, int v, int w, int n, int[] V) 
		{
			int p;
			CCPoint A = m_points[V[u]];
			CCPoint B = m_points[V[v]];
			CCPoint C = m_points[V[w]];
			if (epsilon > (((B.X - A.X) * (C.Y - A.Y)) - ((B.Y - A.Y) * (C.X - A.X))))
				return false;
			for (p = 0; p < n; p++) {
				if ((p == u) || (p == v) || (p == w))
					continue;
				CCPoint P = m_points[V[p]];
				if (InsideTriangle(A, B, C, P))
					return false;
			}
			return true;
		}

		private bool InsideTriangle (CCPoint A, CCPoint B, CCPoint C, CCPoint P) 
		{
			double ax, ay, bx, by, cx, cy, apx, apy, bpx, bpy, cpx, cpy;
			double cCROSSap, bCROSScp, aCROSSbp;

			ax = C.X - B.X; ay = C.Y - B.Y;
			bx = A.X - C.X; by = A.Y - C.Y;
			cx = B.X - A.X; cy = B.Y - A.Y;
			apx = P.X - A.X; apy = P.Y - A.Y;
			bpx = P.X - B.X; bpy = P.Y - B.Y;
			cpx = P.X - C.X; cpy = P.Y - C.Y;

			aCROSSbp = ax * bpy - ay * bpx;
			cCROSSap = cx * apy - cy * apx;
			bCROSScp = bx * cpy - by * cpx;

			return ((aCROSSbp >= 0.0f) && (bCROSScp >= 0.0f) && (cCROSSap >= 0.0f));
		}
	}
}
