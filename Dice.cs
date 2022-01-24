using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RévisionCSHARP
{
    internal class Dice
    {
        char[] _faces;
        char _upFace;

        public Dice(char[] Faces, Random r)
        {
            _faces = Faces;
            Launch(r);
        } 

        public void Launch(Random r)
        {
            _upFace = _faces[r.Next(0, _faces.Length)];
        }

        public override string ToString()
        {
            return _upFace.ToString();
        }

    }
}
