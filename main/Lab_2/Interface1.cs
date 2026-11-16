using OOPGame_Lab2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOPGame_Lab2
{
    public interface Attacker
    {
        void Attack(Unit unit);
    }
    public interface Moveable
    {
        void Move(int newX, int newY);
    }
}
