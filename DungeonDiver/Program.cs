using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class program
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

        }

        static void loadGame()
        {

        }

        static void quitGame()
        {

        }
    }

 }
