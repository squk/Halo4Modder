using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HaloDevelopmentExtender;
using HaloReach3d.Helpers;
using XDevkit;
using System.Xml;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Security.Cryptography;

namespace Halo_4_Modder
{
    public partial class Form1 : DevComponents.DotNetBar.Office2007Form
    {
        Dictionary<string, string> helmets = new Dictionary<string, string>();

        public Form1()
        {
            InitializeComponent();
            checkForUpdate();
            helmets.Add("Recruit", "00");
            helmets.Add("Warrior", "01");
            helmets.Add("Scout", "02");
            helmets.Add("Soldier", "03");
            helmets.Add("Recon", "04");
            helmets.Add("Hazop", "05");
            helmets.Add("E.O.D", "06");
            helmets.Add("Pioneer", "07");
            helmets.Add("Infiltrator", "08");
            helmets.Add("Orbital", "09");
            helmets.Add("Air Assault", "0A");
            helmets.Add("Vanguard", "0B");
            helmets.Add("Enforcer", "0C");
            helmets.Add("Gungnir", "0D");
            helmets.Add("Aviator", "0E");
            helmets.Add("Pathfinder", "0F");
            helmets.Add("C.I.O", "10");
            helmets.Add("Fotus", "11");
            helmets.Add("Oceanic", "12");
            helmets.Add("Defender", "13");
            helmets.Add("Operator", "14");
            helmets.Add("Venator", "15");
            helmets.Add("Commando", "16");
            helmets.Add("Wetwork", "17");
            helmets.Add("War Master", "18");
            helmets.Add("Raider", "19");
            helmets.Add("Ranger", "1A");
            helmets.Add("E.V.A", "1B");
            helmets.Add("Engineer", "1D");
            helmets.Add("Stalker", "1E");
            helmets.Add("Rogue", "1F");
            helmets.Add("Tracker", "20");
            helmets.Add("Mark VI", "21");
            helmets.Add("Protector", "22");
            helmets.Add("Deadeye", "23");
            helmets.Add("Locus", "24");
            helmets.Add("Scanner", "25");
            helmets.Add("Strider", "26");
            ////////////////////////////////
            helmets.Add("Air Assault VERG", "27");
            helmets.Add("Hazop FRST", "28");
            helmets.Add("Oceanic CRCT", "29");
            helmets.Add("Vanguard CNVG", "2A");
            helmets.Add("Infiltrator Trac", "2B");
            helmets.Add("Enforcer TRBL", "2C");
            helmets.Add("Engineer EDGE", "2D");
            helmets.Add("E.O.D. SHDW", "2E");
            helmets.Add("Gungnir PULS", "2F");
            helmets.Add("Aviator BOND", "30");
            helmets.Add("Wetwork SHRD", "31");
            helmets.Add("Pathfinder CORE", "32");
            helmets.Add("Orbital AEON", "33");
            helmets.Add("Raider DSTT", "34");
            helmets.Add("Recon SURG", "35");
            helmets.Add("Pioneer ADPT", "36");
            helmets.Add("Scout APEX", "37");
            helmets.Add("Soldier ZNTH", "38");
            helmets.Add("Warrior MTRX", "39");
            helmets.Add("Stalker CRSH", "3A");
            helmets.Add("Tracker ADRT", "3B");
            helmets.Add("C.I.O. WEB", "3C");
            helmets.Add("Commando FRCT", "3D");
            helmets.Add("E.V.A. BRCH", "3E");
            helmets.Add("Protector DRFT", "3F");
            helmets.Add("WarMaster PRML", "40");
            helmets.Add("Venator RPTR", "41");
            helmets.Add("Defender CTRL", "42");
            helmets.Add("Ranger STRK", "43");
            helmets.Add("Operator SRFC", "44");
            helmets.Add("Rogue FCUS", "45");
            helmets.Add("Recruit PRME", "46");

            helmetBox.DataSource = new BindingSource(helmets, null);
            helmetBox.DisplayMember = "Key";
            helmetBox.ValueMember = "Value";

            xdkName.Text = Properties.Settings.Default.xdkName;

        }

