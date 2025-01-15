using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChipSecuritySystem
{
    internal class Program
    {
      
        private static readonly List<ColorChip> ChipSet = new List<ColorChip>();

        private static void Main()
        {

            Console.Clear();
            Console.WriteLine("Enter Chipset to Unlock Master Panel\n");

            //Display Color Options & Set Input Values
            DisplayChipsetValues();
            Console.WriteLine(); 

            var colorSet = new HashSet<ColorChip>();

            var chipsetColor = ChipSet.Where(chip => chip.StartColor == Color.Blue);
            foreach (var slot in chipsetColor)
            {

                colorSet.Add(slot);
                if (ValidateSequence(slot, colorSet))
                {
                    Console.WriteLine("\nDisplay Sequence\n");

                    foreach (var c in colorSet)
                    {
                        Console.WriteLine("[" + c + "]");
                    }

                    Console.WriteLine("\nMaster panel unlocked!\n");
                    Console.ReadKey();
                    return;
                }

                colorSet.Remove(slot);
            }

            Console.WriteLine(Constants.ErrorMessage + "!\n");
            Console.ReadKey();
        }

        private static void DisplayChipsetValues()
        {
            Console.WriteLine("\nEnter one of the following colors options:");

            var options = Enum.GetValues(typeof(Color));
            for (var i = 0; i < options.Length; i++)
            {
                Console.WriteLine($"{i + 1}. {options.GetValue(i)}");
            }

            for (var j = 0; j < 4; j++)
            {
                Console.WriteLine($"\nChipset {j + 1}:");

                Color initialValue = SetColorValues(nameof(initialValue));
                Color endValue = SetColorValues(nameof(endValue));
                ChipSet.Add(new ColorChip(initialValue, endValue));
            }
        }

        private static Color SetColorValues(string setColorPosition)
        {
            Console.Write($"{setColorPosition}: ");

            // Validate selected option
            int option;
            while (!int.TryParse(Console.ReadLine(), out option) || option < 1 || option > Enum.GetValues(typeof(Color)).Length)
            {
                Console.WriteLine("Invalid option. Please try again.");
            }
            return (Color)(option - 1);
        }


        private static bool ValidateSequence(ColorChip lastSlot, ISet<ColorChip> chipSet)
        {
            if (lastSlot.EndColor == Color.Green)
            {
                return true;
            }

            var chipSetValues = ChipSet.Where(c => c.StartColor == lastSlot.EndColor);
            //foreach (var nextSlot in ChipSet.Where(c => c.StartColor == lastSlot.EndColor))
            foreach (var nextSlot in chipSetValues)
            {
                if (!chipSet.Add(nextSlot)) continue;

                if (ValidateSequence(nextSlot, chipSet))
                {
                    return true;
                }

                chipSet.Remove(nextSlot);
            }

            return false;
        }
    }
}
