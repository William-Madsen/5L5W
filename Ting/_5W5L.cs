using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace Ting
{
    public class _5W5L
    {

        static List<uint[]> wait_word_arr = new List<uint[]>(4096);
        static List<uint[]> bit_masks_tjek = new List<uint[]>();
        static object locker_list = new object();
        static Dictionary<uint, List<string>> Bit_Masks = new Dictionary<uint, List<string>>();
        static List<string> ValidWords = new List<string>();

        public static void bits(string filename, ObservableCollection<string> Text)
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





            Parallel.For(1, t + 1, (id) => Words_counter(id, t, Bit_Masks));
            List<uint> All_Bit_masks = new List<uint>();
            int count = 0;
            int word_count = 0;
            foreach (var bit_array in wait_word_arr)
            {
                for (int w = 0; w < bit_masks_tjek.Count; w++)
                {
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
                    continue;
                }
                word_count = 0;
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

                                    uint already_ex = (bit_array[0]);
                                    if (All_Bit_masks.Contains(already_ex))
                                    {
                                        continue;
                                    }
                                    else
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
            }

        }

        static void Words_counter(int id, int t, Dictionary<uint, List<string>> Bit_Maks)
        {
            List<uint> Keys = new List<uint>(Bit_Maks.Keys);
            List<uint[]> wait_word = new List<uint[]>(4096);


            for (int x = id * (Keys.Count() / t) - (Keys.Count() / t); x < (Keys.Count() / t) + (id * (Keys.Count() / t) - (Keys.Count() / t)); x++)
            {

                for (int y = 0; y < Keys.Count(); y++)
                {
                    if ((Keys[x] & Keys[y]) != 0)
                    {
                        continue;
                    }
                    uint key_1 = (Keys[x] | Keys[y]);

                    for (int z = 0; z < Keys.Count(); z++)
                    {
                        if ((key_1 & Keys[z]) != 0)
                        {
                            continue;
                        }
                        uint key_2 = (key_1 | Keys[z]);
                        for (int i = 0; i < Keys.Count(); i++)
                        {
                            if ((key_2 & Keys[i]) != 0)
                            {
                                continue;
                            }
                            uint key_3 = (key_2 | Keys[i]);
                            for (int j = 0; j < Keys.Count(); j++)
                            {
                                if ((key_3 & Keys[j]) != 0)
                                {
                                    continue;
                                }
                                uint key_4 = (key_3 | Keys[j]);



                                uint[] wait_words = { Keys[x], Keys[y], Keys[z], Keys[i], Keys[j] };

                                wait_word.Add(wait_words);

                                if (wait_word.Count() == 4096)
                                {
                                    lock (locker_list)
                                    {
                                        wait_word_arr.AddRange(wait_word);

                                    }
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
        }


    }
}