        private void pokeButton_Click(object sender, EventArgs e)
        {
            try
            {
                pokeXbox(Convert.ToUInt32(poke_offset.Text, 16), comboBox1.SelectedItem.ToString(), poke_value.Text);
            }
            catch
            {
                MessageBox.Show("Could not poke Changes.");
            }
        }

        public void pokeXbox(uint offset, string poketype, string ammount)
        {
            try
            {
                pokeXbox1(offset, poketype, ammount);
            }
            catch
            {
                MessageBox.Show("Could not poke Changes.");
            }
        }

        public void pokeXbox1(uint offset, string poketype, string ammount)
        {
            try
            {
                if (xdkName.Text == "")
                {
                    MessageBox.Show("XDK Name/IP not set");
                    return;
                }
                XboxDebugCommunicator Xbox_Debug_Communicator = new XboxDebugCommunicator(xdkName.Text);
                //Connect
                if (Xbox_Debug_Communicator.Connected == false)
                {
                    try
                    {
                        Xbox_Debug_Communicator.Connect();
                    }
                    catch { }
                }

                //Get the memoryStream
                XboxMemoryStream xbms = Xbox_Debug_Communicator.ReturnXboxMemoryStream();
                //Endian IO
                HaloReach3d.IO.EndianIO IO = new HaloReach3d.IO.EndianIO(xbms,
                    HaloReach3d.IO.EndianType.BigEndian);
                IO.Open();

                IO.Out.BaseStream.Position = offset;

                if (poketype == "Unicode String")
                    IO.Out.WriteUnicodeString(ammount, ammount.Length);
                if (poketype == "ASCII String")
                    IO.Out.WriteUnicodeString(ammount, ammount.Length);
                if (poketype == "String" | poketype == "string")
                    IO.Out.Write((string)ammount);
                if (poketype == "Float" | poketype == "float")
                    IO.Out.Write((float)float.Parse(ammount));
                if (poketype == "Double" | poketype == "double")
                    IO.Out.Write((double)double.Parse(ammount));
                if (poketype == "Short" | poketype == "short")
                    IO.Out.Write((short)Convert.ToUInt32(ammount, 16));
                if (poketype == "Byte" | poketype == "byte")
                    IO.Out.Write((byte)Convert.ToUInt32(ammount, 16));
                if (poketype == "Long" | poketype == "long")
                    IO.Out.Write((long)Convert.ToUInt32(ammount, 16));
                if (poketype == "Quad" | poketype == "quad")
                    IO.Out.Write((Int64)Convert.ToUInt64(ammount, 16));
                if (poketype == "Int" | poketype == "int")
                    IO.Out.Write(Convert.ToUInt32(ammount, 16));
                if (poketype == "Bytes" | poketype == "bytes")
                    IO.Out.Write(ExtraFunctions.HexStringToBytes(ammount), 0, ExtraFunctions.HexStringToBytes(ammount).Count());
                //IO.Out.Write((byte)Convert.ToUInt32(ammount, 16));

                IO.Close();
                xbms.Close();
                Xbox_Debug_Communicator.Disconnect();
            }
            catch
            {
                MessageBox.Show("Couldn't Poke XDK");
            }
        }

