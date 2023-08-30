using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    const int THOUSAND = 1000;
    [SerializeField] bool prova;
    [SerializeField] string s;


    private void OnValidate()
    {
        if (prova)
        {
            int n = 1234567890; //1'234'567'890
            int _b, _m, _t,
                bil, mil, th, hund;

            _t = DivideByThousand(n);
            _m = DivideByThousand(_t);
            _b = DivideByThousand(_m);

            hund = RestByThousand(n);
            th = RestByThousand(_t);
            mil = RestByThousand(_m);
            bil = RestByThousand(_b);

            s = bil + "'" + mil + "'" + th + "'" + hund;

            //s.Insert(0, hund != 0 ? hund.ToString() : "0");
            //s.Insert(0, th != 0 ? th + "\'"  : "");
            //s.Insert(0, mil != 0 ? mil + "\'" : "");
            //s.Insert(0, bil != 0 ? bil + "\'" : "");


            prova = false;
        }

        int DivideByThousand(int n) { return n / 1000; }
        int RestByThousand(int n) { return n % 1000; }
    }
}
