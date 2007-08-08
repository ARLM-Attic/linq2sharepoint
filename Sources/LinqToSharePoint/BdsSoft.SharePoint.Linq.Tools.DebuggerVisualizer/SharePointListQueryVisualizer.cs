/*
 * LINQ to SharePoint
 * http://www.codeplex.com/LINQtoSharePoint
 * 
 * Copyright Bart De Smet (C) 2007
 * info@bartdesmet.net - http://blogs.bartdesmet.net/bart
 * 
 * This project is subject to licensing restrictions. Visit http://www.codeplex.com/LINQtoSharePoint/Project/License.aspx for more information.
 */

#region Namespace imports

using System.Diagnostics;
using System.Reflection;
using Microsoft.VisualStudio.DebuggerVisualizers;

#endregion

namespace BdsSoft.SharePoint.Linq.Tools.DebuggerVisualizer
{
    /// <summary>
    /// Debugger visualizer for SharePointListSource.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Visualizer")]
    public class SharePointListQueryVisualizer : DialogDebuggerVisualizer
    {
        /// <summary>
        /// Displays the debugger visualizer.
        /// </summary>
        /// <param name="windowService">Window service for use in the debugger visualizer.</param>
        /// <param name="objectProvider">Object provider to gain access to the object to be visualized.</param>
        protected override void Show(IDialogVisualizerService windowService, IVisualizerObjectProvider objectProvider)
        {
            object data = objectProvider.GetObject();

            string caml = (string)data.GetType().GetField("_camlForDebuggerVisualizer", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(data);
            string entity = (string)data.GetType().GetField("_entityForDebuggerVisualizer", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(data);
            string linq = (string)data.GetType().GetField("_linqForDebuggerVisualizer", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(data);
            ParseErrorCollection errors = (ParseErrorCollection)data.GetType().GetField("_errorsForDebuggerVisualizer", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(data);

            using (SharePointListQueryVisualizerForm visualizer = new SharePointListQueryVisualizerForm(entity, caml, linq, errors))
            {
                windowService.ShowDialog(visualizer);
            }
        }

        /// <summary>
        /// Tests the visualizer by hosting it outside of the debugger.
        /// </summary>
        /// <param name="value">The object to display in the visualizer.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Visualizer")]
        public static void TestShowVisualizer(object value)
        {
            VisualizerDevelopmentHost visualizerHost = new VisualizerDevelopmentHost(value, typeof(SharePointListQueryVisualizer));
            visualizerHost.ShowVisualizer();
        }
    }
}