        public string getValue(uint offset, string type)
        {
            string hex = "X";
            //if (checkBox1.Checked) { hex = "X"; } else { hex = ""; }
            object rn = null;
            if (xdkName.Text != "")
            {
                XboxDebugCommunicator Xbox_Debug_Communicator = new XboxDebugCommunicator(xdkName.Text);
                //Connect
                if (Xbox_Debug_Communicator.Connected == false)
                {
                    try
                    {
                        Xbox_Debug_Communicator.Connect();
                    }
                    catch { }
                }

                //Get the memoryStream
                XboxMemoryStream xbms = Xbox_Debug_Communicator.ReturnXboxMemoryStream();
                //Endian IO
                HaloReach3d.IO.EndianIO IO = new HaloReach3d.IO.EndianIO(xbms,
                    HaloReach3d.IO.EndianType.BigEndian);
                IO.Open();
                //try
                //{
                //Go to our value position
                IO.In.BaseStream.Position = offset;

                if (type == "String" | type == "string")
                    rn = IO.In.ReadString();
                if (type == "Unicode String")
                    rn = IO.In.ReadUnicodeString(int.Parse(textBox4.Text));
                if (type == "ASCII String")
                    rn = IO.In.ReadAsciiString(int.Parse(textBox4.Text));

                if (type == "Float" | type == "float")
                    rn = IO.In.ReadSingle();
                if (type == "Double" | type == "double")
                    rn = IO.In.ReadDouble();
                if (type == "Short" | type == "short")
                    rn = IO.In.ReadInt16().ToString(hex);
                if (type == "Byte" | type == "byte")
                    rn = IO.In.ReadByte().ToString(hex);
                if (type == "Long" | type == "long")
                    rn = IO.In.ReadInt32().ToString(hex);
                if (type == "Quad" | type == "quad")
                    rn = IO.In.ReadInt64().ToString(hex);
                byte[] rnarray;
                if (type == "Bytes" | type == "bytes")
                {
                    rnarray = IO.In.ReadBytes(int.Parse(textBox4.Text));
                    rn = ExtraFunctions.BytesToHexString(rnarray);
                }

                IO.Close();
                xbms.Close();
                Xbox_Debug_Communicator.Disconnect();

                return rn.ToString();
            }
            else
            {
                MessageBox.Show("XDK Name/IP not set");
                return "No Console Detected";
            }
        }

        string gravityOffset = "0x82128CC8";
        string jumpHOffset = "0x82128AEC";
        string timeScaleOffset = "0x82128AD0";
        string godModeOffset = "0x82993be4";
        string infinAmmoOffset = "0x8296acbc";
        string[] infinShieldsOffset = new string[] { "0xc8665efc", "0xC85568FC", "0xC855A1FC", "0xC855A22C", "0xC855E79C", "0xC855E7CC", "0xC85ABF0C", "0xC85ABF3C"};
        string infinAmmoOffset2 = "0x82128D4C";
        string fallDamageOffset = "0x82014864";

        string reloadMapOffset = "0x842663B7";
        string endGameOffset = "0x842663B6";
        string rsaOffset = "0x8233392C";
        string noArmorOffset = "0x847FFB4E";
        string invisOffset = "0x847FFB43";

        private void pokeGravity_Click(object sender, EventArgs et)
        {
            pokeXbox(Convert.ToUInt32(gravityOffset, 16), "float", gravityBox.Text);
        }

        private void getGravity_Click(object sender, EventArgs e)
        {
            gravityBox.Text = getValue(Convert.ToUInt32(gravityOffset, 16), "float");
        }

        private void defaultGravity_Click(object sender, EventArgs e)
        {
            gravityBox.Text = "4.1712594";
        }

        private void pokeJumpH_Click(object sender, EventArgs e)
        {
            pokeXbox(Convert.ToUInt32(jumpHOffset, 16), "float", jumpHBox.Text);
        }

        private void getJumpH_Click(object sender, EventArgs e)
        {
            jumpHBox.Text = getValue(Convert.ToUInt32(jumpHOffset, 16), "float");
        }

        private void defaultJumpH_Click(object sender, EventArgs e)
        {
            jumpHBox.Text = "0.75";
        }

        private void pokeTimeScale_Click(object sender, EventArgs e)
        {
            pokeXbox(Convert.ToUInt32(timeScaleOffset, 16), "float", timeScaleBox.Text);
        }

        private void getTimeScale_Click(object sender, EventArgs e)
        {
            timeScaleBox.Text = getValue(Convert.ToUInt32(timeScaleOffset, 16), "float");
        }

        private void defaultTimeScale_Click(object sender, EventArgs e)
        {
            timeScaleBox.Text = "30";
        }

