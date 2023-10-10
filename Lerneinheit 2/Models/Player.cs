using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Lerneinheit_2.Models
{

    internal class Player
    {
        internal string PlayerName;
        internal string Sign;
        internal int Ident;

        internal Player(string p_PlayerName, string p_PlayerSign,int p_Ident = 1)
        {
            PlayerName = p_PlayerName;
            Sign = p_PlayerSign;
            Ident = p_Ident;
        }
        public override string ToString()
        {
            return Convert.ToString(Sign);
        }

    }


}


