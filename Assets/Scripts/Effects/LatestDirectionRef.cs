using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public abstract class LatestDirectionRef : MonoBehaviour
{
    public abstract Vector2 LatestDirection { get; }
}
