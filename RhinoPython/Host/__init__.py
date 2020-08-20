import Rhino

### init scriptcontext
# the offical Rhino.Python.Host module can not working in this environment.
# reimplement Rhino.Python.Host.Coerce3dPointFromEnumerables function.
import RhinoPython.Host
def Coerce3dPointFromEnumerables(point):
    return Rhino.Geometry.Point3d(point[0],point[1],point[2])