using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;


namespace ImageQuantization
{

    public class construct
    {
        public static List<RGBPixel> distinct = new List<RGBPixel>();
        public static Hashtable my_hashtable = new Hashtable();

        public static int distinctColor(RGBPixel[,] Matrix)
        {


            for (int i = 0; i < Matrix.GetLength(0); i++)
            {
                for (int j = 0; j < Matrix.GetLength(1); j++)
                {
                    double ky = 0.5 * (Matrix[i, j].red + Matrix[i, j].green) * (Matrix[i, j].red + Matrix[i, j].green + 1) + Matrix[i, j].green;
                    ky = 0.5 * (Matrix[i, j].blue + ky) * (Matrix[i, j].blue + ky + 1) + ky;
                    if (my_hashtable.ContainsKey(ky) != true)
                    {
                        my_hashtable.Add(ky, 1);
                        distinct.Add(Matrix[i, j]);
                    }
                }


            }
            return distinct.Count;
        }

    }

    public class DistanceCalculators
    {
        public static int[] parent;
        public static double[] distances;
        public static double DistanceCalculator(List<RGBPixel> distinct)
        {
            double dist;
            double redc = 0;   // O(1)
            double greenc = 0; // O(1)
            double bluec = 0; // O(1)
            double y = 0; // O(1)
            bool[] visited = new bool[distinct.Count]; // O(1) + O(1)
            //int[] graph = new int[distinct.Count]; // O(1) + O(1)
            distances = new double[distinct.Count]; // O(1) + O(1)

            for (int i = 0; i < distinct.Count; i++) //D * O(1)
            {

                distances[i] = int.MaxValue; // O(1) + O(1) + O(1)
                visited[i] = false; // O(1) + O(1)
            }

            parent = new int[distinct.Count]; // O(1) + O(1)
            parent[0] = -1; // O(1) + O(1)
            int visNodes = 0; // O(1) 
            int node = 0; // O(1) 
            distances[node] = 0; // O(1) + O(1)
            while (visNodes < distinct.Count) // D * D
            {
                visNodes++; // O(1) 
                visited[node] = true; // O(1) + O(1)
                for (int i = 0; i < distinct.Count; i++) //D *
                {
                    if (visited[i] == true) // O(1) + O(1)
                        continue;
                    redc = (distinct[i].red - distinct[node].red) * (distinct[i].red - distinct[node].red); // O(1)* 4
                    greenc = (distinct[i].green - distinct[node].green) * (distinct[i].green - distinct[node].green); // O(1)* 4
                    bluec = (distinct[i].blue - distinct[node].blue) * (distinct[i].blue - distinct[node].blue); // O(1)* 4
                    y = redc + greenc + bluec; //O(1)
                    dist = Math.Sqrt(y); // O(1) + O(1)
                    if (dist < distances[i]) // O(1) + O(1)
                    {
                        distances[i] = dist; // O(1) + O(1)
                        parent[i] = node; // O(1) + O(1)
                    }

                }
                double mn = int.MaxValue; // 
                for (int i = 0; i < distinct.Count; i++) // D* O(1)
                {
                    if (visited[i] == true) // O(1) + O(1)
                        continue;
                    if (distances[i] < mn) // O(1) + O(1)
                    {
                        mn = distances[i]; // O(1) + O(1)
                        node = i; // O(1) + O(1)
                    }
                }
            }
            double sum = distances.Sum();
            double s = parent.Sum();
            sum = Math.Round(sum, 1);
            return sum; // O(1)
        }
    }
    public class clustering
    {
        public static Dictionary<double, RGBPixel> colorDic = new Dictionary<double, RGBPixel>();

        public static bool[] disconnected;
        public static Dictionary<int, List<int>> g;

        //public static Dictionary<RGBPixel, double> palette;

        public static HashSet<int>[] hashArr;
        public static int clusterCounter = 0;

