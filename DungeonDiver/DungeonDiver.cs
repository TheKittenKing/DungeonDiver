using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace DungeonDiver
{
    class DungeonDiver
    {

        static void Main(string[] args)
        {
            string command = "";
            Regex Parser;

            Console.WriteLine("Welcome to Dungeon Diver.");
            if (File.Exists("./DungeonDiver.sav") == false)
            {
                Console.WriteLine("To start a new game game, type (n)ew"); //new and quit options
                Parser = new Regex("^n|N|q|Q");
            }
            else
            {
                Console.WriteLine("Start a (n)ew game or (l)oad a saved one?"); // new load and quit options
                Parser = new Regex("^n|N|l|L|q|Q");
            }



            do
                command = Console.ReadLine();
            while (Parser.IsMatch(command) == false);


            if (new Regex("^n|N").IsMatch(command))
                newGame();
            else if (new Regex("^l|L").IsMatch(command))
                loadGame();
            //else if ()
            else
                quitGame();

        }

        static void newGame()
        {
            
            string command = "";
            Console.WriteLine("Would you like to be a (Wiz)ard or a (War)rior?");
            do
            {
                command = Console.ReadLine();
            }
            while (new Regex("(?i)^war").IsMatch(command) == false && new Regex("(?i)^wiz").IsMatch(command) == false);
            if (new Regex("(?i)^war").IsMatch(command) == true)
            {
                //start with warrior stats
                Creature player = new Creature("warrior", 12, 14, 4, 2);
                playGame(player);
            }
            else
            {
                //start with wizard stats
                Creature player = new Creature("wizard", 7, 11, 4, 6);
                playGame(player);
            }
        }

        static void loadGame()
        {

        }

        static void playGame(Creature player) //main game loop in here.  fight monsters, check stats, etc
        {
            //in progress
            Random rand = new Random();
            XElement root = XElement.Load("MonsterList.xml");
            string monstercategory = string.Empty;
            string command = "";

            while (0 == 0)
            {
                Console.WriteLine("Would you like to practice or face a boss?");
                do
                {

                    command = Console.ReadLine();
                }
                while (new Regex("(?i)^p").IsMatch(command) == false && new Regex("(?i)^b").IsMatch(command) == false);

                if (new Regex("(?i)^p").IsMatch(command) == true)
                    monstercategory = "lowlevel";
                else
                    monstercategory = "highlevel";
 
                string monstertag = rand.Next(1, 4).ToString();
                IEnumerable<XElement> mon = from el in root.Elements("monster")
                                            where (string)el.Attribute("tag") == (string)monstertag && (string)el.Attribute("category") == "lowlevel"
                                            select el;
                int hp = 1;
                int ac = 1;
                int dmg = 1;
                int tohit = 1;
                string name = "missingno";


                foreach (XElement el in mon)
                {
                    Console.WriteLine(el.Attribute("name").Value);
                    Console.WriteLine(el.Element("hitpoints").Value);
                    Console.WriteLine(el.Element("ac").Value);
                    Console.WriteLine(el.Element("tohit").Value);
                    name = el.Attribute("name").Value;
                    hp = Convert.ToInt16(el.Element("hitpoints").Value);
                    ac = Convert.ToInt16(el.Element("ac").Value);
                    dmg = Convert.ToInt16(el.Element("tohit").Value);
                    tohit = Convert.ToInt16(el.Element("dmg").Value);
                }
                Creature opponent = new Creature(name, hp, ac, tohit, dmg);
                Console.WriteLine("You come across a " + opponent.Name);
                while (opponent.HitPoints > 0)
                {
                    Console.WriteLine("The " + opponent.Name + " has " + opponent.HitPoints + " hitpoints left"); //replace with string approximation of health condition
                    command = Console.ReadLine();
                    if (new Regex("(?i)^a").IsMatch(command) == true)
                    {
                        //attack
                        player.attack(ref opponent);
                        opponent.attack(ref player);

                        if (player.HitPoints <= 0)
                        {
                            Console.WriteLine("You have been defeated.");
                        }
                        if (opponent.HitPoints <= 0)
                        {
                            Console.WriteLine("You have slain the " + opponent.Name);
                        }
                    }
                    else if (new Regex("(?i)^cha").IsMatch(command) == true)
                    {
                        //examine character sheet
                        Console.WriteLine("You are " + player.Name + ". You have " + player.HitPoints + " hitpoints remaining.");
                    }
                }
            }


        }
        static void quitGame()
        {

        }

        static void getRandomMonster(ref String Name, ref int hp, ref int ac, ref int tohit, ref int dmg, bool lowlevel = true)
        {
            XmlTextReader reader = new XmlTextReader("MonsterList.xml");
            Random rand = new Random();

            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element: // The node is an element.
                        Console.Write("<" + reader.Name);
                        Console.WriteLine(">");
                        break;
                    case XmlNodeType.Text: //Display the text in each element.
                        Console.WriteLine(reader.Value);
                        break;
                    case XmlNodeType.EndElement: //Display the end of the element.
                        Console.Write("</" + reader.Name);
                        Console.WriteLine(">");
                        break;
                }
            }
        }
    }

    class Creature
    {
        private String name;
        private int hitpoints;
        private int ac;
        private int tohit;
        private int damage;
        private bool isAlive;

        public Creature(String name, int hp, int ac, int tohit, int dmg)
        {
            this.name = name;
            hitpoints = hp;
            this.ac = ac;
            this.tohit = tohit;
            damage = dmg;
        }

        public String Name
        {
            get { return name; }
            set { name = value; }
        }

        public int HitPoints
        {
            get { return hitpoints; }
        }

        public int ArmorClass
        {
            get { return ac; }
        }


        public bool recieveHit(int hitroll, int damageroll)
        {
            if (hitroll < ac)
            {
                Console.WriteLine("Roll - " + hitroll + " misses");
                return false;
            }
            else
            {
                hitpoints -= damageroll;
                Console.WriteLine("Roll - " + hitroll + " hits for " + damageroll + " damage");
                if (hitpoints <= 0)
                    isAlive = false;
                return true;
            }
        }   
        
        public bool attack(ref Creature target)
        {
            Random rand = new Random(); //342 seed for no particularreason
           // Console.WriteLine("<DEBUG> damage = " + damage);
            return target.recieveHit(rand.Next(1, 20) + tohit, rand.Next(1, damage));
        }
    }
    
 }
