using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicSDNF
{
    class Expression
    {
        public int x;
        public int y;
        public int z;
        public int w;
        public int f;
        public bool IsLonely = true;
        public Expression()
        {
            x = 0; y = 0; z = 0; w = 0;
        }
        public Expression(int a, int b, int c, int d)
        {
            Random rnd = new Random();
            x = a; y = b; z = c; w = d;
            f = rnd.Next(0, 2);
        }
        public Expression(int a, int b, int c, int d, int e)
        {
            x = a; y = b; z = c; w = d; f = e;
        }
        public void Init(int a, int b, int c, int d)
        {
            Random rnd = new Random();
            x = a; y = b; z = c; w = d;
            f = rnd.Next(0, 2);
        }
        public void Init(int a, int b, int c, int d, int e)
        {
            x = a; y = b; z = c; w = d; f = e;
        }
        public void Show()
        {
            Console.Write("(");
            ShowX();
            ShowY();
            ShowZ();
            ShowW();
            Console.Write(" > ");
            ShowF();
            Console.WriteLine(")");
        }
        public void ShowX()
        {
            if (x > 0)
            {
                Console.Write("X");
            }
            if (x == 0)
            {
                Console.Write("-X");
            }
        }
        public void ShowY()
        {
            if (y > 0)
            {
                Console.Write("Y");
            }
            if (y == 0)
            {
                Console.Write("-Y");
            }
        }
        public void ShowZ()
        {
            if (z > 0)
            {
                Console.Write("Z");
            }
            if (z == 0)
            {
                Console.Write("-Z");
            }
        }
        public void ShowW()
        {
            if (w > 0)
            {
                Console.Write("W");
            }
            if (w == 0)
            {
                Console.Write("-W");
            }
        }
        public void ShowF()
        {
            if (f > 0)
            {
                Console.Write("1");
            }
            if (f == 0)
            {
                Console.Write("0");
            }
        }
        public static bool operator !=(Expression ExOne, Expression ExTwo)
        {
            if (ExOne.x == ExTwo.x && ExOne.y == ExTwo.y && ExOne.z == ExTwo.z && ExOne.w == ExTwo.w)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public static bool operator ==(Expression ExOne, Expression ExTwo)
        {
            if (ExOne.x == ExTwo.x && ExOne.y == ExTwo.y && ExOne.z == ExTwo.z && ExOne.w == ExTwo.w)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
    class ExList : List<Expression>
    {
        public ExList() { }
        public ExList(ExList baselist)
        {
            foreach(Expression item in baselist)
            {
                this.Add(item);
            }
        }
        public void Init()
        {
            for(int i = 0; i < 16; i++)
            {
                string str = Convert.ToString(i, 2);
                while(str.Length < 4)
                {
                    str = "0" + str;
                }
                this.Add(new Expression(str[0] - '0', str[1] - '0', str[2] - '0', str[3] - '0'));
            }
        }
        public void Init(string vec)
        {
            for (int i = 0; i < 16; i++)
            {
                string str = Convert.ToString(i, 2);
                while (str.Length < 4)
                {
                    str = "0" + str;
                }
                this.Add(new Expression(str[0] - '0', str[1] - '0', str[2] - '0', str[3] - '0', vec[i] - '0'));
            }
        }
        public void Show()
        {
            for(int i = 0; i < this.Count; i++)
            {
                this[i].Show();
            }
        }
        public void BuildSDNF()
        {
            int count = 0;
            while(count < this.Count)
            {
                if (this[count].f == 0)
                {
                    this.RemoveAt(count);
                }
                else
                {
                    count++;
                }
            }
        }
        public bool IsConnectable(Expression ExOne, Expression ExTwo)
        {
            int count = 0;
            if (ExOne.x != ExTwo.x)
            {
                count++;
            }
            if (ExOne.y != ExTwo.y)
            {
                count++;
            }
            if (ExOne.z != ExTwo.z)
            {
                count++;
            }
            if (ExOne.w != ExTwo.w)
            {
                count++;
            }
            if (count == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public void Connect()
        {
            ExList newList = new ExList();
            int current = 0;
            int compare;
            while (current < this.Count)
            {
                compare = current + 1;
                while (compare < this.Count)
                {
                    if (IsConnectable(this[current], this[compare]))
                    {
                        Expression tmp = new Expression();
                        if (this[current].x != this[compare].x)
                        {
                            tmp.Init(-1, this[current].y, this[current].z, this[current].w, 1);
                        }
                        if (this[current].y != this[compare].y)
                        {
                            tmp.Init(this[current].x, -1, this[current].z, this[current].w, 1);
                        }
                        if (this[current].z != this[compare].z)
                        {
                            tmp.Init(this[current].x, this[current].y, -1, this[current].w, 1);
                        }
                        if (this[current].w != this[compare].w)
                        {
                            tmp.Init(this[current].x, this[current].y, this[current].z, -1, 1);
                        }
                        this[current].IsLonely = false;
                        this[compare].IsLonely = false;
                        newList.Add(tmp);
                    }
                    compare++;
                }
                if (this[current].IsLonely)
                {
                    newList.Add(this[current]);
                }
                current++;
            }
            bool NeedAnotherRound = false;
            foreach (Expression item in this)
            {
                if(item.IsLonely == false)
                {
                    NeedAnotherRound = true;
                }
            }
            this.Clear();
            foreach (Expression item in newList)
            {
                item.IsLonely = true;
                this.Add(item);
            }
            if(NeedAnotherRound)
            {
                Connect();
            }
        }
        public void Compare(ExList baselist)
        {
            Console.Write("            ");
            foreach(Expression item in baselist)
            {
                Console.Write("(");
                item.ShowX();
                item.ShowY();
                item.ShowZ();
                item.ShowW();
                Console.Write(" > ");
                item.ShowF();
                Console.Write(")");
            }
            Console.WriteLine();
            foreach(Expression i in this)
            {
                Console.Write("(");
                i.ShowX();
                i.ShowY();
                i.ShowZ();
                i.ShowW();
                Console.Write(" > ");
                i.ShowF();
                Console.Write(")      ");
                foreach (Expression j in baselist)
                {
                    if (((i.x == -1) || (i.x == j.x)) && ((i.y == -1) || (i.y == j.y)) && ((i.z == -1) || (i.z == j.z)) && ((i.w == -1) || (i.w == j.w)))
                    {
                        Console.Write("     +      ");
                    }
                    else
                    {
                        Console.Write("            ");
                    }
                }
                Console.WriteLine();
            }
        }
        public void Distinct()
        {
            int current = 0;
            while (current < this.Count - 1)
            {
                if (this[current] == this[current + 1])
                {
                    this.RemoveAt(current + 1);
                }
                else
                {
                    current++;
                }
            }
        }
    }
    internal class MainClass
    {
        static void Main()
        {
            // Инициализация
            ExList list = new ExList() { };
            // 0101110101011101 > W, Y-Z
            list.Init("0100000000001111");
            list.Show();

            Console.WriteLine("Building SDNF: ");
            list.BuildSDNF();
            ExList listCopy = new ExList(list);
            list.Show();

            Console.WriteLine("Connecting...");
            list.Connect();
            list.Distinct();
            list.Show();

            Console.WriteLine("Result: ");
            list.Compare(listCopy);
        }
    }
}