        private void godModeOn_Click(object sender, EventArgs e)
        {
            pokeXbox(Convert.ToUInt32(godModeOffset, 16), "bytes", "C1100000");
        }

        private void godModeOff_Click(object sender, EventArgs e)
        {
            pokeXbox(Convert.ToUInt32(godModeOffset, 16), "bytes", "D19B0010");
        }

        private void infinAmmoOn_Click(object sender, EventArgs e)
        {
            pokeXbox(Convert.ToUInt32(infinAmmoOffset, 16), "bytes", "60000000");
            pokeXbox(Convert.ToUInt32(infinAmmoOffset2, 16), "float", "-1");
        }

        private void infinAmmoOff_Click(object sender, EventArgs e)
        {
            pokeXbox(Convert.ToUInt32(infinAmmoOffset, 16), "bytes", "B139000A");
            pokeXbox(Convert.ToUInt32(infinAmmoOffset2, 16), "float", "2,147484E+09");
        }

        private void fallDamageOff_Click(object sender, EventArgs e)
        {
            pokeXbox(Convert.ToUInt32(fallDamageOffset, 16), "float", "200");
        }

        private void fallDamageOn_Click(object sender, EventArgs e)
        {
            pokeXbox(Convert.ToUInt32(fallDamageOffset, 16), "float", "7");
        }

        private void reloadMap_Click(object sender, EventArgs e)
        {
            pokeXbox(Convert.ToUInt32(reloadMapOffset, 16), "byte", "1");
        }

        private void endGame_Click(object sender, EventArgs e)
        {
            pokeXbox(Convert.ToUInt32(endGameOffset, 16), "byte", "1");
        }

        private void labelX2_Click(object sender, EventArgs e)
        {

        }

        private void getButton_Click(object sender, EventArgs e)
        {
            string gotValue = getValue(Convert.ToUInt32(poke_offset.Text, 16), comboBox1.Text);
            Console.Write(gotValue);
            poke_value.Text = gotValue;
        }

        private void shieldsOn_Click(object sender, EventArgs e)
        {
            //step 1
            UInt32 gamestateBuffer = Convert.ToUInt32(getValue(Convert.ToUInt32("0x840DA714", 16), "long"), 16);
            //step 2
            UInt16 objectIndex = Convert.ToUInt16(getValue(gamestateBuffer + 0x5EE276, "short"), 16);
            //step 3
            UInt32 readMemAddr = (Convert.ToUInt32(objectIndex) * 0x10) + gamestateBuffer + 0x707508;
            UInt32 bipdMemAddr = Convert.ToUInt32(getValue(readMemAddr, "long"), 16);
            uint healthAddr = bipdMemAddr + 0x140;
            pokeXbox(healthAddr, "bytes", "FFFFFFFFFFFFFFFF");
        }

        private void shieldsOff_Click(object sender, EventArgs e)
        {

        }

        private void rsaChecks_Click(object sender, EventArgs e)
        {
            pokeXbox(Convert.ToUInt32(rsaOffset, 16), "bytes", "0x3860001");
        }
        private void removeArmor_Click(object sender, EventArgs e)
        {
            string armorType = helmetBox.Text;
            string helmetOffset = helmets[armorType];
            string armorVal = "0x" + helmetOffset + "FFFFFFFFFFFFFF";
            pokeXbox(Convert.ToUInt32(noArmorOffset, 16), "quad", armorVal);
        }
        
        private void addArmor_Click(object sender, EventArgs e)
        {
            string armorType = helmetBox.Text;
            string helmetOffset = helmets[armorType];
            string armorVal = "0x" + helmetOffset + "FFFFFFFFFFFFFF";
            pokeXbox(Convert.ToUInt32(noArmorOffset, 16), "quad", armorVal);
        }
        
        string invisCurrent;

        private void invisButton_Click(object sender, EventArgs e)
        {
            invisCurrent = getValue(Convert.ToUInt32(invisOffset, 16), "float");
            pokeXbox(Convert.ToUInt32(invisOffset, 16), "float", "9.492346E-38");
        }

