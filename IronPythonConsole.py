#coding=utf-8
import clr
import sys
RhinoSystem = "C:\\Program Files\\Rhino 6\\System\\"
RhinoPlugin = "C:\\Program Files\\Rhino 6\\Plug-ins\\"
clr.AddReferenceToFileAndPath(RhinoSystem + "RhinoCommon.dll")
clr.AddReference("Eto")
clr.AddReference("Eto.Wpf")
clr.AddReference("System.Windows.Forms")
clr.AddReference("System.Drawing")
sys.path.append("C:\\Program Files\\Rhino 6\\Plug-ins\\IronPython\\Lib\\")
sys.path.append("C:\\Users\\mahai\\AppData\\Roaming\\McNeel\\Rhinoceros\\6.0\\Plug-ins\\IronPython (814d908a-e25c-493d-97e9-ee3861957f49)\\settings\\lib\\")

import Rhino
import scriptcontext
scriptcontext.doc = Rhino.RhinoDoc.ActiveDoc

# todo 这部分为了与官方的RhinoPython.Host兼容
import RhinoPython.Host


