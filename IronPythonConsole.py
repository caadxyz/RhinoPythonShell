#coding=utf-8

import clr
clr.AddReference("RhinoCommon")
clr.AddReference("Eto")
clr.AddReference("Eto.Wpf")
clr.AddReference("System.Windows.Forms")
clr.AddReference("System.Drawing")

# init sys.path
import Rhino
import sys
sys.path.append(Rhino.ApplicationSettings.FileSettings.InstallFolder.FullName + "Plug-ins\\IronPython\\Lib\\")
sys.path.append(Rhino.ApplicationSettings.FileSettings.GetDataFolder(True) +
                "Plug-ins\\IronPython (814d908a-e25c-493d-97e9-ee3861957f49)\\settings\\lib\\")

### init scriptcontext
# the offical Rhino.Python.Host module can not working in this environment.
# reimplement Rhino.Python.Host.Coerce3dPointFromEnumerables function.
import RhinoPython.Host

# the offical scriptcontext.doc can not working in this environment.
# assignment Rhino.RhinoDoc.ActiveDoc to scriptcontext.doc
import scriptcontext
scriptcontext.doc = Rhino.RhinoDoc.ActiveDoc

