using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Windows;
using System.Windows.Threading;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using ICSharpCode.AvalonEdit.CodeCompletion;
using ICSharpCode.AvalonEdit.Folding;
using ICSharpCode.AvalonEdit.Highlighting;
using Microsoft.Win32;
using System.Resources;
using System.Windows.Controls.WpfPropertyGrid;
using Microsoft.Scripting;
using Microsoft.Scripting.Hosting;

namespace RhinoPythonShell
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            Initialized += new EventHandler(MainWindow_Initialized);
            // Load our custom highlighting definition:
            IHighlightingDefinition pythonHighlighting;
            using (Stream s = typeof(MainWindow).Assembly.GetManifestResourceStream("RhinoPythonShell.Resources.Python.xshd"))
            {
                if (s == null)
                    throw new InvalidOperationException("Could not find embedded resource");
                using (XmlReader reader = new XmlTextReader(s))
                {
                    pythonHighlighting = ICSharpCode.AvalonEdit.Highlighting.Xshd.
                        HighlightingLoader.Load(reader, HighlightingManager.Instance);
                }
            }
            // and register it in the HighlightingManager
            HighlightingManager.Instance.RegisterHighlighting("Python Highlighting", new string[] { ".cool" }, pythonHighlighting);

            InitializeComponent();

            textEditor.SyntaxHighlighting = pythonHighlighting;

            textEditor.PreviewKeyDown += new KeyEventHandler(textEditor_PreviewKeyDown);

            console.Pad.Host.ConsoleCreated += new PythonConsoleControl.ConsoleCreatedEventHandler(Host_ConsoleCreated);


        }

        string currentFileName;

        void Host_ConsoleCreated(object sender, EventArgs e)
        {
            console.Pad.Console.ConsoleInitialized += new PythonConsoleControl.ConsoleInitializedEventHandler(Console_ConsoleInitialized);
        }

        void Console_ConsoleInitialized(object sender, EventArgs e)
        {
            string assemblyLocation = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string assemblyPath = System.IO.Path.GetDirectoryName(assemblyLocation);


            ICollection<string> paths = console.Pad.Console.ScriptScope.Engine.GetSearchPaths();
            paths.Add(assemblyPath);
            console.Pad.Console.ScriptScope.Engine.SetSearchPaths(paths);
            string startupScipt = "import IronPythonConsole";
            ScriptSource scriptSource = console.Pad.Console.ScriptScope.Engine.CreateScriptSourceFromString(startupScipt, SourceCodeKind.Statements);
            try
            {
                scriptSource.Execute();
            }
            catch { }
            /*
            double[] test = new double[] { 1.2, 4.6 };
            console.Pad.Console.ScriptScope.SetVariable("test", test);
            */
        }

        void MainWindow_Initialized(object sender, EventArgs e)
        {
            //propertyGridComboBox.SelectedIndex = 1;
        }

        void openFileClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.CheckFileExists = true;
            if (dlg.ShowDialog() ?? false)
            {
                currentFileName = dlg.FileName;
                textEditor.Load(currentFileName);
                //textEditor.SyntaxHighlighting = HighlightingManager.Instance.GetDefinitionByExtension(Path.GetExtension(currentFileName));
            }
        }

        void saveFileClick(object sender, EventArgs e)
        {
            if (currentFileName == null)
            {
                SaveFileDialog dlg = new SaveFileDialog();
                dlg.DefaultExt = ".txt";
                if (dlg.ShowDialog() ?? false)
                {
                    currentFileName = dlg.FileName;
                }
                else
                {
                    return;
                }
            }
            textEditor.Save(currentFileName);
        }

        void runClick(object sender, EventArgs e)
        {
            RunStatements();
        }

        void textEditor_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F5) RunStatements();
        }

        void RunStatements()
        {
            string statementsToRun = "";
            if (textEditor.TextArea.Selection.Length > 0)
                statementsToRun = textEditor.TextArea.Selection.GetText(textEditor.TextArea.Document);
            else
                statementsToRun = textEditor.TextArea.Document.Text;
            console.Pad.Console.RunStatements(statementsToRun);
        }


    }
}

