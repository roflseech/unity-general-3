
using UnityEngine;

public static class MathHelp
{
    /// <summary>
    /// Угол, соответствующий направлению.
    /// </summary>
    public static float RotationTo2D(Vector2 dir)
    {
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (angle < 0) angle += 360.0f;

        return angle;
    }
    /// <summary>
    /// Вращает вектор по часовой стрелке.
    /// </summary>
    public static Vector2 RotateClockWise(Vector2 dir, float angle)
    {
        angle = -angle * Mathf.Deg2Rad;
        return new Vector2(dir.x * Mathf.Cos(angle) - dir.y * Mathf.Sin(angle),
            dir.x * Mathf.Sin(angle) + dir.y * Mathf.Cos(angle));
    }
    /// <summary>
    /// Угол между дувмя векторами.
    /// </summary>
    public static float AngleBetween(Vector2 a, Vector2 b)
    {
        return Mathf.Acos(Vector2.Dot(a, b) / (a.magnitude * b.magnitude)) * Mathf.Rad2Deg;
    }
    /// <summary>
    /// Возвращает true, если есть пересечение двух отрезков, в intersection записываются координаты точки пересечения.
    /// </summary>
    public static bool HasIntersection2D(
    Vector2 start1, Vector2 end1,
    Vector2 start2, Vector2 end2,
    out Vector2 intersection)
    {
        if (start1.x > end1.x)
        {
            var tmp = start1;
            start1 = end1;
            end1 = tmp;
        }
        if (start2.x > end2.x)
        {
            var tmp = start2;
            start2 = end2;
            end2 = tmp;
        }
        var dir1 = end1 - start1;
        var dir2 = end2 - start2;
        float epsilon = 0.05f;
        if (dir1.x > epsilon && dir2.x > epsilon)
        {
            //y=kx+b
            float k1 = dir1.y / dir1.x;
            float k2 = dir2.y / dir2.x;
            float b1 = start1.y - k1 * start1.x;
            var b2 = start2.y - k2 * start2.x;

            float xIntersection = (b2 - b1) / (k1 - k2);
            if (xIntersection >= start1.x &&
                xIntersection >= start2.x &&
                xIntersection <= end1.x &&
                xIntersection <= end2.x)
            {
                intersection = new Vector2(xIntersection, xIntersection * k1 + b1);

                return true;
            }
            intersection = Vector2.zero;
            return false;
        }
        else if (dir1.x <= epsilon && dir2.x <= epsilon)
        {
            intersection = Vector2.zero;
            return false;
        }
        else
        {
            if (dir1.x <= epsilon)
            {
                float k = dir2.y / dir2.x;
                float b = start2.y - k * start2.x;
                float y = b + k * start1.x;
                float yMin = Mathf.Min(start1.y, end1.y);
                float yMax = Mathf.Max(start1.y, end1.y);
                if (y >= yMin && y <= yMax)
                {
                    intersection = new Vector2(start1.x, y);
                    return true;
                }
                else
                {
                    intersection = Vector2.zero;
                    return false;
                }
            }
            else //dir2.x <= epsilon
            {
                float k = dir1.y / dir1.x;
                float b = start1.y - k * start1.x;
                float y = b + k * start2.x;
                float yMin = Mathf.Min(start2.y, end2.y);
                float yMax = Mathf.Max(start2.y, end2.y);
                if (y >= yMin && y <= yMax)
                {
                    intersection = new Vector2(start2.x, y);
                    return true;
                }
                else
                {
                    intersection = Vector2.zero;
                    return false;
                }
            }
        }
    }
}