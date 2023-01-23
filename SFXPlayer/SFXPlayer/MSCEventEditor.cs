using ExCSS;
using NAudio.Wave;
using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SFXPlayer
{
    public partial class MSCEventEditor : Form
    {
        public MSCEventEditor()
        {
            InitializeComponent();
        }

        private MSCEvent MSC;
        internal static void Edit(MSCEvent msc)
        {
            Debug.WriteLine(msc);
            MSCEventEditor editor = new MSCEventEditor();
            editor.MSC = msc;
            editor.ShowDialog();
        }

        private void MSCEventEditor_Load(object sender, EventArgs e)
        {
            textBox1.Text = MSCEvent.ByteArrayToCSV(MSC.MIDIBytes);
        }

        private void textBox1_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                byte[] result = MSCEvent.CSVToByteArray(textBox1.Text);
                if ((result == null || result.Length == 0) && (MSC.MIDIBytes == null || MSC.MIDIBytes.Length == 0)) {
                    return;     //both empty, no change
                } else if ((result == null || result.Length == 0) || (MSC.MIDIBytes == null || MSC.MIDIBytes.Length == 0))
                {
                    //OK, only one is empty
                }
                else if (result.SequenceEqual(MSC.MIDIBytes))
                {
                    return;     //no change in value
                }
                MSC.MIDIBytes = result;     //arrays are different
                MessageBox.Show("Save changes!");
            }
            catch (Exception)       //can't parse csv hex to array of bytes
            {
                e.Cancel = true;
            }
        }
    }

    //https://gigperformer.com/support/midi-to-hexstring-generator.html

    //    //MIDI Show Control
    //    MIDI Show Control(MSC) operates in Run Mode only.Supported Commands to
    //    control the memory stack are:
    // GO
    // STOP(Pause)
    // RESUME(Un-pause)
    // TIMED_GO(maximum fade time is 5 minutes)
    // LOAD(Set next memory)
    // RESET(Go first memory)
    //Cue number is mandatory for LOAD.Cue numbers are optional for GO, STOP,
    //RESUME and TIMED_GO.Commands are ignored if the cue number is invalid or is
    //not programmed on the desk.Any Cue list and Cue path numbers sent by the
    //controller are ignored by the desk.
    //The following supported commands control the overall output from the desk:
    // ALL_OFF (Blackout ON)
    // RESTORE (Blackout OFF)
    //The desk listens for MSC commands that have command_format 'Lighting' (0x01), or
    //'All-Types' (0x7F). The Device_ID 0-111 that the desk listens to for MSC commands
    //can be set using the <MIDI Setup> menu.The desk will also listen to the MSC
    //broadcast Device_ID(0x7F).
}
