using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using Microsoft.VisualStudio.DebuggerVisualizers;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.Utilities;

[
    assembly: System.Diagnostics.DebuggerVisualizer(
    typeof(Asn1Visualizer.Asn1DebuggerSide),
    typeof(VisualizerObjectSource),
    Target = typeof(System.Byte[]),
    Description = "ASN1 Visualizer")
]

namespace Asn1Visualizer
{
    public class Asn1DebuggerSide : DialogDebuggerVisualizer
    {
        public Asn1DebuggerSide() : base(FormatterPolicy.NewtonsoftJson)
        {
        }

        //protected override void Show(IDialogVisualizerService windowService, IVisualizerObjectProvider objectProvider)
        //{
        //    var bytes = objectProvider.GetObject() as byte[];

        //    var displayForm = new DebuggerForm();
        //    var displayText = new StringBuilder();

        //    if (bytes != null)
        //    {
        //        for (var i = 0; i < bytes.Length; i++)
        //        {
        //            displayText.AppendLine(bytes[i].ToString("X"));
        //        }
        //    }
        //    displayForm.label1.Text = displayText.ToString();
        //    windowService.ShowDialog(displayForm);
        //}

        protected override void Show(IDialogVisualizerService windowService, IVisualizerObjectProvider objectProvider)
        {
            var displayForm = new DebuggerForm();
            var displayText = new StringBuilder();

            if (objectProvider is IVisualizerObjectProvider3 objectProvider3)
            {
                var bytes = objectProvider3.GetObject<byte[]>();

                Asn1InputStream input = new Asn1InputStream(bytes);
                Asn1Object p;
                while ((p = input.ReadObject()) != null)
                {
                    displayText.AppendLine(Asn1Dump.DumpAsString(p));
                }
            }

            displayForm.label1.Text = displayText.ToString();
            windowService.ShowDialog(displayForm);
        }

        public static void TestShowVisualizer(object objectToVisualize)
        {
            VisualizerDevelopmentHost visualizerHost = new VisualizerDevelopmentHost(objectToVisualize, typeof(Asn1DebuggerSide));
            visualizerHost.ShowVisualizer();
        }
    }
}
