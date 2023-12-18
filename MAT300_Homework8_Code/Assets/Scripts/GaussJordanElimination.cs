
public class Gauss_Jordan_Elimination
{
    public static double[] GetEliminationResult(double[,] A, int n)
    {
        int i, j, k = 0;
        int c, flag = 0;

        for (i = 0; i < n; ++i)
        {
            if (A[i, i].CompareTo(0.0) == 0)
            {
                c = 1;
                while ((i + c) < n && A[i + c, i].CompareTo(0) == 0)
                    c++;
                if ((i + c) < n)
                {
                    flag = 1;
                    break;
                }

                for (j = i, k = 0; k <= n; ++k)
                {
                    double temp = A[j, k];
                    A[j, k] = A[j + c, k];
                    A[j + c, k] = temp;
                }
            }

            for (j = 0; j < n; ++j)
            {
                if (i != j)
                {
                    double p = A[j, i] / A[i, i];

                    for (k = 0; k <= n; ++k)
                        A[j, k] = A[j, k] - (A[i, k]) * p;
                }
            }
        }


        double[] result = new double[n];
        if (flag == 1)
            return result;

        for (i = 0; i < n; ++i)
        {
            result[i] = A[i, n] / A[i, i];
        }

        return result;
    }
}
