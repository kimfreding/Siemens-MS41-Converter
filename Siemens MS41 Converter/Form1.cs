using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace Siemens_MS41_Converter
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public class sim
        {
            //public static string file;
            public static double length;
            public static byte[] buffFull;
            public static byte[] buffSave;
            //public static int offset = 0;
            //public static ushort initial = 0;
            //public static bool chksumcorr = false;
            //public static int NumberOfChecksumsCorrected = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (ofd.ShowDialog() != DialogResult.OK)
                return;

            sim.length = new System.IO.FileInfo(this.ofd.FileName).Length; // calculate file lenght
            if (sim.length != 262144) // test if file has correct size
            {
                label1.Text = "Wrong file size!";
                return;
            }

            sim.buffFull = File.ReadAllBytes(ofd.FileName);
            sim.buffSave = new byte[sim.buffFull.Length];

            if (sim.buffFull[0x14000] == 0x4E)
            {
                ConvToMem();
                File.WriteAllBytes(Path.ChangeExtension(ofd.FileName, ".mem"), sim.buffSave);
                label1.Text = "Ram image saved with .ram ext";
            }

            if (sim.buffFull[0x10000] == 0x4E)
            {
                ConvToRom();
                File.WriteAllBytes(Path.ChangeExtension(ofd.FileName, ".rom"), sim.buffSave);
                label1.Text = "Rom image saved with .rom ext";
            }

            
        }

        public void ConvToMem()
        {
            // Convert to memory

            // 0x00000 - 0x10000
            Buffer.BlockCopy(sim.buffFull, 0x04000, sim.buffSave, 0x00000, 0x4000);
            Buffer.BlockCopy(sim.buffFull, 0x00000, sim.buffSave, 0x04000, 0x4000);
            Buffer.BlockCopy(sim.buffFull, 0x0c000, sim.buffSave, 0x08000, 0x4000);
            Buffer.BlockCopy(sim.buffFull, 0x08000, sim.buffSave, 0x0C000, 0x4000);

            // 0x10000 - 0x20000
            Buffer.BlockCopy(sim.buffFull, 0x14000, sim.buffSave, 0x10000, 0x4000);
            Buffer.BlockCopy(sim.buffFull, 0x10000, sim.buffSave, 0x14000, 0x4000);
            Buffer.BlockCopy(sim.buffFull, 0x1c000, sim.buffSave, 0x18000, 0x4000);
            Buffer.BlockCopy(sim.buffFull, 0x18000, sim.buffSave, 0x1C000, 0x4000);

            // 0x20000 - 0x30000
            Buffer.BlockCopy(sim.buffFull, 0x24000, sim.buffSave, 0x20000, 0x4000);
            Buffer.BlockCopy(sim.buffFull, 0x20000, sim.buffSave, 0x24000, 0x4000);
            Buffer.BlockCopy(sim.buffFull, 0x2c000, sim.buffSave, 0x28000, 0x4000);
            Buffer.BlockCopy(sim.buffFull, 0x28000, sim.buffSave, 0x2C000, 0x4000);

            // 0x30000 - 0x40000
            Buffer.BlockCopy(sim.buffFull, 0x34000, sim.buffSave, 0x30000, 0x4000);
            Buffer.BlockCopy(sim.buffFull, 0x30000, sim.buffSave, 0x34000, 0x4000);
            Buffer.BlockCopy(sim.buffFull, 0x3c000, sim.buffSave, 0x38000, 0x4000);
            Buffer.BlockCopy(sim.buffFull, 0x38000, sim.buffSave, 0x3C000, 0x4000);
        }

        public void ConvToRom()
        {
            // 0x00000 - 0x10000
            Buffer.BlockCopy(sim.buffFull, 0x00000, sim.buffSave, 0x04000, 0x4000);
            Buffer.BlockCopy(sim.buffFull, 0x04000, sim.buffSave, 0x00000, 0x4000);
            // fill up space used by ram with ff`s
            for (int i = 0x08000; i < 0x0c000; i++)
            {
                sim.buffSave[i] = 0xFF;
            }
            // Buffer.BlockCopy(sim.buffFull, 0x0C000, sim.buffSave, 0x08000, 0x4000); // skip ram
            Buffer.BlockCopy(sim.buffFull, 0x08000, sim.buffSave, 0x0C000, 0x4000);

            // 0x10000 - 0x20000
            Buffer.BlockCopy(sim.buffFull, 0x10000, sim.buffSave, 0x14000, 0x4000);
            Buffer.BlockCopy(sim.buffFull, 0x14000, sim.buffSave, 0x10000, 0x4000);
            for (int i = 0x18000; i < 0x20000; i++)
            {
                sim.buffSave[i] = 0xFF;
            }
            //Buffer.BlockCopy(sim.buffFull, 0x1c000, sim.buffSave, 0x18000, 0x4000);
            //Buffer.BlockCopy(sim.buffFull, 0x18000, sim.buffSave, 0x1C000, 0x4000);

            // 0x20000 - 0x30000
            Buffer.BlockCopy(sim.buffFull, 0x20000, sim.buffSave, 0x24000, 0x4000);
            Buffer.BlockCopy(sim.buffFull, 0x24000, sim.buffSave, 0x20000, 0x4000);
            Buffer.BlockCopy(sim.buffFull, 0x28000, sim.buffSave, 0x2C000, 0x4000);
            Buffer.BlockCopy(sim.buffFull, 0x2C000, sim.buffSave, 0x28000, 0x4000);

            // 0x30000 - 0x40000
            Buffer.BlockCopy(sim.buffFull, 0x30000, sim.buffSave, 0x34000, 0x4000);
            Buffer.BlockCopy(sim.buffFull, 0x34000, sim.buffSave, 0x30000, 0x4000);
            Buffer.BlockCopy(sim.buffFull, 0x38000, sim.buffSave, 0x3C000, 0x4000);
            Buffer.BlockCopy(sim.buffFull, 0x3C000, sim.buffSave, 0x38000, 0x4000);


        }
    }
}
