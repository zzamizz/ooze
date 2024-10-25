using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace ooze
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            BrowseFile();
            if (validPath == false)
            {
                Application.Current.Shutdown();
            }
        }

        string saveFilePath;
        string currentPath;
        public bool validPath;

        bool orientationComplete; // unlocks urban door
        int lives;
        int health;
        bool cheats;
        int musicvol;
        int sfxvol;
        bool rumble;
        bool stereo;

        int orientationOoze;
        int cityparkOoze;
        int citycentreOoze;
        int docksOoze;
        int marketplaceOoze;
        int oasisOoze;
        int sphinxOoze;
        int tombOoze;
        int pyramidOoze;
        int sugarshackOoze;
        int skiliftOoze;
        int icebergOoze;
        int hotspringsOoze;

        bool[] orientationBitfield = new bool[32];
        bool[] cityparkBitfield = new bool[32];
        bool[] citycentreBitfield = new bool[32];
        bool[] docksBitfield = new bool[32];
        bool[] marketplaceBitfield = new bool[32];
        bool[] oasisBitfield = new bool[32];
        bool[] sphinxBitfield = new bool[32];
        bool[] tombBitfield = new bool[32];
        bool[] pyramidBitfield = new bool[32];
        bool[] sugarshackBitfield = new bool[32];
        bool[] skiliftBitfield = new bool[32];
        bool[] icebergBitfield = new bool[32];
        bool[] hotspringsBitfield = new bool[32];
        bool[] urbanpursuitBitfield = new bool[32];
        // bool[] desertpursuitBitfield = new bool[32]; apparently not a thing
        bool[] arcticpursuitBitfield = new bool[32];

        public void BrowseFile()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                saveFilePath = openFileDialog.FileName;
                LoadSaveFile(saveFilePath);
                currentPath = saveFilePath;
                validPath = true;
            }
        }

        private void ReadBitfield(int offset, bool[] array, string path)
        {
            using (BinaryReader reader = new BinaryReader(File.Open(path, FileMode.Open)))
            {
                reader.BaseStream.Seek(offset, SeekOrigin.Begin);
                Debug.WriteLine($"{offset}");
                byte[] temp = reader.ReadBytes(4);

                uint bitField = BitConverter.ToUInt32(temp, 0);
                for (int i = 0; i < 32; i++)
                {
                    bool isBitSet = (bitField & (1u << i)) != 0;
                    array[i] = isBitSet;
                }
            }
        }

        private void Read4Bytes(int offset, ref int var, string path)
        {
            using (BinaryReader reader = new BinaryReader(File.Open(path, FileMode.Open)))
            {
                reader.BaseStream.Seek(offset, SeekOrigin.Begin);
                var = reader.ReadInt32();
            }
        }

        private void ReadBool(int offset, ref bool var, string path)
        {
            using (BinaryReader reader = new BinaryReader(File.Open(path, FileMode.Open)))
            {
                reader.BaseStream.Seek(offset, SeekOrigin.Begin);
                var = reader.ReadBoolean();
            }
        }

        private void LoadSaveFile(string path)
        {
            ReadBool(0x610, ref cheats, path);
            Read4Bytes(0x634, ref sfxvol, path);
            Read4Bytes(0x638, ref musicvol, path);
            ReadBool(0x640, ref rumble, path);
            ReadBool(0x644, ref stereo, path);
            Read4Bytes(0x4C4, ref health, path);
            Read4Bytes(0x4C8, ref lives, path);
            ReadBool(0x5CC, ref orientationComplete, path); // dumb flag opens urbandoor

            Read4Bytes(0x4CC, ref orientationOoze, path);
            Read4Bytes(0x4D0, ref cityparkOoze, path);
            Read4Bytes(0x4D4, ref citycentreOoze, path);
            Read4Bytes(0x4D8, ref docksOoze, path);
            Read4Bytes(0x4DC, ref marketplaceOoze, path);
            Read4Bytes(0x4E0, ref sphinxOoze, path);
            Read4Bytes(0x4E4, ref oasisOoze, path);
            Read4Bytes(0x4E8, ref tombOoze, path);
            Read4Bytes(0x4EC, ref pyramidOoze, path);
            Read4Bytes(0x4F0, ref sugarshackOoze, path);
            Read4Bytes(0x4F4, ref skiliftOoze, path);
            Read4Bytes(0x4F8, ref icebergOoze, path);
            Read4Bytes(0x4FC, ref hotspringsOoze, path);

            ReadBitfield(0x5D0, orientationBitfield, path);
            ReadBitfield(0x5D4, cityparkBitfield, path);
            ReadBitfield(0x5D8, citycentreBitfield, path);
            ReadBitfield(0x5DC, docksBitfield, path);
            ReadBitfield(0x5E0, marketplaceBitfield, path);
            ReadBitfield(0x5E4, sphinxBitfield, path);
            ReadBitfield(0x5E8, oasisBitfield, path);
            ReadBitfield(0x5EC, tombBitfield, path);
            ReadBitfield(0x5F0, pyramidBitfield, path);
            ReadBitfield(0x5F4, sugarshackBitfield, path);
            ReadBitfield(0x5F8, skiliftBitfield, path);
            ReadBitfield(0x5FC, icebergBitfield, path);
            ReadBitfield(0x600, hotspringsBitfield, path);
            ReadBitfield(0x604, urbanpursuitBitfield, path);
            // ReadBitfield(0x608, desertpursuitBitfield, path);
            ReadBitfield(0x60C, arcticpursuitBitfield, path);

            Cheats.IsChecked = cheats;
            Rumble.IsChecked = rumble;
            Stereo.IsChecked = stereo;
            MusicVolume.Text = musicvol.ToString();
            SFXVolume.Text = sfxvol.ToString();
            Health.Text = health.ToString();
            Lives.Text = lives.ToString();

            OrientationComplete.IsChecked = orientationComplete;

            OrientationOoze.Text = orientationOoze.ToString();
            OrientationToken1.IsChecked = orientationBitfield[0];
            OrientationToken2.IsChecked = orientationBitfield[1];
            OrientationToken3.IsChecked = orientationBitfield[2];
            OrientationToken4.IsChecked = orientationBitfield[3];
            OrientationToken5.IsChecked = orientationBitfield[4];
            OrientationToken6.IsChecked = orientationBitfield[5];
            OrientationToken7.IsChecked = orientationBitfield[6];
            OrientationToken8.IsChecked = orientationBitfield[7];
            OrientationToken9.IsChecked = orientationBitfield[8];
            OrientationToken10.IsChecked = orientationBitfield[9];
            OrientationNerve1.IsChecked = orientationBitfield[10];
            OrientationNerve2.IsChecked = orientationBitfield[11];
            OrientationNerve3.IsChecked = orientationBitfield[12];
            OrientationNerve4.IsChecked = orientationBitfield[13];
            OrientationNerve5.IsChecked = orientationBitfield[14];
            OrientationNerve6.IsChecked = orientationBitfield[15];
            OrientationNerve7.IsChecked = orientationBitfield[16];
            OrientationNerve8.IsChecked = orientationBitfield[17];
            OrientationBronze.IsChecked = orientationBitfield[18];
            OrientationSilver.IsChecked = orientationBitfield[19];
            OrientationGold.IsChecked = orientationBitfield[20];
            OrientationFlag1.IsChecked = orientationBitfield[21];

            CityParkOoze.Text = cityparkOoze.ToString();
            CityParkToken1.IsChecked = cityparkBitfield[0];
            CityParkToken2.IsChecked = cityparkBitfield[1];
            CityParkToken3.IsChecked = cityparkBitfield[2];
            CityParkToken4.IsChecked = cityparkBitfield[3];
            CityParkToken5.IsChecked = cityparkBitfield[4];
            CityParkToken6.IsChecked = cityparkBitfield[5];
            CityParkToken7.IsChecked = cityparkBitfield[6];
            CityParkToken8.IsChecked = cityparkBitfield[7];
            CityParkToken9.IsChecked = cityparkBitfield[8];
            CityParkToken10.IsChecked = cityparkBitfield[9];
            CityParkNerve1.IsChecked = cityparkBitfield[10];
            CityParkNerve2.IsChecked = cityparkBitfield[11];
            CityParkNerve3.IsChecked = cityparkBitfield[12];
            CityParkNerve4.IsChecked = cityparkBitfield[13];
            CityParkNerve5.IsChecked = cityparkBitfield[14];
            CityParkNerve6.IsChecked = cityparkBitfield[15];
            CityParkNerve7.IsChecked = cityparkBitfield[16];
            CityParkNerve8.IsChecked = cityparkBitfield[17];
            CityParkBronze.IsChecked = cityparkBitfield[18];
            CityParkSilver.IsChecked = cityparkBitfield[19];
            CityParkGold.IsChecked = cityparkBitfield[20];
            CityParkFlag1.IsChecked = cityparkBitfield[21];
            CityParkFlag2.IsChecked = cityparkBitfield[22];
            CityParkFlag3.IsChecked = cityparkBitfield[23];
            CityParkFlag4.IsChecked = cityparkBitfield[24];
            CityParkFlag5.IsChecked = cityparkBitfield[25];
            CityParkFlag6.IsChecked = cityparkBitfield[26];
            CityParkFlag7.IsChecked = cityparkBitfield[27];
            CityParkFlag8.IsChecked = cityparkBitfield[31]; // last bit !!

            CityCentreOoze.Text = citycentreOoze.ToString();
            CityCentreToken1.IsChecked = citycentreBitfield[0];
            CityCentreToken2.IsChecked = citycentreBitfield[1];
            CityCentreToken3.IsChecked = citycentreBitfield[2];
            CityCentreToken4.IsChecked = citycentreBitfield[3];
            CityCentreToken5.IsChecked = citycentreBitfield[4];
            CityCentreToken6.IsChecked = citycentreBitfield[5];
            CityCentreToken7.IsChecked = citycentreBitfield[6];
            CityCentreToken8.IsChecked = citycentreBitfield[7];
            CityCentreToken9.IsChecked = citycentreBitfield[8];
            CityCentreToken10.IsChecked = citycentreBitfield[9];
            CityCentreNerve1.IsChecked = citycentreBitfield[10];
            CityCentreNerve2.IsChecked = citycentreBitfield[11];
            CityCentreNerve3.IsChecked = citycentreBitfield[12];
            CityCentreNerve4.IsChecked = citycentreBitfield[13];
            CityCentreNerve5.IsChecked = citycentreBitfield[14];
            CityCentreNerve6.IsChecked = citycentreBitfield[15];
            CityCentreNerve7.IsChecked = citycentreBitfield[16];
            CityCentreNerve8.IsChecked = citycentreBitfield[17];
            CityCentreBronze.IsChecked = citycentreBitfield[18];
            CityCentreSilver.IsChecked = citycentreBitfield[19];
            CityCentreGold.IsChecked = citycentreBitfield[20];
            CityCentreFlag1.IsChecked = citycentreBitfield[21];
            CityCentreFlag2.IsChecked = citycentreBitfield[22];

            DocksOoze.Text = docksOoze.ToString();
            DocksToken1.IsChecked = docksBitfield[0];
            DocksToken2.IsChecked = docksBitfield[1];
            DocksToken3.IsChecked = docksBitfield[2];
            DocksToken4.IsChecked = docksBitfield[3];
            DocksToken5.IsChecked = docksBitfield[4];
            DocksToken6.IsChecked = docksBitfield[5];
            DocksToken7.IsChecked = docksBitfield[6];
            DocksToken8.IsChecked = docksBitfield[7];
            DocksToken9.IsChecked = docksBitfield[8];
            DocksToken10.IsChecked = docksBitfield[9];
            DocksNerve1.IsChecked = docksBitfield[10];
            DocksNerve2.IsChecked = docksBitfield[11];
            DocksNerve3.IsChecked = docksBitfield[12];
            DocksNerve4.IsChecked = docksBitfield[13];
            DocksNerve5.IsChecked = docksBitfield[14];
            DocksNerve6.IsChecked = docksBitfield[15];
            DocksNerve7.IsChecked = docksBitfield[16];
            DocksNerve8.IsChecked = docksBitfield[17];
            DocksBronze.IsChecked = docksBitfield[18];
            DocksSilver.IsChecked = docksBitfield[19];
            DocksGold.IsChecked = docksBitfield[20];
            DocksFlag1.IsChecked = docksBitfield[21];
            DocksFlag2.IsChecked = docksBitfield[22];
            DocksFlag3.IsChecked = docksBitfield[31]; // last bit !!

            MarketplaceOoze.Text = marketplaceOoze.ToString();
            MarketplaceToken1.IsChecked = marketplaceBitfield[0];
            MarketplaceToken2.IsChecked = marketplaceBitfield[1];
            MarketplaceToken3.IsChecked = marketplaceBitfield[2];
            MarketplaceToken4.IsChecked = marketplaceBitfield[3];
            MarketplaceToken5.IsChecked = marketplaceBitfield[4];
            MarketplaceToken6.IsChecked = marketplaceBitfield[5];
            MarketplaceToken7.IsChecked = marketplaceBitfield[6];
            MarketplaceToken8.IsChecked = marketplaceBitfield[7];
            MarketplaceToken9.IsChecked = marketplaceBitfield[8];
            MarketplaceToken10.IsChecked = marketplaceBitfield[9];
            MarketplaceNerve1.IsChecked = marketplaceBitfield[10];
            MarketplaceNerve2.IsChecked = marketplaceBitfield[11];
            MarketplaceNerve3.IsChecked = marketplaceBitfield[12];
            MarketplaceNerve4.IsChecked = marketplaceBitfield[13];
            MarketplaceNerve5.IsChecked = marketplaceBitfield[14];
            MarketplaceNerve6.IsChecked = marketplaceBitfield[15];
            MarketplaceNerve7.IsChecked = marketplaceBitfield[16];
            MarketplaceNerve8.IsChecked = marketplaceBitfield[17];
            MarketplaceBronze.IsChecked = marketplaceBitfield[18];
            MarketplaceSilver.IsChecked = marketplaceBitfield[19];
            MarketplaceGold.IsChecked = marketplaceBitfield[20];
            MarketplaceFlag1.IsChecked = marketplaceBitfield[21];
            MarketplaceFlag2.IsChecked = marketplaceBitfield[22];
            MarketplaceFlag3.IsChecked = marketplaceBitfield[23];
            MarketplaceFlag4.IsChecked = marketplaceBitfield[24];
            MarketplaceFlag5.IsChecked = marketplaceBitfield[31]; // last bit !!

            SphinxOoze.Text = sphinxOoze.ToString();
            SphinxToken1.IsChecked = sphinxBitfield[0];
            SphinxToken2.IsChecked = sphinxBitfield[1];
            SphinxToken3.IsChecked = sphinxBitfield[2];
            SphinxToken4.IsChecked = sphinxBitfield[3];
            SphinxToken5.IsChecked = sphinxBitfield[4];
            SphinxToken6.IsChecked = sphinxBitfield[5];
            SphinxToken7.IsChecked = sphinxBitfield[6];
            SphinxToken8.IsChecked = sphinxBitfield[7];
            SphinxToken9.IsChecked = sphinxBitfield[8];
            SphinxToken10.IsChecked = sphinxBitfield[9];
            SphinxNerve1.IsChecked = sphinxBitfield[10];
            SphinxNerve2.IsChecked = sphinxBitfield[11];
            SphinxNerve3.IsChecked = sphinxBitfield[12];
            SphinxNerve4.IsChecked = sphinxBitfield[13];
            SphinxNerve5.IsChecked = sphinxBitfield[14];
            SphinxNerve6.IsChecked = sphinxBitfield[15];
            SphinxNerve7.IsChecked = sphinxBitfield[16];
            SphinxNerve8.IsChecked = sphinxBitfield[17];
            SphinxBronze.IsChecked = sphinxBitfield[18];
            SphinxSilver.IsChecked = sphinxBitfield[19];
            SphinxGold.IsChecked = sphinxBitfield[20];

            OasisOoze.Text = oasisOoze.ToString();
            OasisToken1.IsChecked = oasisBitfield[0];
            OasisToken2.IsChecked = oasisBitfield[1];
            OasisToken3.IsChecked = oasisBitfield[2];
            OasisToken4.IsChecked = oasisBitfield[3];
            OasisToken5.IsChecked = oasisBitfield[4];
            OasisToken6.IsChecked = oasisBitfield[5];
            OasisToken7.IsChecked = oasisBitfield[6];
            OasisToken8.IsChecked = oasisBitfield[7];
            OasisToken9.IsChecked = oasisBitfield[8];
            OasisToken10.IsChecked = oasisBitfield[9];
            OasisNerve1.IsChecked = oasisBitfield[10];
            OasisNerve2.IsChecked = oasisBitfield[11];
            OasisNerve3.IsChecked = oasisBitfield[12];
            OasisNerve4.IsChecked = oasisBitfield[13];
            OasisNerve5.IsChecked = oasisBitfield[14];
            OasisNerve6.IsChecked = oasisBitfield[15];
            OasisNerve7.IsChecked = oasisBitfield[16];
            OasisNerve8.IsChecked = oasisBitfield[17];
            OasisBronze.IsChecked = oasisBitfield[18];
            OasisSilver.IsChecked = oasisBitfield[19];
            OasisGold.IsChecked = oasisBitfield[20];
            OasisFlag1.IsChecked = oasisBitfield[21];
            OasisFlag2.IsChecked = oasisBitfield[22];
            OasisFlag3.IsChecked = oasisBitfield[23];
            OasisFlag4.IsChecked = oasisBitfield[24];

            TombOoze.Text = tombOoze.ToString();
            TombToken1.IsChecked = tombBitfield[0];
            TombToken2.IsChecked = tombBitfield[1];
            TombToken3.IsChecked = tombBitfield[2];
            TombToken4.IsChecked = tombBitfield[3];
            TombToken5.IsChecked = tombBitfield[4];
            TombToken6.IsChecked = tombBitfield[5];
            TombToken7.IsChecked = tombBitfield[6];
            TombToken8.IsChecked = tombBitfield[7];
            TombToken9.IsChecked = tombBitfield[8];
            TombToken10.IsChecked = tombBitfield[9];
            TombNerve1.IsChecked = tombBitfield[10];
            TombNerve2.IsChecked = tombBitfield[11];
            TombNerve3.IsChecked = tombBitfield[12];
            TombNerve4.IsChecked = tombBitfield[13];
            TombNerve5.IsChecked = tombBitfield[14];
            TombNerve6.IsChecked = tombBitfield[15];
            TombNerve7.IsChecked = tombBitfield[16];
            TombNerve8.IsChecked = tombBitfield[17];
            TombBronze.IsChecked = tombBitfield[18];
            TombSilver.IsChecked = tombBitfield[19];
            TombGold.IsChecked = tombBitfield[20];
            TombFlag1.IsChecked = tombBitfield[21];
            TombFlag2.IsChecked = tombBitfield[22];
            TombFlag3.IsChecked = tombBitfield[26];
            TombFlag4.IsChecked = tombBitfield[27];
            TombFlag5.IsChecked = tombBitfield[28];
            TombFlag6.IsChecked = tombBitfield[29];
            TombFlag7.IsChecked = tombBitfield[30];
            TombFlag8.IsChecked = tombBitfield[31]; // last bit

            PyramidOoze.Text = pyramidOoze.ToString();
            PyramidToken1.IsChecked = pyramidBitfield[0];
            PyramidToken2.IsChecked = pyramidBitfield[1];
            PyramidToken3.IsChecked = pyramidBitfield[2];
            PyramidToken4.IsChecked = pyramidBitfield[3];
            PyramidToken5.IsChecked = pyramidBitfield[4];
            PyramidToken6.IsChecked = pyramidBitfield[5];
            PyramidToken7.IsChecked = pyramidBitfield[6];
            PyramidToken8.IsChecked = pyramidBitfield[7];
            PyramidToken9.IsChecked = pyramidBitfield[8];
            PyramidToken10.IsChecked = pyramidBitfield[9];
            PyramidNerve1.IsChecked = pyramidBitfield[10];
            PyramidNerve2.IsChecked = pyramidBitfield[11];
            PyramidNerve3.IsChecked = pyramidBitfield[12];
            PyramidNerve4.IsChecked = pyramidBitfield[13];
            PyramidNerve5.IsChecked = pyramidBitfield[14];
            PyramidNerve6.IsChecked = pyramidBitfield[15];
            PyramidNerve7.IsChecked = pyramidBitfield[16];
            PyramidNerve8.IsChecked = pyramidBitfield[17];
            PyramidBronze.IsChecked = pyramidBitfield[18];
            PyramidSilver.IsChecked = pyramidBitfield[19];
            PyramidGold.IsChecked = pyramidBitfield[20];
            PyramidFlag1.IsChecked = pyramidBitfield[21];
            PyramidFlag2.IsChecked = pyramidBitfield[22];
            PyramidFlag3.IsChecked = pyramidBitfield[23];
            PyramidFlag4.IsChecked = pyramidBitfield[24];

            SugarshackOoze.Text = sugarshackOoze.ToString();
            SugarShackToken1.IsChecked = sugarshackBitfield[0];
            SugarShackToken2.IsChecked = sugarshackBitfield[1];
            SugarShackToken3.IsChecked = sugarshackBitfield[2];
            SugarShackToken4.IsChecked = sugarshackBitfield[3];
            SugarShackToken5.IsChecked = sugarshackBitfield[4];
            SugarShackToken6.IsChecked = sugarshackBitfield[5];
            SugarShackToken7.IsChecked = sugarshackBitfield[6];
            SugarShackToken8.IsChecked = sugarshackBitfield[7];
            SugarShackToken9.IsChecked = sugarshackBitfield[8];
            SugarShackToken10.IsChecked = sugarshackBitfield[9];
            SugarShackNerve1.IsChecked = sugarshackBitfield[10];
            SugarShackNerve2.IsChecked = sugarshackBitfield[11];
            SugarShackNerve3.IsChecked = sugarshackBitfield[12];
            SugarShackNerve4.IsChecked = sugarshackBitfield[13];
            SugarShackNerve5.IsChecked = sugarshackBitfield[14];
            SugarShackNerve6.IsChecked = sugarshackBitfield[15];
            SugarShackNerve7.IsChecked = sugarshackBitfield[16];
            SugarShackNerve8.IsChecked = sugarshackBitfield[17];
            SugarShackBronze.IsChecked = sugarshackBitfield[18];
            SugarShackSilver.IsChecked = sugarshackBitfield[19];
            SugarShackGold.IsChecked = sugarshackBitfield[20];
            SugarShackFlag1.IsChecked = sugarshackBitfield[21];
            SugarShackFlag2.IsChecked = sugarshackBitfield[27];
            SugarShackFlag3.IsChecked = sugarshackBitfield[28];
            SugarShackFlag4.IsChecked = sugarshackBitfield[29];
            SugarShackFlag5.IsChecked = sugarshackBitfield[30];
            SugarShackFlag6.IsChecked = sugarshackBitfield[31]; // last bit !

            SkiliftOoze.Text = skiliftOoze.ToString();
            SkiliftToken1.IsChecked = skiliftBitfield[0];
            SkiliftToken2.IsChecked = skiliftBitfield[1];
            SkiliftToken3.IsChecked = skiliftBitfield[2];
            SkiliftToken4.IsChecked = skiliftBitfield[3];
            SkiliftToken5.IsChecked = skiliftBitfield[4];
            SkiliftToken6.IsChecked = skiliftBitfield[5];
            SkiliftToken7.IsChecked = skiliftBitfield[6];
            SkiliftToken8.IsChecked = skiliftBitfield[7];
            SkiliftToken9.IsChecked = skiliftBitfield[8];
            SkiliftToken10.IsChecked = skiliftBitfield[9];
            SkiliftNerve1.IsChecked = skiliftBitfield[10];
            SkiliftNerve2.IsChecked = skiliftBitfield[11];
            SkiliftNerve3.IsChecked = skiliftBitfield[12];
            SkiliftNerve4.IsChecked = skiliftBitfield[13];
            SkiliftNerve5.IsChecked = skiliftBitfield[14];
            SkiliftNerve6.IsChecked = skiliftBitfield[15];
            SkiliftNerve7.IsChecked = skiliftBitfield[16];
            SkiliftNerve8.IsChecked = skiliftBitfield[17];
            SkiliftBronze.IsChecked = skiliftBitfield[18];
            SkiliftSilver.IsChecked = skiliftBitfield[19];
            SkiliftGold.IsChecked = skiliftBitfield[20];
            SkiliftFlag1.IsChecked = skiliftBitfield[21];
            SkiliftFlag2.IsChecked = skiliftBitfield[22];
            SkiliftFlag3.IsChecked = skiliftBitfield[23];
            SkiliftFlag4.IsChecked = skiliftBitfield[24];

            IcebergOoze.Text = icebergOoze.ToString();
            IcebergToken1.IsChecked = icebergBitfield[0];
            IcebergToken2.IsChecked = icebergBitfield[1];
            IcebergToken3.IsChecked = icebergBitfield[2];
            IcebergToken4.IsChecked = icebergBitfield[3];
            IcebergToken5.IsChecked = icebergBitfield[4];
            IcebergToken6.IsChecked = icebergBitfield[5];
            IcebergToken7.IsChecked = icebergBitfield[6];
            IcebergToken8.IsChecked = icebergBitfield[7];
            IcebergToken9.IsChecked = icebergBitfield[8];
            IcebergToken10.IsChecked = icebergBitfield[9];
            IcebergNerve1.IsChecked = icebergBitfield[10];
            IcebergNerve2.IsChecked = icebergBitfield[11];
            IcebergNerve3.IsChecked = icebergBitfield[12];
            IcebergNerve4.IsChecked = icebergBitfield[13];
            IcebergNerve5.IsChecked = icebergBitfield[14];
            IcebergNerve6.IsChecked = icebergBitfield[15];
            IcebergNerve7.IsChecked = icebergBitfield[16];
            IcebergNerve8.IsChecked = icebergBitfield[17];
            IcebergBronze.IsChecked = icebergBitfield[18];
            IcebergSilver.IsChecked = icebergBitfield[19];
            IcebergGold.IsChecked = icebergBitfield[20];
            IcebergFlag1.IsChecked = icebergBitfield[21];

            HotspringsOoze.Text = hotspringsOoze.ToString();
            HotspringsToken1.IsChecked = hotspringsBitfield[0];
            HotspringsToken2.IsChecked = hotspringsBitfield[1];
            HotspringsToken3.IsChecked = hotspringsBitfield[2];
            HotspringsToken4.IsChecked = hotspringsBitfield[3];
            HotspringsToken5.IsChecked = hotspringsBitfield[4];
            HotspringsToken6.IsChecked = hotspringsBitfield[5];
            HotspringsToken7.IsChecked = hotspringsBitfield[6];
            HotspringsToken8.IsChecked = hotspringsBitfield[7];
            HotspringsToken9.IsChecked = hotspringsBitfield[8];
            HotspringsToken10.IsChecked = hotspringsBitfield[9];
            HotspringsNerve1.IsChecked = hotspringsBitfield[10];
            HotspringsNerve2.IsChecked = hotspringsBitfield[11];
            HotspringsNerve3.IsChecked = hotspringsBitfield[12];
            HotspringsNerve4.IsChecked = hotspringsBitfield[13];
            HotspringsNerve5.IsChecked = hotspringsBitfield[14];
            HotspringsNerve6.IsChecked = hotspringsBitfield[15];
            HotspringsNerve7.IsChecked = hotspringsBitfield[16];
            HotspringsNerve8.IsChecked = hotspringsBitfield[17];
            HotspringsBronze.IsChecked = hotspringsBitfield[18];
            HotspringsSilver.IsChecked = hotspringsBitfield[19];
            HotspringsGold.IsChecked = hotspringsBitfield[20];
            HotspringsFlag1.IsChecked = hotspringsBitfield[21];
            HotspringsFlag2.IsChecked = hotspringsBitfield[22];
            HotspringsFlag3.IsChecked = hotspringsBitfield[23];
            HotspringsFlag4.IsChecked = hotspringsBitfield[24];

            UrbanFlag1.IsChecked = urbanpursuitBitfield[21]; // life coin
            //>desert pursuit has a coin, why is there no bitfield for it
            ArcticFlag1.IsChecked = arcticpursuitBitfield[21]; // life coin
        }

        // method for turning bits into bytes, argument bitfield = e.g. orientationbitfield
        // call boolarray in savechanges for each different bitfield
        private void SaveBoolArray(int offset, bool[] bitfield)
        {
            using (FileStream fs = new FileStream(currentPath, FileMode.Open, FileAccess.Write))
            using (BinaryWriter writer = new BinaryWriter(fs))
            {
                fs.Position = offset;
                int boolCount = bitfield.Length;
                byte currentByte = 0;
                int bitIndex = 0;

                for (int i = 0; i < boolCount; i++)
                {
                    if (bitfield[i])
                    {
                        currentByte |= (byte)(1 << bitIndex);
                    }

                    bitIndex++;

                    // when 8 bools are processed, write the byte and reset
                    if (bitIndex == 8)
                    {
                        writer.Write(currentByte);
                        currentByte = 0;  // Reset the byte
                        bitIndex = 0;      // Reset the bit index
                    }
                }

                // If there are leftover bits, write the last byte
                if (bitIndex > 0)
                {
                    writer.Write(currentByte);
                }
            }
        }

        private void Save4Bytes(int offset, int var)
        {
            using (FileStream fs = new FileStream(currentPath, FileMode.Open, FileAccess.Write))
            using (BinaryWriter writer = new BinaryWriter(fs))
            {
                fs.Position = offset;
                writer.Write(var);
            }
        }

        private void SaveBool(int offset, bool var)
        {
            using (FileStream fs = new FileStream(currentPath, FileMode.Open, FileAccess.Write))
            using (BinaryWriter writer = new BinaryWriter(fs))
            {
                fs.Position = offset;
                writer.Write(var);
            }
        }

        private void SaveChanges()
        {
            try
            {
                Save4Bytes(0x4C4, health);
                Save4Bytes(0x4C8, lives);

                Save4Bytes(0x4CC, orientationOoze);
                Save4Bytes(0x4D0, cityparkOoze);
                Save4Bytes(0x4D4, citycentreOoze);
                Save4Bytes(0x4D8, docksOoze);
                Save4Bytes(0x4DC, marketplaceOoze);
                Save4Bytes(0x4E0, sphinxOoze);
                Save4Bytes(0x4E4, oasisOoze);
                Save4Bytes(0x4E8, tombOoze);
                Save4Bytes(0x4EC, pyramidOoze);
                Save4Bytes(0x4F0, sugarshackOoze);
                Save4Bytes(0x4F4, skiliftOoze);
                Save4Bytes(0x4F8, icebergOoze);
                Save4Bytes(0x4FC, hotspringsOoze);

                SaveBool(0x5CC, orientationComplete);

                SaveBoolArray(0x5D0, orientationBitfield);
                SaveBoolArray(0x5D4, cityparkBitfield);
                SaveBoolArray(0x5D8, citycentreBitfield);
                SaveBoolArray(0x5DC, docksBitfield);
                SaveBoolArray(0x5E0, marketplaceBitfield);
                SaveBoolArray(0x5E4, sphinxBitfield);
                SaveBoolArray(0x5E8, oasisBitfield);
                SaveBoolArray(0x5EC, tombBitfield);
                SaveBoolArray(0x5F0, pyramidBitfield);
                SaveBoolArray(0x5F4, sugarshackBitfield);
                SaveBoolArray(0x5F8, skiliftBitfield);
                SaveBoolArray(0x5FC, icebergBitfield);
                SaveBoolArray(0x600, hotspringsBitfield);
                SaveBoolArray(0x604, urbanpursuitBitfield);
                // SaveBoolArray(0x608, desertpursuitBitfield);
                SaveBoolArray(0x60C, arcticpursuitBitfield);

                if (sfxvol > 10) // saving your ears
                {
                    Save4Bytes(0x634, 10);
                }
                else
                {
                    Save4Bytes(0x634, sfxvol);
                }

                if (musicvol > 10)
                {
                    Save4Bytes(0x638, 10);
                }
                else
                {
                    Save4Bytes(0x638, musicvol);
                }

                SaveBool(0x610, cheats);
                SaveBool(0x640, rumble);
                SaveBool(0x644, stereo);

                MessageBox.Show("Successfully saved the changes.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"File couldn't be saved: {ex.Message}");
            }
        }

        // Buttons
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            SaveChanges();
        }
        private void BrowseButton_Click(object sender, RoutedEventArgs e)
        {
            BrowseFile();
        }

        // Saving
        // Settings
        private void Cheats_Click(object sender, RoutedEventArgs e) { cheats = Cheats.IsChecked == true; }
        private void SFXVolume_TextChanged(object sender, TextChangedEventArgs e) { int temp; if (int.TryParse(SFXVolume.Text, out temp)) { sfxvol = temp; } }
        private void MusicVolume_TextChanged(object sender, TextChangedEventArgs e)
        {
            int temp;
            if (int.TryParse(MusicVolume.Text, out temp))
            {
                musicvol = temp;
            }
        }
        // Player
        private void Health_TextChanged(object sender, TextChangedEventArgs e) { int temp; if (int.TryParse(Health.Text, out temp)) { health = temp; } }
        private void Lives_TextChanged(object sender, TextChangedEventArgs e) { int temp; if (int.TryParse(Lives.Text, out temp)) { lives = temp; } }
        // Orientation
        private void OrientationToken1_Click(object sender, RoutedEventArgs e) { orientationBitfield[0] = OrientationToken1.IsChecked == true; }
        private void OrientationToken2_Click(object sender, RoutedEventArgs e) { orientationBitfield[1] = OrientationToken2.IsChecked == true; }
        private void OrientationToken3_Click(object sender, RoutedEventArgs e) { orientationBitfield[2] = OrientationToken3.IsChecked == true; }
        private void OrientationToken4_Click(object sender, RoutedEventArgs e) { orientationBitfield[3] = OrientationToken4.IsChecked == true; }
        private void OrientationToken5_Click(object sender, RoutedEventArgs e) { orientationBitfield[4] = OrientationToken5.IsChecked == true; }
        private void OrientationToken6_Click(object sender, RoutedEventArgs e) { orientationBitfield[5] = OrientationToken6.IsChecked == true; }
        private void OrientationToken7_Click(object sender, RoutedEventArgs e) { orientationBitfield[6] = OrientationToken7.IsChecked == true; }
        private void OrientationToken8_Click(object sender, RoutedEventArgs e) { orientationBitfield[7] = OrientationToken8.IsChecked == true; }
        private void OrientationToken9_Click(object sender, RoutedEventArgs e) { orientationBitfield[8] = OrientationToken9.IsChecked == true; }
        private void OrientationToken10_Click(object sender, RoutedEventArgs e) { orientationBitfield[9] = OrientationToken10.IsChecked == true; }
        private void OrientationNerve1_Click(object sender, RoutedEventArgs e) { orientationBitfield[10] = OrientationNerve1.IsChecked == true; }
        private void OrientationNerve2_Click(object sender, RoutedEventArgs e) { orientationBitfield[11] = OrientationNerve2.IsChecked == true; }
        private void OrientationNerve3_Click(object sender, RoutedEventArgs e) { orientationBitfield[12] = OrientationNerve3.IsChecked == true; }
        private void OrientationNerve4_Click(object sender, RoutedEventArgs e) { orientationBitfield[13] = OrientationNerve4.IsChecked == true; }
        private void OrientationNerve5_Click(object sender, RoutedEventArgs e) { orientationBitfield[14] = OrientationNerve5.IsChecked == true; }
        private void OrientationNerve6_Click(object sender, RoutedEventArgs e) { orientationBitfield[15] = OrientationNerve6.IsChecked == true; }
        private void OrientationNerve7_Click(object sender, RoutedEventArgs e) { orientationBitfield[16] = OrientationNerve7.IsChecked == true; }
        private void OrientationNerve8_Click(object sender, RoutedEventArgs e) { orientationBitfield[17] = OrientationNerve8.IsChecked == true; }
        private void OrientationBronze_Click(object sender, RoutedEventArgs e) { orientationBitfield[18] = OrientationBronze.IsChecked == true; }
        private void OrientationSilver_Click(object sender, RoutedEventArgs e) { orientationBitfield[19] = OrientationSilver.IsChecked == true; }
        private void OrientationGold_Click(object sender, RoutedEventArgs e) { orientationBitfield[20] = OrientationGold.IsChecked == true; }
        private void OrientationFlag1_Click(object sender, RoutedEventArgs e) { orientationBitfield[21] = OrientationFlag1.IsChecked == true; }
        private void OrientationOoze_TextChanged(object sender, TextChangedEventArgs e) { int ooze; if (int.TryParse(OrientationOoze.Text, out ooze)) { orientationOoze = ooze; } }
        // City Park
        private void CityParkToken1_Click(object sender, RoutedEventArgs e) { cityparkBitfield[0] = CityParkToken1.IsChecked == true; }
        private void CityParkToken2_Click(object sender, RoutedEventArgs e) { cityparkBitfield[1] = CityParkToken2.IsChecked == true; }
        private void CityParkToken3_Click(object sender, RoutedEventArgs e) { cityparkBitfield[2] = CityParkToken3.IsChecked == true; }
        private void CityParkToken4_Click(object sender, RoutedEventArgs e) { cityparkBitfield[3] = CityParkToken4.IsChecked == true; }
        private void CityParkToken5_Click(object sender, RoutedEventArgs e) { cityparkBitfield[4] = CityParkToken5.IsChecked == true; }
        private void CityParkToken6_Click(object sender, RoutedEventArgs e) { cityparkBitfield[5] = CityParkToken6.IsChecked == true; }
        private void CityParkToken7_Click(object sender, RoutedEventArgs e) { cityparkBitfield[6] = CityParkToken7.IsChecked == true; }
        private void CityParkToken8_Click(object sender, RoutedEventArgs e) { cityparkBitfield[7] = CityParkToken8.IsChecked == true; }
        private void CityParkToken9_Click(object sender, RoutedEventArgs e) { cityparkBitfield[8] = CityParkToken9.IsChecked == true; }
        private void CityParkToken10_Click(object sender, RoutedEventArgs e) { cityparkBitfield[9] = CityParkToken10.IsChecked == true; }
        private void CityParkNerve1_Click(object sender, RoutedEventArgs e) { cityparkBitfield[10] = CityParkNerve1.IsChecked == true; }
        private void CityParkNerve2_Click(object sender, RoutedEventArgs e) { cityparkBitfield[11] = CityParkNerve2.IsChecked == true; }
        private void CityParkNerve3_Click(object sender, RoutedEventArgs e) { cityparkBitfield[12] = CityParkNerve3.IsChecked == true; }
        private void CityParkNerve4_Click(object sender, RoutedEventArgs e) { cityparkBitfield[13] = CityParkNerve4.IsChecked == true; }
        private void CityParkNerve5_Click(object sender, RoutedEventArgs e) { cityparkBitfield[14] = CityParkNerve5.IsChecked == true; }
        private void CityParkNerve6_Click(object sender, RoutedEventArgs e) { cityparkBitfield[15] = CityParkNerve6.IsChecked == true; }
        private void CityParkNerve7_Click(object sender, RoutedEventArgs e) { cityparkBitfield[16] = CityParkNerve7.IsChecked == true; }
        private void CityParkNerve8_Click(object sender, RoutedEventArgs e) { cityparkBitfield[17] = CityParkNerve8.IsChecked == true; }
        private void CityParkBronze_Click(object sender, RoutedEventArgs e) { cityparkBitfield[18] = CityParkBronze.IsChecked == true; }
        private void CityParkSilver_Click(object sender, RoutedEventArgs e) { cityparkBitfield[19] = CityParkSilver.IsChecked == true; }
        private void CityParkGold_Click(object sender, RoutedEventArgs e) { cityparkBitfield[20] = CityParkGold.IsChecked == true; }
        private void CityParkFlag1_Click(object sender, RoutedEventArgs e) { cityparkBitfield[21] = CityParkFlag1.IsChecked == true; }
        private void CityParkFlag2_Click(object sender, RoutedEventArgs e) { cityparkBitfield[22] = CityParkFlag2.IsChecked == true; }
        private void CityParkFlag3_Click(object sender, RoutedEventArgs e) { cityparkBitfield[23] = CityParkFlag3.IsChecked == true; }
        private void CityParkFlag4_Click(object sender, RoutedEventArgs e) { cityparkBitfield[24] = CityParkFlag4.IsChecked == true; }
        private void CityParkFlag5_Click(object sender, RoutedEventArgs e) { cityparkBitfield[25] = CityParkFlag5.IsChecked == true; }
        private void CityParkFlag6_Click(object sender, RoutedEventArgs e) { cityparkBitfield[26] = CityParkFlag6.IsChecked == true; }
        private void CityParkFlag7_Click(object sender, RoutedEventArgs e) { cityparkBitfield[27] = CityParkFlag7.IsChecked == true; }
        private void CityParkFlag8_Click(object sender, RoutedEventArgs e) { cityparkBitfield[31] = CityParkFlag8.IsChecked == true; }
        private void CityParkOoze_TextChanged(object sender, TextChangedEventArgs e) { int ooze; if (int.TryParse(CityParkOoze.Text, out ooze)) { cityparkOoze = ooze; } }
        // City Centre
        private void CityCentreToken1_Click(object sender, RoutedEventArgs e) { citycentreBitfield[0] = CityCentreToken1.IsChecked == true; }
        private void CityCentreToken2_Click(object sender, RoutedEventArgs e) { citycentreBitfield[1] = CityCentreToken2.IsChecked == true; }
        private void CityCentreToken3_Click(object sender, RoutedEventArgs e) { citycentreBitfield[2] = CityCentreToken3.IsChecked == true; }
        private void CityCentreToken4_Click(object sender, RoutedEventArgs e) { citycentreBitfield[3] = CityCentreToken4.IsChecked == true; }
        private void CityCentreToken5_Click(object sender, RoutedEventArgs e) { citycentreBitfield[4] = CityCentreToken5.IsChecked == true; }
        private void CityCentreToken6_Click(object sender, RoutedEventArgs e) { citycentreBitfield[5] = CityCentreToken6.IsChecked == true; }
        private void CityCentreToken7_Click(object sender, RoutedEventArgs e) { citycentreBitfield[6] = CityCentreToken7.IsChecked == true; }
        private void CityCentreToken8_Click(object sender, RoutedEventArgs e) { citycentreBitfield[7] = CityCentreToken8.IsChecked == true; }
        private void CityCentreToken9_Click(object sender, RoutedEventArgs e) { citycentreBitfield[8] = CityCentreToken9.IsChecked == true; }
        private void CityCentreToken10_Click(object sender, RoutedEventArgs e) { citycentreBitfield[9] = CityCentreToken10.IsChecked == true; }
        private void CityCentreNerve1_Click(object sender, RoutedEventArgs e) { citycentreBitfield[10] = CityCentreNerve1.IsChecked == true; }
        private void CityCentreNerve2_Click(object sender, RoutedEventArgs e) { citycentreBitfield[11] = CityCentreNerve2.IsChecked == true; }
        private void CityCentreNerve3_Click(object sender, RoutedEventArgs e) { citycentreBitfield[12] = CityCentreNerve3.IsChecked == true; }
        private void CityCentreNerve4_Click(object sender, RoutedEventArgs e) { citycentreBitfield[13] = CityCentreNerve4.IsChecked == true; }
        private void CityCentreNerve5_Click(object sender, RoutedEventArgs e) { citycentreBitfield[14] = CityCentreNerve5.IsChecked == true; }
        private void CityCentreNerve6_Click(object sender, RoutedEventArgs e) { citycentreBitfield[15] = CityCentreNerve6.IsChecked == true; }
        private void CityCentreNerve7_Click(object sender, RoutedEventArgs e) { citycentreBitfield[16] = CityCentreNerve7.IsChecked == true; }
        private void CityCentreNerve8_Click(object sender, RoutedEventArgs e) { citycentreBitfield[17] = CityCentreNerve8.IsChecked == true; }
        private void CityCentreBronze_Click(object sender, RoutedEventArgs e) { citycentreBitfield[18] = CityCentreBronze.IsChecked == true; }
        private void CityCentreSilver_Click(object sender, RoutedEventArgs e) { citycentreBitfield[19] = CityCentreSilver.IsChecked == true; }
        private void CityCentreGold_Click(object sender, RoutedEventArgs e) { citycentreBitfield[20] = CityCentreGold.IsChecked == true; }
        private void CityCentreFlag1_Click(object sender, RoutedEventArgs e) { citycentreBitfield[21] = CityCentreFlag1.IsChecked == true; }
        private void CityCentreFlag2_Click(object sender, RoutedEventArgs e) { citycentreBitfield[22] = CityCentreFlag2.IsChecked == true; }
        private void CityCentreOoze_TextChanged(object sender, TextChangedEventArgs e) { int ooze; if (int.TryParse(CityCentreOoze.Text, out ooze)) { citycentreOoze = ooze; } }
        // Docks
        private void DocksToken1_Click(object sender, RoutedEventArgs e) { docksBitfield[0] = DocksToken1.IsChecked == true; }
        private void DocksToken2_Click(object sender, RoutedEventArgs e) { docksBitfield[1] = DocksToken2.IsChecked == true; }
        private void DocksToken3_Click(object sender, RoutedEventArgs e) { docksBitfield[2] = DocksToken3.IsChecked == true; }
        private void DocksToken4_Click(object sender, RoutedEventArgs e) { docksBitfield[3] = DocksToken4.IsChecked == true; }
        private void DocksToken5_Click(object sender, RoutedEventArgs e) { docksBitfield[4] = DocksToken5.IsChecked == true; }
        private void DocksToken6_Click(object sender, RoutedEventArgs e) { docksBitfield[5] = DocksToken6.IsChecked == true; }
        private void DocksToken7_Click(object sender, RoutedEventArgs e) { docksBitfield[6] = DocksToken7.IsChecked == true; }
        private void DocksToken8_Click(object sender, RoutedEventArgs e) { docksBitfield[7] = DocksToken8.IsChecked == true; }
        private void DocksToken9_Click(object sender, RoutedEventArgs e) { docksBitfield[8] = DocksToken9.IsChecked == true; }
        private void DocksToken10_Click(object sender, RoutedEventArgs e) { docksBitfield[9] = DocksToken10.IsChecked == true; }
        private void DocksNerve1_Click(object sender, RoutedEventArgs e) { docksBitfield[10] = DocksNerve1.IsChecked == true; }
        private void DocksNerve2_Click(object sender, RoutedEventArgs e) { docksBitfield[11] = DocksNerve2.IsChecked == true; }
        private void DocksNerve3_Click(object sender, RoutedEventArgs e) { docksBitfield[12] = DocksNerve3.IsChecked == true; }
        private void DocksNerve4_Click(object sender, RoutedEventArgs e) { docksBitfield[13] = DocksNerve4.IsChecked == true; }
        private void DocksNerve5_Click(object sender, RoutedEventArgs e) { docksBitfield[14] = DocksNerve5.IsChecked == true; }
        private void DocksNerve6_Click(object sender, RoutedEventArgs e) { docksBitfield[15] = DocksNerve6.IsChecked == true; }
        private void DocksNerve7_Click(object sender, RoutedEventArgs e) { docksBitfield[16] = DocksNerve7.IsChecked == true; }
        private void DocksNerve8_Click(object sender, RoutedEventArgs e) { docksBitfield[17] = DocksNerve8.IsChecked == true; }
        private void DocksBronze_Click(object sender, RoutedEventArgs e) { docksBitfield[18] = DocksBronze.IsChecked == true; }
        private void DocksSilver_Click(object sender, RoutedEventArgs e) { docksBitfield[19] = DocksSilver.IsChecked == true; }
        private void DocksGold_Click(object sender, RoutedEventArgs e) { docksBitfield[20] = DocksGold.IsChecked == true; }
        private void DocksFlag1_Click(object sender, RoutedEventArgs e) { docksBitfield[21] = DocksFlag1.IsChecked == true; }
        private void DocksFlag2_Click(object sender, RoutedEventArgs e) { docksBitfield[22] = DocksFlag2.IsChecked == true; }
        private void DocksFlag3_Click(object sender, RoutedEventArgs e) { docksBitfield[31] = DocksFlag3.IsChecked == true; }
        private void DocksOoze_TextChanged(object sender, TextChangedEventArgs e) { int ooze; if (int.TryParse(DocksOoze.Text, out ooze)) { docksOoze = ooze; } }
        // Marketplace
        private void MarketplaceToken1_Click(object sender, RoutedEventArgs e) { marketplaceBitfield[0] = MarketplaceToken1.IsChecked == true; }
        private void MarketplaceToken2_Click(object sender, RoutedEventArgs e) { marketplaceBitfield[1] = MarketplaceToken2.IsChecked == true; }
        private void MarketplaceToken3_Click(object sender, RoutedEventArgs e) { marketplaceBitfield[2] = MarketplaceToken3.IsChecked == true; }
        private void MarketplaceToken4_Click(object sender, RoutedEventArgs e) { marketplaceBitfield[3] = MarketplaceToken4.IsChecked == true; }
        private void MarketplaceToken5_Click(object sender, RoutedEventArgs e) { marketplaceBitfield[4] = MarketplaceToken5.IsChecked == true; }
        private void MarketplaceToken6_Click(object sender, RoutedEventArgs e) { marketplaceBitfield[5] = MarketplaceToken6.IsChecked == true; }
        private void MarketplaceToken7_Click(object sender, RoutedEventArgs e) { marketplaceBitfield[6] = MarketplaceToken7.IsChecked == true; }
        private void MarketplaceToken8_Click(object sender, RoutedEventArgs e) { marketplaceBitfield[7] = MarketplaceToken8.IsChecked == true; }
        private void MarketplaceToken9_Click(object sender, RoutedEventArgs e) { marketplaceBitfield[8] = MarketplaceToken9.IsChecked == true; }
        private void MarketplaceToken10_Click(object sender, RoutedEventArgs e) { marketplaceBitfield[9] = MarketplaceToken10.IsChecked == true; }
        private void MarketplaceNerve1_Click(object sender, RoutedEventArgs e) { marketplaceBitfield[10] = MarketplaceNerve1.IsChecked == true; }
        private void MarketplaceNerve2_Click(object sender, RoutedEventArgs e) { marketplaceBitfield[11] = MarketplaceNerve2.IsChecked == true; }
        private void MarketplaceNerve3_Click(object sender, RoutedEventArgs e) { marketplaceBitfield[12] = MarketplaceNerve3.IsChecked == true; }
        private void MarketplaceNerve4_Click(object sender, RoutedEventArgs e) { marketplaceBitfield[13] = MarketplaceNerve4.IsChecked == true; }
        private void MarketplaceNerve5_Click(object sender, RoutedEventArgs e) { marketplaceBitfield[14] = MarketplaceNerve5.IsChecked == true; }
        private void MarketplaceNerve6_Click(object sender, RoutedEventArgs e) { marketplaceBitfield[15] = MarketplaceNerve6.IsChecked == true; }
        private void MarketplaceNerve7_Click(object sender, RoutedEventArgs e) { marketplaceBitfield[16] = MarketplaceNerve7.IsChecked == true; }
        private void MarketplaceNerve8_Click(object sender, RoutedEventArgs e) { marketplaceBitfield[17] = MarketplaceNerve8.IsChecked == true; }
        private void MarketplaceBronze_Click(object sender, RoutedEventArgs e) { marketplaceBitfield[18] = MarketplaceBronze.IsChecked == true; }
        private void MarketplaceSilver_Click(object sender, RoutedEventArgs e) { marketplaceBitfield[19] = MarketplaceSilver.IsChecked == true; }
        private void MarketplaceGold_Click(object sender, RoutedEventArgs e) { marketplaceBitfield[20] = MarketplaceGold.IsChecked == true; }
        private void MarketplaceFlag1_Click(object sender, RoutedEventArgs e) { marketplaceBitfield[21] = MarketplaceFlag1.IsChecked == true; }
        private void MarketplaceFlag2_Click(object sender, RoutedEventArgs e) { marketplaceBitfield[22] = MarketplaceFlag2.IsChecked == true; }
        private void MarketplaceFlag3_Click(object sender, RoutedEventArgs e) { marketplaceBitfield[23] = MarketplaceFlag3.IsChecked == true; }
        private void MarketplaceFlag4_Click(object sender, RoutedEventArgs e) { marketplaceBitfield[24] = MarketplaceFlag4.IsChecked == true; }
        private void MarketplaceFlag5_Click(object sender, RoutedEventArgs e) { marketplaceBitfield[31] = MarketplaceFlag5.IsChecked == true; }
        private void MarketplaceOoze_TextChanged(object sender, TextChangedEventArgs e) { int ooze; if (int.TryParse(MarketplaceOoze.Text, out ooze)) { marketplaceOoze = ooze; } }
        // Sphinx
        private void SphinxToken1_Click(object sender, RoutedEventArgs e) { sphinxBitfield[0] = SphinxToken1.IsChecked == true; }
        private void SphinxToken2_Click(object sender, RoutedEventArgs e) { sphinxBitfield[1] = SphinxToken2.IsChecked == true; }
        private void SphinxToken3_Click(object sender, RoutedEventArgs e) { sphinxBitfield[2] = SphinxToken3.IsChecked == true; }
        private void SphinxToken4_Click(object sender, RoutedEventArgs e) { sphinxBitfield[3] = SphinxToken4.IsChecked == true; }
        private void SphinxToken5_Click(object sender, RoutedEventArgs e) { sphinxBitfield[4] = SphinxToken5.IsChecked == true; }
        private void SphinxToken6_Click(object sender, RoutedEventArgs e) { sphinxBitfield[5] = SphinxToken6.IsChecked == true; }
        private void SphinxToken7_Click(object sender, RoutedEventArgs e) { sphinxBitfield[6] = SphinxToken7.IsChecked == true; }
        private void SphinxToken8_Click(object sender, RoutedEventArgs e) { sphinxBitfield[7] = SphinxToken8.IsChecked == true; }
        private void SphinxToken9_Click(object sender, RoutedEventArgs e) { sphinxBitfield[8] = SphinxToken9.IsChecked == true; }
        private void SphinxToken10_Click(object sender, RoutedEventArgs e) { sphinxBitfield[9] = SphinxToken10.IsChecked == true; }
        private void SphinxNerve1_Click(object sender, RoutedEventArgs e) { sphinxBitfield[10] = SphinxNerve1.IsChecked == true; }
        private void SphinxNerve2_Click(object sender, RoutedEventArgs e) { sphinxBitfield[11] = SphinxNerve2.IsChecked == true; }
        private void SphinxNerve3_Click(object sender, RoutedEventArgs e) { sphinxBitfield[12] = SphinxNerve3.IsChecked == true; }
        private void SphinxNerve4_Click(object sender, RoutedEventArgs e) { sphinxBitfield[13] = SphinxNerve4.IsChecked == true; }
        private void SphinxNerve5_Click(object sender, RoutedEventArgs e) { sphinxBitfield[14] = SphinxNerve5.IsChecked == true; }
        private void SphinxNerve6_Click(object sender, RoutedEventArgs e) { sphinxBitfield[15] = SphinxNerve6.IsChecked == true; }
        private void SphinxNerve7_Click(object sender, RoutedEventArgs e) { sphinxBitfield[16] = SphinxNerve7.IsChecked == true; }
        private void SphinxNerve8_Click(object sender, RoutedEventArgs e) { sphinxBitfield[17] = SphinxNerve8.IsChecked == true; }
        private void SphinxBronze_Click(object sender, RoutedEventArgs e) { sphinxBitfield[18] = SphinxBronze.IsChecked == true; }
        private void SphinxSilver_Click(object sender, RoutedEventArgs e) { sphinxBitfield[19] = SphinxSilver.IsChecked == true; }
        private void SphinxGold_Click(object sender, RoutedEventArgs e) { sphinxBitfield[20] = SphinxGold.IsChecked == true; }
        private void SphinxOoze_TextChanged(object sender, TextChangedEventArgs e) { int ooze; if (int.TryParse(SphinxOoze.Text, out ooze)) { sphinxOoze = ooze; } }
        // Oasis
        private void OasisToken1_Click(object sender, RoutedEventArgs e) { oasisBitfield[0] = OasisToken1.IsChecked == true; }
        private void OasisToken2_Click(object sender, RoutedEventArgs e) { oasisBitfield[1] = OasisToken2.IsChecked == true; }
        private void OasisToken3_Click(object sender, RoutedEventArgs e) { oasisBitfield[2] = OasisToken3.IsChecked == true; }
        private void OasisToken4_Click(object sender, RoutedEventArgs e) { oasisBitfield[3] = OasisToken4.IsChecked == true; }
        private void OasisToken5_Click(object sender, RoutedEventArgs e) { oasisBitfield[4] = OasisToken5.IsChecked == true; }
        private void OasisToken6_Click(object sender, RoutedEventArgs e) { oasisBitfield[5] = OasisToken6.IsChecked == true; }
        private void OasisToken7_Click(object sender, RoutedEventArgs e) { oasisBitfield[6] = OasisToken7.IsChecked == true; }
        private void OasisToken8_Click(object sender, RoutedEventArgs e) { oasisBitfield[7] = OasisToken8.IsChecked == true; }
        private void OasisToken9_Click(object sender, RoutedEventArgs e) { oasisBitfield[8] = OasisToken9.IsChecked == true; }
        private void OasisToken10_Click(object sender, RoutedEventArgs e) { oasisBitfield[9] = OasisToken10.IsChecked == true; }
        private void OasisNerve1_Click(object sender, RoutedEventArgs e) { oasisBitfield[10] = OasisNerve1.IsChecked == true; }
        private void OasisNerve2_Click(object sender, RoutedEventArgs e) { oasisBitfield[11] = OasisNerve2.IsChecked == true; }
        private void OasisNerve3_Click(object sender, RoutedEventArgs e) { oasisBitfield[12] = OasisNerve3.IsChecked == true; }
        private void OasisNerve4_Click(object sender, RoutedEventArgs e) { oasisBitfield[13] = OasisNerve4.IsChecked == true; }
        private void OasisNerve5_Click(object sender, RoutedEventArgs e) { oasisBitfield[14] = OasisNerve5.IsChecked == true; }
        private void OasisNerve6_Click(object sender, RoutedEventArgs e) { oasisBitfield[15] = OasisNerve6.IsChecked == true; }
        private void OasisNerve7_Click(object sender, RoutedEventArgs e) { oasisBitfield[16] = OasisNerve7.IsChecked == true; }
        private void OasisNerve8_Click(object sender, RoutedEventArgs e) { oasisBitfield[17] = OasisNerve8.IsChecked == true; }
        private void OasisBronze_Click(object sender, RoutedEventArgs e) { oasisBitfield[18] = OasisBronze.IsChecked == true; }
        private void OasisSilver_Click(object sender, RoutedEventArgs e) { oasisBitfield[19] = OasisSilver.IsChecked == true; }
        private void OasisGold_Click(object sender, RoutedEventArgs e) { oasisBitfield[20] = OasisGold.IsChecked == true; }
        private void OasisFlag1_Click(object sender, RoutedEventArgs e) { oasisBitfield[21] = OasisFlag1.IsChecked == true; }
        private void OasisFlag2_Click(object sender, RoutedEventArgs e) { oasisBitfield[22] = OasisFlag2.IsChecked == true; }
        private void OasisFlag3_Click(object sender, RoutedEventArgs e) { oasisBitfield[23] = OasisFlag3.IsChecked == true; }
        private void OasisFlag4_Click(object sender, RoutedEventArgs e) { oasisBitfield[24] = OasisFlag4.IsChecked == true; }
        private void OasisOoze_TextChanged(object sender, TextChangedEventArgs e) { int ooze; if (int.TryParse(OasisOoze.Text, out ooze)) { oasisOoze = ooze; } }
        // Tomb
        private void TombToken1_Click(object sender, RoutedEventArgs e) { tombBitfield[0] = TombToken1.IsChecked == true; }
        private void TombToken2_Click(object sender, RoutedEventArgs e) { tombBitfield[1] = TombToken2.IsChecked == true; }
        private void TombToken3_Click(object sender, RoutedEventArgs e) { tombBitfield[2] = TombToken3.IsChecked == true; }
        private void TombToken4_Click(object sender, RoutedEventArgs e) { tombBitfield[3] = TombToken4.IsChecked == true; }
        private void TombToken5_Click(object sender, RoutedEventArgs e) { tombBitfield[4] = TombToken5.IsChecked == true; }
        private void TombToken6_Click(object sender, RoutedEventArgs e) { tombBitfield[5] = TombToken6.IsChecked == true; }
        private void TombToken7_Click(object sender, RoutedEventArgs e) { tombBitfield[6] = TombToken7.IsChecked == true; }
        private void TombToken8_Click(object sender, RoutedEventArgs e) { tombBitfield[7] = TombToken8.IsChecked == true; }
        private void TombToken9_Click(object sender, RoutedEventArgs e) { tombBitfield[8] = TombToken9.IsChecked == true; }
        private void TombToken10_Click(object sender, RoutedEventArgs e) { tombBitfield[9] = TombToken10.IsChecked == true; }
        private void TombNerve1_Click(object sender, RoutedEventArgs e) { tombBitfield[10] = TombNerve1.IsChecked == true; }
        private void TombNerve2_Click(object sender, RoutedEventArgs e) { tombBitfield[11] = TombNerve2.IsChecked == true; }
        private void TombNerve3_Click(object sender, RoutedEventArgs e) { tombBitfield[12] = TombNerve3.IsChecked == true; }
        private void TombNerve4_Click(object sender, RoutedEventArgs e) { tombBitfield[13] = TombNerve4.IsChecked == true; }
        private void TombNerve5_Click(object sender, RoutedEventArgs e) { tombBitfield[14] = TombNerve5.IsChecked == true; }
        private void TombNerve6_Click(object sender, RoutedEventArgs e) { tombBitfield[15] = TombNerve6.IsChecked == true; }
        private void TombNerve7_Click(object sender, RoutedEventArgs e) { tombBitfield[16] = TombNerve7.IsChecked == true; }
        private void TombNerve8_Click(object sender, RoutedEventArgs e) { tombBitfield[17] = TombNerve8.IsChecked == true; }
        private void TombBronze_Click(object sender, RoutedEventArgs e) { tombBitfield[18] = TombBronze.IsChecked == true; }
        private void TombSilver_Click(object sender, RoutedEventArgs e) { tombBitfield[19] = TombSilver.IsChecked == true; }
        private void TombGold_Click(object sender, RoutedEventArgs e) { tombBitfield[20] = TombGold.IsChecked == true; }
        private void TombFlag1_Click(object sender, RoutedEventArgs e) { tombBitfield[21] = TombFlag1.IsChecked == true; }
        private void TombFlag2_Click(object sender, RoutedEventArgs e) { tombBitfield[22] = TombFlag2.IsChecked == true; }
        private void TombFlag3_Click(object sender, RoutedEventArgs e) { tombBitfield[26] = TombFlag3.IsChecked == true; }
        private void TombFlag4_Click(object sender, RoutedEventArgs e) { tombBitfield[27] = TombFlag4.IsChecked == true; }
        private void TombFlag5_Click(object sender, RoutedEventArgs e) { tombBitfield[28] = TombFlag5.IsChecked == true; }
        private void TombFlag6_Click(object sender, RoutedEventArgs e) { tombBitfield[29] = TombFlag6.IsChecked == true; }
        private void TombFlag7_Click(object sender, RoutedEventArgs e) { tombBitfield[30] = TombFlag7.IsChecked == true; }
        private void TombFlag8_Click(object sender, RoutedEventArgs e) { tombBitfield[31] = TombFlag8.IsChecked == true; }
        private void TombOoze_TextChanged(object sender, TextChangedEventArgs e) { int ooze; if (int.TryParse(TombOoze.Text, out ooze)) { tombOoze = ooze; } }
        // Pyramid
        private void PyramidToken1_Click(object sender, RoutedEventArgs e) { pyramidBitfield[0] = PyramidToken1.IsChecked == true; }
        private void PyramidToken2_Click(object sender, RoutedEventArgs e) { pyramidBitfield[1] = PyramidToken2.IsChecked == true; }
        private void PyramidToken3_Click(object sender, RoutedEventArgs e) { pyramidBitfield[2] = PyramidToken3.IsChecked == true; }
        private void PyramidToken4_Click(object sender, RoutedEventArgs e) { pyramidBitfield[3] = PyramidToken4.IsChecked == true; }
        private void PyramidToken5_Click(object sender, RoutedEventArgs e) { pyramidBitfield[4] = PyramidToken5.IsChecked == true; }
        private void PyramidToken6_Click(object sender, RoutedEventArgs e) { pyramidBitfield[5] = PyramidToken6.IsChecked == true; }
        private void PyramidToken7_Click(object sender, RoutedEventArgs e) { pyramidBitfield[6] = PyramidToken7.IsChecked == true; }
        private void PyramidToken8_Click(object sender, RoutedEventArgs e) { pyramidBitfield[7] = PyramidToken8.IsChecked == true; }
        private void PyramidToken9_Click(object sender, RoutedEventArgs e) { pyramidBitfield[8] = PyramidToken9.IsChecked == true; }
        private void PyramidToken10_Click(object sender, RoutedEventArgs e) { pyramidBitfield[9] = PyramidToken10.IsChecked == true; }
        private void PyramidNerve1_Click(object sender, RoutedEventArgs e) { pyramidBitfield[10] = PyramidNerve1.IsChecked == true; }
        private void PyramidNerve2_Click(object sender, RoutedEventArgs e) { pyramidBitfield[11] = PyramidNerve2.IsChecked == true; }
        private void PyramidNerve3_Click(object sender, RoutedEventArgs e) { pyramidBitfield[12] = PyramidNerve3.IsChecked == true; }
        private void PyramidNerve4_Click(object sender, RoutedEventArgs e) { pyramidBitfield[13] = PyramidNerve4.IsChecked == true; }
        private void PyramidNerve5_Click(object sender, RoutedEventArgs e) { pyramidBitfield[14] = PyramidNerve5.IsChecked == true; }
        private void PyramidNerve6_Click(object sender, RoutedEventArgs e) { pyramidBitfield[15] = PyramidNerve6.IsChecked == true; }
        private void PyramidNerve7_Click(object sender, RoutedEventArgs e) { pyramidBitfield[16] = PyramidNerve7.IsChecked == true; }
        private void PyramidNerve8_Click(object sender, RoutedEventArgs e) { pyramidBitfield[17] = PyramidNerve8.IsChecked == true; }
        private void PyramidBronze_Click(object sender, RoutedEventArgs e) { pyramidBitfield[18] = PyramidBronze.IsChecked == true; }
        private void PyramidSilver_Click(object sender, RoutedEventArgs e) { pyramidBitfield[19] = PyramidSilver.IsChecked == true; }
        private void PyramidGold_Click(object sender, RoutedEventArgs e) { pyramidBitfield[20] = PyramidGold.IsChecked == true; }
        private void PyramidFlag1_Click(object sender, RoutedEventArgs e) { pyramidBitfield[21] = PyramidFlag1.IsChecked == true; }
        private void PyramidFlag2_Click(object sender, RoutedEventArgs e) { pyramidBitfield[22] = PyramidFlag2.IsChecked == true; }
        private void PyramidFlag3_Click(object sender, RoutedEventArgs e) { pyramidBitfield[23] = PyramidFlag3.IsChecked == true; }
        private void PyramidFlag4_Click(object sender, RoutedEventArgs e) { pyramidBitfield[24] = PyramidFlag4.IsChecked == true; }
        private void PyramidOoze_TextChanged(object sender, TextChangedEventArgs e) { int ooze; if (int.TryParse(PyramidOoze.Text, out ooze)) { pyramidOoze = ooze; } }
        // Sugar Shack
        private void SugarShackToken1_Click(object sender, RoutedEventArgs e) { sugarshackBitfield[0] = SugarShackToken1.IsChecked == true; }
        private void SugarShackToken2_Click(object sender, RoutedEventArgs e) { sugarshackBitfield[1] = SugarShackToken2.IsChecked == true; }
        private void SugarShackToken3_Click(object sender, RoutedEventArgs e) { sugarshackBitfield[2] = SugarShackToken3.IsChecked == true; }
        private void SugarShackToken4_Click(object sender, RoutedEventArgs e) { sugarshackBitfield[3] = SugarShackToken4.IsChecked == true; }
        private void SugarShackToken5_Click(object sender, RoutedEventArgs e) { sugarshackBitfield[4] = SugarShackToken5.IsChecked == true; }
        private void SugarShackToken6_Click(object sender, RoutedEventArgs e) { sugarshackBitfield[5] = SugarShackToken6.IsChecked == true; }
        private void SugarShackToken7_Click(object sender, RoutedEventArgs e) { sugarshackBitfield[6] = SugarShackToken7.IsChecked == true; }
        private void SugarShackToken8_Click(object sender, RoutedEventArgs e) { sugarshackBitfield[7] = SugarShackToken8.IsChecked == true; }
        private void SugarShackToken9_Click(object sender, RoutedEventArgs e) { sugarshackBitfield[8] = SugarShackToken9.IsChecked == true; }
        private void SugarShackToken10_Click(object sender, RoutedEventArgs e) { sugarshackBitfield[9] = SugarShackToken10.IsChecked == true; }
        private void SugarShackNerve1_Click(object sender, RoutedEventArgs e) { sugarshackBitfield[10] = SugarShackNerve1.IsChecked == true; }
        private void SugarShackNerve2_Click(object sender, RoutedEventArgs e) { sugarshackBitfield[11] = SugarShackNerve2.IsChecked == true; }
        private void SugarShackNerve3_Click(object sender, RoutedEventArgs e) { sugarshackBitfield[12] = SugarShackNerve3.IsChecked == true; }
        private void SugarShackNerve4_Click(object sender, RoutedEventArgs e) { sugarshackBitfield[13] = SugarShackNerve4.IsChecked == true; }
        private void SugarShackNerve5_Click(object sender, RoutedEventArgs e) { sugarshackBitfield[14] = SugarShackNerve5.IsChecked == true; }
        private void SugarShackNerve6_Click(object sender, RoutedEventArgs e) { sugarshackBitfield[15] = SugarShackNerve6.IsChecked == true; }
        private void SugarShackNerve7_Click(object sender, RoutedEventArgs e) { sugarshackBitfield[16] = SugarShackNerve7.IsChecked == true; }
        private void SugarShackNerve8_Click(object sender, RoutedEventArgs e) { sugarshackBitfield[17] = SugarShackNerve8.IsChecked == true; }
        private void SugarShackBronze_Click(object sender, RoutedEventArgs e) { sugarshackBitfield[18] = SugarShackBronze.IsChecked == true; }
        private void SugarShackSilver_Click(object sender, RoutedEventArgs e) { sugarshackBitfield[19] = SugarShackSilver.IsChecked == true; }
        private void SugarShackGold_Click(object sender, RoutedEventArgs e) { sugarshackBitfield[20] = SugarShackGold.IsChecked == true; }
        private void SugarShackFlag1_Click(object sender, RoutedEventArgs e) { sugarshackBitfield[21] = SugarShackFlag1.IsChecked == true; }
        private void SugarShackFlag2_Click(object sender, RoutedEventArgs e) { sugarshackBitfield[22] = SugarShackFlag2.IsChecked == true; }
        private void SugarShackFlag3_Click(object sender, RoutedEventArgs e) { sugarshackBitfield[23] = SugarShackFlag3.IsChecked == true; }
        private void SugarShackFlag4_Click(object sender, RoutedEventArgs e) { sugarshackBitfield[24] = SugarShackFlag4.IsChecked == true; }
        private void SugarShackFlag5_Click(object sender, RoutedEventArgs e) { sugarshackBitfield[25] = SugarShackFlag5.IsChecked == true; }
        private void SugarShackFlag6_Click(object sender, RoutedEventArgs e) { sugarshackBitfield[26] = SugarShackFlag6.IsChecked == true; }
        private void SugarshackOoze_TextChanged(object sender, TextChangedEventArgs e) { int ooze; if (int.TryParse(SugarshackOoze.Text, out ooze)) { sugarshackOoze = ooze; } }
        // Skilift
        private void SkiliftToken1_Click(object sender, RoutedEventArgs e) { skiliftBitfield[0] = SkiliftToken1.IsChecked == true; }
        private void SkiliftToken2_Click(object sender, RoutedEventArgs e) { skiliftBitfield[1] = SkiliftToken2.IsChecked == true; }
        private void SkiliftToken3_Click(object sender, RoutedEventArgs e) { skiliftBitfield[2] = SkiliftToken3.IsChecked == true; }
        private void SkiliftToken4_Click(object sender, RoutedEventArgs e) { skiliftBitfield[3] = SkiliftToken4.IsChecked == true; }
        private void SkiliftToken5_Click(object sender, RoutedEventArgs e) { skiliftBitfield[4] = SkiliftToken5.IsChecked == true; }
        private void SkiliftToken6_Click(object sender, RoutedEventArgs e) { skiliftBitfield[5] = SkiliftToken6.IsChecked == true; }
        private void SkiliftToken7_Click(object sender, RoutedEventArgs e) { skiliftBitfield[6] = SkiliftToken7.IsChecked == true; }
        private void SkiliftToken8_Click(object sender, RoutedEventArgs e) { skiliftBitfield[7] = SkiliftToken8.IsChecked == true; }
        private void SkiliftToken9_Click(object sender, RoutedEventArgs e) { skiliftBitfield[8] = SkiliftToken9.IsChecked == true; }
        private void SkiliftToken10_Click(object sender, RoutedEventArgs e) { skiliftBitfield[9] = SkiliftToken10.IsChecked == true; }
        private void SkiliftNerve1_Click(object sender, RoutedEventArgs e) { skiliftBitfield[10] = SkiliftNerve1.IsChecked == true; }
        private void SkiliftNerve2_Click(object sender, RoutedEventArgs e) { skiliftBitfield[11] = SkiliftNerve2.IsChecked == true; }
        private void SkiliftNerve3_Click(object sender, RoutedEventArgs e) { skiliftBitfield[12] = SkiliftNerve3.IsChecked == true; }
        private void SkiliftNerve4_Click(object sender, RoutedEventArgs e) { skiliftBitfield[13] = SkiliftNerve4.IsChecked == true; }
        private void SkiliftNerve5_Click(object sender, RoutedEventArgs e) { skiliftBitfield[14] = SkiliftNerve5.IsChecked == true; }
        private void SkiliftNerve6_Click(object sender, RoutedEventArgs e) { skiliftBitfield[15] = SkiliftNerve6.IsChecked == true; }
        private void SkiliftNerve7_Click(object sender, RoutedEventArgs e) { skiliftBitfield[16] = SkiliftNerve7.IsChecked == true; }
        private void SkiliftNerve8_Click(object sender, RoutedEventArgs e) { skiliftBitfield[17] = SkiliftNerve8.IsChecked == true; }
        private void SkiliftBronze_Click(object sender, RoutedEventArgs e) { skiliftBitfield[18] = SkiliftBronze.IsChecked == true; }
        private void SkiliftSilver_Click(object sender, RoutedEventArgs e) { skiliftBitfield[19] = SkiliftSilver.IsChecked == true; }
        private void SkiliftGold_Click(object sender, RoutedEventArgs e) { skiliftBitfield[20] = SkiliftGold.IsChecked == true; }
        private void SkiliftFlag1_Click(object sender, RoutedEventArgs e) { skiliftBitfield[21] = SkiliftFlag1.IsChecked == true; }
        private void SkiliftFlag2_Click(object sender, RoutedEventArgs e) { skiliftBitfield[22] = SkiliftFlag2.IsChecked == true; }
        private void SkiliftFlag3_Click(object sender, RoutedEventArgs e) { skiliftBitfield[23] = SkiliftFlag3.IsChecked == true; }
        private void SkiliftFlag4_Click(object sender, RoutedEventArgs e) { skiliftBitfield[24] = SkiliftFlag4.IsChecked == true; }
        private void SkiliftOoze_TextChanged(object sender, TextChangedEventArgs e) { int ooze; if (int.TryParse(SugarshackOoze.Text, out ooze)) { sugarshackOoze = ooze; } }
        // Iceberg
        private void IcebergToken1_Click(object sender, RoutedEventArgs e) { icebergBitfield[0] = IcebergToken1.IsChecked == true; }
        private void IcebergToken2_Click(object sender, RoutedEventArgs e) { icebergBitfield[1] = IcebergToken2.IsChecked == true; }
        private void IcebergToken3_Click(object sender, RoutedEventArgs e) { icebergBitfield[2] = IcebergToken3.IsChecked == true; }
        private void IcebergToken4_Click(object sender, RoutedEventArgs e) { icebergBitfield[3] = IcebergToken4.IsChecked == true; }
        private void IcebergToken5_Click(object sender, RoutedEventArgs e) { icebergBitfield[4] = IcebergToken5.IsChecked == true; }
        private void IcebergToken6_Click(object sender, RoutedEventArgs e) { icebergBitfield[5] = IcebergToken6.IsChecked == true; }
        private void IcebergToken7_Click(object sender, RoutedEventArgs e) { icebergBitfield[6] = IcebergToken7.IsChecked == true; }
        private void IcebergToken8_Click(object sender, RoutedEventArgs e) { icebergBitfield[7] = IcebergToken8.IsChecked == true; }
        private void IcebergToken9_Click(object sender, RoutedEventArgs e) { icebergBitfield[8] = IcebergToken9.IsChecked == true; }
        private void IcebergToken10_Click(object sender, RoutedEventArgs e) { icebergBitfield[9] = IcebergToken10.IsChecked == true; }
        private void IcebergNerve1_Click(object sender, RoutedEventArgs e) { icebergBitfield[10] = IcebergNerve1.IsChecked == true; }
        private void IcebergNerve2_Click(object sender, RoutedEventArgs e) { icebergBitfield[11] = IcebergNerve2.IsChecked == true; }
        private void IcebergNerve3_Click(object sender, RoutedEventArgs e) { icebergBitfield[12] = IcebergNerve3.IsChecked == true; }
        private void IcebergNerve4_Click(object sender, RoutedEventArgs e) { icebergBitfield[13] = IcebergNerve4.IsChecked == true; }
        private void IcebergNerve5_Click(object sender, RoutedEventArgs e) { icebergBitfield[14] = IcebergNerve5.IsChecked == true; }
        private void IcebergNerve6_Click(object sender, RoutedEventArgs e) { icebergBitfield[15] = IcebergNerve6.IsChecked == true; }
        private void IcebergNerve7_Click(object sender, RoutedEventArgs e) { icebergBitfield[16] = IcebergNerve7.IsChecked == true; }
        private void IcebergNerve8_Click(object sender, RoutedEventArgs e) { icebergBitfield[17] = IcebergNerve8.IsChecked == true; }
        private void IcebergBronze_Click(object sender, RoutedEventArgs e) { icebergBitfield[18] = IcebergBronze.IsChecked == true; }
        private void IcebergSilver_Click(object sender, RoutedEventArgs e) { icebergBitfield[19] = IcebergSilver.IsChecked == true; }
        private void IcebergGold_Click(object sender, RoutedEventArgs e) { icebergBitfield[20] = IcebergGold.IsChecked == true; }
        private void IcebergFlag1_Click(object sender, RoutedEventArgs e) { icebergBitfield[21] = IcebergFlag1.IsChecked == true; }
        private void IcebergOoze_TextChanged(object sender, TextChangedEventArgs e) { int ooze; if (int.TryParse(IcebergOoze.Text, out ooze)) { icebergOoze = ooze; } }
        // Iceberg
        private void HotspringsToken1_Click(object sender, RoutedEventArgs e) { hotspringsBitfield[0] = HotspringsToken1.IsChecked == true; }
        private void HotspringsToken2_Click(object sender, RoutedEventArgs e) { hotspringsBitfield[1] = HotspringsToken2.IsChecked == true; }
        private void HotspringsToken3_Click(object sender, RoutedEventArgs e) { hotspringsBitfield[2] = HotspringsToken3.IsChecked == true; }
        private void HotspringsToken4_Click(object sender, RoutedEventArgs e) { hotspringsBitfield[3] = HotspringsToken4.IsChecked == true; }
        private void HotspringsToken5_Click(object sender, RoutedEventArgs e) { hotspringsBitfield[4] = HotspringsToken5.IsChecked == true; }
        private void HotspringsToken6_Click(object sender, RoutedEventArgs e) { hotspringsBitfield[5] = HotspringsToken6.IsChecked == true; }
        private void HotspringsToken7_Click(object sender, RoutedEventArgs e) { hotspringsBitfield[6] = HotspringsToken7.IsChecked == true; }
        private void HotspringsToken8_Click(object sender, RoutedEventArgs e) { hotspringsBitfield[7] = HotspringsToken8.IsChecked == true; }
        private void HotspringsToken9_Click(object sender, RoutedEventArgs e) { hotspringsBitfield[8] = HotspringsToken9.IsChecked == true; }
        private void HotspringsToken10_Click(object sender, RoutedEventArgs e) { hotspringsBitfield[9] = HotspringsToken10.IsChecked == true; }
        private void HotspringsNerve1_Click(object sender, RoutedEventArgs e) { hotspringsBitfield[10] = HotspringsNerve1.IsChecked == true; }
        private void HotspringsNerve2_Click(object sender, RoutedEventArgs e) { hotspringsBitfield[11] = HotspringsNerve2.IsChecked == true; }
        private void HotspringsNerve3_Click(object sender, RoutedEventArgs e) { hotspringsBitfield[12] = HotspringsNerve3.IsChecked == true; }
        private void HotspringsNerve4_Click(object sender, RoutedEventArgs e) { hotspringsBitfield[13] = HotspringsNerve4.IsChecked == true; }
        private void HotspringsNerve5_Click(object sender, RoutedEventArgs e) { hotspringsBitfield[14] = HotspringsNerve5.IsChecked == true; }
        private void HotspringsNerve6_Click(object sender, RoutedEventArgs e) { hotspringsBitfield[15] = HotspringsNerve6.IsChecked == true; }
        private void HotspringsNerve7_Click(object sender, RoutedEventArgs e) { hotspringsBitfield[16] = HotspringsNerve7.IsChecked == true; }
        private void HotspringsNerve8_Click(object sender, RoutedEventArgs e) { hotspringsBitfield[17] = HotspringsNerve8.IsChecked == true; }
        private void HotspringsBronze_Click(object sender, RoutedEventArgs e) { hotspringsBitfield[18] = HotspringsBronze.IsChecked == true; }
        private void HotspringsSilver_Click(object sender, RoutedEventArgs e) { hotspringsBitfield[19] = HotspringsSilver.IsChecked == true; }
        private void HotspringsGold_Click(object sender, RoutedEventArgs e) { hotspringsBitfield[20] = HotspringsGold.IsChecked == true; }
        private void HotspringsFlag1_Click(object sender, RoutedEventArgs e) { hotspringsBitfield[21] = HotspringsFlag1.IsChecked == true; }
        private void HotspringsFlag2_Click(object sender, RoutedEventArgs e) { hotspringsBitfield[22] = HotspringsFlag2.IsChecked == true; }
        private void HotspringsFlag3_Click(object sender, RoutedEventArgs e) { hotspringsBitfield[23] = HotspringsFlag3.IsChecked == true; }
        private void HotspringsFlag4_Click(object sender, RoutedEventArgs e) { hotspringsBitfield[24] = HotspringsFlag4.IsChecked == true; }
        private void HotspringsOoze_TextChanged(object sender, TextChangedEventArgs e) { int ooze; if (int.TryParse(HotspringsOoze.Text, out ooze)) { hotspringsOoze = ooze; } }

        // allow only numbers in every text box
        private static readonly Regex _regex = new Regex("[^0-9.-]+");
        private static bool IsTextAllowed(string text)
        {
            return !_regex.IsMatch(text);
        }
        private void PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }

        private void OrientationInfoButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("All savefile data of Orientation is reset each time it is completed.", "Note");
        }

        private void buttonHundo_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult mb = MessageBox.Show("This will toggle every checkbox on. Are you sure?", "Warning", MessageBoxButton.YesNo);
            switch (mb)
            {
                case MessageBoxResult.Yes:
                    Hundo();
                    break;
                default:
                    break;
            }
        }

        private void buttonReset_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult mb = MessageBox.Show("This will reset everything. Are you sure?", "Warning", MessageBoxButton.YesNo);
            switch (mb)
            {
                case MessageBoxResult.Yes:
                    Reset();
                    break;
                default:
                    break;
            }
        }

        private void Hundo()
        {
            SetAll(this, hundo: true);
        }

        private void Reset()
        {
            SetAll(this, hundo: false);
        }

        public void SetAll(DependencyObject parent, bool hundo)
        {
            // loop through all child elements
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);

                if (child is CheckBox checkBox)
                {
                    checkBox.IsChecked = hundo;
                }
                else
                {
                    SetAll(child, hundo);
                }
                if (child is TextBox textBox)
                {
                    if(textBox != MusicVolume && textBox != SFXVolume) textBox.Text = hundo ? "100" : "0";
                }
                else
                {
                    SetAll(child, hundo);
                }
            }
        }
    }
}