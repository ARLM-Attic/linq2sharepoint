/*
 * LINQ-to-SharePoint
 * http://www.codeplex.com/LINQtoSharePoint
 * 
 * Copyright Bart De Smet (C) 2007
 * info@bartdesmet.net - http://blogs.bartdesmet.net/bart
 * 
 * This project is subject to licensing restrictions. Visit http://www.codeplex.com/LINQtoSharePoint/Project/License.aspx for more information.
 */

using Microsoft.VisualStudio.DebuggerVisualizers;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Reflection;

namespace BdsSoft.SharePoint.Linq
{
    /// <summary>
    /// Debugger visualizer for SharePointDataSource.
    /// </summary>
    public class SharePointDataSourceVisualizer : DialogDebuggerVisualizer
    {
        protected override void Show(IDialogVisualizerService windowService, IVisualizerObjectProvider objectProvider)
        {
            object data = objectProvider.GetObject();

            string caml = (string)data.GetType().GetField("_camlForDebuggerVisualizer", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(data);
            string entity = (string)data.GetType().GetField("_entityForDebuggerVisualizer", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(data);

            using (Visualizer visualizer = new Visualizer(entity, caml))
            {
                windowService.ShowDialog(visualizer);
            }
        }

        /// <summary>
        /// Tests the visualizer by hosting it outside of the debugger.
        /// </summary>
        /// <param name="objectToVisualize">The object to display in the visualizer.</param>
        public static void TestShowVisualizer(object objectToVisualize)
        {
            VisualizerDevelopmentHost visualizerHost = new VisualizerDevelopmentHost(objectToVisualize, typeof(SharePointDataSourceVisualizer));
            visualizerHost.ShowVisualizer();
        }
    }
}
