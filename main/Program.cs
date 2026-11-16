using System;


namespace OOPGame_Lab2
{

    // Базовый абстрактный класс для всех объектов
    public abstract class GameObject
    {
        private int _id; private string _name;
        protected int x; protected int y;
        public int ID => _id; public string Name => _name;
        public int GetId() => _id; public string GetName() => _name;
        public int GetX() => x; public int GetY() => y;
        public GameObject(int id, string name)
        {
            _id = id; _name = name;
            x = 0; y = 0;
        }
    }

    // Абстрактный класс юнита, так как в игре есть конкретные виды (например, Стрелок)
    public abstract class Unit : GameObject
    {
        private float _hp; private bool _alive; 
        public float GetHp() => _hp; public bool IsAlive() => _alive;
        public Unit(int id, string name, float hp) : base(id, name)
        { _hp = hp; _alive = hp>0; }


        public void ReceiveDamage(float damage)
        {
            if (damage < 0 || !_alive) return;
            _hp -= damage;
            if (_hp <= 0)
            {
                _hp = 0;
                _alive = false;
            }
        }
    }

    // Стрелок: наследуется от Unit, реализует интерфейсы
    public class Archer : Unit, Attacker, Moveable
    {
        private float _attackPower; public float AttackPower => _attackPower;

        public Archer(int id, string name, float hp, float attackPower) : base(id, name, hp)
        { _attackPower = attackPower; }

        public void Attack(Unit unit)
        {
            if (IsAlive() && unit != null && unit.IsAlive())
            {
                Console.WriteLine($"{GetName()} стреляет из лука в {unit.GetName()}.");
                unit.ReceiveDamage(_attackPower);
            }
            else
            { Console.WriteLine("Атака невозможна."); }
        }

        public void Move(int newX, int newY)
        {
            if (!IsAlive())
            {
                Console.WriteLine($"{GetName()} не может двигаться, он погиб.");
                return;
            }
            x = newX;
            y = newY;
            Console.WriteLine($"{GetName()} перешел на координаты ({x}, {y})");
        }
    }

    // Абстрактный класс постройки
    public abstract class Building : GameObject
    {
        private bool _built; public bool IsBuilt() => _built;
        public Building(int id, string name, bool isBuilt) : base(id, name)
        { _built = isBuilt; }
        public void FinishConstruction()
        { _built = true; }
    }

    // Атакующая постройка
    public class Fort : Building, Attacker
    {
        private float _cannonDamage; public float CannonDamage => _cannonDamage;
        public Fort(int id, string name, bool isBuilt, float cannonDamage) : base(id, name, isBuilt)
        { _cannonDamage = cannonDamage; }
        public void Attack(Unit unit)
        {
            // Крепость может стрелять, только если она уже достроена
            if (!IsBuilt())
            {
                Console.WriteLine($"Крепость {GetName()} еще строится и не может стрелять.");
                return;
            }
            if (unit != null && unit.IsAlive())
            {
                Console.WriteLine($"{GetName()} стреляет из пушек по {unit.GetName()}.");
                unit.ReceiveDamage(_cannonDamage);
            }
        }
    }

    // Дом на колесах
    public class MobileHome : Building, Moveable
    {
        public MobileHome(int id, string name, bool isBuilt) : base(id, name, isBuilt) { }
        public void Move(int newX, int newY)
        {
            if (!IsBuilt())
            {
                Console.WriteLine($"{GetName()} еще не собран, ехать нельзя.");
                return;
            }
            x = newX; y = newY;
            Console.WriteLine($"{GetName()} переехал в точку ({x}, {y})");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Archer e = new Archer(1, "Стрелок", 100f, 25f);
            Fort castle = new Fort(3, "Замок", true, 50f);
            Archer e2 = new Archer(2, "Стрелок противника", 80f, 15f);

            Attacker attacker1 = e;
            Attacker attacker2 = castle;
            Console.WriteLine($"Стартовые координаты Стрелка: ({e.GetX()}, {e.GetY()})");
            e.Move(2, 2); // проверка Moveable
            Console.WriteLine($"HP Стрелка противника до атак: {e2.GetHp()}");

            attacker1.Attack(e2); // Стрелок стреляет из лука
            attacker2.Attack(e2); // Замок стреляет из пушки

            Console.WriteLine($"HP цели после двух атак: {e2.GetHp()}");
            Console.WriteLine($"Жива ли цель: {e2.IsAlive()}");

            MobileHome mh = new MobileHome(4, "Дом на колесах", false);
            Console.WriteLine("\n=== Проверка дома на колесах ===");
            mh.Move(4, 4); // Не поедет
            mh.FinishConstruction();
            mh.Move(4, 4); // Поедет

            Console.ReadLine();
        }
    }
}