        private void invisOff_Click(object sender, EventArgs e)
        {
            pokeXbox(Convert.ToUInt32(invisOffset, 16), "float", invisCurrent);
        }

        static FileInfo haloDir = new FileInfo("Updater.exe");
        string fileDir = haloDir.DirectoryName;

        private void checkForUpdate()
        {
            string downloadUrl = "";
            string updUrl = "";
            Version newVersion = null;
            string aboutUpdate = "";
            string xmlUrl = "http://www.xantecgames.com/halo4modder/halo4update.xml";
            XmlTextReader reader = null;
            try
            {
                reader = new XmlTextReader(xmlUrl);
                reader.MoveToContent();
                string elementName = "";
                if ((reader.NodeType == XmlNodeType.Element) && (reader.Name == "appinfo"))
                {
                    while (reader.Read())
                    {
                        if (reader.NodeType == XmlNodeType.Element)
                        {
                            elementName = reader.Name;
                        }
                        else
                        {
                            if ((reader.NodeType == XmlNodeType.Text) && (reader.HasValue))
                                switch (elementName)
                                {
                                    case "version":
                                        newVersion = new Version(reader.Value);
                                        break;
                                    case "url":
                                        downloadUrl = reader.Value;
                                        break;
                                    case "updUrl":
                                        updUrl = reader.Value;
                                        break;
                                    case "about":
                                        aboutUpdate = reader.Value;
                                        break;
                                }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Environment.Exit(1);
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }
            Version applicationVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            if (applicationVersion.CompareTo(newVersion) < 0)
            {
                MessageBox.Show("New features in this update : \n" + aboutUpdate);
                if (File.Exists("Updater.exe"))
                {
                    File.Delete("Updater.exe");
                }
                else
                {
                    MessageBox.Show("Updater is not found.");
                    Environment.Exit(0);
                }
                WebClient webClient = new WebClient();
                webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(Completed);
                webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(ProgressChanged);
               // MessageBox.Show(fileDir);
                webClient.DownloadFileAsync(new Uri(updUrl), fileDir + "\\Updater.exe");
            }
        }

        private void aboutBtn_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Programmed by Fierce Waffle\n\nOffsets Provided by : \n Bill Vonkova \n DeadCanadian5");
        }

        private void ProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            //progressBar1.Value = e.ProgressPercentage;
        }

        private void Completed(object sender, AsyncCompletedEventArgs e)
        {
           // MessageBox.Show("Download completed!");
            if (File.Exists("Updater.exe"))
            {
                Process.Start("Updater.exe");
                Environment.Exit(0);
            }
            else
            {
                MessageBox.Show("Updater not found.", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Environment.Exit(0);
            }
        }

        public string CalculateMD5Hash(string input)
        {
            // step 1, calculate MD5 hash from input
            MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hash = md5.ComputeHash(inputBytes);

            // step 2, convert byte array to hex string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            return sb.ToString();
        }

        private void xdkName_TextChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.xdkName = xdkName.Text;
            Properties.Settings.Default.Save();
            if (CalculateMD5Hash(xdkName.Text) == "607AC8DCB47FB2015770189C9D823372" || CalculateMD5Hash(xdkName.Text) == "9e5b81c5ca3a381406985ee3cc786ef7")
            {
                //currentArmor = getValue(Convert.ToUInt32(noArmorOffset, 16), "quad");
                helmetBox.DataSource = null;
                helmetBox.Items.Clear();
                helmets.Add("Null", "FF");
                helmetBox.DataSource = new BindingSource(helmets, null);
                helmetBox.DisplayMember = "Key";
                helmetBox.ValueMember = "Value";
            }
        }

        private void refreshShaders_Click(object sender, EventArgs e)
        {
            pokeXbox(Convert.ToUInt32("0x8407fb57", 16), "bytes", "0x01");
        }
    }
}
