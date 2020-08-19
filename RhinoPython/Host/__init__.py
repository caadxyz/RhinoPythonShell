import Rhino
def Coerce3dPointFromEnumerables(point):
    return Rhino.Geometry.Point3d(point[0],point[1],point[2])