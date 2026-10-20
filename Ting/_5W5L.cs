using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using static Ting.Report;

namespace Ting
{
    public class Report(ulong How_long, IProgress<how_long> progess)
    {
        public struct how_long
        {
            public ulong How_far = 0; public ulong total;

            public how_long(ulong How_far, ulong total)
            {
                this.How_far = How_far;
                this.total = total;
            }
        }
        static object locker_list = new object();

        ulong How_far = 0;

        bool Rerun = true;

        public void report(ulong count)
        {
            lock (locker_list)
            {
                How_far += count;
            }
        }
        public async void update()
        {
            while(Rerun)
            {
                Thread.Sleep(50);

                how_long how_longer = new how_long(How_far, How_long);
                progess.Report(how_longer);

            }
        }

        public void dispose()
        {
            Rerun = false;
        }

    }
    public class _5W5L
    {

        static List<uint[]> wait_word_arr = new List<uint[]>(4096);
        static List<uint[]> bit_masks_tjek = new List<uint[]>();
        static object locker_list = new object();
        static Dictionary<uint, List<string>> Bit_Masks = new Dictionary<uint, List<string>>();
        static List<string> ValidWords = new List<string>();


        public static void bits(string filename, List<string> Text, IProgress<Report.how_long> progess)
        {

            // hvor mang threads skal køre
            int t = 14;


            string[] file_word = File.ReadAllLines(filename);


            // Ser kun hvor lang orderne er.
            foreach (string word in file_word)
            {
                if (word.Length == 5)
                {
                    bool isgood = true;
                    for (int q = 0; q < word.Length - 1; q++)
                    {
                        if (word[q] == word[q + 1])
                        {
                            isgood = false;
                            break;
                        }

                    }
                    if (isgood)
                    {

                        uint wait_mask = 0;

                        // looper i gemme hver char
                        foreach (char ch in word.ToCharArray())
                        {
                            // renger ud hvad bit nr det er
                            int bit = ch - 'a';

                            //tilføger wait mask - |= Betyder at den compare de to på hver sin side - 1u det er en unsign int
                            wait_mask |= 1u << bit;
                        }

                        if (Bit_Masks.TryGetValue(wait_mask, out List<string> wait_good_words))
                        {
                            wait_good_words.Add(word);
                        }
                        else
                        {
                            List<string> wait_words = new List<string>();
                            wait_words.Add(word);
                            Bit_Masks.Add(wait_mask, wait_words);
                        }

                    }
                }
            }


            Report report = new Report((ulong)Math.Pow(Bit_Masks.Count, 5), progess);

            Task.Run(() =>
            {
                report.update();
            });

            Parallel.For(1, t + 1, (id) => Words_counter(id, t, Bit_Masks, report));
            List<uint> All_Bit_masks = new List<uint>();
            int count = 0;
            int word_count = 0;
            foreach (var bit_array in wait_word_arr)
            {
                for (int w = 0; w < bit_masks_tjek.Count; w++)
                {
                    word_count = 0;
                    foreach (var ex_word_tjek in bit_masks_tjek[w])
                    {
                        foreach (var word_tjek in bit_array)
                        {
                            if (ex_word_tjek == word_tjek)
                            {
                                word_count++;
                                continue;
                            }
                        }
                    }
                    if (word_count == 5)
                    {
                        break;
                    }
                }
                if (word_count == 5)
                {
                    word_count = 0;
                    continue;
                }
                bit_masks_tjek.Add(bit_array);

                foreach (var word_0 in Bit_Masks.GetValueOrDefault(bit_array[0]))
                {
                    foreach (var word_1 in Bit_Masks.GetValueOrDefault(bit_array[1]))
                    {
                        foreach (var word_2 in Bit_Masks.GetValueOrDefault(bit_array[2]))
                        {
                            foreach (var word_3 in Bit_Masks.GetValueOrDefault(bit_array[3]))
                            {
                                foreach (var word_4 in Bit_Masks.GetValueOrDefault(bit_array[4]))
                                {

                                    All_Bit_masks.Add(bit_array[0]);
                                    string final_word = word_0 + " " + word_1 + " " + word_2 + " " + word_3 + " " + word_4;
                                    Text.Add(final_word);
                                    count++;

                                }
                            }
                        }
                    }

                }
            }
            report.dispose();
        }

        static void Words_counter(int id, int t, Dictionary<uint, List<string>> Bit_Maks, Report report)
        {
            List<uint> Keys = new List<uint>(Bit_Maks.Keys);
            List<uint[]> wait_word = new List<uint[]>(4096);
            ulong search = 0;

            for (int x = id * (Keys.Count() / t) - (Keys.Count() / t); x < (Keys.Count() / t) + (id * (Keys.Count() / t) - (Keys.Count() / t)); x++)
            {

                for (int y = 0; y < Keys.Count(); y++)
                {
                    if ((Keys[x] & Keys[y]) != 0)
                    {
                        search += (ulong)Math.Pow(Bit_Maks.Count, 4);
                        continue;
                    }
                    uint key_1 = (Keys[x] | Keys[y]);

                    for (int z = 0; z < Keys.Count(); z++)
                    {
                        if ((key_1 & Keys[z]) != 0)
                        {
                            search += (ulong)Math.Pow(Bit_Maks.Count, 3);
                            continue;
                        }
                        uint key_2 = (key_1 | Keys[z]);
                        for (int i = 0; i < Keys.Count(); i++)
                        {
                            if ((key_2 & Keys[i]) != 0)
                            {
                                search += (ulong)Math.Pow(Bit_Maks.Count, 2);
                                continue;
                            }
                            uint key_3 = (key_2 | Keys[i]);
                            for (int j = 0; j < Keys.Count(); j++)
                            {
                                if ((key_3 & Keys[j]) != 0)
                                {
                                    search += (ulong)Math.Pow(Bit_Maks.Count, 1);
                                    continue;
                                }
                                uint key_4 = (key_3 | Keys[j]);

                                search += 1;

                                uint[] wait_words = { Keys[x], Keys[y], Keys[z], Keys[i], Keys[j] };

                                wait_word.Add(wait_words);

                                if (wait_word.Count() == 4096)
                                {
                                    lock (locker_list)
                                    {
                                        wait_word_arr.AddRange(wait_word);
                                        wait_word.Clear();
                                    }
                                    report.report(search);
                                }



                            }
                        }
                    }
                }
            }
            lock (locker_list)
            {
                wait_word_arr.AddRange(wait_word);
            }
            wait_word.Clear();
            report.report(search);
        }


    }
}
