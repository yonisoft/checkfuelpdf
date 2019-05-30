using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace pdflooker
{
    class Program
    {

        ///hearot//
        ///https://www.velocityfleet.com/invoices/view/[invoiceid]/
        ///////////


        ///////cfg vars//////
        static int literlimit = 100; // the min number to display if the license plate over.
        static string[] regnums = { "WU12CWG", "DE62OZK", "VE62OZA", "CA12ACY", "KX12WJF" }; // license plate to check.
        static string locofghost = @"C:\Program Files\gs\gs9.27\bin\gswin64c.exe"; // The location of Ghost exe file.
        static string locofpdfs = @"C:\Users\Dan\Desktop\avs\"; // The location of pdf files to convert to txt.
        static string locoftxts = @"C:\Users\Dan\Desktop\avs\"; // The location of txt files to read them.
        /////////////////////
        /////program vars////
        static int numar = 0; //count the lines of num result search.
        static string[] savelinez = new string[900]; // save the founded line in the txt.
        static double[] savelined = new double[900]; // save the value in double.
        static double sumofliter = 0.0; // the result of the liters from the calculate readtxt function.
        static int lengthA = regnums.Length; // the numbers of license plate in regnums.
        ////////////////////
        static void Main(string[] args)
        {
            //////////

            ///

            /*Console.WriteLine("Do you want to convert all pdf to text[Answer only 'y' or 'n']");
            string answer = Console.ReadLine(); // the answer for convert pdf to text.
            if (answer.Equals("y") == true)
            {
                pdftext();
                System.Threading.Thread.Sleep(200);
                launchreadtxt();
            }
            else
            {
                try
                {
                    string[] dirs = Directory.GetFiles(locoftxts, "*.txt");
                    foreach (string dir in dirs)
                    {
                        readtxt(dir);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("The process failed: {0}", e.ToString());
                }
            }
            */
            pdftext();
            System.Threading.Thread.Sleep(200);
            launchreadtxt();
            Console.WriteLine("All files tested.\nThx to Yoni Albu that create the program :)");
            Console.ReadKey();
        }

        static void launchreadtxt()
        {
            try
            {
                string[] dirs = Directory.GetFiles(locoftxts, "*.txt");
                foreach (string dir in dirs)
                {
                    readtxt(dir);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The process failed: {0}", e.ToString());
            }
        }

        static void readtxt(string zbz)
        {
            string testerror = ""; //Check is double is getting.
            for (int bi = 0; bi < lengthA; bi++)
            {
                testerror = ""; // reset value
                sumofliter = 0; // reset  value
                testerror = ""; // reset value
                numar = 0; //reset value
                Array.Clear(savelined, 0, savelined.Length);
                IEnumerable<string> lines = File.ReadAllLines(zbz);

                //We read the input from the user
                //Console.Write("Enter the word to search: ");
                string input = regnums[bi].Trim();
                //string input = "CA12ACY".Trim();

                //We identify the matches. If the input is empty, then we return no matches at all
                IEnumerable<string> matches = !String.IsNullOrEmpty(input) ? lines.Where(line => line.IndexOf(input, StringComparison.OrdinalIgnoreCase) >= 0) : Enumerable.Empty<string>();

                string lala = String.Join("#", matches); //make lines into string with # to split them to arrays.
                if (String.IsNullOrEmpty(lala))
                {
                    //no exists reg.
                }
                else
                {
                    string[] words = lala.Split('#');
                    foreach (var word in words)
                    {
                        savelinez[numar] = word;
                        //Console.WriteLine(savelinez[numar]);
                        numar++;
                    }
                    for (int i = 0; i < numar; i++)
                    {
                        try
                        {
                            //Console.WriteLine(savelinez[i]);
                            //Regex regex = new Regex("Bunker(.+?) L"); for UK Fuels
                            string[] substrings = Regex.Split(savelinez[i], "DIESEL");
                            //Console.WriteLine(substrings[0]);

                            testerror = substrings[1];
                            if (String.IsNullOrEmpty(testerror))
                            {
                                savelinez[i] = "0";
                            }
                            else
                            {
                                savelinez[i] = substrings[1];
                                savelinez[i] = savelinez[i].Replace(" ", ""); // remove whitespaces
                                //Console.WriteLine(savelinez[i]);
                            }
                            //Console.WriteLine(savelinez[i]);
                            savelined[i] = Convert.ToDouble(savelinez[i]);
                        }
                        catch
                        {
                            continue;
                        }
                        
                    }
                    for (int i = 0; i < numar; i++)
                    {
                        sumofliter = sumofliter + savelined[i];
                        //Console.WriteLine("sum: " + sumofliter);
                    }
                    //Console.WriteLine(sumofliter);
                    if (sumofliter > literlimit) Console.WriteLine(input + " in file: " + zbz + " is: " + sumofliter);
                    //Console.WriteLine(zbz + " Checked.");
                }
            }
        }

        static void pdftext()
        {
            try
            {
                // Only get files that begin with the letter "c".
                string[] dirs = Directory.GetFiles(locofpdfs, "*.pdf");
                //Console.WriteLine("Number of files {0}.", dirs.Length);
                foreach (string dir in dirs)
                {
                    string cmdz = @"""" + locofghost + @"""" + " -sDEVICE=txtwrite -o " + dir.Remove(dir.Length - 4) + ".txt" + " " + dir.Remove(dir.Length - 4) + ".pdf"; // command of convert pdf to txt
                    //Console.WriteLine(dir);

                    ////////cmd execute//////
                    Process cmd = new Process();
                    cmd.StartInfo.FileName = "cmd.exe";
                    cmd.StartInfo.RedirectStandardInput = true;
                    cmd.StartInfo.RedirectStandardOutput = true;
                    cmd.StartInfo.CreateNoWindow = true;
                    cmd.StartInfo.UseShellExecute = false;
                    cmd.Start();

                    cmd.StandardInput.WriteLine(cmdz);
                    cmd.StandardInput.Flush();
                    cmd.StandardInput.Close();
                    cmd.WaitForExit();
                    //Console.WriteLine(cmd.StandardOutput.ReadToEnd());
                    ///////////////
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The process failed: {0}", e.ToString());
            }
        }
    }
}
