using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SecondOrderDynamics
{
    private float xp;
    private float y, yd;
    private float k1, k2, k3, T_crit;
    public SecondOrderDynamics(float f, float z, float r, float x0) //https://www.youtube.com/watch?v=KPoeNZZ6H4s&t=529s
    {
        Set_fzr(f,z,r);
        xp = x0;
        y = x0;
        yd = 0;
    }

    public void Set_fzr(float f, float z, float r)
    {
        k1 = z / (Mathf.PI * f);
        k2 = 1 / ((2 * Mathf.PI * f) * (2 * Mathf.PI * f));
        k3 = r * z / (2 * Mathf.PI * f);
        T_crit = 0.8f * (Mathf.Sqrt(4 * k2 + k1 * k1) - k1);
    }

    public void Sey_x0(float x0) {
        y = x0;
        xp = x0;
    }
    public float Update(float T, float x)
    {

        float xd = (x - xp) / T;
        xp = x;
        // Debug.Log(x);
        return Update(T, x, xd);
    }

    public float Update(float T, float x, float xd)
    {

        int i = (int)Mathf.Ceil(T / T_crit);
        T = T / i;
        for (int j = 0; j < i; j++)
        {
            y = y + T * yd;
            yd = yd + T * (x + k3 * xd - y - k1 * yd) / k2;
        }
        return y;
    }


}