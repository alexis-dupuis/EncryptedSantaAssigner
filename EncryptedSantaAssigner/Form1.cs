using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EncryptedSantaAssigner
{
    public partial class Form1 : Form
    {
        private readonly CspParameters _cspp = new CspParameters();
        private RSACryptoServiceProvider _rsa = null;

        private const string KeyName = "PersonalKey";

        private Dictionary<string, string> participants_rsaKeys = new Dictionary<string, string>();
        private string destinationFolder = "";

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            drawResultsButton.Enabled = false;
        }



        private void importParticipantsButton_Click(object sender, EventArgs e)
        {
            // Display a dialog box to select the setup file.
            openFileDialog1.InitialDirectory = initialFileBrowserFolder();
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string fName = openFileDialog1.FileName;
                if (fName != null)
                {
                    try
                    {
                        FileInfo fInfo = new FileInfo(fName);
                        readSetupFile(fInfo);

                        destinationFolder = fInfo.DirectoryName;
                        drawResultsButton.Enabled = true;
                    }
                    catch (Exception exc)
                    {
                        MessageBox.Show("Couldn't read setup file: " + exc.Message);
                    }
                }
            }
        }

        private string initialFileBrowserFolder()
        {
            return Application.StartupPath;
        }

        private void readSetupFile(FileInfo setupFileInfo)
        {
            parseParticipantsFromFile(setupFileInfo);
            displayParticipantsListInLabel();
        }

        private void parseParticipantsFromFile(FileInfo setupFileInfo)
        {
            participants_rsaKeys.Clear();

            foreach (string line in File.ReadLines(setupFileInfo.FullName))
            {
                string[] splitLine = line.Split(':');

                if (splitLine.Length == 2)
                {
                    string participantName = splitLine[0].Trim();
                    string participantKey = splitLine[1].Trim();

                    if (!participants_rsaKeys.ContainsKey(participantName))
                    {
                        participants_rsaKeys.Add(participantName, participantKey);
                    }
                    else
                    {
                        MessageBox.Show("Duplicate in participants names: " + participantName + ". This is not allowed: please edit your setup file.");
                    }
                }
                else
                {
                    Console.WriteLine("Couldn't read participant info from following line's content: " + line);
                }
            }
        }

        private void displayParticipantsListInLabel()
        {
            string participantsListStr = "";
            foreach(string participantName in participants_rsaKeys.Keys)
            {
                participantsListStr += participantName + ", ";
            }

            if (participantsListStr.Length > 0)
                participantsListStr.Remove(participantsListStr.Length - 2);

            participantsListLabel.Text = participantsListStr;
        }



        private void drawResultsButton_Click(object sender, EventArgs e)
        {
            // Assign participants to each other randomly
            List<string> assigneesNames = new List<string>(participants_rsaKeys.Keys); // Put participants in a hat
            assigneesNames.DerangementShuffle(); // Shuffle it

            // For each participant, encrypt its assignee's name using the participant's key
            List<string> assigneesNamesEncrypted = new List<string>();
            for (int i = 0; i < participants_rsaKeys.Count; i++)
            {
                importParticipantPublicKey(participants_rsaKeys.Keys.ElementAt(i)); // Updates the _rsa with the participant's public key info, so that each assignee is encrypted with the participant's Key (only he will be able to decrypt the assignee's name)

                string assigNmeEncryp = EncryptionUtilities.ToEncryptedString(assigneesNames[i], _rsa);
                assigneesNamesEncrypted.Add(assigNmeEncryp);
            }

            // Print results in an output file
            using (StreamWriter swr = new StreamWriter(destinationFolder + "/Results.txt"))
            {
                for (int i = 0; i < participants_rsaKeys.Count; i++)
                {
                    swr.WriteLine(participants_rsaKeys.Keys.ElementAt(i) + " : " + assigneesNamesEncrypted[i]);
                }
            }
            
            MessageBox.Show("Everyone has been assigned a person to gift!\n See results here: " + destinationFolder + "/Results.txt");
            drawResultsButton.Enabled = false;
        }

        private void importParticipantPublicKey(string participantName)
        {
            _cspp.KeyContainerName = KeyName;
            _rsa = new RSACryptoServiceProvider(_cspp);

            string keytxt = participants_rsaKeys[participantName];
            _rsa.FromXmlString(keytxt);
            _rsa.PersistKeyInCsp = true;
        }
    }
}