        public static bool[] flags;
        public static void phaseOne(double[] dist, int k)
        {
            disconnected = new bool[dist.Length];
            double m;
            int index = -1;
            for (int i = 0; i < dist.Length; i++)
            {
                disconnected[i] = false; // O(1) + O(1)
            }

            for (int i = 0; i < k - 1; i++)
            {
                m = int.MinValue;
                for (int j = 0; j < dist.Length; j++)
                {

                    if (disconnected[j] == true)
                        continue;
                    else
                    {
                        if (dist[j] > m)
                        {
                            m = dist[j];
                            index = j;
                        }

                    }
                }
                disconnected[index] = true;

            }

        }
        public static int[] phaseTwo(int[] parent, bool[] v)
        {

            for (int i = 0; i < v.Length; i++)
            {
                if (v[i] == true)
                {
                    parent[i] = -1;
                }
            }
            return parent;

        }
        public static void graph(int[] parent, bool[] v)
        {
            g = new Dictionary<int, List<int>>();
            for (int i = 0; i < v.Length; i++)
            {
                int index = parent[i];
                if (!g.ContainsKey(index))
                {
                    g.Add(index, new List<int>());
                }

            }
            for (int i = 0; i < v.Length; i++)
            {
                int index = parent[i];
                if (g.ContainsKey(parent[i]))
                {
                    g[index].Add(i);
                }
            }

        }
        public static void phaseThree(Dictionary<int, List<int>> dict, int k)
        {
            List<int> val = dict[-1];
            hashArr = new HashSet<int>[k];
            for (int i = 0; i < val.Count; i++)
            {
                hashArr[i] = new HashSet<int>();
                DFS(val[i], dict, k);
                clusterCounter++;

            }


        }
        public static void DFS(int num, Dictionary<int, List<int>> d, int clust)
        {


            if (!d.ContainsKey(num))
            {

                hashArr[clusterCounter].Add(num);
                return;
            }
            else
            {

                hashArr[clusterCounter].Add(num);
                List<int> children = d[num];
                int childrenCounter = children.Count;
                for (int j = 0; j < children.Count; )
                {
                    DFS(children[j], d, clust);
                    j++;

                }
                return;
            }

        }



        public static void AVG(int k)
        {


            int q = 0;

            foreach (var n in hashArr) //K
            {
                double r = 0;
                double b = 0;
                double g = 0;
                RGBPixel avg;

                int counter = n.Count;
                int[] carrier = new int[counter];
                hashArr[q].CopyTo(carrier);
                q++;


                if (counter == 1)
                {
                    double ky = 0.5 * (construct.distinct[carrier[0]].red + construct.distinct[carrier[0]].green) * (construct.distinct[carrier[0]].red + construct.distinct[carrier[0]].green + 1) + construct.distinct[carrier[0]].green;
                    ky = 0.5 * (construct.distinct[carrier[0]].blue + ky) * (construct.distinct[carrier[0]].blue + ky + 1) + ky;
                    colorDic.Add(ky, construct.distinct[carrier[0]]);


                }
                else
                {
                    for (int i = 0; i < counter; i++)
                    {
                        int index = carrier[i];
                        b = b + construct.distinct[index].blue;
                        g = g + construct.distinct[index].green;
                        r = r + construct.distinct[index].red;
                        double ky = 0.5 * (construct.distinct[index].red + construct.distinct[index].green) * (construct.distinct[index].red + construct.distinct[index].green + 1) + construct.distinct[index].green;
                        ky = 0.5 * (construct.distinct[index].blue + ky) * (construct.distinct[index].blue + ky + 1) + ky;

                        colorDic.Add(ky, new RGBPixel());
                    }
                    avg.blue = (byte)Math.Ceiling(b / counter);
                    avg.green = (byte)Math.Ceiling(g / counter);
                    avg.red = (byte)Math.Ceiling(r / counter);
                    for (int c = 0; c < counter; c++)
                    {
                        int index = carrier[c];
                        double ky = 0.5 * (construct.distinct[index].red + construct.distinct[index].green) * (construct.distinct[index].red + construct.distinct[index].green + 1) + construct.distinct[index].green;
                        ky = 0.5 * (construct.distinct[index].blue + ky) * (construct.distinct[index].blue + ky + 1) + ky;

                        colorDic[ky] = avg;
                    }
                }
            }

        }
        public static void Gausssmooth(RGBPixel[,] Matrix)
        {


            for (int i = 0; i < Matrix.GetLength(0); i++)
            {
                for (int j = 0; j < Matrix.GetLength(1); j++)
                {
                    double ky = 0.5 * (Matrix[i, j].red + Matrix[i, j].green) * (Matrix[i, j].red + Matrix[i, j].green + 1) + Matrix[i, j].green;
                    ky = 0.5 * (Matrix[i, j].blue + ky) * (Matrix[i, j].blue + ky + 1) + ky;
                    Matrix[i, j] = colorDic[ky];
                }

            }
        }
    }
}


