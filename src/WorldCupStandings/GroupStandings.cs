namespace WorldCupStandings;

public static class GroupStandings
{
    // matches look like "ESP 3-3 POR" or "ESP 3-3 POR 2/0/0 1/0/1"
    // (cards are yellow/second-yellow red/straight red, home team first)
    public static string Calculate(string[] matches)
    {
        // 0=P 1=W 2=D 3=L 4=GF 5=GA 6=Pts 7=FP
        var d = new Dictionary<string, int[]>();
        var names = new List<string>();

        for (int i = 0; i < matches.Length; i++)
        {
            var p = matches[i].Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var t1 = p[0];
            var t2 = p[2];
            var g1 = int.Parse(p[1].Split('-')[0]);
            var g2 = int.Parse(p[1].Split('-')[1]);

            if (!d.ContainsKey(t1)) { d[t1] = new int[8]; names.Add(t1); }
            if (!d.ContainsKey(t2)) { d[t2] = new int[8]; names.Add(t2); }

            d[t1][0]++;
            d[t2][0]++;
            d[t1][4] += g1;
            d[t1][5] += g2;
            d[t2][4] += g2;
            d[t2][5] += g1;

            if (g1 > g2)
            {
                d[t1][1]++;
                d[t2][3]++;
                d[t1][6] += 3;
            }
            else if (g1 < g2)
            {
                d[t2][1]++;
                d[t1][3]++;
                d[t2][6] += 3;
            }
            else
            {
                d[t1][2]++;
                d[t2][2]++;
                d[t1][6] += 1;
                d[t2][6] += 1;
            }

            if (p.Length > 3)
            {
                var c1 = p[3].Split('/');
                d[t1][7] -= int.Parse(c1[0]) * 1 + int.Parse(c1[1]) * 3 + int.Parse(c1[2]) * 4;
                var c2 = p[4].Split('/');
                d[t2][7] -= int.Parse(c2[0]) * 1 + int.Parse(c2[1]) * 3 + int.Parse(c2[2]) * 4;
            }
        }

        // sort
        for (int i = 0; i < names.Count; i++)
        {
            for (int j = 0; j < names.Count - 1 - i; j++)
            {
                var a = names[j];
                var b = names[j + 1];
                bool swap = false;

                if (d[a][6] < d[b][6])
                {
                    swap = true;
                }
                else if (d[a][6] == d[b][6])
                {
                    // head to head between all the teams on these points
                    var tied = new List<string>();
                    for (int k = 0; k < names.Count; k++)
                    {
                        if (d[names[k]][6] == d[a][6]) tied.Add(names[k]);
                    }

                    int hpa = 0, hgfa = 0, hgaa = 0;
                    for (int m = 0; m < matches.Length; m++)
                    {
                        var q = matches[m].Split(' ', StringSplitOptions.RemoveEmptyEntries);
                        if (!tied.Contains(q[0]) || !tied.Contains(q[2])) continue;
                        var x1 = int.Parse(q[1].Split('-')[0]);
                        var x2 = int.Parse(q[1].Split('-')[1]);
                        if (q[0] == a)
                        {
                            hgfa += x1; hgaa += x2;
                            if (x1 > x2) hpa += 3; else if (x1 == x2) hpa += 1;
                        }
                        else if (q[2] == a)
                        {
                            hgfa += x2; hgaa += x1;
                            if (x2 > x1) hpa += 3; else if (x1 == x2) hpa += 1;
                        }
                    }

                    int hpb = 0, hgfb = 0, hgab = 0;
                    for (int m = 0; m < matches.Length; m++)
                    {
                        var q = matches[m].Split(' ', StringSplitOptions.RemoveEmptyEntries);
                        if (!tied.Contains(q[0]) || !tied.Contains(q[2])) continue;
                        var x1 = int.Parse(q[1].Split('-')[0]);
                        var x2 = int.Parse(q[1].Split('-')[1]);
                        if (q[0] == b)
                        {
                            hgfb += x1; hgab += x2;
                            if (x1 > x2) hpb += 3; else if (x1 == x2) hpb += 1;
                        }
                        else if (q[2] == b)
                        {
                            hgfb += x2; hgab += x1;
                            if (x2 > x1) hpb += 3; else if (x1 == x2) hpb += 1;
                        }
                    }

                    if (hpa < hpb) swap = true;
                    else if (hpa == hpb)
                    {
                        if (hgfa - hgaa < hgfb - hgab) swap = true;
                        else if (hgfa - hgaa == hgfb - hgab)
                        {
                            if (hgfa < hgfb) swap = true;
                            else if (hgfa == hgfb)
                            {
                                if (d[a][4] - d[a][5] < d[b][4] - d[b][5]) swap = true;
                                else if (d[a][4] - d[a][5] == d[b][4] - d[b][5])
                                {
                                    if (d[a][4] < d[b][4]) swap = true;
                                    else if (d[a][4] == d[b][4])
                                    {
                                        if (d[a][7] < d[b][7]) swap = true;
                                        else if (d[a][7] == d[b][7])
                                        {
                                            if (string.CompareOrdinal(a, b) > 0) swap = true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                if (swap)
                {
                    names[j] = b;
                    names[j + 1] = a;
                }
            }
        }

        var s = "Pos Team  P  W  D  L GF GA  GD Pts  FP\n";
        for (int i = 0; i < names.Count; i++)
        {
            var t = d[names[i]];
            var gd = t[4] - t[5];
            s += (i + 1).ToString().PadLeft(3) + " " + names[i].PadRight(4)
                + " " + t[0].ToString().PadLeft(2)
                + " " + t[1].ToString().PadLeft(2)
                + " " + t[2].ToString().PadLeft(2)
                + " " + t[3].ToString().PadLeft(2)
                + " " + t[4].ToString().PadLeft(2)
                + " " + t[5].ToString().PadLeft(2)
                + " " + (gd > 0 ? "+" + gd : gd.ToString()).PadLeft(3)
                + " " + t[6].ToString().PadLeft(3)
                + " " + t[7].ToString().PadLeft(3)
                + "\n";
        }
        return s;
    }
}
